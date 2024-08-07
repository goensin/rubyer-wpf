using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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
            DependencyProperty.RegisterAttached("ShowPercent", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

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
            obj.SetValue(ShowPercentProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 百分比格式
        /// </summary>
        public static readonly DependencyProperty PercentFormatProperty =
          DependencyProperty.RegisterAttached("PercentFormat", typeof(string), typeof(ProgressBarHelper), new PropertyMetadata(string.Empty));

        public static string GetPercentFormat(DependencyObject obj)
        {
            return (string)obj.GetValue(PercentFormatProperty);
        }

        public static void SetPercentFormat(DependencyObject obj, string value)
        {
            obj.SetValue(PercentFormatProperty, value);
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

        /// <summary>
        /// 是否启用动画
        /// </summary>
        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.RegisterAttached("IsAnimation", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsAnimationChanged));

        public static bool GetIsAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimationProperty);
        }

        public static void SetIsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimationProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 是否进行动画中
        /// </summary>
        internal static readonly DependencyPropertyKey IsAnimatingPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsAnimating", typeof(bool), typeof(ProgressBarHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否进行动画中
        /// </summary>
        public static readonly DependencyProperty IsAnimatingProperty = IsAnimatingPropertyKey.DependencyProperty;

        public static bool GetIsAnimating(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimatingProperty);
        }

        internal static void SetIsAnimating(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimatingPropertyKey, BooleanBoxes.Box(value));
        }

        private static void OnIsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBar progressBar)
            {
                if (GetIsAnimation(progressBar))
                {
                    progressBar.ValueChanged += ProgressBar_ValueChanged;
                }
                else
                {
                    progressBar.ValueChanged -= ProgressBar_ValueChanged;
                }
            }
        }

        private static void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var progressBar = (ProgressBar)sender;
            if (GetIsAnimating(progressBar))
            {
                return;
            }

            var doubleAnimation = new DoubleAnimation(e.OldValue, e.NewValue, new Duration(TimeSpan.FromSeconds(0.5)));
            doubleAnimation.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            doubleAnimation.Completed += (a, b) =>
            {
                SetIsAnimating(progressBar, false);
                progressBar.BeginAnimation(ProgressBar.ValueProperty, null);
            };
            SetIsAnimating(progressBar, true);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }
    }
}