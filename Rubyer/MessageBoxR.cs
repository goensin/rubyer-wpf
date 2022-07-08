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
    /// <summary>
    /// 消息框操作类
    /// </summary>
    public class MessageBoxR
    {
        /// <summary>
        /// 默认消息框容器标识
        /// </summary>
        public const string DefaultMessageBoxContainerIdentifier = "Rubyer.MessageBox";

        private static readonly Brush infoBrush = (Brush)Application.Current.Resources["Info"];
        private static readonly Brush warningBrush = (Brush)Application.Current.Resources["Warning"];
        private static readonly Brush successBrush = (Brush)Application.Current.Resources["Success"];
        private static readonly Brush errorBrush = (Brush)Application.Current.Resources["Error"];
        private static readonly Brush questionBrush = (Brush)Application.Current.Resources["Question"];

        /// <summary>
        /// 所有容器集合
        /// </summary>
        internal static Dictionary<string, MessageBoxContainer> Containers = new Dictionary<string, MessageBoxContainer>();

        /// <summary>
        /// 更新信息框容器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="identify"></param>
        internal static void UpdateContainer(MessageBoxContainer container, string identify)
        {
            if (Containers.ContainsKey(identify))
            {
                _ = Containers.Remove(identify);
            }

            Containers.Add(identify, container);
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
        public static MessageBoxResult ShowGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
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
        public static MessageBoxResult ConfirmGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return ShowGlobal(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示信息框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult InfoGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return ShowGlobal(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示警告框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult WaringGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return ShowGlobal(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示成功框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult SuccessGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return ShowGlobal(message, title, button, icon);
        }

        /// <summary>
        /// 全局显示错误框
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static MessageBoxResult ErrorGlobal(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return ShowGlobal(message, title, button, icon);
        }

        #endregion 全局

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
        public static async Task<MessageBoxResult> Show(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            if (!Containers.ContainsKey(containerIdentifier))
            {
                throw new NullReferenceException($"The container Identifier '{containerIdentifier}' could not be found");
            }

            TaskCompletionSource<MessageBoxResult> taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();
            MessageBoxContainer container = Containers[containerIdentifier];
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
        /// 容器内显示信息
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Show(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return await Show(DefaultMessageBoxContainerIdentifier, message, title, button, icon);
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
        public static async Task<MessageBoxResult> Confirm(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return await Show(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示确认框
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Confirm(string message, string title = "", MessageBoxButton button = MessageBoxButton.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return await Show(message, title, button, icon);
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
        public static async Task<MessageBoxResult> Info(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return await Show(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示信息框
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Info(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return await Show(message, title, button, icon);
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
        public static async Task<MessageBoxResult> Waring(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return await Show(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示警告框
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Waring(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return await Show(message, title, button, icon);
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
        public static async Task<MessageBoxResult> Success(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return await Show(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示成功框
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Success(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return await Show(message, title, button, icon);
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
        public static async Task<MessageBoxResult> Error(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return await Show(containerIdentifier, message, title, button, icon);
        }

        /// <summary>
        /// 容器内显示错误框
        /// (默认 RubyerWindow 下 MessageBoxContainer 容器)
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <returns>结果</returns>
        public static async Task<MessageBoxResult> Error(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return await Show(message, title, button, icon);
        }

        #endregion 指定容器
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