using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static Rubyer.DialogContainer;

namespace Rubyer
{
    /// <summary>
    /// 对话框操作类
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// 对话框集合
        /// </summary>
        internal static Dictionary<string, DialogContainer> Dialogs { get; private set; } = new Dictionary<string, DialogContainer>();

        private static IDialogDataContextConfiguration dataContextConfiguration = new DialogDataContextConfiguration();

        /// <summary>
        /// 配置对话框 DataContext 操作逻辑
        /// </summary>
        /// <param name="configuration">配置逻辑</param>
        public static void ConfigureDataContextAction(IDialogDataContextConfiguration configuration)
        {
            dataContextConfiguration = configuration;
        }

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

        private static async Task<object> ShowInternal(DialogContainer dialog, object content, object parameters, string title, Action<DialogContainer> openHandler, Action<DialogContainer, object> closeHandle, bool? showCloseButton)
        {
            dialog.Dispatcher.VerifyAccess();

            if (!dataContextConfiguration.OnOpenAction(dialog, content, parameters))
            {
                dialog.Title = title;
            }

            dialog.DialogContent = content;
            dialog.IsShowCloseButton = showCloseButton == null ? dialog.IsShowCloseButton : showCloseButton.Value;
            dialog.BeforeOpenHandler = openHandler;
            dialog.AfterCloseHandler = closeHandle;
            DialogContainer.OpenDialogCommand.Execute(parameters, dialog);

            var taskCompletionSource = new TaskCompletionSource<object>();
            dialog.Tag = taskCompletionSource;

            dialog.AfterClose += OnDialogClosed;

            var result = await taskCompletionSource.Task;

            dialog.AfterClose -= OnDialogClosed;

            dataContextConfiguration.OnCloseAction();

            return result;
        }

        private static void OnDialogClosed(object sender, DialogResultRoutedEventArgs e)
        {
            var dialog = sender as DialogContainer;
            var taskCompletionSource = dialog.Tag as TaskCompletionSource<object>;
            taskCompletionSource?.TrySetResult(e.Result);
            dialog.DialogContent = null;
            dialog.AfterClose -= OnDialogClosed;
            dialog.Tag = null;
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
        public static async Task<object> Show(string identifier, object content, object parameters = null, string title = null, Action<DialogContainer> openHandler = null, Action<DialogContainer, object> closeHandle = null, bool? showCloseButton = null)
        {
            if (Dialogs.TryGetValue(identifier, out DialogContainer container))
            {
                return await ShowInternal(container, content, parameters, title, openHandler, closeHandle, showCloseButton);
            }

            throw new NullReferenceException($"The dialog container Identifier '{identifier}' could not be found");
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
        public static async Task<object> Show(object content, object parameters = null, string title = null, Action<DialogContainer> openHandler = null, Action<DialogContainer, object> closeHandle = null, bool? showCloseButton = null)
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            DialogContainer container = activedWindow.TryGetChildFromVisualTree<DialogContainer>(null) ?? throw new NullReferenceException("Can't Find the DialogContainer");
            return await ShowInternal(container, content, parameters, title, openHandler, closeHandle, showCloseButton);
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
        public static void Close(DialogContainer dialog, object parameter = null)
        {
            DialogContainer.CloseDialogCommand.Execute(parameter, dialog);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <exception cref="NullReferenceException">找不到对话框</exception>
        public static void Close(object parameter = null)
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            DialogContainer container = activedWindow.TryGetChildFromVisualTree<DialogContainer>(null) ?? throw new NullReferenceException("Can't Find the DialogContainer");
            Close(container, parameter);
        }
    }
}