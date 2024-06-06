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
        /// 多选项间隔大小
        /// </summary>
        public static readonly DependencyProperty MultiSelectSpacingProperty = DependencyProperty.RegisterAttached(
            "MultiSelectSpacing", typeof(double), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(5D, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetMultiSelectSpacing(DependencyObject element, double value)
        {
            element.SetValue(MultiSelectSpacingProperty,value);
        }

        public static double GetMultiSelectSpacing(DependencyObject element)
        {
            return (double)element.GetValue(MultiSelectSpacingProperty);
        }

        /// <summary>
        /// 选中项集合
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems", typeof(IList), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
        }
    }
}
