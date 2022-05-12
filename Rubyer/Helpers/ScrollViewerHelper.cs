using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public class ScrollViewerHelper
    {
        /// <summary>
        /// 滚动条前景色
        /// </summary>
        public static readonly DependencyProperty ScrollBarForegroundProperty = DependencyProperty.RegisterAttached(
            "ScrollBarForeground", typeof(Brush), typeof(ScrollViewerHelper), new PropertyMetadata(default(Brush)));

        public static void SetScrollBarForeground(DependencyObject element, Brush value)
        {
            element.SetValue(ScrollBarForegroundProperty, value);
        }

        public static Brush GetScrollBarForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(ScrollBarForegroundProperty);
        }

        /// <summary>
        /// 滚动条背景色
        /// </summary>
        public static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.RegisterAttached(
            "ScrollBarBackground", typeof(Brush), typeof(ScrollViewerHelper), new PropertyMetadata(default(Brush)));

        public static void SetScrollBarBackground(DependencyObject element, Brush value)
        {
            element.SetValue(ScrollBarBackgroundProperty, value);
        }

        public static Brush GetScrollBarBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(ScrollBarBackgroundProperty);
        }

        /// <summary>
        /// 滚动条大小
        /// </summary>
        public static readonly DependencyProperty ScrollBarSizeProperty = DependencyProperty.RegisterAttached(
            "ScrollBarSize", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(default(double)));

        public static void SetScrollBarSize(DependencyObject element, double value)
        {
            element.SetValue(ScrollBarSizeProperty, value);
        }

        public static double GetScrollBarSize(DependencyObject element)
        {
            return (double)element.GetValue(ScrollBarSizeProperty);
        }

        /// <summary>
        /// 显示箭头按钮
        /// </summary>
        public static readonly DependencyProperty ShowArrowButtonProperty = DependencyProperty.RegisterAttached(
            "ShowArrowButton", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(default(bool)));

        public static void SetShowArrowButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowArrowButtonProperty, value);
        }

        public static bool GetShowArrowButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowArrowButtonProperty);
        }
    }
}
