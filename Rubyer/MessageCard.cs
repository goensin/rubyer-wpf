using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class MessageCard : ContentControl
    {
        public static Dictionary<string, Panel> messageContainers = new Dictionary<string, Panel>();
        public static readonly RoutedEvent CloseEvent;

        static MessageCard()
        {
            CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageCard));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        #region 事件
        // 关闭消息事件
        public event RoutedEventHandler Close
        {
            add { base.AddHandler(MessageCard.CloseEvent, value); }
            remove { base.RemoveHandler(MessageCard.CloseEvent, value); }
        }
        #endregion

        #region 附加属性
        // 显示消息通知的容器的 ID
        public static readonly DependencyProperty ContainerIdentifyProperty = DependencyProperty.RegisterAttached(
            "ContainerIdentify", typeof(string), typeof(MessageCard), new PropertyMetadata(null, OnMessageContainerChanged));

        private static void OnMessageContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Panel panel = d as Panel;
            string identify = e.NewValue.ToString();

            if (messageContainers.ContainsKey(identify))
            {
                messageContainers.Remove(identify);
            }

            messageContainers.Add(identify, panel);
        }

        public static void SetContainerIdentify(DependencyObject element, string value)
        {
            element.SetValue(ContainerIdentifyProperty, value);
        }

        public static string GetContainerIdentify(DependencyObject element)
        {
            return (string)element.GetValue(ContainerIdentifyProperty);
        }
        #endregion

        #region 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
          DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessageCard), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        public static readonly DependencyProperty ThemeColorBrushProperty =
            DependencyProperty.Register("ThemeColorBrush", typeof(SolidColorBrush), typeof(MessageCard), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush ThemeColorBrush
        {
            get { return (SolidColorBrush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }


        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType", typeof(IconType), typeof(MessageCard), new PropertyMetadata(default(IconType)));

        public IconType IconType
        {
            get { return (IconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }


        public static readonly DependencyProperty IsShwoIconProperty =
            DependencyProperty.Register("IsShwoIcon", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        public bool IsShwoIcon
        {
            get { return (bool)GetValue(IsShwoIconProperty); }
            set { SetValue(IsShwoIconProperty, value); }
        }


        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool), OnIsClearbleChanged));

        public bool IsClearable
        {
            get { return (bool)GetValue(IsClearableProperty); }
            set { SetValue(IsClearableProperty, value); }
        }

        private static void OnIsClearbleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageCard messageCard)
            {
                RoutedEventHandler handle = (sender, args) =>
                {
                    if (VisualTreeHelper.GetParent(messageCard) is Panel panel)
                    {
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

                        // 动画完成
                        exitStoryboard.Completed += (a, b) =>
                        {
                            panel.Children.Remove(messageCard);
                            RoutedEventArgs eventArgs = new RoutedEventArgs(MessageCard.CloseEvent, messageCard);
                            messageCard.RaiseEvent(eventArgs);
                        };

                        messageCard.BeginStoryboard(exitStoryboard);    // 执行动画
                    }
                };

                messageCard.Loaded += (sender, arg) =>
                {
                    if (messageCard.Template.FindName("clearButton", messageCard) is Button clearButton)
                    {
                        if (messageCard.IsClearable)
                        {
                            clearButton.Click += handle;
                        }
                        else
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };

                messageCard.Unloaded += (sender, arg) =>
                {
                    if (messageCard.Template.FindName("clearButton", messageCard) is Button clearButton)
                    {
                        if (messageCard.IsClearable)
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };
            }
        }
        #endregion


    }
}
