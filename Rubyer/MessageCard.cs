using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息卡片
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    public class MessageCard : ContentControl
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionPartName = "Path_Transition";

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public const string CloseButtonName = "PART_CloseButton";

        static MessageCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        /// <inheritdoc/>
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

            if (GetTemplateChild(TransitionPartName) is Transition transition)
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

        #endregion 事件

        #region 依赖属性

        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
          DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessageCard), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// 主题色
        /// </summary>
        public static readonly DependencyProperty ThemeColorBrushProperty =
            DependencyProperty.Register("ThemeColorBrush", typeof(SolidColorBrush), typeof(MessageCard), new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        /// 主题色
        /// </summary>
        public SolidColorBrush ThemeColorBrush
        {
            get { return (SolidColorBrush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType", typeof(IconType), typeof(MessageCard), new PropertyMetadata(default(IconType)));

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType IconType
        {
            get { return (IconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        /// <summary>
        /// 是否显示图标
        /// </summary>
        public static readonly DependencyProperty IsShwoIconProperty =
            DependencyProperty.Register("IsShwoIcon", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示图标
        /// </summary>
        public bool IsShwoIcon
        {
            get { return (bool)GetValue(IsShwoIconProperty); }
            set { SetValue(IsShwoIconProperty, value); }
        }

        /// <summary>
        /// 是否可清除
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否可清除
        /// </summary>
        public bool IsClearable
        {
            get { return (bool)GetValue(IsClearableProperty); }
            set { SetValue(IsClearableProperty, value); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        #endregion 依赖属性
    }
}