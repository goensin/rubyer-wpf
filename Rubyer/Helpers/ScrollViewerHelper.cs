using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    /// <summary>
    /// ScrollViewer 帮助类
    /// </summary>
    public class ScrollViewerHelper
    {
        /// <summary>
        /// 滚动条前景色
        /// </summary>
        public static readonly DependencyProperty ScrollBarForegroundProperty = DependencyProperty.RegisterAttached(
            "ScrollBarForeground", typeof(Brush), typeof(ScrollViewerHelper), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets the scroll bar foreground.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetScrollBarForeground(DependencyObject element, Brush value)
        {
            element.SetValue(ScrollBarForegroundProperty, value);
        }

        /// <summary>
        /// Gets the scroll bar foreground.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A Brush.</returns>
        public static Brush GetScrollBarForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(ScrollBarForegroundProperty);
        }

        /// <summary>
        /// 滚动条背景色
        /// </summary>
        public static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.RegisterAttached(
            "ScrollBarBackground", typeof(Brush), typeof(ScrollViewerHelper), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets the scroll bar background.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetScrollBarBackground(DependencyObject element, Brush value)
        {
            element.SetValue(ScrollBarBackgroundProperty, value);
        }

        /// <summary>
        /// Gets the scroll bar background.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A Brush.</returns>
        public static Brush GetScrollBarBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(ScrollBarBackgroundProperty);
        }

        /// <summary>
        /// 滚动条大小
        /// </summary>
        public static readonly DependencyProperty ScrollBarSizeProperty = DependencyProperty.RegisterAttached(
            "ScrollBarSize", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the scroll bar size.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetScrollBarSize(DependencyObject element, double value)
        {
            element.SetValue(ScrollBarSizeProperty, value);
        }

        /// <summary>
        /// Gets the scroll bar size.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetScrollBarSize(DependencyObject element)
        {
            return (double)element.GetValue(ScrollBarSizeProperty);
        }

        /// <summary>
        /// 显示箭头按钮
        /// </summary>
        public static readonly DependencyProperty ShowArrowButtonProperty = DependencyProperty.RegisterAttached(
            "ShowArrowButton", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Sets the show arrow button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetShowArrowButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowArrowButtonProperty, value);
        }

        /// <summary>
        /// Gets the show arrow button.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowArrowButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowArrowButtonProperty);
        }

        /// <summary>
        /// 垂直偏移变化值
        /// </summary>
        public static readonly DependencyProperty VerticalDeltaProperty = DependencyProperty.RegisterAttached(
            "VerticalDelta", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the vertical delta.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetVerticalDelta(DependencyObject element, double value)
        {
            element.SetValue(VerticalDeltaProperty, value);
        }

        /// <summary>
        /// Gets the vertical delta.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetVerticalDelta(DependencyObject element)
        {
            return (double)element.GetValue(VerticalDeltaProperty);
        }

        /// <summary>
        /// 水平偏移变化值
        /// </summary>
        public static readonly DependencyProperty HorizontalDeltaProperty = DependencyProperty.RegisterAttached(
            "HorizontalDelta", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the horizontal delta.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetHorizontalDelta(DependencyObject element, double value)
        {
            element.SetValue(HorizontalDeltaProperty, value);
        }

        /// <summary>
        /// Gets the horizontal delta.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetHorizontalDelta(DependencyObject element)
        {
            return (double)element.GetValue(HorizontalDeltaProperty);
        }

        /// <summary>
        /// 垂直偏移
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached(
            "VerticalOffset", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double), OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        /// <summary>
        /// Sets the vertical offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetVerticalOffset(DependencyObject element, double value)
        {
            element.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the vertical offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetVerticalOffset(DependencyObject element)
        {
            return (double)element.GetValue(VerticalOffsetProperty);
        }

        /// <summary>
        /// 水平偏移
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached(
            "HorizontalOffset", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double), OnHorizontalOffsetChanged));

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        /// <summary>
        /// Sets the horizontal offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetHorizontalOffset(DependencyObject element, double value)
        {
            element.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the horizontal offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetHorizontalOffset(DependencyObject element)
        {
            return (double)element.GetValue(HorizontalOffsetProperty);
        }

        /// <summary>
        /// 是否显示动画
        /// </summary>
        public static readonly DependencyProperty IsOnlyArrowProperty = DependencyProperty.RegisterAttached(
            "IsOnlyArrow", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(default(bool), OnIsOnlyArrowChanged));

        private static void OnIsOnlyArrowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.Loaded += (sender, args) =>
                {
                    if (GetIsOnlyArrow(scrollViewer))
                    {
                        var horizontalDelta = GetHorizontalDelta(scrollViewer);
                        var verticalDelt = GetVerticalDelta(scrollViewer);
                        if (scrollViewer.Template.FindName("upButton", scrollViewer) is ButtonBase upButton)
                        {
                            upButton.Click += (a, b) => VerticalAnimation(scrollViewer, -verticalDelt);
                        }

                        if (scrollViewer.Template.FindName("downButton", scrollViewer) is ButtonBase downButton)
                        {
                            downButton.Click += (a, b) => VerticalAnimation(scrollViewer, verticalDelt);
                        }

                        if (scrollViewer.Template.FindName("leftButton", scrollViewer) is ButtonBase leftButton)
                        {
                            leftButton.Click += (a, b) => HorizontalAnimation(scrollViewer, -horizontalDelta);
                        }

                        if (scrollViewer.Template.FindName("rightButton", scrollViewer) is ButtonBase rightButton)
                        {
                            rightButton.Click += (a, b) => HorizontalAnimation(scrollViewer, horizontalDelta);
                        }
                    }
                };
            }
        }

        private static void VerticalAnimation(ScrollViewer scrollViewer, double offset)
        {
            double actullOffset = scrollViewer.VerticalOffset + offset;
            if (actullOffset < 0)
            {
                actullOffset = 0;
            }
            else if (actullOffset > scrollViewer.ScrollableHeight)
            {
                actullOffset = scrollViewer.ScrollableHeight;
            }

            var Animation = new DoubleAnimation
            {
                From = scrollViewer.VerticalOffset,
                To = actullOffset,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
            };

            scrollViewer.BeginAnimation(VerticalOffsetProperty, Animation);
        }

        private static void HorizontalAnimation(ScrollViewer scrollViewer, double offset)
        {
            double actullOffset = scrollViewer.HorizontalOffset + offset;
            if (actullOffset < 0)
            {
                actullOffset = 0;
            }
            else if (actullOffset > scrollViewer.ScrollableWidth)
            {
                actullOffset = scrollViewer.ScrollableWidth;
            }

            var Animation = new DoubleAnimation
            {
                From = scrollViewer.HorizontalOffset,
                To = actullOffset,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
            };

            scrollViewer.BeginAnimation(HorizontalOffsetProperty, Animation);
        }

        /// <summary>
        /// Sets the is only arrow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsOnlyArrow(DependencyObject element, bool value)
        {
            element.SetValue(IsOnlyArrowProperty, value);
        }

        /// <summary>
        /// Gets the is only arrow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsOnlyArrow(DependencyObject element)
        {
            return (bool)element.GetValue(IsOnlyArrowProperty);
        }
    }
}