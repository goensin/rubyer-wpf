using Rubyer.Commons;
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
        /// 所有容器集合
        /// </summary>
        internal static Dictionary<string, DialogContainer> Containers = new Dictionary<string, DialogContainer>();

        /// <summary>
        /// 更新信息框容器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="identify"></param>
        internal static void UpdateContainer(DialogContainer container, string identify)
        {
            if (Containers.ContainsKey(identify))
            {
                _ = Containers.Remove(identify);
            }

            Containers.Add(identify, container);
        }

        private static DialogCard GetDialogCard(object content, string title, bool? showCloseButton)
        {
            DialogCard card = new()
            {
                Content = content,
            };

            if (showCloseButton is { })
            {
                card.IsShowCloseButton = showCloseButton.Value;
            }

            if (!string.IsNullOrEmpty(title))
            {
                card.Title = title;
            }

            return card;
        }

        private static async Task<object> ShowInternal(DialogContainer container, object content, object parameters, string title, Action<DialogCard> openHandler, Action<DialogCard, object> closeHandle, bool? showCloseButton)
        {
            TaskCompletionSource<object> taskCompletionSource = new();
            container.Dispatcher.VerifyAccess();
            DialogCard card = GetDialogCard(content, title, showCloseButton);

            if (!dataContextConfiguration.OnOpenAction(container, card, content, parameters))
            {
                card.Title = title;
            }

            card.BeforeOpenHandler = openHandler;
            card.AfterCloseHandler = closeHandle;

            WeakEventManager<DialogCard, DialogResultRoutedEventArgs>.AddHandler(card, "Closed", (sender, e) =>
            {
                taskCompletionSource.TrySetResult(e.Result);
            });

            container.AddCard(card);

            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="parameters">参数</param>
        /// <param name="title">标题</param>
        /// <param name="openHandler">打开时回调</param>
        /// <param name="closeHandle">关闭时回调</param>
        /// <param name="showCloseButton">显示关闭按钮</param>
        /// <returns>返回数据</returns>
        /// <exception cref="NullReferenceException">找不到对话框 ID</exception>
        public static async Task<object> Show(object content, object parameters = null, string title = null, Action<DialogCard> openHandler = null, Action<DialogCard, object> closeHandle = null, bool? showCloseButton = null)
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            DialogContainer container = activedWindow.TryGetChildFromVisualTree<DialogContainer>(null) ?? throw new NullReferenceException("Can't Find the DialogContainer");
            return await ShowInternal(container, content, parameters, title, openHandler, closeHandle, showCloseButton);
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="identifier">对话框 ID</param>
        /// <param name="content">内容</param>
        /// <param name="parameters">参数</param>
        /// <param name="title">标题</param>
        /// <param name="openHandler">打开时回调</param>
        /// <param name="closeHandle">关闭时回调</param>
        /// <param name="showCloseButton">显示关闭按钮</param>
        /// <returns>返回数据</returns>
        /// <exception cref="NullReferenceException">找不到对话框 ID</exception>
        public static async Task<object> Show(string identifier, object content, object parameters = null, string title = null, Action<DialogCard> openHandler = null, Action<DialogCard, object> closeHandle = null, bool? showCloseButton = null)
        {
            if (Containers.TryGetValue(identifier, out DialogContainer container))
            {
                return await ShowInternal(container, content, parameters, title, openHandler, closeHandle, showCloseButton);
            }

            throw new NullReferenceException($"The dialog container Identifier '{identifier}' could not be found");
        }

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
        /// 关闭对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="parameter">参数</param>
        public static void Close(string identifier, object parameter = null)
        {
            if (Containers.TryGetValue(identifier, out DialogContainer dialog))
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