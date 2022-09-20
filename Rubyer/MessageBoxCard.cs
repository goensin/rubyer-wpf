using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息框卡片
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = OkButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = CancelButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = YesButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = NoButtonPartName, Type = typeof(Button))]
    public class MessageBoxCard : Control
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionPartName = "Path_Transition";

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public const string CloseButtonPartName = "PART_CloseButton";

        /// <summary>
        /// 确认按钮名称
        /// </summary>
        public const string OkButtonPartName = "PART_OkButton";

        /// <summary>
        /// 取消按钮名称
        /// </summary>
        public const string CancelButtonPartName = "PART_CancelButton";

        /// <summary>
        /// 是按钮名称
        /// </summary>
        public const string YesButtonPartName = "PART_YesButton";

        /// <summary>
        /// 否按钮名称
        /// </summary>
        public const string NoButtonPartName = "PART_NoButton";

        /// <summary>
        /// 消息框结果事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MessageBoxResultRoutedEventHandler(object sender, MessageBoxResultRoutedEventArgs e);

        static MessageBoxCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxCard), new FrameworkPropertyMetadata(typeof(MessageBoxCard)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                InternalReturnResult(this, MessageBoxResult.Cancel);
            };

            if (GetTemplateChild(OkButtonPartName) is ButtonBase okButton)
            {
                okButton.Click += (sender, args) => InternalReturnResult(this, MessageBoxResult.OK);
            }

            if (GetTemplateChild(CancelButtonPartName) is ButtonBase cancelButton)
            {
                cancelButton.Click += (sender, args) => InternalReturnResult(this, MessageBoxResult.Cancel);
            }

            if (GetTemplateChild(YesButtonPartName) is ButtonBase yesButton)
            {
                yesButton.Click += (sender, args) => InternalReturnResult(this, MessageBoxResult.Yes);
            }

            if (GetTemplateChild(NoButtonPartName) is ButtonBase noButton)
            {
                noButton.Click += (sender, args) => InternalReturnResult(this, MessageBoxResult.No);
            }

            if (GetTemplateChild(TransitionPartName) is Transition transition)
            {
                transition.Closed += (sender, e) =>
                {
                    RoutedEventArgs eventArgs = new RoutedEventArgs(ClosedEvent, this);
                    this.RaiseEvent(eventArgs);
                };
            }
        }

        #region 事件

        /// <summary>
        /// 返回结果事件
        /// </summary>
        public static readonly RoutedEvent ReturnResultEvent = EventManager.RegisterRoutedEvent("ReturnResult", RoutingStrategy.Bubble, typeof(MessageBoxResultRoutedEventHandler), typeof(MessageBoxCard));

        /// <summary>
        ///  /// <summary>
        /// 返回结果事件处理
        /// </summary>
        /// </summary>
        public event MessageBoxResultRoutedEventHandler ReturnResult
        {
            add { AddHandler(ReturnResultEvent, value); }
            remove { RemoveHandler(ReturnResultEvent, value); }
        }

        /// <summary>
        /// 关闭消息事件
        /// </summary>
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBoxCard));

        /// <summary>
        /// 关闭消息事件处理
        /// </summary>
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        #endregion 事件

        #region 依赖属性

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
           DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(MessageBoxCard), new PropertyMetadata(default(string)));

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(MessageBoxCard), new PropertyMetadata(default(string)));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(MessageBoxCard), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// 主题颜色
        /// </summary>
        public static readonly DependencyProperty ThemeColorBrushProperty = DependencyProperty.Register(
            "ThemeColorBrush", typeof(Brush), typeof(MessageBoxCard), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 主题颜色
        /// </summary>
        public Brush ThemeColorBrush
        {
            get { return (Brush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(MessageBoxType), typeof(MessageBoxCard), new PropertyMetadata(default(MessageBoxType)));

        /// <summary>
        /// 图标类型
        /// </summary>
        public MessageBoxType Type
        {
            get { return (MessageBoxType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 是否显示阴影
        /// </summary>
        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.Register(
            "ShowShadow", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示阴影
        /// </summary>
        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        /// <summary>
        /// 消息框按钮类型
        /// </summary>
        public static readonly DependencyProperty MessageBoxButtonProperty = DependencyProperty.Register(
            "MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxCard), new PropertyMetadata(default(MessageBoxButton)));

        /// <summary>
        /// 消息框按钮类型
        /// </summary>
        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        #endregion 依赖属性

        private void InternalReturnResult(MessageBoxCard card, MessageBoxResult result)
        {
            IsShow = false;
            MessageBoxResultRoutedEventArgs eventArgs = new MessageBoxResultRoutedEventArgs(ReturnResultEvent, result, card);
            card.RaiseEvent(eventArgs);
        }
    }

    /// <summary>
    /// 消息框框结果路由参数
    /// </summary>
    public class MessageBoxResultRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 结果
        /// </summary>
        public MessageBoxResult Result { get; set; }

        /// <summary>
        /// 消息框
        /// </summary>
        public MessageBoxCard Card { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxResultRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="result">The result.</param>
        /// <param name="card">消息框卡片</param>
        public MessageBoxResultRoutedEventArgs(RoutedEvent routedEvent, MessageBoxResult result, MessageBoxCard card) : base(routedEvent)
        {
            Result = result;
            Card = card;
        }
    }
}