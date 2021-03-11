using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static Brush infoBrush;
        private static Brush warningBrush;
        private static Brush successBrush;
        private static Brush errorBrush;
        private static Brush questionBrush;
        private static Color containerBackgroun;

        public MessageBoxR()
        {
            infoBrush = (Brush)Application.Current.Resources["InfoBrush"];
            warningBrush = (Brush)Application.Current.Resources["WarningBrush"];
            successBrush = (Brush)Application.Current.Resources["SuccessBrush"];
            errorBrush = (Brush)Application.Current.Resources["ErrorBrush"];
            questionBrush = (Brush)Application.Current.Resources["QuestionBrush"];
            containerBackgroun = (Color)Application.Current.Resources["DialogBackground"];
        }

        #region 全局
        public static MessageBoxResult Show(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBoxCard card = new MessageBoxCard
            {
                Content = message,
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

            var windows = Application.Current.Windows;
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

        public static MessageBoxResult Confirm(string message, string title = "", MessageBoxButton button = MessageBoxButton.OKCancel, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return Show(message, title, button, icon);
        }

        public static MessageBoxResult Info(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return Show(message, title, button, icon);
        }

        public static MessageBoxResult Waring(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return Show(message, title, button, icon);
        }

        public static MessageBoxResult Success(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return Show(message, title, button, icon);
        }

        public static MessageBoxResult Error(string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return Show(message, title, button, icon);
        }
        #endregion

        #region 指定容器
        public static async Task<MessageBoxResult> ShowInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBoxResult result = MessageBoxResult.No;
            bool isReturnResult = false;

            if (!MessageBoxContainer.Containers.ContainsKey(containerIdentifier))
            {
                return result;
            }

            MessageBoxContainer container = MessageBoxContainer.Containers[containerIdentifier];

            container.Dispatcher.VerifyAccess();

            MessageBoxCard card = new MessageBoxCard
            {
                Content = message,
                Title = title,
                MessageBoxButton = button,
            };

            card.ReturnResult += (a, b) =>
            {
                isReturnResult = true;
                result = b.Result;
            };

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
            ColorAnimation backgroundAnimation = new ColorAnimation
            {
                From = Colors.Transparent,
                To = containerBackgroun,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            backgroundAnimation.Completed += (sender, e) =>
            {
                container.Child = card;
                card.BeginStoryboard(cardStoryboard);
                card.Focus();
            };

            container.Background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnimation);

            await Task.Run(() => { while (!isReturnResult) { Thread.Sleep(10); } });

            return result;
        }

        public static async Task<MessageBoxResult> ConfirmInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OKCancel, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        public static async Task<MessageBoxResult> InfoInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        public static async Task<MessageBoxResult> WaringInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Warining)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        public static async Task<MessageBoxResult> SuccessInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Success)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }

        public static async Task<MessageBoxResult> ErrorInContainer(string containerIdentifier, string message, string title = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return await ShowInContainer(containerIdentifier, message, title, button, icon);
        }
        #endregion
    }

    public enum MessageBoxIcon
    {
        None = 0,
        Info,
        Success,
        Warining,
        Error,
        Question
    }
}
