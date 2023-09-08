using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Rubyer.Commons;

namespace Rubyer
{
    /// <summary>
    /// 通知操作类
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Transition 动画时长
        /// </summary>
        public const int TransitionTime = 300;

        /// <summary>
        /// 所有容器集合
        /// </summary>
        internal static Dictionary<string, NotificationContainer> Containers = new Dictionary<string, NotificationContainer>();

        /// <summary>
        /// 更新容器
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="identify">标识</param>
        internal static void UpdateContainer(NotificationContainer container, string identify)
        {
            if (Containers.ContainsKey(identify))
            {
                _ = Containers.Remove(identify);
            }

            Containers.Add(identify, container);
        }

        private static NotificationCard GetNotificationCard(NotificationType type, object content, string title, int millisecondTimeOut, bool isClearable)
        {
            var notificationCard = new NotificationCard
            {
                Type = type,
                Content = content,
                IsClearable = isClearable,
            };

            if (!string.IsNullOrEmpty(title))
            {
                notificationCard.Title = title;
            }

            return notificationCard;
        }

        /// <summary>
        /// 延期关闭通知卡片
        /// </summary>
        /// <param name="millisecondTimeOut">显示时间，为 0 时不自动关闭</param>
        /// <param name="notificationCard">通知卡片</param>
        /// <param name="token">取消令牌</param>
        private static void DelayCloseNotificationCard(int millisecondTimeOut, NotificationCard notificationCard, CancellationToken token)
        {
            if (millisecondTimeOut > 0)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(millisecondTimeOut + TransitionTime, token);

                    if (notificationCard is { })
                    {
                        notificationCard.Dispatcher.Invoke(() => notificationCard.IsShow = false);
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
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowGlobal(NotificationType type, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            NotificationWindow notificationWindow = NotificationWindow.GetInstance();
            notificationWindow.Dispatcher.VerifyAccess();
            NotificationCard notificationCard = GetNotificationCard(type, content, title, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            notificationCard.Close += (sender, e) => notificationWindow.Remove(notificationCard);
            notificationCard.MouseEnter += (sender, e) => cts.Cancel();
            notificationCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseNotificationCard(millisecondTimeOut, notificationCard, cts.Token);
            };

            notificationWindow.Show();
            notificationWindow.Add(notificationCard);
            DelayCloseNotificationCard(millisecondTimeOut, notificationCard, cts.Token);
        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ShowGlobal(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(NotificationType.None, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示信息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void InfoGlobal(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(NotificationType.Info, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示成功
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void SuccessGlobal(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(NotificationType.Success, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示警告
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void WarningGlobal(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(NotificationType.Warning, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局显示错误
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void ErrorGlobal(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            ShowGlobal(NotificationType.Error, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 全局清除所有通知
        /// </summary>
        public static void ClearAllGlobal()
        {
            NotificationWindow notificationWindow = NotificationWindow.GetInstance();
            notificationWindow.Dispatcher.VerifyAccess();
            notificationWindow.ClearAll();
        }

        #endregion 全局

        #region 指定容器

        private static NotificationContainer GetRootContainer()
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            NotificationContainer container = activedWindow.TryGetChildFromVisualTree<NotificationContainer>(null) ?? throw new NullReferenceException("Can't Find the NotificationContainer");
            return container;
        }

        private static void ShowInternal(NotificationContainer container, NotificationType type, object content, string title, int millisecondTimeOut, bool isClearable)
        {
            container.Dispatcher.VerifyAccess();
            NotificationCard notificationCard = GetNotificationCard(type, content, title, millisecondTimeOut, isClearable);
            CancellationTokenSource cts = new CancellationTokenSource();

            notificationCard.Close += (sender, e) => container.RemoveCard(notificationCard);
            notificationCard.MouseEnter += (sender, e) => cts.Cancel();
            notificationCard.MouseLeave += (sender, e) =>
            {
                cts = new CancellationTokenSource();
                DelayCloseNotificationCard(millisecondTimeOut, notificationCard, cts.Token);
            };

            container.AddCard(notificationCard);
            DelayCloseNotificationCard(millisecondTimeOut, notificationCard, cts.Token);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="type">类型</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(string containerIdentifier, NotificationType type, object content, string title = "", int millisecondTimeOut = 5000, bool isClearable = true)
        {
            if (!Containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"The notification container Identifier '{containerIdentifier}' could not be found");
            }

            NotificationContainer container = Containers[containerIdentifier];
            ShowInternal(container, type, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(NotificationType type, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            var activedWindow = WindowHelper.GetCurrentWindow() ?? throw new NullReferenceException("Can't find the actived window");
            NotificationContainer container = activedWindow.TryGetChildFromVisualTree<NotificationContainer>(null) ?? throw new NullReferenceException("Can't Find the NotificationContainer");
            ShowInternal(container, type, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(string containerIdentifier, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, NotificationType.None, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Show(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(NotificationType.None, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Info(string containerIdentifier, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, NotificationType.Info, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示信息
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Info(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(NotificationType.Info, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Success(string containerIdentifier, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, NotificationType.Success, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示成功
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Success(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(NotificationType.Success, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Warning(string containerIdentifier, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, NotificationType.Warning, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示警告
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Warning(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(NotificationType.Warning, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Error(string containerIdentifier, object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, NotificationType.Error, content, title, millisecondTimeOut, isClearable);
        }

        /// <summary>
        /// 容器内显示错误
        /// (默认 Actived Window 下顶层 MessageContainer 容器)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="millisecondTimeOut">关闭延时，为 0 时不自动关闭</param>
        /// <param name="isClearable">是否显示关闭按钮</param>
        public static void Error(object content, string title = "", int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(NotificationType.Error, content, title, millisecondTimeOut, isClearable);
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
    /// 通知类型
    /// </summary>
    public enum NotificationType
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