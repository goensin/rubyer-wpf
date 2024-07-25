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

        /// <summary>
        /// 移除命令
        /// </summary>
        public static readonly DependencyProperty RemoveCommandProperty =
            DependencyProperty.RegisterAttached("RemoveCommand", typeof(ICommand), typeof(TabControlHelper), new PropertyMetadata(null));

        public static ICommand GetRemoveCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(RemoveCommandProperty);
        }

        public static void SetRemoveCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(RemoveCommandProperty, value);
        }

        #endregion

        /// <summary>
        /// 未选中 item 时显示视图
        /// </summary>
        public static readonly DependencyProperty EmptyViewProperty = DependencyProperty.RegisterAttached(
            "EmptyView", typeof(object), typeof(TabControlHelper), new PropertyMetadata(null));

        public static void SetEmptyView(DependencyObject element, object value)
        {
            element.SetValue(EmptyViewProperty, value);
        }

        public static object GetEmptyView(DependencyObject element)
        {
            return (object)element.GetValue(EmptyViewProperty);
        }

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

                    var removeCommand = GetRemoveCommand(tabControl);
                    if (removeCommand is { }) // 如果绑定 RemoveCommand，不执行自动移除
                    {
                        removeCommand.Execute(tabItem.DataContext);
                        return;
                    }

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