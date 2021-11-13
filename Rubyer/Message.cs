﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    public class Message
    {
        private const int TransitionTime = 300;
        private static readonly Style infoStyle = (Style)Application.Current.Resources["InfoMessage"];
        private static readonly Style warningStyle = (Style)Application.Current.Resources["WarningMessage"];
        private static readonly Style successStyle = (Style)Application.Current.Resources["SuccessMessage"];
        private static readonly Style errorStyle = (Style)Application.Current.Resources["ErrorMessage"];

        public static Dictionary<string, MessageContainer> containers = new Dictionary<string, MessageContainer>();

        #region 全局

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(MessageType type, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            MessageWindow messageWindow = MessageWindow.GetInstance();
            messageWindow.Dispatcher.VerifyAccess();
            MessageCard messageCard = GetMessageCard(type, element, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            messageCard.Close += (sender, e) => messageWindow.RemoveMessageCard(messageCard);
            messageCard.MouseEnter += (sender, e) => cts.Cancel();
            messageCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
            };

            messageWindow.Show();
            messageWindow.AddMessageCard(messageCard);
            DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.None, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示信息
        /// </summary>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInfo(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Info, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示成功
        /// </summary>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowSuccess(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Success, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示警告
        /// </summary>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowWarning(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Warning, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示错误
        /// </summary>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowError(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Error, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(type, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.None, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInfo(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Info, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示成功
        /// </summary>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowSuccess(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Success, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示警告
        /// </summary>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowWarning(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Warning, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示错误
        /// </summary>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowError(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Error, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }
        #endregion

        #region 指定容器

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="type">类型</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInContainer(string containerIdentifier, MessageType type, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            if (!containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"找不到 Identifier 为{containerIdentifier}消息容器");
            }

            MessageContainer container = containers[containerIdentifier];
            container.Dispatcher.VerifyAccess();
            MessageCard messageCard = GetMessageCard(type, element, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            messageCard.Close += (sender, e) => container.RemoveMessageCard(messageCard);
            messageCard.MouseEnter += (sender, e) => cts.Cancel();
            messageCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
            };

            container.AddMessageCard(messageCard);
            DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInContainer(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.None, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void InfoInContainer(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Info, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void SuccessInContainer(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Success, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void WarningInContainer(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Warning, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="element">内容元素</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ErrorInContainer(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Error, element, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="type">类型</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInContainer(string containerIdentifier, MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, type, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowInContainer(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.None, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void InfoInContainer(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Info, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void SuccessInContainer(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Success, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void WarningInContainer(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Warning, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ErrorInContainer(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowInContainer(containerIdentifier, MessageType.Error, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }


        #endregion

        /// <summary>
        /// 更新容器
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="identify">标识</param>
        internal static void UpdateContainer(MessageContainer container, string identify)
        {
            if (containers.ContainsKey(identify))
            {
                _ = containers.Remove(identify);
            }

            containers.Add(identify, container);
        }

        private static MessageCard GetMessageCard(MessageType type, UIElement element, int millisecondTimeOut, bool isClearable)
        {
            isClearable = millisecondTimeOut <= 0 || isClearable;
            MessageCard messageCard = new MessageCard
            {
                Content = element,
                IsClearable = isClearable,
            };

            switch (type)
            {
                default:
                case MessageType.None:
                    break;
                case MessageType.Info:
                    messageCard.Style = infoStyle;
                    break;
                case MessageType.Warning:
                    messageCard.Style = warningStyle;
                    break;
                case MessageType.Success:
                    messageCard.Style = successStyle;
                    break;
                case MessageType.Error:
                    messageCard.Style = errorStyle;
                    break;
            }

            return messageCard;
        }

        /// <summary>
        /// 延期关闭消息卡片
        /// </summary>
        /// <param name="millisecondTimeOut">显示时间，为 0 时不自动关闭</param>
        /// <param name="messageCard">消息卡片</param>
        /// <param name="token">取消令牌</param>
        private static void DelayCloseMessageCard(int millisecondTimeOut, MessageCard messageCard, CancellationToken token)
        {
            if (millisecondTimeOut > 0)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(millisecondTimeOut + TransitionTime, token);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        messageCard.IsShow = false;
                    });
                }, token);
            }
        }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 信息
        /// </summary>
        [Description("信息")]
        Info,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error
    }
}
