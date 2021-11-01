using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息卡片
    /// </summary>
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    [TemplateVisualState(GroupName = "ShowStates", Name = OpenStateName)]
    [TemplateVisualState(GroupName = "ShowStates", Name = CloseStateName)]
    public class MessageCard : ContentControl
    {
        public const string CloseButtonName = "PART_CloseButton";
        public const string OpenStateName = "Open";
        public const string CloseStateName = "Close";

        public static readonly RoutedEvent CloseEvent;

        static MessageCard()
        {
            CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageCard));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild(CloseButtonName) is ButtonBase closeButton)
            {
                closeButton.Click += (sender, e) =>
                {
                    GoToCloseState(this);
                };
            }

            GoToOpenState(this);
            base.OnApplyTemplate();
        }

        #region 事件

        /// <summary>
        /// 关闭消息事件
        /// </summary>
        public event RoutedEventHandler Close
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
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
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        public bool IsClearable
        {
            get { return (bool)GetValue(IsClearableProperty); }
            set { SetValue(IsClearableProperty, value); }
        }

        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool), OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var messageCard = d as MessageCard;
            if (messageCard.IsShow)
            {
                GoToOpenState(messageCard);
            }
            else
            {
                GoToCloseState(messageCard);
            }
        }

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register("IsClosed", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool), OnIsClosedChanged));

        private static void OnIsClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageCard messageCard)
            {
                if (messageCard.IsClosed)
                {
                    RoutedEventArgs eventArgs = new RoutedEventArgs(CloseEvent, messageCard);
                    messageCard.RaiseEvent(eventArgs);
                }
            }
        }

        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        #endregion

        private static void GoToOpenState(MessageCard messageCard)
        {
            _ = VisualStateManager.GoToState(messageCard, OpenStateName, true);
        }

        private static void GoToCloseState(MessageCard messageCard)
        {
            _ = VisualStateManager.GoToState(messageCard, CloseStateName, true);
        }
    }
}
