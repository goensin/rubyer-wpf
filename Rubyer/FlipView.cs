using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
    public class FlipView : ListBox
    {
        const string LeftButtonName = "PART_LeftButton";
        const string RightButtonName = "PART_RightButton";
        const string UpButtonName = "PART_UpButton";
        const string DownButtonName = "PART_DownButton";
        const string ContentScrollViewerName = "PART_ContentScrollViewer";

        private ScrollViewer scrollViewer;
        private DispatcherTimer timer;
        private bool horizontalAnimating;
        private bool verticalAnimating;
        private bool sorting;

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

        /// <inheritdoc/>
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
            scrollViewer.SizeChanged += ScrollViewer_SizeChanged;

            timer = new DispatcherTimer();
            timer.Interval = AutoPlayInterval;
            timer.Tick += Timer_Tick;
            if (IsAutoPlay)
            {
                timer.Start();
            }

            PreviewMouseWheel += FlipView_PreviewMouseWheel;
            //Loaded += FlipView_Loaded;

            UpdateItemSort(this);
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
            DependencyProperty.Register("IsButtonFloat", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsButtonFloatChanged));

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
            DependencyProperty.Register("IsLoop", typeof(bool), typeof(FlipView), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsLoopChanged));

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

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (IsLoaded && !sorting)
            {
                ScrollSelectedItemToCenter(this);
            }
        }

        private void RestartTimer()
        {
            timer.Stop();
            timer.Start();
        }

        /// <inheritdoc/>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (IsLoaded && !sorting)
            {
                RestartTimer();
                ScrollSelectedItemToCenter(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (horizontalAnimating || verticalAnimating)
            {
                e.Handled = true;
                return;
            }

            var point = e.GetPosition(scrollViewer);
            var hitTestResult = VisualTreeHelper.HitTest(this, point);
            if (hitTestResult == null)
            {
                return;
            }

            var flipViewItem = hitTestResult.VisualHit.TryGetParentFromVisualTree<FlipViewItem>();
            if (flipViewItem != null)
            {
                e.Handled = true;
                flipViewItem.IsSelected = true;
            }
        }

        /// <summary>
        /// 开始滚动偏移动画
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        /// <param name="offset">偏移</param>
        private static void StartOffsetAnimation(FlipView flipView, double offset)
        {
            if (flipView.Orientation == Orientation.Horizontal)
            {
                HorizontalAnimation(flipView, offset, flipView.AnimateDuration);
            }
            else
            {
                VerticalAnimation(flipView, offset);
            }
        }

        /// <summary>
        /// 改变滚动偏移
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        /// <param name="offset">偏移</param>
        private static void ChangeOffset(FlipView flipView, double offset)
        {
            if (flipView.Orientation == Orientation.Horizontal)
            {
                flipView.BeginAnimation(HorizontalOffsetProperty, null);
                flipView.HorizontalOffset = offset;
            }
            else
            {
                flipView.BeginAnimation(VerticalOffsetProperty, null);
                flipView.VerticalOffset = offset;
            }
        }

        /// <summary>
        /// 滚动到当前子项
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        private static void ScrollSelectedItemToCenter(FlipView flipView)
        {
            if (!flipView.IsLoaded || flipView.horizontalAnimating || flipView.verticalAnimating)
            {
                return;
            }

            var item = flipView.ItemContainerGenerator.ContainerFromIndex(flipView.SelectedIndex) as FlipViewItem;
            var point = new Point(flipView.scrollViewer.HorizontalOffset, flipView.scrollViewer.VerticalOffset);
            var targetPosition = item.TransformToVisual(flipView.scrollViewer).Transform(point);
            if (flipView.Orientation == Orientation.Horizontal)
            {
                StartOffsetAnimation(flipView, targetPosition.X - (flipView.scrollViewer.ViewportWidth - item.ActualWidth) / 2);
            }
            else
            {
                StartOffsetAnimation(flipView, targetPosition.Y - (flipView.scrollViewer.ViewportHeight - item.ActualHeight) / 2);
            }
        }

        /// <summary>
        /// 更新子项排序
        /// </summary>
        private static void UpdateItemSort(FlipView flipView)
        {
            flipView.sorting = true;
            var count = flipView.Items.Count;
            if (flipView.Items.Count > 0)
            {
                if (flipView.SelectedIndex < 0)
                {
                    flipView.SelectedIndex = 0;
                }

                if (flipView.IsLoop)
                {
                    int frontCount = (count - 1) / 2; // 当前选中项前面应该有多少个
                    int index = flipView.Items.IndexOf(flipView.SelectedItem);
                    if (index < frontCount)  // 需要向前补 item
                    {
                        index = flipView.Items.IndexOf(flipView.SelectedItem);


                        int num = frontCount - index; // 补 item 数量
                        while (num-- > 0)
                        {
                            var item = flipView.Items[count - 1];
                            if (flipView.ItemContainerGenerator.ContainerFromItem(item) is FlipViewItem flipViewItem)
                            {
                                var offset = flipView.Orientation == Orientation.Horizontal ?
                                             flipView.HorizontalOffset + flipViewItem.ActualWidth :
                                             flipView.VerticalOffset + flipViewItem.ActualHeight;

                                if (flipView.IsLoaded)
                                {
                                    ChangeOffset(flipView, offset);
                                }
                            }

                            flipView.Items.Remove(item);
                            flipView.Items.Insert(0, item);
                        }
                    }
                    else if (index > frontCount)  // 需要向后补 item
                    {
                        int num = index - frontCount; // 补 item 数量
                        while (num-- > 0)
                        {
                            var item = flipView.Items[0];
                            if (flipView.ItemContainerGenerator.ContainerFromItem(item) is FlipViewItem flipViewItem)
                            {
                                var offset = flipView.Orientation == Orientation.Horizontal ?
                                             flipView.HorizontalOffset - flipViewItem.ActualWidth :
                                             flipView.VerticalOffset - flipViewItem.ActualHeight;

                                if (flipView.IsLoaded)
                                {
                                    ChangeOffset(flipView, offset);
                                }
                            }

                            flipView.Items.Remove(item);
                            flipView.Items.Insert(count - 1, item);
                        }
                    }
                }
            }

            flipView.sorting = false;
        }

        /// <summary>
        /// 下一张
        /// </summary>
        private void ClickNextItem(object sender, RoutedEventArgs e)
        {
            if (horizontalAnimating || verticalAnimating || SelectedIndex >= Items.Count - 1)
            {
                return;
            }

            SelectedIndex++;
        }

        /// <summary>
        /// 上一张
        /// </summary>
        private void ClickLastItem(object sender, RoutedEventArgs e)
        {
            if (horizontalAnimating || verticalAnimating || SelectedIndex <= 0)
            {
                return;
            }

            SelectedIndex--;
        }

        /// <summary>
        /// 水平动画
        /// </summary>
        /// <param name="flipView">滑动视图</param>
        /// <param name="offset">偏移</param>
        private static void HorizontalAnimation(FlipView flipView, double offset, Duration duration)
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
                Duration = duration,
                EasingFunction = flipView.AnimateEasingFunction,
            };

            animation.Completed += (sender, e) =>
            {
                flipView.horizontalAnimating = false;
                UpdateItemSort(flipView);
            };

            flipView.horizontalAnimating = true;

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
                flipView.verticalAnimating = false;
                UpdateItemSort(flipView);
            };

            flipView.verticalAnimating = true;
            flipView.BeginAnimation(VerticalOffsetProperty, animation);
        }

        private static void OnIsLoopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                UpdateItemSort(d as FlipView);
            }
        }

        private static void OnIsButtonFloatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = d as FlipView;
            if (!flipView.IsLoaded)
            {
                return;
            }

            ScrollSelectedItemToCenter(flipView);
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
        private void FlipView_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (!IsMouseWheel)
            {
                return;
            }

            if (e.Delta > 0)
            {
                ClickLastItem(null, null);
            }
            else
            {
                ClickNextItem(null, null);
            }
        }

        /// <summary>
        /// 滚动区域大小变化
        /// </summary>
        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(ItemContainerGenerator.ContainerFromIndex(SelectedIndex) is FlipViewItem item) ||
                horizontalAnimating || verticalAnimating)
            {
                return;
            }

            var point = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
            var targetPosition = item.TransformToVisual(scrollViewer).Transform(point);
            if (Orientation == Orientation.Horizontal)
            {
                ChangeOffset(this, targetPosition.X - (scrollViewer.ViewportWidth - item.ActualWidth) / 2);
            }
            else
            {
                ChangeOffset(this, targetPosition.Y - (scrollViewer.ViewportHeight - item.ActualHeight) / 2);
            }
        }
    }
}
