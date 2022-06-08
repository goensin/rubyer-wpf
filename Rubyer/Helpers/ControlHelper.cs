using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 控件帮助基类
    /// </summary>
    public class ControlHelper
    {
        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(ControlHelper), new PropertyMetadata(default(CornerRadius)));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        /// <summary>
        /// 聚焦颜色
        /// </summary>
        public static readonly DependencyProperty FocusedBrushProperty = DependencyProperty.RegisterAttached(
            "FocusedBrush", typeof(Brush), typeof(ControlHelper), new PropertyMetadata(default(Brush)));

        public static void SetFocusedBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FocusedBrushProperty, value);
        }

        public static Brush GetFocusedBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusedBrushProperty);
        }

        /// <summary>
        /// 聚焦前景色
        /// </summary>
        public static readonly DependencyProperty FocusedForegroundBrushProperty = DependencyProperty.RegisterAttached(
            "FocusedForegroundBrush", typeof(Brush), typeof(ControlHelper), new PropertyMetadata(default(Brush)));

        public static void SetFocusedForegroundBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FocusedForegroundBrushProperty, value);
        }

        public static Brush GetFocusedForegroundBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusedForegroundBrushProperty);
        }

        /// <summary>
        /// 聚焦边框颜色
        /// </summary>
        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached(
             "FocusBorderBrush", typeof(Brush), typeof(ControlHelper), new PropertyMetadata(default(Brush)));

        public static void SetFocusBorderBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FocusBorderBrushProperty, value);
        }

        public static Brush GetFocusBorderBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusBorderBrushProperty);
        }

        /// <summary>
        /// 聚焦边框厚度
        /// </summary>
        public static readonly DependencyProperty FocusBorderThicknessProperty = DependencyProperty.RegisterAttached(
            "FocusBorderThickness", typeof(Thickness), typeof(ControlHelper), new PropertyMetadata(default(Thickness)));

        public static void SetFocusBorderThickness(DependencyObject element, Thickness value)
        {
            element.SetValue(FocusBorderThicknessProperty, value);
        }

        public static Thickness GetFocusBorderThickness(DependencyObject element)
        {
            return (Thickness)element.GetValue(FocusBorderThicknessProperty);
        }

        /// <summary>
        /// 鼠标悬停颜色
        /// </summary>
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBrush", typeof(Brush), typeof(ControlHelper), new PropertyMetadata(default(Brush)));

        public static void SetMouseOverBrush(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverBrushProperty, value);
        }

        public static Brush GetMouseOverBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverBrushProperty);
        }

        /// <summary>
        /// 按下颜色
        /// </summary>
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.RegisterAttached(
           "PressedBrush", typeof(Brush), typeof(ControlHelper), new PropertyMetadata(default(Brush)));

        public static void SetPressedBrush(DependencyObject element, Brush value)
        {
            element.SetValue(PressedBrushProperty, value);
        }

        public static Brush GetPressedBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(PressedBrushProperty);
        }

        /// <summary>
        /// 是否聚焦
        /// </summary>
        public static readonly DependencyProperty IsKeyBoardFocusedProperty = DependencyProperty.RegisterAttached(
            "IsKeyBoardFocused", typeof(bool), typeof(ControlHelper), new PropertyMetadata(false));

        public static bool GetIsKeyBoardFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsKeyBoardFocusedProperty);
        }

        public static void SetIsKeyBoardFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsKeyBoardFocusedProperty, value);
        }

        /// <summary>
        /// 遮罩透明度
        /// </summary>
        public static readonly DependencyProperty MaskOpacityProperty = DependencyProperty.RegisterAttached(
            "MaskOpacity", typeof(double), typeof(ControlHelper), new PropertyMetadata(0.6));

        public static double GetMaskOpacity(DependencyObject obj)
        {
            return (double)obj.GetValue(MaskOpacityProperty);
        }

        public static void SetMaskOpacity(DependencyObject obj, double value)
        {
            obj.SetValue(MaskOpacityProperty, value);
        }
    }
}
