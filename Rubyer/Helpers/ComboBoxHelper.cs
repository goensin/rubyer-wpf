using Rubyer.Commons.KnownBoxes;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public static void SetIsMultiSelect(DependencyObject element, bool value)
        {
            element.SetValue(IsMultiSelectProperty, BooleanBoxes.Box(value));
        }

        public static bool GetIsMultiSelect(DependencyObject element)
        {
            return (bool)element.GetValue(IsMultiSelectProperty);
        }

        /// <summary>
        /// 子项是否选中
        /// </summary>
        internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached(
            "IsSelected", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSelectedChanged));

        internal static void SetIsSelected(DependencyObject element, bool value)
        {
            element.SetValue(IsSelectedProperty, BooleanBoxes.Box(value));
        }

        internal static bool GetIsSelected(DependencyObject element)
        {
            return (bool)element.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// 多选时显示文本
        /// </summary>
        public static readonly DependencyProperty MultiSelectTextProperty = DependencyProperty.RegisterAttached(
            "MultiSelectText", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(null));

        public static void SetMultiSelectText(DependencyObject element, string value)
        {
            element.SetValue(MultiSelectTextProperty, value);
        }

        public static string GetMultiSelectText(DependencyObject element)
        {
            return (string)element.GetValue(MultiSelectTextProperty);
        }

        /// <summary>
        /// 多选时文本分隔符
        /// </summary>
        public static readonly DependencyProperty MultiSelectSeparatorProperty = DependencyProperty.RegisterAttached(
            "MultiSelectSeparator", typeof(string), typeof(ComboBoxHelper), new PropertyMetadata(", ", OnMultiSelectSeparatorChanged));

        public static void SetMultiSelectSeparator(DependencyObject element, string value)
        {
            element.SetValue(MultiSelectSeparatorProperty, value);
        }

        public static string GetMultiSelectSeparator(DependencyObject element)
        {
            return (string)element.GetValue(MultiSelectSeparatorProperty);
        }

        /// <summary>
        /// 选中项集合
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems", typeof(IList), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static void OnIsMultiSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;
            if ((bool)e.NewValue)
            {
                comboBox.DropDownOpened += ComboBox_DropDownOpened;
            }
            else
            {
                comboBox.DropDownOpened -= ComboBox_DropDownOpened;
            }
        }

        private static async void ComboBox_DropDownOpened(object sender, System.EventArgs e)
        {
            await Task.Delay(100);
            var comboBox = sender as ComboBox;
            var selectedItems = GetSelectedItems(comboBox);
            foreach (var item in comboBox.Items)
            {
                if (comboBox.ItemContainerGenerator.ContainerFromItem(item) is not ComboBoxItem comboBoxItem)
                {
                    continue;
                }

                if (selectedItems.Contains(item))
                {
                    SetIsSelected(comboBoxItem, true);
                }
                else
                {
                    SetIsSelected(comboBoxItem, false);
                }
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;

            void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            {
                UpdateMultiSelectText(comboBox);
            }

            if (e.NewValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnCollectionChanged;
            }

            UpdateMultiSelectText(comboBox);
        }

        private static void UpdateMultiSelectText(ComboBox comboBox)
        {
            var selectedItems = GetSelectedItems(comboBox);
            if (selectedItems is null)
            {
                SetMultiSelectText(comboBox, string.Empty);
                return;
            }

            var texts = new List<string>();
            foreach (var item in selectedItems)
            {
                var typeConverter = TypeDescriptor.GetConverter(item.GetType());
                var text = typeConverter.ConvertToString(item);
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

            var selectedItems = GetSelectedItems(comboBox);
            if (selectedItems == null)
            {
                selectedItems = new ObservableCollection<object>();
                SetSelectedItems(comboBox, selectedItems);
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

            // UpdateMultiSelectText(comboBox);
        }
    }
}
