using System;
using System.Collections.Generic;

namespace Rubyer
{
    /// <summary>
    /// 对话框
    /// </summary>
    public class Dialog
    {
        public static Dictionary<string, DialogBox> dialogs = new Dictionary<string, DialogBox>();

        /// <summary>
        /// 添加对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="dialogBox">对话框</param>
        public static void AddDialogBox(string identifier, DialogBox dialogBox)
        {
            if (dialogs.ContainsKey(identifier))
            {
                _ = dialogs.Remove(identifier);
            }

            dialogs.Add(identifier, dialogBox);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="title">标题</param>
        /// <param name="isShowCloseButton">是否显示默认关闭按钮</param>
        public static void Show(string identifier, object content, string title, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle, bool isShowCloseButton)
        {
            if (!dialogs.ContainsKey(identifier))
            {
                return;
            }

            DialogBox dialog = dialogs[identifier];
            dialog.Dispatcher.VerifyAccess();
            dialog.DialogContent = content;
            dialog.Title = string.IsNullOrEmpty(title) ? dialog.Title : title;
            dialog.IsShowCloseButton = isShowCloseButton;
            dialog.BeforeOpenHandler = openHandler;
            dialog.AfterCloseHandler = closeHandle;
            dialog.IsShow = true;
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        public static void Show(string identifier, object content)
        {
            Show(identifier, content, "", null, null, true);
        }
        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        public static void Show(string identifier, object content, string title)
        {
            Show(identifier, content, title, null, null, true);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="isShowCloseButton">是否显示默认关闭按钮</param>
        public static void Show(string identifier, object content, string title, bool isShowCloseButton)
        {
            Show(identifier, content, title, null, null, isShowCloseButton);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        public static void Show(string identifier, object content, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, string.Empty, null, closeHandle, true);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        public static void Show(string identifier, object content, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, string.Empty, openHandler, closeHandle, true);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="title">标题</param>
        public static void Show(string identifier, object content, string title, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, title, null, closeHandle, true);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="title">标题</param>
        public static void Show(string identifier, object content, string title, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, title, openHandler, closeHandle, true);
        }

    }
}
