using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public static class ListViewHelper
    {
        // 表头背景色
        public static readonly DependencyProperty HeadBackgroundProperty =
            DependencyProperty.RegisterAttached("HeadBackground", typeof(Brush), typeof(ListViewHelper));

        public static void SetHeadBackground(DependencyObject element, Brush value)
        {
            element.SetValue(HeadBackgroundProperty, value);
        }

        public static Brush GetHeadBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(HeadBackgroundProperty);
        }

        // 表头前景色
        public static readonly DependencyProperty HeadForegroundProperty =
            DependencyProperty.RegisterAttached("HeadForeground", typeof(Brush), typeof(ListViewHelper));

        public static void SetHeadForeground(DependencyObject element, Brush value)
        {
            element.SetValue(HeadForegroundProperty, value);
        }

        public static Brush GetHeadForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(HeadForegroundProperty);
        }
    }
}
