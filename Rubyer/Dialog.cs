using Rubyer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// 对话框操作类
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// 默认对话框标识
        /// </summary>
        public const string DefaultDialogIdentifier = "Rubyer.Dialog";

        /// <summary>
        /// 对话框集合
        /// </summary>
        internal static Dictionary<string, DialogContainer> Dialogs { get; private set; } = new Dictionary<string, DialogContainer>();

        /// <summary>
        /// 添加对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="dialog">对话框</param>
        internal static void AddDialogContainer(string identifier, DialogContainer dialog)
        {
            if (Dialogs.ContainsKey(identifier))
            {
                _ = Dialogs.Remove(identifier);
            }

            Dialogs.Add(identifier, dialog);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogContainer 标识</param>
        /// <param name="content">内容</param>
        /// <param name="parameters">参数</param>
        /// <param name="title">标题</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="showCloseButton">是否显示默认关闭按钮</param>
        /// <returns>结果</returns>
        public static async Task<object> Show(string identifier, object content, object parameters = null, string title = null, Action<DialogContainer> openHandler = null, Action<DialogContainer, object> closeHandle = null, bool showCloseButton = true)
        {
            if (Dialogs.TryGetValue(identifier, out DialogContainer dialog))
            {
                dialog.Dispatcher.VerifyAccess();

                if (content is FrameworkElement element && element.DataContext is IDialogViewModel dialogContext)
                {
                    dialog.Title = string.IsNullOrEmpty(dialogContext.Title) ? title : dialogContext.Title;
                    dialogContext.RequestClose += (param) =>
                    {
                        DialogContainer.CloseDialogCommand.Execute(param, dialog);
                    };
                }
                else
                {
                    dialog.Title = title;
                }

                dialog.DialogContent = content;
                dialog.IsShowCloseButton = showCloseButton;
                dialog.BeforeOpenHandler = openHandler;
                dialog.AfterCloseHandler = closeHandle;
                DialogContainer.OpenDialogCommand.Execute(parameters, dialog);

                var taskCompletionSource = new TaskCompletionSource<object>();
                DialogContainer.DialogResultRoutedEventHandler handle = (sender, e) => taskCompletionSource.TrySetResult(e.Result);
                dialog.AfterClose -= handle;
                dialog.AfterClose += handle;

                return await taskCompletionSource.Task;
            }

            return default;
        }

        /// <summary>
        /// 显示指定对话框
        /// (默认 RubyerWindow 下 DialogContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="parameters">参数</param>
        /// <param name="title">标题</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="showCloseButton">是否显示默认关闭按钮</param>
        /// <returns>结果</returns>
        public static async Task<object> Show(object content, object parameters = null, string title = null, Action<DialogContainer> openHandler = null, Action<DialogContainer, object> closeHandle = null, bool showCloseButton = true)
        {
            return await Show(DefaultDialogIdentifier, content, parameters, title, openHandler, closeHandle, showCloseButton);
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="parameter">参数</param>
        public static void Close(string identifier, object parameter = null)
        {
            if (Dialogs.TryGetValue(identifier, out DialogContainer dialog))
            {
                DialogContainer.CloseDialogCommand.Execute(parameter, dialog);
            }
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="dialog">对话框</param>
        /// <param name="parameter">参数</param>
        public static void Close(DialogContainer dialog, IParameters parameter = null)
        {
            DialogContainer.CloseDialogCommand.Execute(parameter, dialog);
        }
    }
}