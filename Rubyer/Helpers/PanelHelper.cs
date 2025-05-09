using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System;
using System.Collections.Generic;

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
                panel.SizeChanged -= Panel_SizeChanged;
                panel.SizeChanged += Panel_SizeChanged;

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

        private static void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPanelChildrenSpacing(sender, null);
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

                case DockPanel dockPanel:
                    SetDockPanelSpacing(dockPanel);
                    break;

                case WrapPanel wrapPanel:
                    SetWrapPanelSpacing(wrapPanel);
                    break;

                case UniformGrid uniformGrid:
                    SetUniformGridSpacing(uniformGrid);
                    break;
            }
        }

        private static void SetHorizontalSpacing(SpacingType type, FrameworkElement element, double spacing, Thickness oldMargin, bool isHalf = true)
        {
            var margin = isHalf ? spacing / 2 : spacing;
            element.Margin = type switch
            {
                SpacingType.Start => new Thickness(margin, oldMargin.Top, 0, oldMargin.Bottom),
                SpacingType.End => new Thickness(0, oldMargin.Top, margin, oldMargin.Bottom),
                SpacingType.All => new Thickness(margin, oldMargin.Top, margin, oldMargin.Bottom),
                _ => new Thickness(0, oldMargin.Top, 0, oldMargin.Bottom),
            };
        }

        private static void SetVerticalSpacing(SpacingType type, FrameworkElement element, double spacing, Thickness oldMargin, bool isHalf = true)
        {
            var margin = isHalf ? spacing / 2 : spacing;
            element.Margin = type switch
            {
                SpacingType.Start => new Thickness(oldMargin.Left, margin, oldMargin.Right, 0),
                SpacingType.End => new Thickness(oldMargin.Left, 0, oldMargin.Right, margin),
                SpacingType.All => new Thickness(oldMargin.Left, margin, oldMargin.Right, margin),
                _ => new Thickness(oldMargin.Left, 0, oldMargin.Right, 0),
            };
        }

        // StackPanel
        private static void SetStackPanelSpacing(StackPanel stackPanel)
        {
            var children = stackPanel.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var count = children.Count;
            var index = 0;
            var spacing = GetSpacing(stackPanel);
            foreach (FrameworkElement element in children)
            {
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
                    SetHorizontalSpacing(type, element, spacing, new Thickness(0));
                }
                else
                {
                    SetVerticalSpacing(type, element, spacing, new Thickness(0));
                }

                index++;
            }
        }

        // Grid
        private static void SetGridSpacing(Grid grid)
        {
            var children = grid.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var spacing = GetSpacing(grid);
            foreach (FrameworkElement element in children)
            {
                var oldMargin = element.Margin;

                // 水平间距
                var column = Grid.GetColumn(element);
                var columnSpan = Grid.GetColumnSpan(element);
                var gridColumns = grid.ColumnDefinitions.Count;
                SpacingType type;
                if (gridColumns == 0 || (column == 0 && column + columnSpan == gridColumns))
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

                if (gridRows == 0 || (row == 0 && row + rowSpan == gridRows))
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

        // DockPanel
        private static void SetDockPanelSpacing(DockPanel dockPanel)
        {
            var children = dockPanel.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var count = children.Count;
            var index = 0;
            var spacing = GetSpacing(dockPanel);
            foreach (FrameworkElement element in children)
            {
                var oldMargin = element.Margin;
                var dock = DockPanel.GetDock(element);
                if (++index < count)
                {
                    SpacingType type = dock switch
                    {
                        Dock.Left => SpacingType.End,
                        Dock.Top => SpacingType.End,
                        Dock.Right => SpacingType.Start,
                        Dock.Bottom => SpacingType.Start,
                        _ => SpacingType.End,
                    };

                    switch (dock)
                    {
                        case Dock.Left:
                        case Dock.Right:
                            SetHorizontalSpacing(type, element, spacing, oldMargin, isHalf: false);
                            break;
                        case Dock.Top:
                        case Dock.Bottom:
                            SetVerticalSpacing(type, element, spacing, oldMargin, isHalf: false);
                            break;
                    }
                }
            }
        }

        // WrapPanel
        private static void SetWrapPanelHorizontalSpacing(WrapPanel wrapPanel, List<FrameworkElement> children, double spacing)
        {
            foreach (FrameworkElement element in children)
            {
                SpacingType type;
                var point = element.TranslatePoint(new Point(), wrapPanel);
                if (point.X < element.ActualWidth)
                {
                    type = SpacingType.End;
                }
                else if (wrapPanel.ActualWidth - (point.X + element.ActualWidth) < element.ActualWidth)
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.All;
                }

                SetHorizontalSpacing(type, element, spacing, new Thickness(0, element.Margin.Top, 0, element.Margin.Bottom));
            }
        }

        private static void SetWrapPanelVerticalSpacing(WrapPanel wrapPanel, List<FrameworkElement> children, double spacing)
        {
            foreach (FrameworkElement element in children)
            {
                SpacingType type;
                var point = element.TranslatePoint(new Point(), wrapPanel);

                if (point.Y < element.ActualHeight)
                {
                    type = SpacingType.End;
                }
                else if (wrapPanel.ActualHeight - (point.Y + element.ActualHeight) < element.ActualHeight)
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.All;
                }

                SetVerticalSpacing(type, element, spacing, new Thickness(element.Margin.Left, 0, element.Margin.Right, 0));
            }
        }


        private static void SetWrapPanelSpacing(WrapPanel wrapPanel)
        {
            var children = wrapPanel.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var count = children.Count;
            var spacing = GetSpacing(wrapPanel);
            if (wrapPanel.Orientation == Orientation.Horizontal)
            {
                SetWrapPanelHorizontalSpacing(wrapPanel, children, spacing);
                SetWrapPanelVerticalSpacing(wrapPanel, children, spacing);
            }
            else
            {
                SetWrapPanelVerticalSpacing(wrapPanel, children, spacing);
                SetWrapPanelHorizontalSpacing(wrapPanel, children, spacing);
            }
        }

        // UniformGrid
        private static void SetUniformGridSpacing(UniformGrid uniformGrid)
        {
            var children = uniformGrid.Children.OfType<FrameworkElement>().Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var count = children.Count;
            var spacing = GetSpacing(uniformGrid);
            var points = children.Select(x => x.TranslatePoint(new Point(), uniformGrid)).ToList();
            int index = 0;
            foreach (FrameworkElement element in children)
            {
                SpacingType type = SpacingType.No;
                var point = points[index];
                if (point.X < points.Max(p => p.X) && point.X > points.Min(p => p.X))
                {
                    type = SpacingType.All;
                }
                else if (point.X < points.Max(p => p.X))
                {
                    type = SpacingType.End;
                }
                else if (point.X > points.Min(p => p.X))
                {
                    type = SpacingType.Start;
                }

                SetHorizontalSpacing(type, element, spacing, new Thickness(0));

                if (point.Y < points.Max(p => p.Y) && point.Y > points.Min(p => p.Y))
                {
                    type = SpacingType.All;
                }
                else if (point.Y < points.Max(p => p.Y))
                {
                    type = SpacingType.End;
                }
                else if (point.Y > points.Min(p => p.Y))
                {
                    type = SpacingType.Start;
                }
                else
                {
                    type = SpacingType.No;
                }

                SetVerticalSpacing(type, element, spacing, new Thickness(element.Margin.Left, 0, element.Margin.Right, 0));

                index++;
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
