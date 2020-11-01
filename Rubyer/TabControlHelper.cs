using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    public static class TabControlHelper
    {
        // 聚焦时颜色
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(SolidColorBrush), typeof(TabControlHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedBrushProperty);
        }

        public static void SetFocusedBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedBrushProperty, value);
        }

        // 聚焦前景色时颜色
        public static readonly DependencyProperty FocusedForegroundBrushProperty =
            DependencyProperty.RegisterAttached("FocusedForegroundBrush", typeof(SolidColorBrush), typeof(TabControlHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedForegroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedForegroundBrushProperty);
        }

        public static void SetFocusedForegroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedForegroundBrushProperty, value);
        }

        // 是否显示清除按钮
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.RegisterAttached("IsClearable", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(false, OnIsClearbleChanged));

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
            if (d is TabItem tabItem)
            {
                RoutedEventHandler handle = (sender, args) =>
                {
                    var panel = VisualTreeHelper.GetParent(tabItem);
                    var grid = VisualTreeHelper.GetParent(panel);
                    var tabControl = VisualTreeHelper.GetParent(grid) as TabControl;

                    IEditableCollectionView items = tabControl.Items;

                    if (items.CanRemove)
                    {
                        items.Remove(tabItem.DataContext);      // Binding 移除方式
                    }
                    else
                    {
                        tabControl.Items.Remove(tabItem);       // TabItem 移除方式
                    }
                };

                tabItem.Loaded += (sender, arg) =>
                {
                    if (tabItem.Template.FindName("clearButton", tabItem) is Button clearButton)
                    {
                        if (GetIsClearable(tabItem))
                        {
                            clearButton.Click += handle;
                        }
                        else
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };

                tabItem.Unloaded += (sender, arg) =>
                {
                    if (tabItem.Template.FindName("clearButton", tabItem) is Button clearButton)
                    {
                        if (GetIsClearable(tabItem))
                        {
                            clearButton.Click -= handle;
                        }
                    }
                };
            }
        }
    }
}
