using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public class MessageBoxR
    {
        private static readonly Brush infoBrush = (Brush)Application.Current.Resources["Info"];
        private static readonly Brush warningBrush = (Brush)Application.Current.Resources["Warning"];
        private static readonly Brush successBrush = (Brush)Application.Current.Resources["Success"];
        private static readonly Brush errorBrush = (Brush)Application.Current.Resources["Error"];
        private static readonly Brush questionBrush = (Brush)Application.Current.Resources["Question"];

        public static Dictionary<string, MessageBoxContainer> containers = new Dictionary<string, MessageBoxContainer>();

        /// <summary>
        /// 更新信息框容器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="identify"></param>
        internal static void UpdateContainer(MessageBoxContainer container, string identify)
        {
            if (containers.ContainsKey(identify))
            {
                _ = containers.Remove(identify);
            }

            containers.Add(identify, container);
        }

        private static MessageBoxCard GetMessageBoxCard(string message, string title, MessageBoxButton button, MessageBoxIcon icon)
        {
            MessageBoxCard card = new MessageBoxCard
            {
                Message = message,
                Title = title,
                MessageBoxButton = button,
            };

            card.Dispatcher.VerifyAccess();

            switch (icon)
            {
                case MessageBoxIcon.None:
                default:
                    card.IsShowIcon = false;
                    break;
                case MessageBoxIcon.Info:
                    card.IsShowIcon = true;
                    card.IconType = IconType.InformationFill;
                    card.ThemeColorBrush = infoBrush;
                    break;
                case MessageBoxIcon.Success:
                    card.IsShowIcon = true;
                    card.IconType = IconType.CheckboxCircleFill;
                    card.ThemeColorBrush = successBrush;
                    break;
                case MessageBoxIcon.Warining:
                    card.IsShowIcon = true;
                    card.IconType = IconType.ErrorWarningFill;
                    card.ThemeColorBrush = warningBrush;
                    break;
                case MessageBoxIcon.Error:
                    card.IsShowIcon = true;
                    card.IconType = IconType.CloseCircleFill;
                    card.ThemeColorBrush = errorBrush;
                    break;
                case MessageBoxIcon.Question:
                    card.IsShowIcon = true;
                    card.IconType = IconType.QuestionFill;
                    card.ThemeColorBrush = questionBrush;
                    break;
            }

            return card;
        }

        #region 全局
        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Show(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBoxCard card = GetMessageBoxCard(message, title, button, icon);
            MessageBoxWindow window = new MessageBoxWindow();
            window.AddMessageBoxCard(card);
            IEnumerable<Window> windows = Application.Current.Windows.OfType<Window>();
            foreach (Window itemWindow in windows)
            {
                if (itemWindow.IsActive)
                {
                    window.Owner = itemWindow;
                }
            }

            return window.ShowDialog().GetValueOrDefault() ? window.MessageBoxResult : MessageBoxResult.Cancel;
        }

        /// <summary>
        /// 全局显示确认框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Confirm(string message, string title = "", MessageBoxButton button = MessageBoxButton.OKCancel, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return Show(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示信息框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Info(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return Show(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示警告框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Waring(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return Show(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示成功框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Success(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return Show(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示错误框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult Error(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return Show(message, title, button, icon);
        }
        #endregion

        #region 指定容器

        /// <summary>
        /// 容器内显示信息
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> ShowInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            if (!containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"找不到 Identifier 为{containerIdentifier}消息框容器");
            }

            TaskCompletionSource<MessageBoxResult> taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();
            MessageBoxContainer container = containers[containerIdentifier];
            container.Dispatcher.VerifyAccess();
            MessageBoxCard card = GetMessageBoxCard(message, title, button, icon);
            card.ReturnResult += (a, b) =>
            {
                container.IsShow = false;
                taskCompletionSource.SetResult(b.Result);
            };

            container.DialogContent = card;
            container.IsShow = true;
            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// 容器内显示确认框
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> ConfirmInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OKCancel, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示信息框
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> InfoInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }


        /// <summary>
        /// 容器内显示警告框
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> WaringInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示成功框
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> SuccessInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示错误框
        /// </summary>
        /// <param name="containerIdentifier">容器 ID</param>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> ErrorInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }
        #endregion
    }

    /// <summary>
    /// 按钮图标类型
    /// </summary>
    public enum MessageBoxIcon
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
        Warining,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,

        /// <summary>
        /// 询问
        /// </summary>
        [Description("询问")]
        Question
    }
}
