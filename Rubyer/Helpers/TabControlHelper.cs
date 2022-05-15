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

                    if (GetIsAnimation(tabControl))
                    {
                        if (tabControl.TabStripPlacement == Dock.Top || tabControl.TabStripPlacement == Dock.Bottom)
                        {
                            StartRowAnimation(tabControl);
                        }
                        else
                        {
                            StartColAnimation(tabControl);
                        }
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

        // 选中动画
        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.RegisterAttached("IsAnimation", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(false, OnIsAnimationChanged));

        private static void OnIsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                tabControl.Loaded += (sender, arg) =>
                {
                    if (tabControl.TabStripPlacement == Dock.Top || tabControl.TabStripPlacement == Dock.Bottom)
                    {
                        if (tabControl.Template.FindName("selectedRectRow", tabControl) is Rectangle rectangleRow)
                        {
                            if (GetIsAnimation(tabControl))
                            {
                                tabControl.SelectionChanged += (a, b) =>
                                {
                                    StartRowAnimation(tabControl);
                                };

                                if (tabControl.SelectedIndex >= 0)
                                {
                                    int index = tabControl.SelectedIndex;
                                    double left = GetListBoxItemLeft(tabControl, index);
                                    TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
                                    rectangleRow.Width = item.ActualWidth;
                                    rectangleRow.SetValue(Canvas.LeftProperty, left);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tabControl.Template.FindName("selectedRectCol", tabControl) is Rectangle rectangleCol)
                        {
                            if (GetIsAnimation(tabControl))
                            {
                                tabControl.SelectionChanged += (a, b) =>
                                {
                                    StartColAnimation(tabControl);
                                };

                                if (tabControl.SelectedIndex >= 0)
                                {
                                    int index = tabControl.SelectedIndex;
                                    double top = GetListBoxItemTop(tabControl, index);
                                    TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
                                    rectangleCol.Height = item.ActualHeight;
                                    rectangleCol.SetValue(Canvas.TopProperty, top);
                                }
                            }
                        }
                    }
                };
            }
        }

        private static double GetListBoxItemTop(TabControl tabControl, int index)
        {
            double top = 0;

            for (int i = 0; i < index; i++)
            {
                TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[i]) as TabItem;
                top += item.ActualHeight;
            }

            return top;
        }

        private static double GetListBoxItemLeft(TabControl tabControl, int index)
        {
            double left = 0;

            for (int i = 0; i < index; i++)
            {
                TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[i]) as TabItem;
                left += item.ActualWidth;
            }

            return left;
        }

        public static bool GetIsAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimationProperty);
        }

        public static void SetIsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimationProperty, value);
        }

        private static void StartColAnimation(TabControl tabControl)
        {
            Rectangle rectangleCol = tabControl.Template.FindName("selectedRectCol", tabControl) as Rectangle;
            int index = tabControl.SelectedIndex;

            if (tabControl.Items.Count == 0 || index < 0 || index >= tabControl.Items.Count)
            {
                return;
            }

            double top = GetListBoxItemTop(tabControl, index);
            IEditableCollectionView items = tabControl.Items;
            TabItem item;
            if (items.CanRemove)
            {
                item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
            }
            else
            {
                item = tabControl.Items[index] as TabItem;
            }

            rectangleCol.Height = item.ActualHeight;

            DoubleAnimation canvasAnimation = new DoubleAnimation
            {
                To = top,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation sizeAnimation = new DoubleAnimation
            {
                To = item.ActualHeight,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            rectangleCol.BeginAnimation(Canvas.TopProperty, canvasAnimation);
            rectangleCol.BeginAnimation(FrameworkElement.HeightProperty, sizeAnimation);
        }

        private static void StartRowAnimation(TabControl tabControl)
        {
            Rectangle rectangleRow = tabControl.Template.FindName("selectedRectRow", tabControl) as Rectangle;
            int index = tabControl.SelectedIndex;

            if (tabControl.Items.Count == 0 || index < 0 || index >= tabControl.Items.Count)
            {
                return;
            }

            double left = GetListBoxItemLeft(tabControl, index);
            IEditableCollectionView items = tabControl.Items;
            TabItem item;
            if (items.CanRemove)
            {
                item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
            }
            else
            {
                item = tabControl.Items[index] as TabItem;
            }

            rectangleRow.Width = item.ActualWidth;

            DoubleAnimation canvasAnimation = new DoubleAnimation
            {
                To = left,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation sizeAnimation = new DoubleAnimation
            {
                To = item.ActualWidth,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            rectangleRow.BeginAnimation(Canvas.LeftProperty, canvasAnimation);
            rectangleRow.BeginAnimation(FrameworkElement.WidthProperty, sizeAnimation);
        }
    }
}
