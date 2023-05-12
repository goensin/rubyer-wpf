﻿using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Rubyer
{
    /// <summary>
    /// 滑动视图
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FlipViewItem))]
    [TemplatePart(Name = LeftButtonName, Type = typeof(Button))]
    [TemplatePart(Name = RightButtonName, Type = typeof(Button))]
    [TemplatePart(Name = UpButtonName, Type = typeof(Button))]
    [TemplatePart(Name = DownButtonName, Type = typeof(Button))]
    [TemplatePart(Name = ContentScrollViewerName, Type = typeof(ScrollViewer))]
    public class FlipView : ItemsControl
    {
        const string LeftButtonName = "PART_LeftButton";
        const string RightButtonName = "PART_RightButton";
        const string UpButtonName = "PART_UpButton";
        const string DownButtonName = "PART_DownButton";
        const string ContentScrollViewerName = "PART_ContentScrollViewer";

        private ScrollViewer scrollViewer;
        private DispatcherTimer timer;
        private bool animating;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is FlipViewItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new FlipViewItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(LeftButtonName) is ButtonBase leftButton)
            {
                leftButton.Click += ClickLastItem;
            }

            if (GetTemplateChild(RightButtonName) is ButtonBase rightButton)
            {
                rightButton.Click += ClickNextItem;
            }

            if (GetTemplateChild(UpButtonName) is ButtonBase upButton)
            {
                upButton.Click += ClickLastItem;
            }

            if (GetTemplateChild(DownButtonName) is ButtonBase downButton)
            {
                downButton.Click += ClickNextItem;
            }

            scrollViewer = GetTemplateChild(ContentScrollViewerName) as ScrollViewer;
            scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

            timer = new DispatcherTimer();
            timer.Interval = AutoPlayInterval;
            timer.Tick += Timer_Tick;
            if (IsAutoPlay)
            {
                timer.Start();
            }
        }

        #region properties

        /// <summary>
        /// 箭头按钮样式
        /// </summary>
        public static readonly DependencyProperty ArrowButtonStyleProperty =
            DependencyProperty.Register("ArrowButtonStyle", typeof(Style), typeof(FlipView), new PropertyMetadata(default(Style)));

        /// <summary>
        ///  箭头按钮样式
        /// </summary>
        public Style ArrowButtonStyle
        {
            get { return (Style)GetValue(ArrowButtonStyleProperty); }
            set { SetValue(ArrowButtonStyleProperty, value); }
        }

        /// <summary>
        /// 当前索引
        /// </summary>
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(FlipView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentIndexChanged));

        /// <summary>
        ///  当前索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        /// <summary>
        /// 方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(FlipView), new PropertyMetadata(default(Orientation)));

        /// <summary>
        ///  方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 垂直偏移
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(FlipView), new PropertyMetadata(default(double)));

        /// <summary>
        ///  垂直偏移
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// 水平偏移
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(FlipView), new PropertyMetadata(default(double)));

        /// <summary>
        ///  水平偏移
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// 动画时长
        /// </summary>
        public static readonly DependencyProperty AnimateDurationProperty =
            DependencyProperty.Register("AnimateDuration", typeof(Duration), typeof(FlipView), new PropertyMetadata(default(Duration)));

        /// <summary>
        /// 动画时长
        /// </summary>
        public Duration AnimateDuration
        {
            get { return (Duration)GetValue(AnimateDurationProperty); }
            set { SetValue(AnimateDurationProperty, value); }
        }

        /// <summary>
        /// 动画缓动函数
        /// </summary>
        public static readonly DependencyProperty AnimateEasingFunctionProperty =
            DependencyProperty.Register("AnimateEasingFunction", typeof(IEasingFunction), typeof(FlipView), new PropertyMetadata(default(IEasingFunction)));

        /// <summary>
        /// 动画缓动函数
        /// </summary>
        public IEasingFunction AnimateEasingFunction
        {
            get { return (IEasingFunction)GetValue(AnimateEasingFunctionProperty); }
            set { SetValue(AnimateEasingFunctionProperty, value); }
        }

        /// <summary>
        /// 是否按钮浮动
        /// </summary>
        public static readonly DependencyProperty IsButtonFloatProperty =
            DependencyProperty.Register("IsButtonFloat", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsButtonFloatChanged));

        /// <summary>
        /// 是否按钮浮动
        /// </summary>
        public bool IsButtonFloat
        {
            get { return (bool)GetValue(IsButtonFloatProperty); }
            set { SetValue(IsButtonFloatProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public static readonly DependencyProperty IsLoopProperty =
            DependencyProperty.Register("IsLoop", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool IsLoop
        {
            get { return (bool)GetValue(IsLoopProperty); }
            set { SetValue(IsLoopProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 自动播放
        /// </summary>
        public static readonly DependencyProperty IsAutoPlayProperty =
            DependencyProperty.Register("IsAutoPlay", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsAutoPlayChanged));

        /// <summary>
        /// 自动播放
        /// </summary>
        public bool IsAutoPlay
        {
            get { return (bool)GetValue(IsAutoPlayProperty); }
            set { SetValue(IsAutoPlayProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否滚动鼠标
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelProperty =
            DependencyProperty.Register("IsMouseWheel", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// 是否滚动鼠标
        /// </summary>
        public bool IsMouseWheel
        {
            get { return (bool)GetValue(IsMouseWheelProperty); }
            set { SetValue(IsMouseWheelProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 播放间隔
        /// </summary>
        public static readonly DependencyProperty AutoPlayIntervalProperty =
            DependencyProperty.Register("AutoPlayInterval", typeof(TimeSpan), typeof(FlipView), new PropertyMetadata(TimeSpan.FromSeconds(3), OnAutoPlayIntervalChanged));

        /// <summary>
        /// 播放间隔
        /// </summary>
        public TimeSpan AutoPlayInterval
        {
            get { return (TimeSpan)GetValue(AutoPlayIntervalProperty); }
            set { SetValue(AutoPlayIntervalProperty, value); }
        }

        #endregion

        /// <summary>
        /// 下一张
        /// </summary>
        private void ClickNextItem(object sender, RoutedEventArgs e)
        {
            if (animating)
            {
                return;
            }

            CurrentIndex++;
        }

        /// <summary>
        /// 上一张
        /// </summary>
        private void ClickLastItem(object sender, RoutedEventArgs e)
        {
            if (animating)
            {
                return;
            }

            CurrentIndex--;
        }

        /// <summary>
        /// 水平动画
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        /// <param name="offset">偏移</param>
        private static void HorizontalAnimation(FlipView flipView, double offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            else if (offset > flipView.scrollViewer.ScrollableWidth)
            {
                offset = flipView.scrollViewer.ScrollableWidth;
            }

            var animation = new DoubleAnimation
            {
                From = flipView.HorizontalOffset,
                To = offset,
                Duration = flipView.AnimateDuration,
                EasingFunction = flipView.AnimateEasingFunction,
            };

            animation.Completed += (sender, e) =>
            {
                flipView.animating = false;
            };

            flipView.animating = true;

            flipView.BeginAnimation(HorizontalOffsetProperty, animation);
        }

        /// <summary>
        /// 垂直动画
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        /// <param name="offset">偏移</param>
        private static void VerticalAnimation(FlipView flipView, double offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            else if (offset > flipView.scrollViewer.ScrollableHeight)
            {
                offset = flipView.scrollViewer.ScrollableHeight;
            }

            var animation = new DoubleAnimation
            {
                From = flipView.VerticalOffset,
                To = offset,
                Duration = flipView.AnimateDuration,
                EasingFunction = flipView.AnimateEasingFunction,
            };

            animation.Completed += (sender, e) =>
            {
                flipView.animating = false;
            };

            flipView.animating = true;
            flipView.BeginAnimation(VerticalOffsetProperty, animation);
        }

        /// <summary>
        /// 更新滚动条偏移
        /// </summary>
        /// <param name="flipView">滚动视图</param>
        private static void UpdateScrollOffset(FlipView flipView)
        {
            if (flipView.Orientation == Orientation.Horizontal)
            {
                var offset = flipView.CurrentIndex * flipView.scrollViewer.ActualWidth;
                HorizontalAnimation(flipView, offset);
            }
            else
            {
                var offset = flipView.CurrentIndex * flipView.scrollViewer.ActualHeight;
                VerticalAnimation(flipView, offset);
            }
        }

        /// <summary>
        /// 当前索引改变
        /// </summary>
        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = d as FlipView;

            if (flipView.CurrentIndex >= flipView.Items.Count)
            {
                if (flipView.IsLoop)
                {
                    flipView.CurrentIndex = 0;
                    flipView.HorizontalOffset = 0;
                    flipView.VerticalOffset = 0;
                }
                else
                {
                    flipView.CurrentIndex = flipView.Items.Count - 1;
                }
            }

            if (flipView.CurrentIndex < 0)
            {
                if (flipView.IsLoop)
                {
                    flipView.CurrentIndex = flipView.Items.Count - 1;
                }
                else
                {
                    flipView.CurrentIndex = 0;
                }
            }

            UpdateScrollOffset(flipView);
        }

        /// <summary>
        /// 浮动按钮变化
        /// </summary>
        private static void OnIsButtonFloatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = d as FlipView;
            flipView.CurrentIndex = 0;
        }

        /// <summary>
        /// 定时触发翻页
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            ClickNextItem(this, null);
        }

        /// <summary>
        /// 是否自动播放改变
        /// </summary>
        private static void OnIsAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = d as FlipView;

            if (flipView.IsAutoPlay)
            {
                flipView.timer?.Start();
            }
            else
            {
                flipView.timer.Stop();
            }
        }

        /// <summary>
        /// 自动播放间隔改变
        /// </summary>
        private static void OnAutoPlayIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = d as FlipView;
            flipView.timer.Interval = flipView.AutoPlayInterval;
        }

        /// <summary>
        /// 滚动鼠标翻页
        /// </summary>
        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!IsMouseWheel)
            {
                return;
            }

            e.Handled = true;

            if (!animating)
            {
                CurrentIndex += e.Delta > 0 ? -1 : 1;
            }
        }
    }
}
