using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    public static class TextBoxHelper
    {
        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(TextBoxHelper));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        // 聚焦时前景色
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(SolidColorBrush), typeof(TextBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedBrushProperty);
        }

        public static void SetFocusedBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedBrushProperty, value);
        }


        // 前置内容
        public static readonly DependencyProperty PreContentProperty =
            DependencyProperty.RegisterAttached("PreContent", typeof(object), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static object GetPreContent(DependencyObject obj)
        {
            return (object)obj.GetValue(PreContentProperty);
        }

        public static void SetPreContent(DependencyObject obj, object value)
        {
            obj.SetValue(PreContentProperty, value);
        }

        // 前置内容
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

        // 是否显示清除按钮
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

        // 提示占位符
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBoxHelper), new PropertyMetadata(""));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
    }
}
