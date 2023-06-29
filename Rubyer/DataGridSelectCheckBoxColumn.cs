using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// DataGrid 选择列
    /// </summary>
    public class DataGridSelectCheckBoxColumn : DataGridCheckBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridSelectCheckBoxColumn"/> class.
        /// </summary>
        static DataGridSelectCheckBoxColumn()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridSelectCheckBoxColumn"/> class.
        /// </summary>
        public DataGridSelectCheckBoxColumn()
        {
            var headStyle = (Style)Application.Current.FindResource("DataGridSelectCheckBoxColumnHeader");
            HeaderStyle = headStyle;
            CanUserSort = false;
        }

        /// <inheritdoc/>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return GenerateCheckBox(false, cell);
        }

        /// <inheritdoc/>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateCheckBox(true, cell);
        }

        private CheckBox GenerateCheckBox(bool isEditing, DataGridCell cell)
        {
            CheckBox checkBox = (cell != null) ? (cell.Content as CheckBox) : null;
            if (checkBox == null)
            {
                checkBox = new CheckBox();

                if (!isEditing)
                {
                    checkBox.Loaded += CheckBox_Checked;
                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Checked;
                }
            }

            checkBox.IsThreeState = IsThreeState;
            ApplyStyle(isEditing, defaultToElementStyle: true, checkBox);
            ApplyBinding(checkBox, ToggleButton.IsCheckedProperty);
            return checkBox;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                var dataGrid = checkBox.TryGetParentFromVisualTree<DataGrid>();
                var columnHeader = GetHeader(this, dataGrid);
                var headerCheckBox = columnHeader.TryGetChildFromVisualTree<CheckBox>(x => x is CheckBox);
                if (headerCheckBox is null)
                {
                    return;
                }

                var allValues = new List<bool?>();
                foreach (var item in dataGrid.Items)
                {
                    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    var cellsPresenter = row.TryGetChildFromVisualTree<DataGridCellsPresenter>(x => x is DataGridCellsPresenter);
                    if (cellsPresenter is null)
                    {
                        continue;
                    }

                    var cell = cellsPresenter.ItemContainerGenerator.ContainerFromIndex(DisplayIndex) as DataGridCell;
                    var currentCheckBox = cell.TryGetChildFromVisualTree<CheckBox>(x => x is CheckBox);
                    allValues.Add(currentCheckBox.IsChecked);
                }

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

        /// <inheritdoc/>
        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            return base.CommitCellEdit(editingElement);
        }

        private DataGridColumnHeader GetHeader(DataGridColumn column, DependencyObject reference)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(reference); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(reference, i);
                if ((child is DataGridColumnHeader colHeader) && (colHeader.Column == column))
                {
                    return colHeader;
                }

                colHeader = GetHeader(column, child);
                if (colHeader != null)
                {
                    return colHeader;
                }
            }

            return null;
        }

        /// <summary>
        /// Applies the binding.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="property">The property.</param>
        internal void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            BindingBase binding = Binding;
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.ClearBinding(target, property);
            }
        }

        /// <summary>
        /// Applies the style.
        /// </summary>
        /// <param name="isEditing">If true, is editing.</param>
        /// <param name="defaultToElementStyle">If true, default to element style.</param>
        /// <param name="element">The element.</param>
        internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }

        /// <summary>
        /// Picks the style.
        /// </summary>
        /// <param name="isEditing">If true, is editing.</param>
        /// <param name="defaultToElementStyle">If true, default to element style.</param>
        /// <returns>A Style.</returns>
        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style style = (isEditing ? EditingElementStyle : ElementStyle);
            if (isEditing && defaultToElementStyle && style == null)
            {
                style = ElementStyle;
            }

            return style;
        }
    }
}