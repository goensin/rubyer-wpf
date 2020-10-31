using System.Windows;

namespace Rubyer
{
    public static class ButtonHelper
    {
        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(ButtonHelper));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        // 圆型按钮直径
        public static readonly DependencyProperty CircleDimaProperty = DependencyProperty.RegisterAttached(
            "CircleDima", typeof(double), typeof(ButtonHelper));

        public static void SetCircleDima(DependencyObject element, double value)
        {
            element.SetValue(CircleDimaProperty, value);
        }

        public static double GetCircleDima(DependencyObject element)
        {
            return (double)element.GetValue(CircleDimaProperty);
        }
    }
}
