using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    public static class ComboBoxHelper
    {
        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(ComboBoxHelper));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }


        // 聚焦时颜色
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(SolidColorBrush), typeof(ComboBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

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
            DependencyProperty.RegisterAttached("PreContent", typeof(object), typeof(ComboBoxHelper), new PropertyMetadata(null));

        public static object GetPreContent(DependencyObject obj)
        {
            return (object)obj.GetValue(PreContentProperty);
        }

        public static void SetPreContent(DependencyObject obj, object value)
        {
            obj.SetValue(PreContentProperty, value);
        }

        // 后置内容
        public static readonly DependencyProperty PostContentProperty =
            DependencyProperty.RegisterAttached("PostContent", typeof(object), typeof(ComboBoxHelper), new PropertyMetadata(null));

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
            DependencyProperty.RegisterAttached("IsClearable", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(false, OnIsClearbleChanged));

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
                    if (control is ComboBox comboBox)
                    {
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

        // 提示占位符
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(""));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        // 是否聚焦
        public static readonly DependencyProperty IsKeyBoardFocusedProperty =
            DependencyProperty.RegisterAttached("IsKeyBoardFocused", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(false));

        public static bool GetIsKeyBoardFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsKeyBoardFocusedProperty);
        }

        public static void SetIsKeyBoardFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsKeyBoardFocusedProperty, value);
        }
    }
}
