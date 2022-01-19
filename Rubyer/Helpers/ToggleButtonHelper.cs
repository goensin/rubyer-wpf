using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public static class ToggleButtonHelper
    {
        // 选中背景色
        public static readonly DependencyProperty CheckedBackgroundProperty =
           DependencyProperty.RegisterAttached("CheckedBackground", typeof(Brush), typeof(ToggleButtonHelper));

        public static Brush GetCheckedBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(CheckedBackgroundProperty);
        }

        public static void SetCheckedBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(CheckedBackgroundProperty, value);
        }
    }
}
