using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Controls.Primitives;

namespace Rubyer
{
    /// <summary>
    /// items 控件助手
    /// </summary>
    public class ItemsControlHelper
    {
        /// <summary>
        /// item margin
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.RegisterAttached(
            "ItemMargin", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// Gets the item margin.
        /// </summary>
        public static Thickness GetItemMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ItemMarginProperty);
        }

        /// <summary>
        /// Sets the item margin.
        /// </summary>
        public static void SetItemMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// item padding
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.RegisterAttached(
            "ItemPadding", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// Gets the item padding.
        /// </summary>
        public static Thickness GetItemPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ItemPaddingProperty);
        }

        /// <summary>
        /// Sets the item padding.
        /// </summary>
        public static void SetItemPadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(ItemPaddingProperty, value);
        }

        /// <summary>
        /// 绑定 enum 类型所有值给 ItemsSource 赋值
        /// 必须绑定 SelectedItem
        /// </summary>
        public static readonly DependencyProperty EnumValuesToItemsSourceProperty = DependencyProperty.RegisterAttached(
            "EnumValuesToItemsSource", typeof(bool), typeof(ItemsControlHelper), new PropertyMetadata(default(bool), OnEnumValuesToItemsSourceChanged));

        /// <summary>
        /// set 绑定 enum 类型所有值给 ItemsSource 赋值
        /// </summary>
        public static void SetEnumValuesToItemsSource(DependencyObject element, bool value)
        {
            element.SetValue(EnumValuesToItemsSourceProperty, value);
        }

        /// <summary>
        /// get 绑定 enum 类型所有值给 ItemsSource 赋值
        /// </summary>
        public static bool GetEnumValuesToItemsSource(DependencyObject element)
        {
            return (bool)element.GetValue(EnumValuesToItemsSourceProperty);
        }

        private static void OnEnumValuesToItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsControl itemsControl && GetEnumValuesToItemsSource(itemsControl))
            {
                if (itemsControl.IsLoaded)
                {
                    SetItemsSource(itemsControl);
                }
                else
                {
                    itemsControl.Loaded += ItemsControl_Loaded;
                }
            }
        }

        private static void SetItemsSource(ItemsControl itemsControl)
        {
            var itemsBindingExpression = BindingOperations.GetBinding(itemsControl, ItemsControl.ItemsSourceProperty);
            if (itemsBindingExpression != null)
            {
                throw new InvalidOperationException("When using ItemsControlHelper.EnumValuesToItemsSource, cannot be used ItemsSource at the same time.");
            }

            if (itemsControl.Items.Count > 0)
            {
                throw new InvalidOperationException("When using ItemsControlHelper.EnumValuesToItemsSource, Items Collection must be null");
            }

            var bindingExpression = BindingOperations.GetBindingExpression(itemsControl, Selector.SelectedItemProperty);
            if (bindingExpression == null)
            {
                throw new InvalidOperationException("ItemsControl must be binding SelectedItem property");
            }

            var binding = bindingExpression.ParentBinding;
            var dataType = bindingExpression.DataItem?.GetType();
            var paths = binding.Path.Path.Split('.');
            foreach (var path in paths)
            {
                var propertyInfo = dataType?.GetProperty(path);
                if (propertyInfo == null)
                {
                    return;
                }

                dataType = propertyInfo.PropertyType;
            }

            if (!dataType.IsEnum)
            {
                var underlyingType = Nullable.GetUnderlyingType(dataType);
                if (underlyingType == null)
                {
                    return;
                }

                dataType = underlyingType;
            }

            List<object> itemValues = new List<object>();
            var values = Enum.GetValues(dataType);
            for (int i = 0; i < values.Length; i++)
            {
                var value = values.GetValue(i);
                FieldInfo fieldInfo = dataType.GetField(value.ToString());
                if (fieldInfo != null)
                {
                    var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>(inherit: false);
                    if (displayAttribute != null && !displayAttribute.GetAutoGenerateField().GetValueOrDefault())
                    {
                        continue;
                    }

                    itemValues.Add(value);
                }
            }

            var itemsSourceBinding = new Binding();
            itemsSourceBinding.Source = itemValues;
            itemsSourceBinding.Mode = BindingMode.OneWay;
            itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
        }

        private static void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            itemsControl.Loaded -= ItemsControl_Loaded;
            SetItemsSource(itemsControl);
        }
    }
}