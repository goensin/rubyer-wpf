using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public static class ListBoxHelper
    {
        // 聚焦时颜色
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(SolidColorBrush), typeof(ListBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedBrushProperty);
        }

        public static void SetFocusedBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedBrushProperty, value);
        }

        // 聚焦前景色时颜色
        public static readonly DependencyProperty FocusedForegroundBrushProperty =
            DependencyProperty.RegisterAttached("FocusedForegroundBrush", typeof(SolidColorBrush), typeof(ListBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedForegroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedForegroundBrushProperty);
        }

        public static void SetFocusedForegroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedForegroundBrushProperty, value);
        }
    }
}
