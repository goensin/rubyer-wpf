using System;
using System.Windows;
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
        internal static readonly DependencyPropertyKey UncheckContentPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "UncheckContent", typeof(object), typeof(ToggleButtonHelper), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty UncheckContentProperty = UncheckContentPropertyKey.DependencyProperty;

        private static void SetUncheckContent(DependencyObject element, object value)
        {
            element.SetValue(UncheckContentPropertyKey, value);
        }

        public static object GetUncheckContent(DependencyObject element)
        {
            return (object)element.GetValue(UncheckContentProperty);
        }

        /// <summary>
        /// Checked 显示内容
        /// </summary>
        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.RegisterAttached(
            "CheckedContent", typeof(object), typeof(ToggleButtonHelper), new PropertyMetadata(default(object), OnCheckedContentChanged));

        public static void SetCheckedContent(DependencyObject element, object value)
        {
            element.SetValue(CheckedContentProperty, value);
        }

        public static object GetCheckedContent(DependencyObject element)
        {
            return (object)element.GetValue(CheckedContentProperty);
        }

        private static void OnCheckedContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    CheckAction(toggleButton);
                }

                toggleButton.Checked -= ToggleButton_Checked;
                toggleButton.Unchecked -= ToggleButton_Checked;
                if (GetCheckedContent(toggleButton) != null)
                {
                    toggleButton.Checked += ToggleButton_Checked;
                    toggleButton.Unchecked += ToggleButton_Checked;
                }
            }
        }

        private static void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            if (toggleButton.IsChecked == true)
            {
                CheckAction(toggleButton);
            }
            else
            {
                UncheckAction(toggleButton);
            }
        }

        private static void CheckAction(ToggleButton toggleButton)
        {
            if (GetUncheckContent(toggleButton) == null)
            {
                SetUncheckContent(toggleButton, toggleButton.Content);
               
            }

            toggleButton.Content = GetCheckedContent(toggleButton);
        }

        private static void UncheckAction(ToggleButton toggleButton)
        {
            toggleButton.Content = GetUncheckContent(toggleButton);
        }
    }
}
