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

namespace Rubyer
{
    public class DataGridDetailToggleButtonColumn : DataGridBoundColumn
    {
        static DataGridDetailToggleButtonColumn()
        {
            var elementStyle = (Style)Application.Current.FindResource("RubyerDataGridDetailToggleButtonColumn");
            DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridDetailToggleButtonColumn), new FrameworkPropertyMetadata(elementStyle));
            DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridDetailToggleButtonColumn), new FrameworkPropertyMetadata(elementStyle));
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return GenerateToggleButton(isEditing: false, cell);
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateToggleButton(isEditing: true, cell);
        }

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

        internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }

        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style style = (isEditing ? EditingElementStyle : ElementStyle);
            if (isEditing && defaultToElementStyle && style == null)
            {
                style = ElementStyle;
            }

            return style;
        }

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

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            ToggleButton toggleButton = editingElement as ToggleButton;
            if (toggleButton != null)
            {
                toggleButton.Focus();
                bool? isChecked = toggleButton.IsChecked;
                if ((IsMouseLeftButtonDown(editingEventArgs) && IsMouseOver(toggleButton, editingEventArgs)) || IsSpaceKeyDown(editingEventArgs))
                {
                    toggleButton.IsChecked = isChecked != true;
                }

                return isChecked;
            }

            return false;
        }

        private static bool IsMouseLeftButtonDown(RoutedEventArgs e)
        {
            MouseButtonEventArgs mouseButtonEventArgs = e as MouseButtonEventArgs;
            if (mouseButtonEventArgs != null && mouseButtonEventArgs.ChangedButton == MouseButton.Left)
            {
                return mouseButtonEventArgs.ButtonState == MouseButtonState.Pressed;
            }

            return false;
        }

        private static bool IsMouseOver(ToggleButton toggleButton, RoutedEventArgs e)
        {
            return toggleButton.InputHitTest(((MouseButtonEventArgs)e).GetPosition(toggleButton)) != null;
        }

        private static bool IsSpaceKeyDown(RoutedEventArgs e)
        {
            KeyEventArgs keyEventArgs = e as KeyEventArgs;
            if (keyEventArgs != null && keyEventArgs.RoutedEvent == Keyboard.KeyDownEvent && (keyEventArgs.KeyStates & KeyStates.Down) == KeyStates.Down)
            {
                return keyEventArgs.Key == Key.Space;
            }

            return false;
        }
    }
}