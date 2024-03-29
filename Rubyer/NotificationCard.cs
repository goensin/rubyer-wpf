﻿using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rubyer.Commons.KnownBoxes;

namespace Rubyer
{
    /// <summary>
    /// 通知卡片
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    public class NotificationCard : ContentControl
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionPartName = "Path_Transition";

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public const string CloseButtonName = "PART_CloseButton";

        static NotificationCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationCard), new FrameworkPropertyMetadata(typeof(NotificationCard)));
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
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotificationCard));

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
          DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NotificationCard), new PropertyMetadata(default(CornerRadius)));

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
            DependencyProperty.Register("ThemeColorBrush", typeof(SolidColorBrush), typeof(NotificationCard), new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        /// 主题色
        /// </summary>
        public SolidColorBrush ThemeColorBrush
        {
            get { return (SolidColorBrush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(NotificationType), typeof(NotificationCard), new PropertyMetadata(default(NotificationType)));

        /// <summary>
        /// 通知类型
        /// </summary>
        public NotificationType Type
        {
            get { return (NotificationType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 是否可清除
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(NotificationCard), new PropertyMetadata(BooleanBoxes.FalseBox));

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
            DependencyProperty.Register("IsShow", typeof(bool), typeof(NotificationCard), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsShowChanged));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NotificationCard), new PropertyMetadata(default(string)));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty =
            DependencyProperty.Register("TransitionType", typeof(TransitionType), typeof(NotificationCard), new PropertyMetadata(default(TransitionType)));

        /// <summary>
        /// 转换类型
        /// </summary>
        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        #endregion 依赖属性

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notificationCard = d as NotificationCard;
            if (!notificationCard.IsLoaded && !notificationCard.IsShow)
            {
                RoutedEventArgs eventArgs = new RoutedEventArgs(CloseEvent, notificationCard);
                notificationCard.RaiseEvent(eventArgs);
            }
        }
    }
}