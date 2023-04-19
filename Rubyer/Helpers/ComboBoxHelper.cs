using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            "IsMultiSelect", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

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
            "SelectedItems", typeof(IList), typeof(ComboBoxHelper), new PropertyMetadata(default(IList)));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static void OnMultiSelectSeparatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateSelectedContent(d as ComboBox);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = ItemsControl.ItemsControlFromItemContainer(d) as ComboBox;
            UpdateSelectedContent(comboBox);
        }

        private static void UpdateSelectedContent(ComboBox comboBox)
        {
            var texts = new List<string>(); // 存放选中项文本
            var items = new List<object>(); // 存放选中项数据
            for (int i = 0; i < comboBox.Items.Count; i++)
            {

                var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem;
                var itemData = comboBox.Items[i];
                if (GetIsSelected(comboBoxItem))
                {
                    var type = comboBoxItem.Content.GetType();
                    var typeConverter = TypeDescriptor.GetConverter(type);
                    var text = typeConverter.ConvertToString(comboBoxItem.Content);
                    texts.Add(text);
                    items.Add(itemData);
                }
            }

            var separator = GetMultiSelectSeparator(comboBox);
            SetMultiSelectText(comboBox, string.Join(separator, texts));
            SetSelectedItems(comboBox, items);
        }
    }
}
