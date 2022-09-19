using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 标题帮助类
    /// </summary>
    public class HeaderHelper
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the background.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Brush.</returns>
        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BackgroundProperty);
        }

        /// <summary>
        /// Sets the background.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// 前景色
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the foreground.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Brush.</returns>
        public static Brush GetForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        /// <summary>
        /// Sets the foreground.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// 字体
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.RegisterAttached("FontFamily", typeof(FontFamily), typeof(HeaderHelper), new PropertyMetadata(SystemFonts.CaptionFontFamily));

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A FontFamily.</returns>
        public static FontFamily GetFontFamily(DependencyObject obj)
        {
            return (FontFamily)obj.GetValue(FontFamilyProperty);
        }

        /// <summary>
        /// Sets the font family.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetFontFamily(DependencyObject obj, FontFamily value)
        {
            obj.SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(HeaderHelper), new PropertyMetadata(SystemFonts.CaptionFontSize));

        /// <summary>
        /// Gets the font size.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static double GetFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }

        /// <summary>
        /// Sets the font size.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// 字体加粗
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(HeaderHelper), new PropertyMetadata(SystemFonts.CaptionFontWeight));

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A FontWeight.</returns>
        public static FontWeight GetFontWeight(DependencyObject obj)
        {
            return (FontWeight)obj.GetValue(FontWeightProperty);
        }

        /// <summary>
        /// Sets the font weight.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetFontWeight(DependencyObject obj, FontWeight value)
        {
            obj.SetValue(FontWeightProperty, value);
        }

        /// <summary>
        /// padding
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.RegisterAttached("Padding", typeof(Thickness), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the padding.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Thickness.</returns>
        public static Thickness GetPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(PaddingProperty);
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetPadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// margin
        /// </summary>
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the margin.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Thickness.</returns>
        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        /// <summary>
        /// Sets the margin.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        /// <summary>
        /// 水平对齐
        /// </summary>
        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("HorizontalAlignment", typeof(HorizontalAlignment), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the horizontal alignment.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A HorizontalAlignment.</returns>
        public static HorizontalAlignment GetHorizontalAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(HorizontalAlignmentProperty);
        }

        /// <summary>
        /// Sets the horizontal alignment.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(HorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// 垂直对齐
        /// </summary>
        public static readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.RegisterAttached("VerticalAlignment", typeof(VerticalAlignment), typeof(HeaderHelper), new PropertyMetadata(default));

        /// <summary>
        /// Gets the vertical alignment.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A VerticalAlignment.</returns>
        public static VerticalAlignment GetVerticalAlignment(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(VerticalAlignmentProperty);
        }

        /// <summary>
        /// Sets the vertical alignment.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetVerticalAlignment(DependencyObject obj, VerticalAlignment value)
        {
            obj.SetValue(VerticalAlignmentProperty, value);
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(HeaderHelper), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// Sets the corner radius.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets the corner radius.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A CornerRadius.</returns>
        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }
    }
}