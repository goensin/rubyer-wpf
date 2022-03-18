using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rubyer
{
    public class SliderHelper
    {
        /// <summary>
        /// 选择范围颜色
        /// </summary>
        public static readonly DependencyProperty SelectionRangeBrushProperty = DependencyProperty.RegisterAttached(
           "SelectionRangeBrush", typeof(Brush), typeof(SliderHelper), new PropertyMetadata(default(Brush)));

        public static void SetSelectionRangeBrush(DependencyObject element, Brush value)
        {
            element.SetValue(SelectionRangeBrushProperty, value);
        }

        public static Brush GetSelectionRangeBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(SelectionRangeBrushProperty);
        }

        // <summary>
        /// 拖拽时显示当前值
        /// </summary>
        public static readonly DependencyProperty DraggingShowValueProperty = DependencyProperty.RegisterAttached(
            "DraggingShowValue", typeof(bool), typeof(SliderHelper), new PropertyMetadata(default(bool), OnDraggingShowValueChanged));

        public static void SetDraggingShowValue(DependencyObject element, bool value)
        {
            element.SetValue(DraggingShowValueProperty, value);
        }

        public static bool GetDraggingShowValue(DependencyObject element)
        {
            return (bool)element.GetValue(DraggingShowValueProperty);
        }

        private static void OnDraggingShowValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider)
            {
                RoutedPropertyChangedEventHandler<double> handle = (sender, args) =>
                {
                    if (slider.Template.FindName("valuePopup", slider) is Popup popup)
                    {
                        System.Reflection.MethodInfo mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        _ = mi.Invoke(popup, null);
                    }
                };

                slider.Loaded += (sender, arg) =>
                {
                    if (GetDraggingShowValue(slider))
                    {
                        slider.ValueChanged += handle;
                    }
                    else
                    {
                        slider.ValueChanged -= handle;
                    }
                };

                slider.Unloaded += (sender, arg) =>
                {
                    if (GetDraggingShowValue(slider))
                    {
                        slider.ValueChanged -= handle;
                    }
                };
            }
        }
    }
}
