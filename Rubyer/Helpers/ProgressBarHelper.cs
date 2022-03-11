using System.Windows;

namespace Rubyer
{
    public static class ProgressBarHelper
    {
        // 厚度
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
        public static readonly DependencyProperty ShowPercentProperty =
            DependencyProperty.RegisterAttached("ShowPercent", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(false));

        public static bool GetShowPercent(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowPercentProperty);
        }

        public static void SetShowPercent(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowPercentProperty, value);
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
