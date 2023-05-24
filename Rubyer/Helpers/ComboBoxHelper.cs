using Rubyer.Commons;
using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;

namespace Rubyer
{
    /// <summary>
    /// ComboBox 帮助类
    /// </summary>
    public class ComboBoxHelper
    {
        /// <summary>
        /// 是否多选
        /// </summary>
        public static readonly DependencyProperty IsMultiSelectProperty = DependencyProperty.RegisterAttached(
            "IsMultiSelect", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsMultiSelectChanged));

        public static void SetIsMultiSelect(DependencyObject element, bool value) => element.SetValue(IsMultiSelectProperty, BooleanBoxes.Box(value));

        public static bool GetIsMultiSelect(DependencyObject element) => (bool)element.GetValue(IsMultiSelectProperty);

        /// <summary>
        /// 子项是否选中
        /// </summary>
        internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached(
            "IsSelected", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSelectedChanged));

        internal static void SetIsSelected(DependencyObject element, bool value) => element.SetValue(IsSelectedProperty, BooleanBoxes.Box(value));

        internal static bool GetIsSelected(DependencyObject element) => (bool)element.GetValue(IsSelectedProperty);

        /// <summary>
        /// 多选时显示文本
        /// </summary>
        public static readonly DependencyProperty MultiSelectTextProperty = DependencyProperty.RegisterAttached(
            "MultiSelectText", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(null));

        public static void SetMultiSelectText(DependencyObject element, string value) => element.SetValue(MultiSelectTextProperty, value);

        public static string GetMultiSelectText(DependencyObject element) => (string)element.GetValue(MultiSelectTextProperty);

        /// <summary>
        /// 多选时文本分隔符
        /// </summary>
        public static readonly DependencyProperty MultiSelectSeparatorProperty = DependencyProperty.RegisterAttached(
            "MultiSelectSeparator", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(", ", OnMultiSelectSeparatorChanged));

        public static void SetMultiSelectSeparator(DependencyObject element, string value) => element.SetValue(MultiSelectSeparatorProperty, value);

        public static string GetMultiSelectSeparator(DependencyObject element) => (string)element.GetValue(MultiSelectSeparatorProperty);

        /// <summary>
        /// 内部选中项集合
        /// </summary>
        internal static readonly DependencyProperty InternalSelectedItemsProperty = DependencyProperty.RegisterAttached(
            "InternalSelectedItems", typeof(IList), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal static void SetInternalSelectedItems(DependencyObject element, IList value) => element.SetValue(InternalSelectedItemsProperty, value);

        internal static IList GetInternalSelectedItems(DependencyObject element) => (IList)element.GetValue(InternalSelectedItemsProperty);

        /// <summary>
        /// 选中项集合
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems", typeof(IList), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value) => element.SetValue(SelectedItemsProperty, value);

        public static IList GetSelectedItems(DependencyObject element) => (IList)element.GetValue(SelectedItemsProperty);

        private static void OnIsMultiSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox)
            {
                if (GetIsMultiSelect(comboBox))
                {
                    var internalItems = new ElementObservableCollection<object>(comboBox);
                    internalItems.CollectionChanged += Items_CollectionChanged;
                    SetInternalSelectedItems(comboBox, internalItems);
                }
                else
                {
                    var internalItems = GetInternalSelectedItems(comboBox) as INotifyCollectionChanged;
                    internalItems.CollectionChanged -= Items_CollectionChanged;
                    SetInternalSelectedItems(comboBox, null);
                }
            }
        }

        private static void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ElementObservableCollection<object>;
            var comboBox = collection.Owner as ComboBox;
            var selectedItems = GetSelectedItems(comboBox);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        selectedItems.Add(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
                        SetIsSelected(comboBoxItem, false);

                        selectedItems.Remove(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        var item = comboBox.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem;
                        SetIsSelected(item, false);

                        selectedItems.Remove(item);
                    }

                    break;
            }

            if (comboBox.IsLoaded)
            {
                UpdateMultiSelectText(comboBox);
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;
            if (GetInternalSelectedItems(comboBox) is ElementObservableCollection<object> internalItems)
            {
                internalItems.CollectionChanged -= Items_CollectionChanged;
                internalItems.Clear();
                if (e.NewValue is IList newSelectedItems)
                {
                    foreach (var item in newSelectedItems)
                    {
                        internalItems.Add(item);
                    }
                }
                internalItems.CollectionChanged += Items_CollectionChanged;
            }

          

            //void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            //{
            //    switch (args.Action)
            //    {
            //        case NotifyCollectionChangedAction.Add:
            //            break;

            //        case NotifyCollectionChangedAction.Remove:
            //            foreach (var item in args.OldItems)
            //            {
            //                var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
            //                SetIsSelected(comboBoxItem, false);
            //            }
            //            break;

            //        case NotifyCollectionChangedAction.Replace:
            //            break;

            //        case NotifyCollectionChangedAction.Move:
            //            break;

            //        case NotifyCollectionChangedAction.Reset:
            //            for (int i = 0; i < comboBox.Items.Count; i++)
            //            {
            //                var item = comboBox.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem;
            //                SetIsSelected(item, false);
            //            }

            //            break;
            //    }

            //    UpdateMultiSelectText(comboBox);
            //}

            //void UpdateItemSelected()
            //{
            //    if (e.OldValue is INotifyCollectionChanged oldCollection)
            //    {
            //        oldCollection.CollectionChanged -= OnCollectionChanged;
            //    }

            //    if (e.NewValue is INotifyCollectionChanged newCollection)
            //    {
            //        newCollection.CollectionChanged += OnCollectionChanged;

            //        var list = e.NewValue as IList;
            //        foreach (var item in comboBox.Items)
            //        {

            //            if (comboBox.ItemContainerGenerator.ContainerFromItem(item) is ComboBoxItem comboBoxItem)
            //            {
            //                if (list.Contains(item))
            //                {
            //                    SetIsSelected(comboBoxItem, true);
            //                }
            //                else
            //                {
            //                    SetIsSelected(comboBoxItem, false);
            //                }
            //            }
            //        }

            //        UpdateMultiSelectText(comboBox);
            //    }
            //}

            //if (comboBox.IsLoaded)
            //{
            //    UpdateItemSelected();
            //}
            //else
            //{
            //    comboBox.Loaded += (sender, args) =>
            //    {
            //        comboBox.IsDropDownOpen = true;
            //        comboBox.IsDropDownOpen = false;
            //        UpdateItemSelected();
            //    };
            //}
        }

        private static void UpdateMultiSelectText(ComboBox comboBox)
        {
            var selectedItems = GetSelectedItems(comboBox);
            var texts = new List<string>();
            foreach (var item in selectedItems)
            {
                var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
                var type = comboBoxItem.Content.GetType();
                var typeConverter = TypeDescriptor.GetConverter(type);
                var text = typeConverter.ConvertToString(comboBoxItem.Content);
                texts.Add(text);
            }

            var separator = GetMultiSelectSeparator(comboBox);
            SetMultiSelectText(comboBox, string.Join(separator, texts));
        }

        private static void OnMultiSelectSeparatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateMultiSelectText(d as ComboBox);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBoxItem = d as ComboBoxItem;
            var comboBox = ItemsControl.ItemsControlFromItemContainer(d) as ComboBox;
            var index = comboBox.ItemContainerGenerator.IndexFromContainer(comboBoxItem);
            var itemData = comboBox.Items[index];

            var selectedItems = GetInternalSelectedItems(comboBox);
            if (selectedItems == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                if (!selectedItems.Contains(itemData))
                {
                    selectedItems.Add(itemData); // 添加
                }
            }
            else
            {
                if (selectedItems.Contains(itemData))
                {
                    selectedItems.Remove(itemData); // 移除
                }
            }
        }
    }
}
