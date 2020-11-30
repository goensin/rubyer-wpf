using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Rubyer
{
    public class Message
    {
        private static readonly Style warningStyle = (Style)Application.Current.Resources["WarningMessage"];
        private static readonly Style successStyle = (Style)Application.Current.Resources["SuccessMessage"];
        private static readonly Style errorStyle = (Style)Application.Current.Resources["ErrorMessage"];

        #region 全局
        public static void Show(MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            if (millisecondTimeOut <= 0)
            {
                isClearable = true;
            }

            MessageWindow messageWindow = MessageWindow.GetInstance();
            MessageCard messageCard;
            switch (type)
            {
                default:
                case MessageType.Info:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable
                    };
                    break;
                case MessageType.Warning:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = warningStyle
                    };
                    break;
                case MessageType.Success:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = successStyle
                    };
                    break;
                case MessageType.Error:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = errorStyle
                    };
                    break;
            }

            messageWindow.AddMessageCard(messageCard, millisecondTimeOut);
            messageWindow.Show();
        }

        public static void Show(string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            Show(MessageType.Info, message, millisecondTimeOut, isClearable);
        }
        #endregion


        #region 指定容器
        public static void Show(string containerIdentify, MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            if (millisecondTimeOut <= 0)
            {
                isClearable = true;
            }

            Panel messagePanel = MessageCard.messageContainers[containerIdentify];
            if (messagePanel == null)
            {
                return;
            }

            MessageCard messageCard;
            switch (type)
            {
                default:
                case MessageType.Info:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable
                    };
                    break;
                case MessageType.Warning:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = warningStyle
                    };
                    break;
                case MessageType.Success:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = successStyle
                    };
                    break;
                case MessageType.Error:
                    messageCard = new MessageCard
                    {
                        Content = message,
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
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));

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
            Storyboard.SetTargetProperty(exitOpacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));

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


        public static void Show(string containerIdentify, string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            Show(containerIdentify, MessageType.Info, message, millisecondTimeOut, isClearable);
        }
        #endregion
    }

    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
