using Microsoft.VisualBasic;
using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rubyer
{
    /// <summary>
    /// 树形数据表格
    /// </summary>
    public class TreeDataGrid : DataGrid
    {
        private bool isPreparing = false;

        static TreeDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeDataGrid), new FrameworkPropertyMetadata(typeof(TreeDataGrid)));
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal new IEnumerable ItemsSource
        {
            get
            {
                return Items.SourceCollection;
            }
            set
            {
                if (value == null)
                {
                    ClearValue(ItemsSourceProperty);
                }
                else
                {
                    SetValue(ItemsSourceProperty, value);
                }
            }
        }

        /// <summary>
        /// 树形数据源
        /// </summary>
        public static readonly DependencyProperty HierarchicalItemsSourceProperty =
            DependencyProperty.Register("HierarchicalItemsSource", typeof(IEnumerable), typeof(TreeDataGrid), new PropertyMetadata(default(IEnumerable), OnHierarchicalItemsSourceChanged));

        private static void OnHierarchicalItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeDataGrid = (TreeDataGrid)d;
            if (treeDataGrid.HierarchicalItemsSource is not null && treeDataGrid.ChildrenPath is not null)
            {
                treeDataGrid.AddNotifyCollectionChanged(treeDataGrid.HierarchicalItemsSource);
                treeDataGrid.ItemsSource = new ObservableCollection<object>(treeDataGrid.HierarchicalItemsSource as IEnumerable<object>);
            }
            else
            {
                treeDataGrid.ItemsSource = null;
            }
        }

        /// <summary>
        /// 树形数据源
        /// </summary>
        public IEnumerable HierarchicalItemsSource
        {
            get { return (IEnumerable)GetValue(HierarchicalItemsSourceProperty); }
            set { SetValue(HierarchicalItemsSourceProperty, value); }
        }

        /// <summary>
        /// 子项路径
        /// </summary>
        public static readonly DependencyProperty ChildrenPathProperty =
            DependencyProperty.Register("ChildrenPath", typeof(string), typeof(TreeDataGrid), new PropertyMetadata(default(string)));

        /// <summary>
        /// 子项路径
        /// </summary>
        public string ChildrenPath
        {
            get { return (string)GetValue(ChildrenPathProperty); }
            set { SetValue(ChildrenPathProperty, value); }
        }

        /// <summary>
        /// 展开图标
        /// </summary>
        public static readonly DependencyProperty ExpanderIconTypeProperty =
            DependencyProperty.Register("ExpanderIconType", typeof(IconType), typeof(TreeDataGrid), new PropertyMetadata(default(IconType)));

        /// <summary>
        /// 展开图标
        /// </summary>
        public IconType ExpanderIconType
        {
            get { return (IconType)GetValue(ExpanderIconTypeProperty); }
            set { SetValue(ExpanderIconTypeProperty, value); }
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private object FindParent(IEnumerable<object> enumerable)
        {
            var collection = ItemsSource as ObservableCollection<object>;
            foreach (var item in collection)
            {
                var items = GetItemsFromPath(item);
                if (items.Equals(enumerable))
                {
                    return item;
                }
            }

            return null;
        }

        private void HierarchicalItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsSource is not ObservableCollection<object> collection)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var parent = FindParent(sender as IEnumerable<object>);

                    var isExpanded = GetIsExpanded(parent);
                    if (isExpanded)
                    {
                        RemoveChildren(parent, collection); // 清除后再加载

                        InsertChildren(parent, collection);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (Items.Contains(item))
                        {
                            RemoveChildren(item, collection);
                            collection.Remove(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems.Count > 0 && e.NewItems.Count > 0)
                    {
                        var oldItem = e.OldItems[0];
                        var index = collection.IndexOf(oldItem);
                        RemoveChildren(oldItem, collection);
                        collection[index] = e.NewItems[0];
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    collection.Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    collection.Clear();
                    break;
            }
        }

        private void AddNotifyCollectionChanged(IEnumerable collection)
        {
            if (collection is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += HierarchicalItemsSource_CollectionChanged;
                foreach (var item in collection)
                {
                    var items = GetItemsFromPath(item);
                    AddNotifyCollectionChanged(items);
                }
            }
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeDataGridRow();
        }

        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            isPreparing = true;

            var row = (TreeDataGridRow)element;

            var parentRow = GetParentRowFromData(HierarchicalItemsSource, item); // 获取父级行
            row.ParentRow = parentRow;

            int level = -1;
            row.HasChildRow = GetHasChildRow(HierarchicalItemsSource, item, ref level).GetValueOrDefault();  // 是否有子行 
            row.NodeLevel = level; // 所处节点级数

            row.IsExpanded = GetIsExpanded(item);

            isPreparing = false;
        }

        /// <summary>
        /// 递归获取行父级
        /// </summary>
        /// <param name="items">树形数据集合</param>
        /// <param name="child">子项数据</param>
        /// <param name="parent">父级数据</param>
        /// <returns>行父级</returns>
        private TreeDataGridRow GetParentRowFromData(IEnumerable items, object child, object parent = null)
        {
            if (items is null)
            {
                return null;
            }

            foreach (var item in items)
            {
                if (item == child)
                {
                    if (parent is { })
                    {
                        return ItemContainerGenerator.ContainerFromItem(parent) as TreeDataGridRow;
                    }
                    else
                    {
                        return null;
                    }
                }

                var parentRow = GetParentRowFromData(GetItemsFromPath(item), child, item);
                if (parentRow is { })
                {
                    return parentRow;
                }
            }

            return null;
        }

        /// <summary>
        /// 递归判断是否有子行
        /// </summary>
        /// <param name="collection">树形数据集合</param>
        /// <param name="child">子项数据</param>
        /// <param name="level">节点等级</param>
        /// <returns>是否有子行</returns>
        private bool? GetHasChildRow(IEnumerable collection, object child, ref int level)
        {
            level++;
            foreach (var item in collection)
            {
                if (item == child)
                {
                    var children = GetItemsFromPath(child);
                    return children.Any();
                }
                else
                {
                    var childCollection = GetItemsFromPath(item);
                    if (childCollection.Any())
                    {
                        var result = GetHasChildRow(childCollection, child, ref level);
                        if (result.HasValue)
                        {
                            return result.GetValueOrDefault();
                        }
                    }
                }
            }

            level--;
            return null;
        }

        /// <summary>
        /// 获取行是否展开
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns>是否展开</returns>
        private bool GetIsExpanded(object model)
        {
            var collection = ItemsSource as ObservableCollection<object>;
            var firstItem = GetItemsFromPath(model).FirstOrDefault();
            return collection is { } && firstItem is { } && collection.Contains(firstItem);
        }

        /// <inheritdoc/>
        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            var row = e.Row as TreeDataGridRow;
            row.Expanded += Row_Expanded;
            row.Collapsed += Row_Collapsed;
        }

        /// <inheritdoc/>
        protected override void OnUnloadingRow(DataGridRowEventArgs e)
        {
            base.OnUnloadingRow(e);

            var row = e.Row as TreeDataGridRow;
            row.Expanded -= Row_Expanded;
            row.Collapsed -= Row_Collapsed;
        }

        /// <summary>
        /// 根据路径获取子项
        /// </summary>
        /// <param name="model">数据对象</param>
        /// <returns>子项集合</returns>
        private IEnumerable<object> GetItemsFromPath(object model)
        {
            var itemType = model.GetType();
            var propertyInfo = itemType.GetProperty(ChildrenPath);
            if (propertyInfo is null)
            {
                return Enumerable.Empty<object>();
            }

            var value = propertyInfo.GetValue(model);
            return value is IEnumerable<object> children ? children : Enumerable.Empty<object>();
        }

        private void InsertChildren(object model, ObservableCollection<object> collection)
        {
            if (model is { } && collection is { })
            {
                int index = -1;
                bool finded = false;
                foreach (var item in collection)
                {
                    index++;
                    if (item == model)
                    {
                        finded = true;
                        break;
                    }
                }

                if (finded)
                {
                    var items = GetItemsFromPath(model);
                    foreach (var newItem in items)
                    {
                        // .net6 以下版本 DataGrid 快速插入子项可能会出现异常报错: https://github.com/dotnet/wpf/issues/2854
#if !NET6_0_OR_GREATER
                        try
                        {
#endif
                            collection.Insert(++index, newItem);
#if !NET6_0_OR_GREATER
                        }
                        catch (System.Exception ex)
                        {
                            Debug.WriteLine(ex, "TreeDataGrid 插入数据异常");
                        }
#endif
                    }
                }
            }
        }


        private void Row_Expanded(object sender, RoutedEventArgs e)
        {
            if (isPreparing)
            {
                return;
            }

            var row = (TreeDataGridRow)sender;
            var model = row.DataContext;
            var collection = ItemsSource as ObservableCollection<object>;

            RemoveChildren(model, collection); // 清除后再加载

            InsertChildren(model, collection);
        }

        /// <summary>
        /// 递归移除所有子项
        /// </summary>
        /// <param name="model">数据对象</param>
        /// <param name="collection">数据集合</param>
        private void RemoveChildren(object model, ObservableCollection<object> collection)
        {
            var items = GetItemsFromPath(model);
            foreach (var item in items)
            {
                collection.Remove(item);
                RemoveChildren(item, collection);
            }
        }

        private void Row_Collapsed(object sender, RoutedEventArgs e)
        {
            if (isPreparing)
            {
                return;
            }

            var row = (TreeDataGridRow)sender;
            var model = row.DataContext;
            var collection = ItemsSource as ObservableCollection<object>;
            RemoveChildren(model, collection);
        }
    }
}
