using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class Message
    {
        private static readonly Style infoStyle = (Style)Application.Current.Resources["InfoMessage"];
        private static readonly Style warningStyle = (Style)Application.Current.Resources["WarningMessage"];
        private static readonly Style successStyle = (Style)Application.Current.Resources["SuccessMessage"];
        private static readonly Style errorStyle = (Style)Application.Current.Resources["ErrorMessage"];

        #region 全局
        public static void Show(MessageType type, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            if (millisecondTimeOut <= 0)
            {
                isClearable = true;
            }

            MessageWindow messageWindow = MessageWindow.GetInstance();
            messageWindow.Dispatcher.VerifyAccess();

            MessageCard messageCard;
            switch (type)
            {
                default:
                case MessageType.None:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable
                    };
                    break;
                case MessageType.Info:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = infoStyle
                    };
                    break;
                case MessageType.Warning:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = warningStyle
                    };
                    break;
                case MessageType.Success:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = successStyle
                    };
                    break;
                case MessageType.Error:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = errorStyle
                    };
                    break;
            }

            messageWindow.AddMessageCard(messageCard, millisecondTimeOut);
            messageWindow.Show();
        }

        public static void Show(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.None, element, millisecondTimeOut, isClearable);
        }

        public static void ShowInfo(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Info, element, millisecondTimeOut, isClearable);
        }

        public static void ShowSuccess(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Success, element, millisecondTimeOut, isClearable);
        }

        public static void ShowWarning(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Warning, element, millisecondTimeOut, isClearable);
        }

        public static void ShowError(UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Error, element, millisecondTimeOut, isClearable);
        }

        public static void Show(MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(type, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void Show(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.None, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowInfo(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Info, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowSuccess(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Success, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowWarning(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Warning, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowError(string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(MessageType.Error, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }
        #endregion


        #region 指定容器
        public static void Show(string containerIdentifier, MessageType type, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            if (!MessageContainer.Containers.ContainsKey(containerIdentifier))
            {
                return;
            }

            if (millisecondTimeOut <= 0)
            {
                isClearable = true;
            }

            Panel messagePanel = MessageContainer.Containers[containerIdentifier];
            messagePanel.Dispatcher.VerifyAccess();

            MessageCard messageCard;
            switch (type)
            {
                default:
                case MessageType.None:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable
                    };
                    break;
                case MessageType.Info:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = infoStyle
                    };
                    break;
                case MessageType.Warning:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = warningStyle
                    };
                    break;
                case MessageType.Success:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = successStyle
                    };
                    break;
                case MessageType.Error:
                    messageCard = new MessageCard
                    {
                        Content = element,
                        IsClearable = isClearable,
                        Style = errorStyle
                    };
                    break;
            }

            messagePanel.Children.Add(messageCard);

            // 进入动画
            Storyboard enterStoryboard = new Storyboard();

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation transformAnimation = new DoubleAnimation
            {
                From = -30,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            enterStoryboard.Children.Add(opacityAnimation);
            enterStoryboard.Children.Add(transformAnimation);

            // 退出动画
            Storyboard exitStoryboard = new Storyboard();

            DoubleAnimation exitOpacityAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(exitOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation exitTransformAnimation = new DoubleAnimation
            {
                From = 0,
                To = -30,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(exitTransformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            exitStoryboard.Children.Add(exitOpacityAnimation);
            exitStoryboard.Children.Add(exitTransformAnimation);

            if (millisecondTimeOut > 0)
            {
                // 进入动画完成
                enterStoryboard.Completed += async (sender, e) =>
                {
                    await Task.Run(() =>
                    {
                        Thread.Sleep(millisecondTimeOut);
                    });

                    messageCard.BeginStoryboard(exitStoryboard);
                };

            }

            // 退出动画完成
            exitStoryboard.Completed += (sender, e) =>
            {
                messagePanel.Children.Remove(messageCard);
            };

            messageCard.BeginStoryboard(enterStoryboard);
        }

        public static void Show(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.None, element, millisecondTimeOut, isClearable);
        }

        public static void ShowInfo(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Info, element, millisecondTimeOut, isClearable);
        }

        public static void ShowSuccess(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Success, element, millisecondTimeOut, isClearable);
        }

        public static void ShowWarning(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Warning, element, millisecondTimeOut, isClearable);
        }

        public static void ShowError(string containerIdentifier, UIElement element, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Error, element, millisecondTimeOut, isClearable);
        }

        public static void Show(string containerIdentifier, MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, type, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void Show(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.None, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowInfo(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Info, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowSuccess(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Success, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowWarning(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Warning, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }

        public static void ShowError(string containerIdentifier, string message, int millisecondTimeOut = 3000, bool isClearable = true)
        {
            Show(containerIdentifier, MessageType.Error, new TextBlock { Text = message }, millisecondTimeOut, isClearable);
        }
        #endregion
    }

    public enum MessageType
    {
        None = 0,
        Info,
        Success,
        Warning,
        Error
    }
}
