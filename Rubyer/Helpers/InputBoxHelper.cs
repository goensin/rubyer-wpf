using Rubyer.Commons.KnownBoxes;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 输入框帮助类
    /// </summary>
    public static class InputBoxHelper
    {
        /// <summary>
        /// 前置内容
        /// </summary>
        public static readonly DependencyProperty PreContentProperty =
            DependencyProperty.RegisterAttached("PreContent", typeof(object), typeof(InputBoxHelper), new PropertyMetadata(null));

        /// <summary>
        /// Gets the pre content.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An object.</returns>
        public static object GetPreContent(DependencyObject obj)
        {
            return obj.GetValue(PreContentProperty);
        }

        /// <summary>
        /// Sets the pre content.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetPreContent(DependencyObject obj, object value)
        {
            obj.SetValue(PreContentProperty, value);
        }

        /// <summary>
        /// 前置内容
        /// </summary>
        public static readonly DependencyProperty PostContentProperty =
            DependencyProperty.RegisterAttached("PostContent", typeof(object), typeof(InputBoxHelper), new PropertyMetadata(null));

        /// <summary>
        /// Gets the post content.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An object.</returns>
        public static object GetPostContent(DependencyObject obj)
        {
            return (object)obj.GetValue(PostContentProperty);
        }

        /// <summary>
        /// Sets the post content.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetPostContent(DependencyObject obj, object value)
        {
            obj.SetValue(PostContentProperty, value);
        }

        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.RegisterAttached("IsClearable", typeof(bool), typeof(InputBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsClearbleChanged));

        /// <summary>
        /// Gets the is clearable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsClearable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsClearableProperty);
        }

        /// <summary>
        /// Sets the is clearable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsClearable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearableProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(InputBoxHelper), new PropertyMetadata(default(string)));

        /// <summary>
        /// Sets the watermark.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        /// <summary>
        /// Gets the watermark.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A string.</returns>
        public static string GetWatermark(DependencyObject element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }

        /// <summary>
        /// Ons the is clearble changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnIsClearbleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                RoutedEventHandler handle = (sender, args) =>
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Focus();
                        textBox.Text = null;
                    }
                    else if (control is PasswordBox passwordBox)
                    {
                        passwordBox.Focus();
                        passwordBox.Password = null;
                    }
                    else if (control is ComboBox comboBox)
                    {
                        comboBox.Focus();
                        if (comboBox.IsEditable)
                        {
                            comboBox.Text = null;
                        }

                        comboBox.SelectedItem = null;
                    }
                };

                control.Loaded += (sender, arg) =>
                {
                    if (control.Template.FindName("clearButton", control) is Button clearButton)
                    {
                        if (GetIsClearable(control))
                        {
                            clearButton.Click += handle;
                        }
                        else
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };

                control.Unloaded += (sender, arg) =>
                {
                    if (control.Template.FindName("clearButton", control) is Button clearButton)
                    {
                        if (GetIsClearable(control))
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };
            }
        }
    }
}