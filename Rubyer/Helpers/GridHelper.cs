using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// Grid 助手
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// 列数
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
            "Columns", typeof(int), typeof(GridHelper), new PropertyMetadata(default(int), OnColumnsChanged));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.ColumnDefinitions.Clear();

                var columns = (int)e.NewValue;
                for (int i = 0; i < columns; i++)
                {
                    var columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = i == columns - 1 ? new GridLength(1, GridUnitType.Star) : GridLength.Auto;
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static int GetColumns(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnsProperty);
        }

        /// <summary>
        /// Sets the columns.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetColumns(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        /// <summary>
        /// 行数
        /// </summary>
        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
            "Rows", typeof(int), typeof(GridHelper), new PropertyMetadata(default(int), OnRowsChanged));

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.RowDefinitions.Clear();
                var rows = (int)e.NewValue;
                for (int i = 0; i < rows; i++)
                {
                    var rowDefinition = new RowDefinition();
                    rowDefinition.Height = i == rows - 1 ? new GridLength(1, GridUnitType.Star) : GridLength.Auto;
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static int GetRows(DependencyObject obj)
        {
            return (int)obj.GetValue(RowsProperty);
        }

        /// <summary>
        /// Sets the columns.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetRows(DependencyObject obj, int value)
        {
            obj.SetValue(RowsProperty, value);
        }
    }
}