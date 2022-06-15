using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
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

        /// <summary>
        /// 当前值显示位置
        /// </summary>
        public static readonly DependencyProperty ValuePlacementProperty = DependencyProperty.RegisterAttached(
           "ValuePlacement", typeof(Dock), typeof(SliderHelper), new PropertyMetadata(default(Dock)));

        public static void SetValuePlacement(DependencyObject element, Dock value)
        {
            element.SetValue(ValuePlacementProperty, value);
        }

        public static Dock GetValuePlacement(DependencyObject element)
        {
            return (Dock)element.GetValue(ValuePlacementProperty);
        }

        /// <summary>
        /// 当前值显示位置偏移
        /// </summary>
        public static readonly DependencyProperty ValueOffsetProperty = DependencyProperty.RegisterAttached(
           "ValueOffset", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        public static void SetValueOffset(DependencyObject element, double value)
        {
            element.SetValue(ValueOffsetProperty, value);
        }

        public static double GetValueOffset(DependencyObject element)
        {
            return (double)element.GetValue(ValueOffsetProperty);
        }

        /// <summary>
        /// 拖拽时显示当前值
        /// </summary>
        public static readonly DependencyProperty DraggingShowValueProperty = DependencyProperty.RegisterAttached(
            "DraggingShowValue", typeof(bool), typeof(SliderHelper), new PropertyMetadata(default(bool), OnDraggingShowValueChanged));

        private static void OnDraggingShowValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && GetDraggingShowValue(slider))
            {
                slider.Loaded += (sender, args) =>
                {
                    if (slider.Template.FindName("Thumb", slider) is Thumb grip)
                    {
                        var adornerLayer = AdornerLayer.GetAdornerLayer(grip);

                        grip.MouseEnter += (a, b) =>
                        {
                            var placement = GetValuePlacement(slider);
                            var offset = GetValueOffset(slider);
                            adornerLayer.Add(new SliderValueAdorner(grip, slider, placement, offset));
                        };

                        grip.MouseLeave += (a, b) =>
                        {
                            Adorner[] toRemoveArray = adornerLayer.GetAdorners(grip);
                            if (toRemoveArray != null)
                            {
                                for (int x = 0; x < toRemoveArray.Length; x++)
                                {
                                    adornerLayer.Remove(toRemoveArray[x]);
                                }
                            }
                        };
                    }
                };
            }

        }

        public static void SetDraggingShowValue(DependencyObject element, bool value)
        {
            element.SetValue(DraggingShowValueProperty, value);
        }

        public static bool GetDraggingShowValue(DependencyObject element)
        {
            return (bool)element.GetValue(DraggingShowValueProperty);
        }

        /// <summary>
        /// 拖拽圆示直径
        /// </summary>
        public static readonly DependencyProperty GripDiameterProperty = DependencyProperty.RegisterAttached(
            "GripDiameter", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        public static void SetGripDiameter(DependencyObject element, double value)
        {
            element.SetValue(GripDiameterProperty, value);
        }

        public static double GetGripDiameter(DependencyObject element)
        {
            return (double)element.GetValue(GripDiameterProperty);
        }
    }
}
