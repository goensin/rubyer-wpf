using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public class HeaderHelper
    {
        // 背景色
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(SolidColorBrush), typeof(HeaderHelper), new PropertyMetadata(default));

        public static SolidColorBrush GetBackground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        // 前景色
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(SolidColorBrush), typeof(HeaderHelper), new PropertyMetadata(default));

        public static SolidColorBrush GetForeground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        // 字体大小
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(HeaderHelper), new PropertyMetadata(default));

        public static double GetFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        // 字体加粗
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(HeaderHelper), new PropertyMetadata(default));

        public static FontWeight GetFontWeight(DependencyObject obj)
        {
            return (FontWeight)obj.GetValue(FontWeightProperty);
        }

        public static void SetFontWeight(DependencyObject obj, FontWeight value)
        {
            obj.SetValue(FontWeightProperty, value);
        }

        // padding
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.RegisterAttached("Padding", typeof(Thickness), typeof(HeaderHelper), new PropertyMetadata(default));

        public static Thickness GetPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(PaddingProperty);
        }

        public static void SetPadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(PaddingProperty, value);
        }

        // margin
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(HeaderHelper), new PropertyMetadata(default));

        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        // 水平对齐
        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("HorizontalAlignment", typeof(HorizontalAlignment), typeof(HeaderHelper), new PropertyMetadata(default));

        public static HorizontalAlignment GetHorizontalAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(HorizontalAlignmentProperty);
        }

        public static void SetHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(HorizontalAlignmentProperty, value);
        }

        // 垂直对齐
        public static readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.RegisterAttached("VerticalAlignment", typeof(VerticalAlignment), typeof(HeaderHelper), new PropertyMetadata(default));

        public static VerticalAlignment GetVerticalAlignment(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(VerticalAlignmentProperty);
        }

        public static void SetVerticalAlignment(DependencyObject obj, VerticalAlignment value)
        {
            obj.SetValue(VerticalAlignmentProperty, value);
        }
    }
}
