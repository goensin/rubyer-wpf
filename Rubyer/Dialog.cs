﻿using Rubyer.Commons;
using System;
using System.Collections.Generic;

namespace Rubyer
{
    /// <summary>
    /// 对话框
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// 对话框集合
        /// </summary>
        public static Dictionary<string, DialogBox> Dialogs { get; private set; } = new Dictionary<string, DialogBox>();

        /// <summary>
        /// 添加对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="dialogBox">对话框</param>
        internal static void AddDialogBox(string identifier, DialogBox dialogBox)
        {
            if (Dialogs.ContainsKey(identifier))
            {
                _ = Dialogs.Remove(identifier);
            }

            Dialogs.Add(identifier, dialogBox);
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="parameters">参数</param>
        /// <param name="title">标题</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="isShowCloseButton">是否显示默认关闭按钮</param>
        public static void Show(string identifier, object content, IParameters parameters = null, string title = null, Action<DialogBox> openHandler = null, Action<DialogBox, IParameters> closeHandle = null, bool isShowCloseButton = true)
        {
            if (Dialogs.TryGetValue(identifier, out DialogBox dialogBox))
            {
                dialogBox.Dispatcher.VerifyAccess();

                if (dialogBox.DataContext is IDialogContext dialogContext)
                {
                    dialogBox.Title = string.IsNullOrEmpty(dialogContext.Title) ? dialogBox.Title : dialogContext.Title;
                    dialogContext.RequestClose += (param) =>
                    {
                        DialogBox.CloseDialogCommand.Execute(param, dialogBox);
                    };
                }
                else
                {
                    dialogBox.Title = string.IsNullOrEmpty(title) ? title : dialogBox.Title;
                }

                dialogBox.DialogContent = content;
                dialogBox.IsShowCloseButton = isShowCloseButton;
                dialogBox.BeforeOpenHandler = openHandler;
                dialogBox.AfterCloseHandler = closeHandle;
                DialogBox.OpenDialogCommand.Execute(parameters, dialogBox);
            }
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="identifier">标识</param>
        /// <param name="parameter">参数</param>
        public static void Close(string identifier, IParameters parameter = null)
        {
            if (Dialogs.TryGetValue(identifier, out DialogBox dialogBox))
            {
                DialogBox.CloseDialogCommand.Execute(parameter, dialogBox);
            }
        }
    }
}
