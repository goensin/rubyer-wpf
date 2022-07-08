using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// DataGrid 详细内容展开列
    /// </summary>
    public class DataGridDetailToggleButtonColumn : DataGridBoundColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridDetailToggleButtonColumn"/> class.
        /// </summary>
        static DataGridDetailToggleButtonColumn()
        {
            var elementStyle = (Style)Application.Current.FindResource("RubyerDataGridDetailToggleButtonColumn");
            DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridDetailToggleButtonColumn), new FrameworkPropertyMetadata(elementStyle));
            DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridDetailToggleButtonColumn), new FrameworkPropertyMetadata(elementStyle));
        }

        /// <inheritdoc/>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return GenerateToggleButton(isEditing: false, cell);
        }

        /// <inheritdoc/>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateToggleButton(isEditing: true, cell);
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

        /// <summary>
        /// Generates the toggle button.
        /// </summary>
        /// <param name="isEditing">If true, is editing.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>A ToggleButton.</returns>
        private ToggleButton GenerateToggleButton(bool isEditing, DataGridCell cell)
        {
            ToggleButton toggleButton = (cell != null) ? (cell.Content as ToggleButton) : null;
            if (toggleButton == null)
            {
                toggleButton = new ToggleButton();
            }

            toggleButton.IsThreeState = false;
            ApplyStyle(isEditing, defaultToElementStyle: true, toggleButton);
            ApplyBinding(toggleButton, ToggleButton.IsCheckedProperty);
            return toggleButton;
        }

        /// <inheritdoc/>
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            ToggleButton toggleButton = editingElement as ToggleButton;
            if (toggleButton != null)
            {
                toggleButton.Focus();
                bool? isChecked = toggleButton.IsChecked;
                if (IsMouseLeftButtonDown(editingEventArgs))
                {
                    toggleButton.IsChecked = !isChecked.GetValueOrDefault();
                    var dataGridCell = editingElement.GetVisualAncestry().OfType<DataGridCell>().FirstOrDefault();
                    dataGridCell.IsEditing = false;
                }

                return isChecked;
            }

            return false;
        }

        /// <summary>
        /// Are the mouse left button down.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>A bool.</returns>
        private static bool IsMouseLeftButtonDown(RoutedEventArgs e)
        {
            if (e is MouseButtonEventArgs mouseButtonEventArgs && mouseButtonEventArgs.ChangedButton == MouseButton.Left)
            {
                return mouseButtonEventArgs.ButtonState == MouseButtonState.Pressed;
            }

            return false;
        }
    }
}