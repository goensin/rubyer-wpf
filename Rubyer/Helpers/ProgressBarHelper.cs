using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// ProgressBar 帮助类
    /// </summary>
    public static class ProgressBarHelper
    {
        /// <summary>
        /// 厚度
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.RegisterAttached(
            "Thickness", typeof(double), typeof(ProgressBarHelper));

        /// <summary>
        /// Sets the thickness.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetThickness(DependencyObject element, double value)
        {
            element.SetValue(ThicknessProperty, value);
        }

        /// <summary>
        /// Gets the thickness.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetThickness(DependencyObject element)
        {
            return (double)element.GetValue(ThicknessProperty);
        }

        /// <summary>
        /// 是否显示百分比
        /// </summary>
        public static readonly DependencyProperty ShowPercentProperty =
            DependencyProperty.RegisterAttached("ShowPercent", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(false));

        /// <summary>
        /// Gets the show percent.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowPercent(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowPercentProperty);
        }

        /// <summary>
        /// Sets the show percent.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetShowPercent(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowPercentProperty, value);
        }

        /// <summary>
        /// 是否显示背景
        /// </summary>
        public static readonly DependencyProperty IsShowBackgroundProperty =
            DependencyProperty.RegisterAttached("IsShowBackground", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(true));

        /// <summary>
        /// Gets the is show background.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsShowBackground(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowBackgroundProperty);
        }

        /// <summary>
        /// Sets the is show background.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsShowBackground(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowBackgroundProperty, value);
        }

        /// <summary>
        /// 不确定进度值
        /// </summary>
        public static readonly DependencyProperty IndeterminateValueProperty =
            DependencyProperty.RegisterAttached("IndeterminateValue", typeof(double), typeof(ProgressBarHelper));

        /// <summary>
        /// Gets the indeterminate value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static double GetIndeterminateValue(DependencyObject obj)
        {
            return (double)obj.GetValue(IndeterminateValueProperty);
        }

        /// <summary>
        /// Sets the indeterminate value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetIndeterminateValue(DependencyObject obj, double value)
        {
            obj.SetValue(IndeterminateValueProperty, value);
        }
    }
}