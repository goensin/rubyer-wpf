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

        private static void OnDraggingShowValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is Slider slider)
            //{
            //    slider.Loaded += (sender, args) =>
            //    {
            //        if (slider.Template.FindName("Thumb", slider) is Thumb grip)
            //        {
            //            //得到层容器
            //            var adornerLayer = AdornerLayer.GetAdornerLayer(grip);
            //            //在层容器中加层
            //            adornerLayer.Add(new SliderValueAdorner(grip, slider));
            //        }
            //    };
            //}

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
