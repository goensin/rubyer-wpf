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

        // 提示占位符
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(""));

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
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
