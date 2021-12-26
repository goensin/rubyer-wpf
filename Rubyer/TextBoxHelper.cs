using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    public static class TextBoxHelper
    {
        /// <summary>
        /// 前置内容
        /// </summary>
        public static readonly DependencyProperty PreContentProperty =
            DependencyProperty.RegisterAttached("PreContent", typeof(object), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static object GetPreContent(DependencyObject obj)
        {
            return obj.GetValue(PreContentProperty);
        }

        public static void SetPreContent(DependencyObject obj, object value)
        {
            obj.SetValue(PreContentProperty, value);
        }

        /// <summary>
        /// 前置内容
        /// </summary>
        public static readonly DependencyProperty PostContentProperty =
            DependencyProperty.RegisterAttached("PostContent", typeof(object), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static object GetPostContent(DependencyObject obj)
        {
            return (object)obj.GetValue(PostContentProperty);
        }

        public static void SetPostContent(DependencyObject obj, object value)
        {
            obj.SetValue(PostContentProperty, value);
        }

        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.RegisterAttached("IsClearable", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnIsClearbleChanged));

        public static bool GetIsClearable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsClearableProperty);
        }

        public static void SetIsClearable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearableProperty, value);
        }

        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(TextBoxHelper), new PropertyMetadata(default(string)));
        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(DependencyObject element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }

        private static void OnIsClearbleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                RoutedEventHandler handle = (sender, args) =>
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Text = "";
                    }
                    else if (control is PasswordBox passwordBox)
                    {
                        passwordBox.Password = "";
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
