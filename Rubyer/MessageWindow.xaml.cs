using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        private static MessageWindow messageWindow = null;

        private MessageWindow()
        {
            InitializeComponent();
        }

        public static MessageWindow GetInstance()
        {
            if (messageWindow == null)
            {
                messageWindow = new MessageWindow();
            }
            else if (!messageWindow.IsLoaded)
            {
                messageWindow = new MessageWindow();
            }
            return messageWindow;
        }

        public void AddMessageCard(MessageCard messageCard, int millisecondTimeOut)
        {
            messageStackPanel.Children.Add(messageCard);

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

            // 进入动画完成
            enterStoryboard.Completed += (sender,e)=>{
                Task.Run(() =>
                {
                    Thread.Sleep(millisecondTimeOut);
                    Dispatcher.Invoke(() =>
                    {
                        messageCard.BeginStoryboard(exitStoryboard);
                    });
                });
            };

            // 退出动画完成
            exitStoryboard.Completed += (sender, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    messageStackPanel.Children.Remove(messageCard);
                    if (messageStackPanel.Children.Count == 0)
                    {
                        this.Close();
                    }
                });
            };

            messageCard.BeginStoryboard(enterStoryboard);
        }

    }
}
