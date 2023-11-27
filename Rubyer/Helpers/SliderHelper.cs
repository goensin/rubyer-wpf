using Rubyer.Commons.KnownBoxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

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
        /// 轨道厚度
        /// </summary>
        public static readonly DependencyProperty TrackThicknessProperty = DependencyProperty.RegisterAttached(
            "TrackThickness", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        public static void SetTrackThickness(DependencyObject element, double value)
        {
            element.SetValue(TrackThicknessProperty, value);
        }

        public static double GetTrackThickness(DependencyObject element)
        {
            return (double)element.GetValue(TrackThicknessProperty);
        }
    }
}