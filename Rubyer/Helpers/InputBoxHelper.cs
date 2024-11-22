using Rubyer.Commons.KnownBoxes;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
        /// 是否边框圆角
        /// </summary>
        public static readonly DependencyProperty IsRoundProperty =
            DependencyProperty.RegisterAttached("IsRound", typeof(bool), typeof(InputBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        public static bool GetIsRound(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRoundProperty);
        }

        public static void SetIsRound(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRoundProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 内部元素间隔
        /// </summary>
        public static readonly DependencyProperty InternalSpacingProperty =
            DependencyProperty.RegisterAttached("InternalSpacing", typeof(double), typeof(InputBoxHelper), new PropertyMetadata(3D));

        public static double GetInternalSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(InternalSpacingProperty);
        }

        public static void SetInternalSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(InternalSpacingProperty, value);
        }

        /// <summary>
        /// 获取焦点时全选文本
        /// </summary>
        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(InputBoxHelper), new PropertyMetadata(false, OnSelectAllOnFocusChanged));

        public static void SetSelectAllOnFocus(UIElement element, bool value)
        {
            element.SetValue(SelectAllOnFocusProperty, value);
        }

        public static bool GetSelectAllOnFocus(UIElement element)
        {
            return (bool)element.GetValue(SelectAllOnFocusProperty);
        }

        private static void OnSelectAllOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.GotFocus += UIElement_GotFocus;
                    element.LostFocus += UIElement_LostFocus;
                    element.PreviewMouseDown += TextBox_PreviewMouseDown;
                }
                else
                {
                    element.GotFocus -= UIElement_GotFocus;
                    element.LostFocus -= UIElement_LostFocus;
                    element.PreviewMouseDown -= TextBox_PreviewMouseDown;
                }
            }
        }

        private static void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.Focus();

                if (e.OriginalSource is UIElement element && element.TryGetParentFromVisualTree<ButtonBase>() is null)
                {
                    e.Handled = true;
                }
            }
            else if (sender is PasswordBox passwordBox && !passwordBox.IsFocused)
            {
                passwordBox.Focus();

                if (e.OriginalSource is UIElement element && element.TryGetParentFromVisualTree<ButtonBase>() is null)
                {
                    e.Handled = true;
                }
            }
        }

        private static void UIElement_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
            else if (sender is PasswordBox passwordBox)
            {
                passwordBox.SelectAll();
            }
            else if (e.OriginalSource is FrameworkElement frameworkElement &&
                     frameworkElement.TryGetChildFromVisualTree<TextBox>(e => e is TextBox) is TextBox textBox2)
            {
                textBox2.Focus();
            }
        }

        private static void UIElement_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectionLength = 0;
            }
            else if (sender is PasswordBox passwordBox)
            {
            }
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
                        if (ComboBoxHelper.GetIsMultiSelect(comboBox))
                        {
                            var items = ComboBoxHelper.GetSelectedItems(comboBox);
                            if (items is { })
                            {
                                items.Clear();
                                comboBox.IsDropDownOpen = false;
                            }
                        }
                        else
                        {
                            if (comboBox.IsEditable)
                            {
                                comboBox.Text = null;
                            }

                            comboBox.SelectedItem = null;
                        }
                    }
                };


                if (control.IsLoaded)
                {
                    var clearButton = GetClearButton(control);
                    SetClickToClear(control, handle, clearButton);
                }
                else
                {
                    control.Loaded += (sender, arg) =>
                    {
                        var clearButton = GetClearButton(control);
                        SetClickToClear(control, handle, clearButton);
                    };
                }

                control.Unloaded += (sender, arg) =>
                {
                    Button clearButton = GetClearButton(control);
                    if (clearButton != null)
                    {
                        if (GetIsClearable(control))
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };
            }
        }

        private static Button GetClearButton(Control control) => control.Template.FindName("clearButton", control) as Button;

        private static void SetClickToClear(Control control, RoutedEventHandler handle, Button clearButton)
        {
            if (clearButton != null)
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
        }
    }
}