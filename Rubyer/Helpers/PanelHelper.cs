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

        private static void SetHorizontalSpacing(SpacingType type, FrameworkElement element, double spacing)
        {
            switch (type)
            {
                case SpacingType.No:
                default:
                    element.Margin = new Thickness(0);
                    break;
                case SpacingType.Start:
                    element.Margin = new Thickness(spacing / 2, 0, 0, 0);
                    break;
                case SpacingType.End:
                    element.Margin = new Thickness(0, 0, spacing / 2, 0);
                    break;
                case SpacingType.All:
                    element.Margin = new Thickness(spacing / 2, 0, spacing / 2, 0);
                    break;
            }
        }

        private static void SetVerticalSpacing(SpacingType type, FrameworkElement element, double spacing)
        {
            switch (type)
            {
                case SpacingType.No:
                default:
                    element.Margin = new Thickness(0, 0, 0, 0);
                    break;
                case SpacingType.Start:
                    element.Margin = new Thickness(0, spacing / 2, 0, 0);
                    break;
                case SpacingType.End:
                    element.Margin = new Thickness(0, 0, 0, spacing / 2);
                    break;
                case SpacingType.All:
                    element.Margin = new Thickness(0, spacing / 2, 0, spacing / 2);
                    break;
            }
        }

        // StackPanel
        private static void SetStackPanelSpacing(StackPanel stackPanel)
        {
            var children = stackPanel.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var count = children.Count;
            var index = 0;
            foreach (FrameworkElement element in children)
            {
                var spacing = GetSpacing(stackPanel);
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
                    SetHorizontalSpacing(type, element, spacing);
                }
                else
                {
                    SetVerticalSpacing(type, element, spacing);
                }

                index++;
            }
        }

        // Grid
        private static void SetGridSpacing(Grid grid)
        {
            var children = grid.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            foreach (FrameworkElement element in children)
            {
                var spacing = GetSpacing(grid);

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

                SetHorizontalSpacing(type, element, spacing);

                // 垂直间距
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

                SetVerticalSpacing(type, element, spacing);
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
