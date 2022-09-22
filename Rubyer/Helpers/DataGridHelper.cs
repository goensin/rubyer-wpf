using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Reflection;
using Rubyer.Converters;

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
            "IsHeaderSelectCheckBox", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(default(bool), OnIsHeaderSelectCheckBoxChanged));

        /// <summary>
        /// Sets the is header select check box.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        internal static void SetIsHeaderSelectCheckBox(DependencyObject element, bool value)
        {
            element.SetValue(IsHeaderSelectCheckBoxProperty, value);
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
                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Checked;
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
            ChangeAllCheckBoxCell(datagrid, columnHeader, checkBox.IsChecked);
        }

        /// <summary>
        /// Changes the all check box cell.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="columnHeader">column header</param>
        /// <param name="isChecked">If true, is checked.</param>
        private static void ChangeAllCheckBoxCell(DataGrid dataGrid, DataGridColumnHeader columnHeader, bool? isChecked)
        {
            var rows = dataGrid.VisualDepthFirstTraversal().OfType<DataGridRow>();

            if (!rows.Any())
            {
                return;
            }

            foreach (var row in rows)
            {
                var cellContent = columnHeader.Column.GetCellContent(row);
                var cell = cellContent.TryGetParentFromVisualTree<DataGridCell>();
                if (cellContent is CheckBox checkBox)
                {
                    dataGrid.BeginEdit();
                    dataGrid.CurrentCell = new DataGridCellInfo(cell);
                    checkBox.IsChecked = isChecked;
                }
            }

            dataGrid.BeginEdit();
            dataGrid.CurrentCell = new DataGridCellInfo();
        }

        /// <summary>
        /// 单击直接编辑
        /// </summary>
        public static readonly DependencyProperty ClickToEditProperty = DependencyProperty.RegisterAttached(
            "ClickToEdit", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(default(bool), OnClickToEditChanged));

        /// <summary>
        /// Sets the click to edit.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetClickToEdit(DependencyObject element, bool value)
        {
            element.SetValue(ClickToEditProperty, value);
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

            // Readonly has to be handled as the passthrough ignores the
            // cell and interacts directly with the content
            if (dataGridCell?.IsReadOnly ?? true || dataGridCell.IsEditing)
            {
                return;
            }

            if (dataGridCell?.Content is UIElement element)
            {
                var dataGrid = (DataGrid)sender;

                // Check if the cursor actually hit the element and not just the cell
                var mousePosition = mouseArgs.GetPosition(element);
                var elementHitBox = new Rect(element.RenderSize);
                if (elementHitBox.Contains(mousePosition))
                {
                    // If it is a DataGridTemplateColumn we want the
                    // click to be handled naturally by the control
                    if (dataGridCell.Column.GetType() == typeof(DataGridTemplateColumn))
                    {
                        return;
                    }

                    dataGrid.CurrentCell = new DataGridCellInfo(dataGridCell);
                    dataGrid.BeginEdit();
                    //Begin edit likely changes the visual tree, trigger the mouse down event to cause the DataGrid to adjust selection
                    var mouseDownEvent = new MouseButtonEventArgs(mouseArgs.MouseDevice, mouseArgs.Timestamp, mouseArgs.ChangedButton)
                    {
                        RoutedEvent = Mouse.MouseDownEvent,
                        Source = mouseArgs.Source
                    };

                    dataGridCell.RaiseEvent(mouseDownEvent);

                    switch (dataGridCell?.Content)
                    {
                        // Send a 'left click' routed command to the toggleButton
                        case ToggleButton toggleButton:
                            {
                                var newMouseEvent = new MouseButtonEventArgs(mouseArgs.MouseDevice, 0, MouseButton.Left)
                                {
                                    RoutedEvent = Mouse.MouseDownEvent,
                                    Source = dataGrid
                                };

                                toggleButton.RaiseEvent(newMouseEvent);
                                break;
                            }

                        // Open the dropdown explicitly. Left clicking is not
                        // viable, as it would edit the text and not open the
                        // dropdown
                        case ComboBox comboBox:
                            {
                                comboBox.IsDropDownOpen = true;
                                mouseArgs.Handled = true;
                                break;
                            }

                        case TextBlock textBlock:
                            {
                                var newMouseEvent = new MouseButtonEventArgs(mouseArgs.MouseDevice, 0, MouseButton.Left)
                                {
                                    RoutedEvent = Mouse.MouseDownEvent,
                                    Source = dataGrid
                                };

                                textBlock.RaiseEvent(newMouseEvent);
                                break;
                            }

                        default:
                            {
                                break;
                            }
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
        /// 应用 DataAnnotations 特性内容
        /// </summary>
        public static readonly DependencyProperty ApplyDataAnnotationsProperty = DependencyProperty.RegisterAttached(
            "ApplyDataAnnotations", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false, OnApplyDataAnnotationsChanged));

        public static void SetApplyDataAnnotations(DependencyObject element, bool value)
        {
            element.SetValue(ApplyDataAnnotationsProperty, value);
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
                }
                else
                {
                    dataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
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

            var propertyInfo = (e.PropertyDescriptor as PropertyDescriptor).ComponentType.GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);
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
    }
}