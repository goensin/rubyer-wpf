using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 消息操作类
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Transition 动画时长
        /// </summary>
        public const int TransitionTime = 300;

        /// <summary>
        /// 所有容器集合
        /// </summary>
        internal static Dictionary<string, MessageContainer> Containers = new Dictionary<string, MessageContainer>();

        /// <summary>
        /// 更新容器
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="identify">标识</param>
        internal static void UpdateContainer(MessageContainer container, string identify)
        {
            if (Containers.ContainsKey(identify))
            {
                _ = Containers.Remove(identify);
            }

            Containers.Add(identify, container);
        }

        private static MessageCard GetMessageCard(MessageType type, object content, int millisecondTimeOut, bool isClearable)
        {
            isClearable = millisecondTimeOut <= 0 || isClearable;
            return new MessageCard
            {
                Type = type,
                Content = content,
                IsClearable = isClearable
            };
        }

        /// <summary>
        /// 延期关闭消息卡片
        /// </summary>
        /// <param name="millisecondTimeOut">显示时间，小于 0 时不自动关闭</param>
        /// <param name="messageCard">消息卡片</param>
        /// <param name="token">取消令牌</param>
        private static void DelayCloseMessageCard(int millisecondTimeOut, MessageCard messageCard, CancellationToken token)
        {
            if (millisecondTimeOut > 0)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(millisecondTimeOut + TransitionTime, token);

                    if (messageCard is { })
                    {
                        messageCard.Dispatcher.Invoke(() => messageCard.IsShow = false);
                    }
                }, token);
            }
        }

        #region 全局

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowGlobal(MessageType type, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            MessageWindow messageWindow = MessageWindow.GetInstance();
            messageWindow.Dispatcher.VerifyAccess();

            MessageCard messageCard = GetMessageCard(type, content, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            messageCard.Close += (sender, e) => messageWindow.Remove(messageCard);
            messageCard.MouseEnter += (sender, e) => cts.Cancel();
            messageCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
            };

            messageWindow.Show();
            messageWindow.Add(messageCard);
            DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowGlobal(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(MessageType.None, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示信息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void InfoGlobal(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(MessageType.Info, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示成功
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void SuccessGlobal(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(MessageType.Success, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示警告
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void WarningGlobal(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(MessageType.Warning, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示错误
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ErrorGlobal(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(MessageType.Error, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局清除所有消息
        /// </summary>
        public static void ClearAllGlobal()
        {
            MessageWindow messageWindow = MessageWindow.GetInstance();
            messageWindow.Dispatcher.VerifyAccess();
            messageWindow.ClearAll();
        }

        #endregion 全局

        #region 指定容器

        private static MessageContainer GetRootContainer()
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            MessageContainer container = activedWindow.TryGetChildFromVisualTree<MessageContainer>(null) ?? throw new NullReferenceException("Can't Find the MessageContainer");
            return container;
        }

        private static void ShowInternal(MessageContainer container, MessageType type, object content, int millisecondTimeOut, bool isClearable)
        {
            container.Dispatcher.VerifyAccess();
            MessageCard messageCard = GetMessageCard(type, content, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            messageCard.Close += (sender, e) => container.RemoveCard(messageCard);
            messageCard.MouseEnter += (sender, e) => cts.Cancel();
            messageCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
            };

            container.AddCard(messageCard);
            DelayCloseMessageCard(millisecondTimeOut, messageCard, cts.Token);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="type">类型</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(string containerIdentifier, MessageType type, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            if (!Containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"The message container Identifier '{containerIdentifier}' could not be found");
            }

            MessageContainer container = Containers[containerIdentifier];
            ShowInternal(container, type, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(MessageType type, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            MessageContainer container = GetRootContainer();
            ShowInternal(container, type, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(string containerIdentifier, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.None, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.None, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Info(string containerIdentifier, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Info, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Info(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Info, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Success(string containerIdentifier, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Success, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Success(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Success, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Warning(string containerIdentifier, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Warning, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Warning(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Warning, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Error(string containerIdentifier, object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Error, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Error(object content, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Error, content, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 清除所有通知
        /// </summary>
        public static void ClearAll(string containerIdentifier)
        {
            if (!Containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"The notification container Identifier '{containerIdentifier}' could not be found");
            }

            Containers[containerIdentifier]?.ClearCards();
        }

        /// <summary>
        /// 清除所有通知
        /// </summary>
        public static void ClearAll()
        {
            GetRootContainer()?.ClearCards();
        }

        #endregion 指定容器
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