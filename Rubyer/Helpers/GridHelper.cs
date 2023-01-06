using Rubyer.Commons;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Rubyer
{
    /// <summary>
    /// Grid 帮助类
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// 列数
        /// (最后一列填充)
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
        /// (最后一行填充)
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

        /// <summary>
        /// 列间距
        /// </summary>
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.RegisterAttached(
            "ColumnSpacing", typeof(double), typeof(GridHelper), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnColumnSpacingChanged));

        private static void OnColumnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                if (grid.IsLoaded)
                {
                    SetGridChildrenColumnSpacing(grid, null);
                }
                else
                {
                    grid.Loaded += SetGridChildrenColumnSpacing;
                }
            }
        }

        private static void SetGridChildrenColumnSpacing(object sender, RoutedEventArgs e)
        {
            var grid = (Grid)sender;
            grid.Loaded -= SetGridChildrenColumnSpacing;
            var count = grid.Children.Count;
            foreach (FrameworkElement element in grid.Children)
            {
                var column = Grid.GetColumn(element);
                var columnSpan = Grid.GetColumnSpan(element);
                var gridColumns = grid.ColumnDefinitions.Count;

                var oldMargin = element.Margin;
                var spacing = GetColumnSpacing(grid);
                element.Margin = (gridColumns == 0) || (column + columnSpan == gridColumns) ?
                                 new Thickness(0, oldMargin.Top, 0, oldMargin.Bottom) :
                                 new Thickness(0, oldMargin.Top, spacing, oldMargin.Bottom);

                if (gridColumns == 0)
                {
                    element.Margin = new Thickness(0, oldMargin.Top, 0, oldMargin.Bottom);
                }
                else if (column == 0)
                {
                    element.Margin = new Thickness(0, oldMargin.Top, spacing / 2, oldMargin.Bottom);
                }
                else if (column + columnSpan == gridColumns)
                {
                    element.Margin = new Thickness(spacing / 2, oldMargin.Top, 0, oldMargin.Bottom);
                }
                else
                {
                    element.Margin = new Thickness(spacing / 2, oldMargin.Top, spacing / 2, oldMargin.Bottom);
                }
            }
        }

        public static double GetColumnSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(ColumnSpacingProperty);
        }

        public static void SetColumnSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(ColumnSpacingProperty, value);
        }

        /// <summary>
        /// 行间距
        /// </summary>
        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.RegisterAttached(
            "RowSpacing", typeof(double), typeof(GridHelper), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnRowSpacingChanged));

        private static void OnRowSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                if (grid.IsLoaded)
                {
                    SetGridChildrenRowSpacing(grid, null);
                }
                else
                {
                    grid.Loaded += SetGridChildrenRowSpacing;
                }
            }
        }

        private static void SetGridChildrenRowSpacing(object sender, RoutedEventArgs e)
        {
            var grid = (Grid)sender;
            grid.Loaded -= SetGridChildrenRowSpacing;

            var count = grid.Children.Count;
            foreach (FrameworkElement element in grid.Children)
            {
                var row = Grid.GetRow(element);
                var rowSpan = Grid.GetRowSpan(element);
                var gridRows = grid.RowDefinitions.Count;

                var oldMargin = element.Margin;
                var spacing = GetRowSpacing(grid);

                if (gridRows == 0)
                {
                    element.Margin = new Thickness(oldMargin.Left, 0, oldMargin.Right, 0);
                }
                else if (row == 0)
                {
                    element.Margin = new Thickness(oldMargin.Left, 0, oldMargin.Right, spacing / 2);
                }
                else if (row + rowSpan == gridRows)
                {
                    element.Margin = new Thickness(oldMargin.Left, spacing / 2, oldMargin.Right, 0);
                }
                else
                {
                    element.Margin = new Thickness(oldMargin.Left, spacing / 2, oldMargin.Right, spacing / 2);
                }
            }
        }

        public static double GetRowSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(RowSpacingProperty);
        }

        public static void SetRowSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(RowSpacingProperty, value);
        }
    }
}