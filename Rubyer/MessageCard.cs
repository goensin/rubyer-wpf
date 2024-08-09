using Rubyer.Commons.KnownBoxes;
using System;
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
                    if (!IsShow)
                    {
                        IsShow = true;
                    }

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
            DependencyProperty.Register("ThemeColorBrush", typeof(Brush), typeof(MessageCard), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 主题色
        /// </summary>
        public Brush ThemeColorBrush
        {
            get { return (Brush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(MessageType), typeof(MessageCard), new PropertyMetadata(default(MessageType)));

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type
        {
            get { return (MessageType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 是否可清除
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(MessageCard), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否可清除
        /// </summary>
        public bool IsClearable
        {
            get { return (bool)GetValue(IsClearableProperty); }
            set { SetValue(IsClearableProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageCard), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsShowChanged));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty =
            DependencyProperty.Register("TransitionType", typeof(TransitionType), typeof(MessageCard), new PropertyMetadata(default(TransitionType)));

        /// <summary>
        /// 转换类型
        /// </summary>
        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public static readonly DependencyProperty TransitionInitialScaleProperty =
            DependencyProperty.Register("TransitionInitialScale", typeof(double), typeof(MessageCard), new PropertyMetadata(default(double)));

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public double TransitionInitialScale
        {
            get { return (double)GetValue(TransitionInitialScaleProperty); }
            set { SetValue(TransitionInitialScaleProperty, value); }
        }

        #endregion 依赖属性

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var messageCard = d as MessageCard;
            if (!messageCard.IsLoaded && !messageCard.IsShow)
            {
                RoutedEventArgs eventArgs = new RoutedEventArgs(CloseEvent, messageCard);
                messageCard.RaiseEvent(eventArgs);
            }
        }
    }
}