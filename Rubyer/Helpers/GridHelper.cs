using Rubyer.Commons;
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
        /// Gets the rows.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static int GetRows(DependencyObject obj)
        {
            return (int)obj.GetValue(RowsProperty);
        }

        /// <summary>
        /// Sets the rows.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetRows(DependencyObject obj, int value)
        {
            obj.SetValue(RowsProperty, value);
        }

        /// <summary>
        /// 列定义
        /// </summary>
        public static readonly DependencyProperty ColumnDefinitionsProperty = DependencyProperty.RegisterAttached(
            "ColumnDefinitions", typeof(string), typeof(GridHelper), new PropertyMetadata(null, OnColumnDefinitionsChanged));

        private static void OnColumnDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.ColumnDefinitions.Clear();
                var converter = new ColumnDefinitionCollectionTypeConverter();
                var columnDefinitions = converter.ConvertFromString(GetColumnDefinitions(grid)) as ColumnDefinition[];
                for (int i = 0; i < columnDefinitions.Length; i++)
                {
                    grid.ColumnDefinitions.Add(columnDefinitions[i]);
                }
            }
        }

        /// <summary>
        /// Gets the columns Definitions.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static string GetColumnDefinitions(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnDefinitionsProperty);
        }

        /// <summary>
        /// Sets the columns Definitions.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetColumnDefinitions(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnDefinitionsProperty, value);
        }

        /// <summary>
        /// 行定义
        /// </summary>
        public static readonly DependencyProperty RowDefinitionsProperty = DependencyProperty.RegisterAttached(
            "RowDefinitions", typeof(string), typeof(GridHelper), new PropertyMetadata(null, OnRowDefinitionsChanged));

        private static void OnRowDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.RowDefinitions.Clear();
                var converter = new RowDefinitionCollectionTypeConverter();
                var rowDefinitions = converter.ConvertFromString(GetRowDefinitions(grid)) as RowDefinition[];
                for (int i = 0; i < rowDefinitions.Length; i++)
                {
                    grid.RowDefinitions.Add(rowDefinitions[i]);
                }
            }
        }

        /// <summary>
        /// Gets the rows Definitions.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A double.</returns>
        public static string GetRowDefinitions(DependencyObject obj)
        {
            return (string)obj.GetValue(RowDefinitionsProperty);
        }

        /// <summary>
        /// Sets the rows Definitions.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetRowDefinitions(DependencyObject obj, string value)
        {
            obj.SetValue(RowDefinitionsProperty, value);
        }
    }
}