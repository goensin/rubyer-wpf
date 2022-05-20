using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubyer
{
    public static class TabControlHelper
    {
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
                    TabControl tabControl = FindTabControl(tabItem);
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

                if (tabItem.IsLoaded)
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
                }

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

        private static TabControl FindTabControl(DependencyObject dependencyObject)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent != null && !(parent is TabControl))
            {
                return FindTabControl(parent);
            }

            return parent != null ? (TabControl)parent : null;
        }
    }
}
