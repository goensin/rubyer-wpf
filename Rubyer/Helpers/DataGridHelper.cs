using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Rubyer
{
    public static class DataGridHelper
    {
        /// <summary>
        /// 是否为表头全选 CheckBox
        /// </summary>
        public static readonly DependencyProperty IsHeaderSelectCheckBoxProperty = DependencyProperty.RegisterAttached(
            "IsHeaderSelectCheckBox", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(default(bool), OnIsHeaderSelectCheckBoxChanged));

        public static void SetIsHeaderSelectCheckBox(DependencyObject element, bool value)
        {
            element.SetValue(IsHeaderSelectCheckBoxProperty, value);
        }

        public static bool GetIsHeaderSelectCheckBox(DependencyObject element)
        {
            return (bool)element.GetValue(IsHeaderSelectCheckBoxProperty);
        }

        private static void OnIsHeaderSelectCheckBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox checkBox)
            {
                if (GetIsHeaderSelectCheckBox(checkBox))
                {
                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Checked;
                }
                else
                {
                    checkBox.Checked -= CheckBox_Checked;
                    checkBox.Unchecked -= CheckBox_Checked;
                }
            }
        }

        private static void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var columnHeader = checkBox.TryGetParentFromVisualTree<DataGridColumnHeader>();
            var headersPanel = checkBox.TryGetParentFromVisualTree<DataGridCellsPanel>();
            var index = headersPanel.Children.IndexOf(columnHeader);
            var datagrid = checkBox.TryGetParentFromVisualTree<DataGrid>();
            ChangeAllCheckBoxCell(datagrid, index, checkBox.IsChecked);
        }

        private static void ChangeAllCheckBoxCell(DataGrid dataGrid, int index, bool? isChecked)
        {
            var rows = dataGrid.VisualDepthFirstTraversal().OfType<DataGridRow>();

            if (!rows.Any())
            {
                return;
            }

            foreach (var row in rows)
            {
                var cells = row.VisualDepthFirstTraversal().OfType<DataGridCell>().ToList();
                var cell = cells[index];
                if (cell.Content is CheckBox checkBox)
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

        public static void SetClickToEdit(DependencyObject element, bool value)
        {
            element.SetValue(ClickToEditProperty, value);
        }

        public static bool GetClickToEdit(DependencyObject element)
        {
            return (bool)element.GetValue(ClickToEditProperty);
        }

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
    }
}
