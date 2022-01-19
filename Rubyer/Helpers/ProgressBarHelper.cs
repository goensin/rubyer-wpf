using System.Windows;

namespace Rubyer
{
    public static class ProgressBarHelper
    {
        // 圆型半径
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.RegisterAttached(
            "Thickness", typeof(double), typeof(ProgressBarHelper));

        public static void SetThickness(DependencyObject element, double value)
        {
            element.SetValue(ThicknessProperty, value);
        }

        public static double GetThickness(DependencyObject element)
        {
            return (double)element.GetValue(ThicknessProperty);
        }

        // 是否显示百分比
        public static readonly DependencyProperty IsShowPercentProperty =
            DependencyProperty.RegisterAttached("IsShowPercent", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(false));

        public static bool GetIsShowPercent(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowPercentProperty);
        }

        public static void SetIsShowPercent(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowPercentProperty, value);
        }

        // 是否显示背景
        public static readonly DependencyProperty IsShowBackgroundProperty =
            DependencyProperty.RegisterAttached("IsShowBackground", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(true));

        public static bool GetIsShowBackground(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowBackgroundProperty);
        }

        public static void SetIsShowBackground(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowBackgroundProperty, value);
        }


        // 不确定进度值
        public static readonly DependencyProperty IndeterminateValueProperty =
            DependencyProperty.RegisterAttached("IndeterminateValue", typeof(double), typeof(ProgressBarHelper));

        public static double GetIndeterminateValue(DependencyObject obj)
        {
            return (double)obj.GetValue(IndeterminateValueProperty);
        }

        public static void SetIndeterminateValue(DependencyObject obj, double value)
        {
            obj.SetValue(IndeterminateValueProperty, value);
        }
    }
}
