using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubyer
{
    public static class TabControlHelper
    {
        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
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
                        ScrollViewer scrollViewer = tabControl.Template.FindName("headerPanel", tabControl) as ScrollViewer;
                        if (tabControl.TabStripPlacement == Dock.Top || tabControl.TabStripPlacement == Dock.Bottom)
                        {
                            StartRowAnimation(tabControl, scrollViewer);
                        }
                        else
                        {
                            StartColAnimation(tabControl, scrollViewer);
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

        /// <summary>
        /// 选中动画
        /// </summary>
        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.RegisterAttached("IsAnimation", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(false, OnIsAnimationChanged));

        public static bool GetIsAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimationProperty);
        }

        public static void SetIsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimationProperty, value);
        }

        private static void OnIsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                tabControl.Loaded += (sender, arg) =>
                {
                    ScrollViewer scrollViewer = tabControl.Template.FindName("headerPanel", tabControl) as ScrollViewer;

                    if (tabControl.TabStripPlacement == Dock.Top || tabControl.TabStripPlacement == Dock.Bottom)
                    {
                        if (tabControl.Template.FindName("selectedRectRow", tabControl) is Rectangle rectangleRow)
                        {
                            if (GetIsAnimation(tabControl))
                            {
                                tabControl.SelectionChanged += (a, b) =>
                                {
                                    StartRowAnimation(tabControl, scrollViewer);
                                };

                                scrollViewer.ScrollChanged += (a, b) =>
                                {
                                    StartRowAnimation(tabControl, scrollViewer);
                                };

                                if (tabControl.SelectedIndex >= 0)
                                {
                                    int index = tabControl.SelectedIndex;
                                    TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
                                    var bounds = item.TransformToAncestor(scrollViewer).TransformBounds(new Rect(new Point(0, 0), item.RenderSize));
                                    rectangleRow.Width = item.ActualWidth;
                                    rectangleRow.SetValue(Canvas.LeftProperty, bounds.X);
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
                                    StartColAnimation(tabControl, scrollViewer);
                                };

                                scrollViewer.ScrollChanged += (a, b) =>
                                {
                                    StartColAnimation(tabControl, scrollViewer);
                                };

                                if (tabControl.SelectedIndex >= 0)
                                {
                                    int index = tabControl.SelectedIndex;
                                    TabItem item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
                                    var bounds = item.TransformToAncestor(scrollViewer).TransformBounds(new Rect(new Point(0, 0), item.RenderSize));
                                    rectangleCol.Height = item.ActualHeight;
                                    rectangleCol.SetValue(Canvas.TopProperty, bounds.Y);
                                }
                            }
                        }
                    }
                };
            }
        }

        private static TabItem GetItem(TabControl tabControl, int index, IEditableCollectionView items)
        {
            TabItem item;
            if (items.CanRemove)
            {
                item = tabControl.ItemContainerGenerator.ContainerFromItem(tabControl.Items[index]) as TabItem;
            }
            else
            {
                item = tabControl.Items[index] as TabItem;
            }

            return item;
        }

        private static void StartColAnimation(TabControl tabControl, ScrollViewer scrollViewer)
        {
            int index = tabControl.SelectedIndex;
            if (tabControl.Items.Count == 0 || index < 0 || index >= tabControl.Items.Count)
            {
                return;
            }

            Rectangle rectangleCol = tabControl.Template.FindName("selectedRectCol", tabControl) as Rectangle;
            IEditableCollectionView items = tabControl.Items;
            TabItem item = GetItem(tabControl, index, items);
            rectangleCol.Height = item.ActualHeight;
            var bounds = item.TransformToAncestor(scrollViewer).TransformBounds(new Rect(new Point(0, 0), item.RenderSize));

            DoubleAnimation canvasAnimation = new DoubleAnimation
            {
                To = bounds.Y,
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

        private static void StartRowAnimation(TabControl tabControl, ScrollViewer scrollViewer)
        {
            int index = tabControl.SelectedIndex;
            if (tabControl.Items.Count == 0 || index < 0 || index >= tabControl.Items.Count)
            {
                return;
            }

            Rectangle rectangleRow = tabControl.Template.FindName("selectedRectRow", tabControl) as Rectangle;
            IEditableCollectionView items = tabControl.Items;
            TabItem item = GetItem(tabControl, index, items);
            rectangleRow.Width = item.ActualWidth;

            var bounds = item.TransformToAncestor(scrollViewer).TransformBounds(new Rect(new Point(0, 0), item.RenderSize));

            DoubleAnimation canvasAnimation = new DoubleAnimation
            {
                To = bounds.X,
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
