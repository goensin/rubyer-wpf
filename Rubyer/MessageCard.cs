using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息卡片
    /// </summary>
    [TemplatePart(Name = TransitionName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    public class MessageCard : ContentControl
    {
        public const string TransitionName = "Path_Transition";
        public const string CloseButtonName = "PART_CloseButton";

        static MessageCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(CloseButtonName) is ButtonBase closeButton)
            {
                closeButton.Click += (sender, e) =>
                {
                    IsShow = false;
                };
            }

            if (GetTemplateChild(TransitionName) is Transition transition)
            {
                transition.Closed += (sender, e) =>
                {
                    RoutedEventArgs eventArgs = new RoutedEventArgs(CloseEvent, this);
                    this.RaiseEvent(eventArgs);
                };
            }
        }

        #region 事件

        /// <summary>
        /// 关闭消息事件
        /// </summary>
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageCard));

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
            DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        #endregion
    }
}
