using Rubyer.Commons.KnownBoxes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// TabControl 帮助类
    /// </summary>
    public static class TabControlHelper
    {
        #region 事件

        /// <summary>
        /// 关闭 TabItem 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CloseTabItemRoutedEventHandler(object sender, CloseTabItemRoutedEventArgs e);

        /// <summary>
        /// 关闭 TabItem 事件
        /// </summary>
        public static readonly RoutedEvent CloseItemEvent = EventManager.RegisterRoutedEvent(
            "CloseItem", RoutingStrategy.Bubble, typeof(CloseTabItemRoutedEventHandler), typeof(TabControlHelper));

        public static void AddCloseItemHandler(DependencyObject dependencyObject, CloseTabItemRoutedEventHandler handler)
        {
            if (dependencyObject is TabControl tabControl)
            {
                tabControl.AddHandler(CloseItemEvent, handler);
            }
        }

        public static void RemoveCloseItemHandler(DependencyObject dependencyObject, CloseTabItemRoutedEventHandler handler)
        {
            if (dependencyObject is TabControl tabControl)
            {
                tabControl.RemoveHandler(CloseItemEvent, handler);
            }
        }

        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
            "AddButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControlHelper));

        public static void AddAddButtonClickHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is TabControl tabControl)
            {
                tabControl.AddHandler(AddButtonClickEvent, handler);
            }
        }

        public static void RemoveAddButtonClickHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is TabControl tabControl)
            {
                tabControl.RemoveHandler(AddButtonClickEvent, handler);
            }
        }

        #endregion

        #region 命令

        /// <summary>
        /// 添加命令
        /// </summary>
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.RegisterAttached("AddCommand", typeof(ICommand), typeof(TabControlHelper), new PropertyMetadata(null));

        public static ICommand GetAddCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AddCommandProperty);
        }

        public static void SetAddCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AddCommandProperty, value);
        }

        #endregion

        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.RegisterAttached("IsClearable", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsClearbleChanged));

        public static bool GetIsClearable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsClearableProperty);
        }

        public static void SetIsClearable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearableProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 是否显示添加按钮
        /// </summary>
        public static readonly DependencyProperty IsShowAddButtonProperty =
            DependencyProperty.RegisterAttached("IsShowAddButton", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsShowAddButtonChanged));

        /// <summary>
        /// Gets the is clearable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsShowAddButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowAddButtonProperty);
        }

        /// <summary>
        /// Sets the is clearable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsShowAddButton(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowAddButtonProperty, BooleanBoxes.Box(value));
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

        private static void OnIsClearbleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem tabItem)
            {
                void OnCloseButtonClicked(object sender, RoutedEventArgs args)
                {
                    TabControl tabControl = FindTabControl(tabItem);
                    IEditableCollectionView items = tabControl.Items;

                    if (items.CanRemove)
                    {
                        var eventArgs = new CloseTabItemRoutedEventArgs(tabItem.DataContext, CloseItemEvent, tabItem);
                        tabControl.RaiseEvent(eventArgs); // 触发移除事件
                        if (!eventArgs.Cancel)
                        {
                            items.Remove(tabItem.DataContext); // Binding 移除方式
                        }
                    }
                    else
                    {
                        var eventArgs = new CloseTabItemRoutedEventArgs(tabItem, CloseItemEvent, tabItem);
                        tabControl.RaiseEvent(eventArgs); // 触发移除事件
                        if (!eventArgs.Cancel)
                        {
                            tabControl.Items.Remove(tabItem); // TabItem 移除方式
                        }
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
                }

                if (tabItem.IsLoaded)
                {
                    if (tabItem.Template.FindName("clearButton", tabItem) is Button clearButton)
                    {
                        if (GetIsClearable(tabItem))
                        {
                            clearButton.Click += OnCloseButtonClicked;
                        }
                        else
                        {
                            clearButton.Click -= OnCloseButtonClicked;
                        }
                    }
                }

                tabItem.Loaded += (sender, arg) =>
                {
                    if (tabItem.Template.FindName("clearButton", tabItem) is Button clearButton)
                    {
                        if (GetIsClearable(tabItem))
                        {
                            clearButton.Click += OnCloseButtonClicked;
                        }
                        else
                        {
                            clearButton.Click -= OnCloseButtonClicked;
                        }
                    }
                };

                tabItem.Unloaded += (sender, arg) =>
                {
                    if (tabItem.Template.FindName("clearButton", tabItem) is Button clearButton)
                    {
                        if (GetIsClearable(tabItem))
                        {
                            clearButton.Click -= OnCloseButtonClicked;
                        }
                    }
                };
            }
        }


        private static void OnIsShowAddButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                void OnAddButtonClicked(object sender, RoutedEventArgs args)
                {
                    var eventArgs = new RoutedEventArgs(AddButtonClickEvent, tabControl);
                    tabControl.RaiseEvent(eventArgs);

                    var command = GetAddCommand(tabControl);
                    command?.Execute(null);
                }

                if (tabControl.IsLoaded)
                {
                    if (tabControl.Template.FindName("PART_AddButton", tabControl) is Button addButton)
                    {
                        if (GetIsShowAddButton(tabControl))
                        {
                            addButton.Click += OnAddButtonClicked;
                        }
                        else
                        {
                            addButton.Click -= OnAddButtonClicked;
                        }
                    }
                }

                tabControl.Loaded += (sender, arg) =>
                {
                    if (tabControl.Template.FindName("PART_AddButton", tabControl) is Button addButton)
                    {
                        if (GetIsShowAddButton(tabControl))
                        {
                            addButton.Click += OnAddButtonClicked;
                        }
                        else
                        {
                            addButton.Click -= OnAddButtonClicked;
                        }
                    }
                };

                tabControl.Unloaded += (sender, arg) =>
                {
                    if (tabControl.Template.FindName("PART_AddButton", tabControl) is Button addButton)
                    {
                        if (GetIsShowAddButton(tabControl))
                        {
                            addButton.Click -= OnAddButtonClicked;
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 选中动画
        /// </summary>
        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.RegisterAttached("IsAnimation", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsAnimationChanged));

        /// <summary>
        /// Gets the is animation.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimationProperty);
        }

        /// <summary>
        /// Sets the is animation.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimationProperty, BooleanBoxes.Box(value));
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
                                    StartRowAnimation(tabControl, scrollViewer, durationMilliseconds: 100);
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
                                    StartColAnimation(tabControl, scrollViewer, durationMilliseconds: 0);
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

        private static void StartColAnimation(TabControl tabControl, ScrollViewer scrollViewer, int durationMilliseconds = 400)
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
                Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation sizeAnimation = new DoubleAnimation
            {
                To = item.ActualHeight,
                Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            rectangleCol.BeginAnimation(Canvas.TopProperty, canvasAnimation);
            rectangleCol.BeginAnimation(FrameworkElement.HeightProperty, sizeAnimation);
        }

        private static void StartRowAnimation(TabControl tabControl, ScrollViewer scrollViewer, int durationMilliseconds = 400)
        {
            int index = tabControl.SelectedIndex;
            if (tabControl.Items.Count == 0 || index < 0 || index >= tabControl.Items.Count)
            {
                return;
            }

            if (tabControl.Template.FindName("selectedRectRow", tabControl) is Rectangle rectangleRow)
            {
                IEditableCollectionView items = tabControl.Items;
                TabItem item = GetItem(tabControl, index, items);
                rectangleRow.Width = item.ActualWidth;

                var bounds = item.TransformToAncestor(scrollViewer).TransformBounds(new Rect(new Point(0, 0), item.RenderSize));

                DoubleAnimation canvasAnimation = new DoubleAnimation
                {
                    To = bounds.X,
                    Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                DoubleAnimation sizeAnimation = new DoubleAnimation
                {
                    To = item.ActualWidth,
                    Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                rectangleRow.BeginAnimation(Canvas.LeftProperty, canvasAnimation);
                rectangleRow.BeginAnimation(FrameworkElement.WidthProperty, sizeAnimation);
            }
        }
    }

    /// <summary>
    /// 关闭 tab 子项事件
    /// </summary>
    public class CloseTabItemRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 是否取消关闭
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 子项数据
        /// </summary>
        public object Item { get; }

        public CloseTabItemRoutedEventArgs(object item)
            : base()
        {
            Item = item;
        }


        public CloseTabItemRoutedEventArgs(object item, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Item = item;
        }


        public CloseTabItemRoutedEventArgs(object item, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            Item = item;
        }
    }
}