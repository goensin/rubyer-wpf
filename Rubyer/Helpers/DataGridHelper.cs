﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Reflection;
using System;
using Rubyer.Commons.KnownBoxes;
using Rubyer.DataAnnotations;
using Rubyer.Enums;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rubyer
{
    /// <summary>
    /// DataGrid 帮助类
    /// </summary>
    public static class DataGridHelper
    {
        /// <summary>
        /// 是否为表头全选 CheckBox
        /// </summary>
        internal static readonly DependencyProperty IsHeaderSelectCheckBoxProperty = DependencyProperty.RegisterAttached(
            "IsHeaderSelectCheckBox", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsHeaderSelectCheckBoxChanged));

        /// <summary>
        /// Sets the is header select check box.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        internal static void SetIsHeaderSelectCheckBox(DependencyObject element, bool value)
        {
            element.SetValue(IsHeaderSelectCheckBoxProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Gets the is header select check box.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        internal static bool GetIsHeaderSelectCheckBox(DependencyObject element)
        {
            return (bool)element.GetValue(IsHeaderSelectCheckBoxProperty);
        }

        /// <summary>
        /// Ons the is header select check box changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnIsHeaderSelectCheckBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox checkBox)
            {
                if (GetIsHeaderSelectCheckBox(checkBox))
                {
                    UpdateSelectCheckBoxStatus(checkBox, null);

                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Checked;
                }
            }
        }

        /// <summary>
        /// 更新 DataGridSelectCheckBoxColumn 的 Header CheckBox 状态
        /// </summary>
        /// <param name="headerCheckBox">头部 CheckBox</param>
        /// <param name="currentCheckBox">当前 CheckBox</param>
        internal static void UpdateSelectCheckBoxStatus(CheckBox headerCheckBox, CheckBox currentCheckBox)
        {
            var dataGrid = headerCheckBox.TryGetParentFromVisualTree<DataGrid>();
            var columnHeader = headerCheckBox.TryGetParentFromVisualTree<DataGridColumnHeader>();

            var allValues = new List<bool?>();
            foreach (var item in dataGrid.Items)
            {
                if (((DataGridSelectCheckBoxColumn)columnHeader.Column).Binding is Binding binding &&
                    item.GetType().GetProperty(binding.Path.Path) is PropertyInfo property &&
                    property.GetValue(item) is bool isChecked)
                {
                    allValues.Add(currentCheckBox is null ?
                                    isChecked :
                                    item.Equals(currentCheckBox.DataContext) ? currentCheckBox.IsChecked : isChecked);
                }
                else
                {
                    break;
                }
            }

            if (allValues.Any())
            {
                if (allValues.All(x => x == true))
                {
                    headerCheckBox.IsChecked = true;
                }
                else if (allValues.All(x => x == false))
                {
                    headerCheckBox.IsChecked = false;
                }
                else
                {
                    headerCheckBox.IsChecked = null;
                }
            }
        }

        /// <summary>
        /// Checks the box_ checked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var columnHeader = checkBox.TryGetParentFromVisualTree<DataGridColumnHeader>();
            var datagrid = checkBox.TryGetParentFromVisualTree<DataGrid>();
            ChangeAllCheckBoxCell(datagrid, columnHeader, checkBox.IsChecked, checkBox);
        }

        /// <summary>
        /// Changes the all check box cell.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="columnHeader">column header</param>
        /// <param name="isChecked">If true, is checked.</param>
        private static void ChangeAllCheckBoxCell(DataGrid dataGrid, DataGridColumnHeader columnHeader, bool? isChecked, CheckBox headerCheckBox)
        {
            foreach (var item in dataGrid.Items)
            {
                if (((DataGridSelectCheckBoxColumn)columnHeader.Column).Binding is Binding binding &&
                        item.GetType().GetProperty(binding.Path.Path) is PropertyInfo property)
                {
                    property.SetValue(item, isChecked);
                }
            }

            headerCheckBox.IsChecked = isChecked;
        }

        /// <summary>
        /// 单击直接编辑
        /// </summary>
        public static readonly DependencyProperty ClickToEditProperty = DependencyProperty.RegisterAttached(
            "ClickToEdit", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnClickToEditChanged));

        /// <summary>
        /// Sets the click to edit.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetClickToEdit(DependencyObject element, bool value)
        {
            element.SetValue(ClickToEditProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Gets the click to edit.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetClickToEdit(DependencyObject element)
        {
            return (bool)element.GetValue(ClickToEditProperty);
        }

        /// <summary>
        /// Ons the click to edit changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnClickToEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var enableCheckBoxAssist = (bool)e.NewValue;

            if (enableCheckBoxAssist)
            {
                dataGrid.PreviewMouseLeftButtonDown += AllowDirectEditWithoutFocus;
                dataGrid.KeyDown += EditOnSpacebarPress;
            }
            else
            {
                dataGrid.PreviewMouseLeftButtonDown -= AllowDirectEditWithoutFocus;
                dataGrid.KeyDown -= EditOnSpacebarPress;
            }
        }

        #region form https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

        /// <summary>
        /// Allows editing of components inside of a datagrid cell with a single left click.
        /// form https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
        /// MaterialDesignThemes.Wpf.DataGridAssist
        /// </summary>
        private static void AllowDirectEditWithoutFocus(object sender, MouseButtonEventArgs mouseArgs)
        {
            var originalSource = (DependencyObject)mouseArgs.OriginalSource;
            var dataGridCell = originalSource
                .GetVisualAncestry()
                .OfType<DataGridCell>()
                .FirstOrDefault();

            // Readonly has to be handled as the pass-through ignores the
            // cell and interacts directly with the content
            if (dataGridCell?.IsReadOnly ?? true)
            {
                return;
            }

            if (dataGridCell.Content is UIElement element)
            {
                var dataGrid = (DataGrid)sender;
                // If it is a DataGridTemplateColumn we want the
                // click to be handled naturally by the control
                if (dataGridCell.Column.GetType() == typeof(DataGridTemplateColumn))
                {
                    return;
                }
                if (dataGridCell.IsEditing)
                {
                    // If the cell is already being edited, we don't want to (re)start editing
                    return;
                }
                //NB: Issue 2852 - Don't handle events from nested data grids
                var parentDataGrid = dataGridCell
                    .GetVisualAncestry()
                    .OfType<DataGrid>()
                    .FirstOrDefault();
                if (parentDataGrid != dataGrid)
                {
                    return;
                }

                dataGrid.CurrentCell = new DataGridCellInfo(dataGridCell);
                dataGrid.BeginEdit();

                switch (dataGridCell.Content)
                {
                    case TextBoxBase textBox:
                        {
                            // Send a 'left-click' routed event to the TextBox to place the I-beam at the position of the mouse cursor
                            var mouseDownEvent = new MouseButtonEventArgs(mouseArgs.MouseDevice, mouseArgs.Timestamp, mouseArgs.ChangedButton)
                            {
                                RoutedEvent = Mouse.MouseDownEvent,
                                Source = mouseArgs.Source
                            };
                            textBox.RaiseEvent(mouseDownEvent);
                            break;
                        }

                    case ToggleButton toggleButton:
                        {
                            // Check if the cursor actually hit the checkbox and not just the cell
                            var mousePosition = mouseArgs.GetPosition(element);
                            var elementHitBox = new Rect(element.RenderSize);
                            if (elementHitBox.Contains(mousePosition))
                            {
                                // Send a 'left click' routed command to the toggleButton to toggle the state
                                var newMouseEvent = new MouseButtonEventArgs(mouseArgs.MouseDevice, mouseArgs.Timestamp, mouseArgs.ChangedButton)
                                {
                                    RoutedEvent = Mouse.MouseDownEvent,
                                    Source = dataGrid
                                };

                                toggleButton.RaiseEvent(newMouseEvent);
                            }

                            break;
                        }

                    // Open the dropdown explicitly. Left clicking is not
                    // viable, as it would edit the text and not open the
                    // dropdown
                    case ComboBox comboBox:
                        {
                            comboBox.IsDropDownOpen = true;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// form https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
        /// MaterialDesignThemes.Wpf.DataGridAssist
        /// </summary>
        private static void EditOnSpacebarPress(object sender, KeyEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            if (e.Key == Key.Space && e.OriginalSource is DataGridCell cell
                && !cell.IsReadOnly && cell.Column is DataGridComboBoxColumn)
            {
                dataGrid.BeginEdit();
            }
        }

        #endregion form https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

        /// <summary>
        /// 列位置
        /// </summary>
        public static readonly DependencyProperty ColumnPositionProperty = DependencyProperty.RegisterAttached(
            "ColumnPosition", typeof(ColumnPosition), typeof(DataGridHelper), new PropertyMetadata(ColumnPosition.Start));

        public static void SetColumnPosition(DependencyObject element, ColumnPosition value)
        {
            element.SetValue(ColumnPositionProperty, value);
        }

        public static ColumnPosition GetColumnPosition(DependencyObject element)
        {
            return (ColumnPosition)element.GetValue(ColumnPositionProperty);
        }

        /// <summary>
        /// 应用 DataAnnotations 特性内容
        /// </summary>
        public static readonly DependencyProperty ApplyDataAnnotationsProperty = DependencyProperty.RegisterAttached(
            "ApplyDataAnnotations", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnApplyDataAnnotationsChanged));

        public static void SetApplyDataAnnotations(DependencyObject element, bool value)
        {
            element.SetValue(ApplyDataAnnotationsProperty, BooleanBoxes.Box(value));
        }

        public static bool GetApplyDataAnnotations(DependencyObject element)
        {
            return (bool)element.GetValue(ApplyDataAnnotationsProperty);
        }

        private static void OnApplyDataAnnotationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (GetApplyDataAnnotations(dataGrid))
                {
                    dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                    dataGrid.AutoGeneratedColumns += DataGrid_AutoGeneratedColumns;
                }
                else
                {
                    dataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
                    dataGrid.AutoGeneratedColumns -= DataGrid_AutoGeneratedColumns;
                }
            }
        }

        private static void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var propertyName = e.PropertyName;

            if (dataGrid.Columns.Any(x => x.ClipboardContentBinding != null && ((Binding)x.ClipboardContentBinding).Path.Path == propertyName))
            {
                e.Cancel = true;
                return;
            }

            var propertyInfo = (e.PropertyDescriptor as PropertyDescriptor).ComponentType.GetProperty(e.PropertyName);
            if (propertyInfo != null)
            {
                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    propertyName = displayAttribute.Name;

                    if (displayAttribute.GetAutoGenerateField() == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var column = e.Column;
                    Binding binding;
                    if (e.Column is DataGridBoundColumn boundColumn)
                    {
                        binding = boundColumn.Binding as Binding;
                    }
                    else if (e.Column is DataGridComboBoxColumn comboBoxColumn)
                    {
                        binding = comboBoxColumn.SelectedItemBinding as Binding;
                    }
                    else
                    {
                        binding = e.Column.ClipboardContentBinding as Binding;
                    }

                    ApplyDataFormatAttribute(propertyInfo, binding);
                    ApplyEditableAttribute(propertyInfo, column);
                    ApplyColumnWidthAttribute(propertyInfo, column);
                }
            }

            e.Column.Header = propertyName;
        }

        private static void ApplyDataFormatAttribute(PropertyInfo propertyInfo, Binding binding)
        {
            var displayFormatAttribute = propertyInfo.GetCustomAttribute<DisplayFormatAttribute>();
            if (displayFormatAttribute != null)
            {
                binding.StringFormat = displayFormatAttribute.DataFormatString;
            }
        }

        private static void ApplyEditableAttribute(PropertyInfo propertyInfo, DataGridColumn column)
        {
            var editableAttribute = propertyInfo.GetCustomAttribute<EditableAttribute>();
            if (editableAttribute != null && !editableAttribute.AllowEdit)
            {
                column.IsReadOnly = true;
            }
        }

        private static void ApplyColumnWidthAttribute(PropertyInfo propertyInfo, DataGridColumn column)
        {
            var editableAttribute = propertyInfo.GetCustomAttribute<ColumnWidthAttribute>();
            if (editableAttribute != null)
            {
                column.Width = editableAttribute.Width;
            }
        }

        private static void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var columns = dataGrid.Columns;
            var noGeneratedColumns = columns.Where(x => !x.IsAutoGenerated).ToList();

            foreach (var column in noGeneratedColumns)
            {
                ColumnPosition columnPosition = GetColumnPosition(column);
                if (columnPosition == ColumnPosition.End)
                {
                    var oldIndex = columns.IndexOf(column);
                    var newIndex = columns.Count - 1;
                    columns[oldIndex].DisplayIndex = newIndex;
                }
            }
        }

        /// <summary>
        /// Items 为空时显示视图
        /// </summary>
        public static readonly DependencyProperty EmptyViewProperty = DependencyProperty.RegisterAttached(
            "EmptyView", typeof(object), typeof(DataGridHelper), new PropertyMetadata(null));

        public static void SetEmptyView(DependencyObject element, object value)
        {
            element.SetValue(EmptyViewProperty, value);
        }

        public static object GetEmptyView(DependencyObject element)
        {
            return (object)element.GetValue(EmptyViewProperty);
        }

        /// <summary>
        /// 行头标题
        /// </summary>
        public static readonly DependencyProperty RowHeaderTitleProperty = DependencyProperty.RegisterAttached(
            "RowHeaderTitle", typeof(object), typeof(DataGridHelper), new PropertyMetadata(null));

        public static void SetRowHeaderTitle(DependencyObject element, object value)
        {
            element.SetValue(RowHeaderTitleProperty, value);
        }

        public static object GetRowHeaderTitle(DependencyObject element)
        {
            return (object)element.GetValue(RowHeaderTitleProperty);
        }

        /// <summary>
        /// 加载中
        /// </summary>
        public static readonly DependencyProperty LoadingProperty = DependencyProperty.RegisterAttached(
            "Loading", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        public static void SetLoading(DependencyObject element, bool value)
        {
            element.SetValue(LoadingProperty, BooleanBoxes.Box(value));
        }

        public static bool GetLoading(DependencyObject element)
        {
            return (bool)element.GetValue(LoadingProperty);
        }

        /// <summary>
        /// 加载中内容
        /// </summary>
        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.RegisterAttached(
            "LoadingContent", typeof(object), typeof(DataGridHelper), new PropertyMetadata(null));

        public static void SetLoadingContent(DependencyObject element, object value)
        {
            element.SetValue(LoadingContentProperty, value);
        }

        public static object GetLoadingContent(DependencyObject element)
        {
            return (object)element.GetValue(LoadingContentProperty);
        }

        private static void ApplyDefaultElementStyle(DataGrid dataGrid)
        {
            if (GetApplyColumnElementStyle(dataGrid))
            {
                foreach (var column in dataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.EditingElementStyle = Application.Current.Resources["RubyerDataGridTextColumn"] as Style;
                    }
                    else if (column is DataGridComboBoxColumn comboBoxColumn)
                    {
                        //comboBoxColumn.ElementStyle = Application.Current.Resources["RubyerDataGridComboBoxColumn"] as Style;
                        comboBoxColumn.EditingElementStyle = Application.Current.Resources["RubyerDataGridComboBoxColumnEditting"] as Style;
                    }
                    else if (column is DataGridCheckBoxColumn checkBoxColumn)
                    {
                        checkBoxColumn.ElementStyle = Application.Current.Resources["RubyerDataGridCheckBoxColumn"] as Style;
                        checkBoxColumn.EditingElementStyle = Application.Current.Resources["RubyerDataGridCheckBoxColumnEditting"] as Style;
                    }
                }
            }
        }

        /// <summary>
        /// 应用 rubyer Element Style
        /// </summary>
        public static readonly DependencyProperty ApplyColumnElementStyleProperty = DependencyProperty.RegisterAttached(
            "ApplyColumnElementStyle", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnApplyColumnElementStyleChanged));

        private static void OnApplyColumnElementStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (GetApplyColumnElementStyle(dataGrid))
                {
                    if (dataGrid.IsLoaded)
                    {
                        ApplyDefaultElementStyle(dataGrid);
                    }
                    else
                    {
                        dataGrid.Loaded += DataGrid_Loaded;
                    }
                }
            }
        }

        private static void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            dataGrid.Loaded -= DataGrid_Loaded;
            ApplyDefaultElementStyle(dataGrid);
        }

        public static void SetApplyColumnElementStyle(DependencyObject element, bool value)
        {
            element.SetValue(ApplyColumnElementStyleProperty, BooleanBoxes.Box(value));
        }

        public static bool GetApplyColumnElementStyle(DependencyObject element)
        {
            return (bool)element.GetValue(ApplyColumnElementStyleProperty);
        }

        /// <summary>
        /// 自动生成行号，显示在 RowHeader。
        /// </summary>
        public static readonly DependencyProperty AutoGenerateRowNumberProperty =
            DependencyProperty.RegisterAttached("AutoGenerateRowNumber", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnAutoGenerateRowNumberChanged));

        public static bool GetAutoGenerateRowNumber(DependencyObject obj) => (bool)obj.GetValue(AutoGenerateRowNumberProperty);

        public static void SetAutoGenerateRowNumber(DependencyObject obj, bool value) => obj.SetValue(AutoGenerateRowNumberProperty, BooleanBoxes.Box(value));

        private static void OnAutoGenerateRowNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (GetAutoGenerateRowNumber(dataGrid) && dataGrid.HeadersVisibility.HasFlag(DataGridHeadersVisibility.Row))
                {
                    dataGrid.LoadingRow += DataGrid_LoadingRow;
                    dataGrid.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
                }
                else
                {
                    dataGrid.LoadingRow -= DataGrid_LoadingRow;
                    dataGrid.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
                }
            }
        }


        private static void UpdateRowHeader(DataGridRow row)
        {
            var number = row.GetIndex() + 1;
            row.Header = number;
        }

        private static void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            var generator = (ItemContainerGenerator)sender;
            foreach (var item in generator.Items)
            {
                if (generator.ContainerFromItem(item) is DataGridRow row)
                {
                    UpdateRowHeader(row);
                }
            }
        }

        private static void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            UpdateRowHeader(e.Row);
        }
    }
}