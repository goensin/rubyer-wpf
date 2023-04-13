using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using System.Drawing;
using System.Reflection;

namespace Rubyer
{
    /// <summary>
    /// Panel 帮助类
    /// </summary>
    public class PanelHelper
    {
        /// <summary>
        /// 间距
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached(
            "Spacing", typeof(double), typeof(PanelHelper), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnSpacingChanged));

        public static double GetSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(SpacingProperty);
        }

        public static void SetSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(SpacingProperty, value);
        }

        private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Panel panel)
            {
                if (panel.IsLoaded)
                {
                    SetPanelChildrenSpacing(panel, null);
                }
                else
                {
                    panel.Loaded += SetPanelChildrenSpacing;
                }
            }
        }

        private static void SetPanelChildrenSpacing(object sender, RoutedEventArgs e)
        {
            var panel = (Panel)sender;
            panel.Loaded -= SetPanelChildrenSpacing;

            switch (panel)
            {
                case StackPanel stackPanel:
                    SetStackPanelSpacing(stackPanel);
                    break;

                case Grid grid:
                    SetGridSpacing(grid);
                    break;

                default:
                    break;
            }

        }

        private static void SetHorizontalSpacing(SpacingType type, FrameworkElement element, double spacing, Thickness oldMargin)
        {
            switch (type)
            {
                case SpacingType.No:
                default:
                    element.Margin = new Thickness(0, oldMargin.Top, 0, oldMargin.Bottom);
                    break;
                case SpacingType.Start:
                    element.Margin = new Thickness(spacing / 2, oldMargin.Top, 0, oldMargin.Bottom);
                    break;
                case SpacingType.End:
                    element.Margin = new Thickness(0, oldMargin.Top, spacing / 2, oldMargin.Bottom);
                    break;
                case SpacingType.All:
                    element.Margin = new Thickness(spacing / 2, oldMargin.Top, spacing / 2, oldMargin.Bottom);
                    break;
            }
        }

        private static void SetVerticalSpacing(SpacingType type, FrameworkElement element, double spacing, Thickness oldMargin)
        {
            switch (type)
            {
                case SpacingType.No:
                default:
                    element.Margin = new Thickness(oldMargin.Left, 0, oldMargin.Right, 0);
                    break;
                case SpacingType.Start:
                    element.Margin = new Thickness(oldMargin.Left, spacing / 2, oldMargin.Right, 0);
                    break;
                case SpacingType.End:
                    element.Margin = new Thickness(oldMargin.Left, 0, oldMargin.Right, spacing / 2);
                    break;
                case SpacingType.All:
                    element.Margin = new Thickness(oldMargin.Left, spacing / 2, oldMargin.Right, spacing / 2);
                    break;
            }
        }

        // StackPanel
        private static void SetStackPanelSpacing(StackPanel stackPanel)
        {
            var count = stackPanel.Children.Count;
            var index = 0;
            foreach (FrameworkElement element in stackPanel.Children)
            {
                var spacing = GetSpacing(stackPanel);
                var oldMargin = element.Margin;
                SpacingType type;
                if (index == 0)
                {
                    type = SpacingType.End;
                }
                else if (index == count - 1)
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.All;
                }

                if (stackPanel.Orientation == Orientation.Horizontal)
                {
                    SetHorizontalSpacing(type, element, spacing, oldMargin);
                }
                else
                {
                    SetVerticalSpacing(type, element, spacing, oldMargin);
                }

                index++;
            }
        }

        // Grid
        private static void SetGridSpacing(Grid grid)
        {
            foreach (FrameworkElement element in grid.Children)
            {
                var spacing = GetSpacing(grid);
                var oldMargin = element.Margin;

                // 水平间距
                var column = Grid.GetColumn(element);
                var columnSpan = Grid.GetColumnSpan(element);
                var gridColumns = grid.ColumnDefinitions.Count;
                SpacingType type;
                if (gridColumns == 0)
                {
                    type = SpacingType.No;
                }
                else if (column == 0)
                {
                    type = SpacingType.End;
                }
                else if (column + columnSpan == gridColumns)
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.All;
                }

                SetHorizontalSpacing(type, element, spacing, oldMargin);

                // 垂直间距
                oldMargin = element.Margin;
                var row = Grid.GetRow(element);
                var rowSpan = Grid.GetRowSpan(element);
                var gridRows = grid.RowDefinitions.Count;

                if (gridRows == 0)
                {
                    type = SpacingType.No;
                }
                else if (row == 0)
                {
                    type = SpacingType.End;
                }
                else if (row + rowSpan == gridRows)
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.All;
                }

                SetVerticalSpacing(type, element, spacing, oldMargin);
            }
        }
    }
}

/// <summary>
/// 间隔类型
/// </summary>
internal enum SpacingType
{
    /// <summary>
    /// 无间隔
    /// </summary>
    No = 0,

    /// <summary>
    /// 后面间隔
    /// </summary>
    Start,

    /// <summary>
    /// 后面间隔
    /// </summary>
    End,

    /// <summary>
    /// 前后间隔
    /// </summary>
    All,
}
}
