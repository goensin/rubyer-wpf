using Rubyer.Commons.KnownBoxes;
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
    /// <summary>
    /// Slider 帮助类
    /// </summary>
    public class SliderHelper
    {
        /// <summary>
        /// 选择范围颜色
        /// </summary>
        public static readonly DependencyProperty SelectionRangeBrushProperty = DependencyProperty.RegisterAttached(
           "SelectionRangeBrush", typeof(Brush), typeof(SliderHelper), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets the selection range brush.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionRangeBrush(DependencyObject element, Brush value)
        {
            element.SetValue(SelectionRangeBrushProperty, value);
        }

        /// <summary>
        /// Gets the selection range brush.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A Brush.</returns>
        public static Brush GetSelectionRangeBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(SelectionRangeBrushProperty);
        }

        /// <summary>
        /// 当前值显示位置
        /// </summary>
        public static readonly DependencyProperty ValuePlacementProperty = DependencyProperty.RegisterAttached(
           "ValuePlacement", typeof(Dock), typeof(SliderHelper), new PropertyMetadata(default(Dock)));

        /// <summary>
        /// Sets the value placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetValuePlacement(DependencyObject element, Dock value)
        {
            element.SetValue(ValuePlacementProperty, value);
        }

        /// <summary>
        /// Gets the value placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A Dock.</returns>
        public static Dock GetValuePlacement(DependencyObject element)
        {
            return (Dock)element.GetValue(ValuePlacementProperty);
        }

        /// <summary>
        /// 当前值显示位置偏移
        /// </summary>
        public static readonly DependencyProperty ValueOffsetProperty = DependencyProperty.RegisterAttached(
           "ValueOffset", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the value offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetValueOffset(DependencyObject element, double value)
        {
            element.SetValue(ValueOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetValueOffset(DependencyObject element)
        {
            return (double)element.GetValue(ValueOffsetProperty);
        }

        /// <summary>
        /// 拖拽时显示当前值
        /// </summary>
        public static readonly DependencyProperty DraggingShowValueProperty = DependencyProperty.RegisterAttached(
            "DraggingShowValue", typeof(bool), typeof(SliderHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnDraggingShowValueChanged));

        private static void OnDraggingShowValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && GetDraggingShowValue(slider))
            {
                if (slider.IsLoaded)
                {
                    AddCurrentValueAdorner(slider);
                }
                else
                {
                    slider.Loaded += (sender, args) =>
                    {
                        AddCurrentValueAdorner(slider);
                    };
                }
            }
        }

        private static void AddCurrentValueAdorner(Slider slider)
        {
            if (slider.Template.FindName("Thumb", slider) is Thumb grip)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(grip);

                grip.MouseEnter += (a, b) =>
                {
                    var placement = GetValuePlacement(slider);
                    var offset = GetValueOffset(slider);
                    var stringFormat = GetValueStringFormat(slider);
                    adornerLayer.Add(new SliderValueAdorner(grip, slider, placement, offset, stringFormat));
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
        }

        /// <summary>
        /// Sets the dragging show value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetDraggingShowValue(DependencyObject element, bool value)
        {
            element.SetValue(DraggingShowValueProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Gets the dragging show value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetDraggingShowValue(DependencyObject element)
        {
            return (bool)element.GetValue(DraggingShowValueProperty);
        }

        /// <summary>
        /// 拖拽圆示直径
        /// </summary>
        public static readonly DependencyProperty GripDiameterProperty = DependencyProperty.RegisterAttached(
            "GripDiameter", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the grip diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetGripDiameter(DependencyObject element, double value)
        {
            element.SetValue(GripDiameterProperty, value);
        }

        /// <summary>
        /// Gets the grip diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetGripDiameter(DependencyObject element)
        {
            return (double)element.GetValue(GripDiameterProperty);
        }

        /// <summary>
        /// 当前值显示字符串格式
        /// </summary>
        public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.RegisterAttached(
           "ValueStringFormat", typeof(string), typeof(SliderHelper), new PropertyMetadata(default(string)));

        /// <summary>
        /// Sets the value offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetValueStringFormat(DependencyObject element, string value) => element.SetValue(ValueStringFormatProperty, value);

        /// <summary>
        /// Gets the value offset.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static string GetValueStringFormat(DependencyObject element) => (string)element.GetValue(ValueStringFormatProperty);
    }
}