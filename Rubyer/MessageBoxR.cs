using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class MessageBoxR
    {
        private static readonly Brush infoBrush = (Brush)Application.Current.Resources["InfoBrush"];
        private static readonly Brush warningBrush = (Brush)Application.Current.Resources["WarningBrush"];
        private static readonly Brush successBrush = (Brush)Application.Current.Resources["SuccessBrush"];
        private static readonly Brush errorBrush = (Brush)Application.Current.Resources["ErrorBrush"];
        private static readonly Brush questionBrush = (Brush)Application.Current.Resources["QuestionBrush"];

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
            MessageBoxCard card = new MessageBoxCard
            {
                Message = message,
                Title = title,
                MessageBoxButton = button,
                ShowShadow = false
            };

            card.Dispatcher.VerifyAccess();

            switch (icon)
            {
                case MessageBoxIcon.None:
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

            MessageBoxWindow window = new MessageBoxWindow();
            window.AddMessageBoxCard(card);

            WindowCollection windows = Application.Current.Windows;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].IsActive)
                {
                    window.Owner = windows[i];
                }
            }

            if (window.ShowDialog() == true)
            {
                return window.MessageBoxResult;
            }

            return MessageBoxResult.Cancel;
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
            bool isReturnResult = false;
            MessageBoxResult result = MessageBoxResult.No;

            if (!containers.ContainsKey(containerIdentifier))
            {
                return result;
            }

            MessageBoxContainer container = containers[containerIdentifier];
            container.Dispatcher.VerifyAccess();
            DependencyObject child = VisualTreeHelper.GetChild(container, 0);
            Border containerRootBorder = (Border)VisualTreeHelper.GetChild(child, 1);

            MessageBoxCard card = new MessageBoxCard
            {
                Message = message,
                Title = title,
                MessageBoxButton = button,
            };

            card.SetOwnerContainer(container);
            card.ReturnResult += (a, b) =>
            {
                isReturnResult = true;
                result = b.Result;
            };

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

            container.Visibility = Visibility.Visible;

            // 动画
            Storyboard cardStoryboard = new Storyboard();

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation transformAnimation = new DoubleAnimation
            {
                From = 50,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            cardStoryboard.Children.Add(opacityAnimation);
            cardStoryboard.Children.Add(transformAnimation);

            // 背景动画
            DoubleAnimation backgroundAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            backgroundAnimation.Completed += (sender, e) =>
            {
                container.DialogContent = card;
                card.BeginStoryboard(cardStoryboard);
                _ = card.Focus();
            };

            containerRootBorder.Visibility = Visibility.Visible;
            containerRootBorder.BeginAnimation(UIElement.OpacityProperty, backgroundAnimation);

            await Task.Run(() => { while (!isReturnResult) { Thread.Sleep(10); } });

            return result;
        }

        /// <summary>
        /// 关闭信息框
        /// </summary>
        /// <param name="ownerContaioner"></param>
        /// <param name="messageBoxCard"></param>
        internal static void CloseInContainer(MessageBoxContainer ownerContaioner, MessageBoxCard messageBoxCard)
        {
            if (ownerContaioner != null)
            {
                DependencyObject child = VisualTreeHelper.GetChild(ownerContaioner, 0);
                Border containerRootBorder = (Border)VisualTreeHelper.GetChild(child, 1);

                // 退出动画
                Storyboard exitStoryboard = new Storyboard();
                DoubleAnimation exitOpacityAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                };

                Storyboard.SetTargetProperty(exitOpacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));

                DoubleAnimation exitTransformAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 50,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                };

                Storyboard.SetTargetProperty(exitTransformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                exitStoryboard.Children.Add(exitOpacityAnimation);
                exitStoryboard.Children.Add(exitTransformAnimation);

                // 背景动画
                DoubleAnimation backgroundAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                };

                // 动画完成
                exitStoryboard.Completed += (a, b) =>
                {
                    containerRootBorder.BeginAnimation(UIElement.OpacityProperty, backgroundAnimation);
                    ownerContaioner.DialogContent = null;
                };

                backgroundAnimation.Completed += (a, b) =>
                {
                    containerRootBorder.Visibility = Visibility.Hidden;
                };

                messageBoxCard.BeginStoryboard(exitStoryboard);    // 执行动画
            }
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
