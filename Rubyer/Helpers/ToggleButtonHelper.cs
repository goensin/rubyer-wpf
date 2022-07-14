using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Rubyer
{
    /// <summary>
    /// ToggleButton 帮助类
    /// </summary>
    public class ToggleButtonHelper
    {
        /// <summary>
        /// Unchecked 显示内容,内部使用
        /// </summary>
        internal static readonly DependencyPropertyKey UncheckedContentPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "UncheckedContent", typeof(object), typeof(ToggleButtonHelper), new PropertyMetadata(default(object)));

        /// <summary>
        ///  Unchecked 显示内容,内部使用
        /// </summary>
        public static readonly DependencyProperty UncheckedContentProperty = UncheckedContentPropertyKey.DependencyProperty;

        private static void SetUncheckedContent(DependencyObject element, object value)
        {
            element.SetValue(UncheckedContentPropertyKey, value);
        }

        private static object GetUncheckedContent(DependencyObject element)
        {
            return (object)element.GetValue(UncheckedContentProperty);
        }

        /// <summary>
        /// Checked 显示内容
        /// </summary>
        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.RegisterAttached(
            "CheckedContent", typeof(object), typeof(ToggleButtonHelper), new PropertyMetadata(default(object), OnCheckedContentChanged));

        /// <summary>
        /// Sets the checked content.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetCheckedContent(DependencyObject element, object value)
        {
            element.SetValue(CheckedContentProperty, value);
        }

        /// <summary>
        /// Gets the checked content.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>An object.</returns>
        public static object GetCheckedContent(DependencyObject element)
        {
            return (object)element.GetValue(CheckedContentProperty);
        }

        private static void OnCheckedContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToggleButton toggleButton)
            {
                SetUncheckedContent(toggleButton, toggleButton.Content);

                if (toggleButton.IsChecked == true)
                {
                    CheckAction(toggleButton);
                }

                toggleButton.Checked -= ToggleButton_Checked;
                toggleButton.Unchecked -= ToggleButton_Checked;
                toggleButton.Indeterminate -= ToggleButton_Checked;

                if (GetCheckedContent(toggleButton) != null)
                {
                    toggleButton.Checked += ToggleButton_Checked;
                    toggleButton.Unchecked += ToggleButton_Checked;
                    toggleButton.Indeterminate += ToggleButton_Checked;
                }
            }
        }

        private static void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            if (toggleButton.IsChecked == false)
            {
                UncheckAction(toggleButton);
            }
            else
            {
                CheckAction(toggleButton);
            }
        }

        private static void CheckAction(ToggleButton toggleButton)
        {
            if (GetUncheckedContent(toggleButton) == null)
            {
                SetUncheckedContent(toggleButton, toggleButton.Content);
            }

            toggleButton.Content = GetCheckedContent(toggleButton);
        }

        private static void UncheckAction(ToggleButton toggleButton)
        {
            toggleButton.Content = GetUncheckedContent(toggleButton);
        }
    }
}