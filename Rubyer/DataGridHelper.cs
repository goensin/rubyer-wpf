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

namespace Rubyer
{
    public static class DataGridHelper
    {
        // 表头背景色
        public static readonly DependencyProperty HeadBackgroundProperty =
            DependencyProperty.RegisterAttached("HeadBackground", typeof(Brush), typeof(DataGridHelper));

        public static void SetHeadBackground(DependencyObject element, Brush value)
        {
            element.SetValue(HeadBackgroundProperty, value);
        }

        public static Brush GetHeadBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(HeadBackgroundProperty);
        }

        // 表头前景色
        public static readonly DependencyProperty HeadForegroundProperty =
            DependencyProperty.RegisterAttached("HeadForeground", typeof(Brush), typeof(DataGridHelper));

        public static void SetHeadForeground(DependencyObject element, Brush value)
        {
            element.SetValue(HeadForegroundProperty, value);
        }

        public static Brush GetHeadForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(HeadForegroundProperty);
        }


        // 当然行背景色
        public static readonly DependencyProperty FocusRowBackgroundProperty =
            DependencyProperty.RegisterAttached("FocusRowBackground", typeof(Brush), typeof(DataGridHelper));

        public static void SetFocusRowBackground(DependencyObject element, Brush value)
        {
            element.SetValue(FocusRowBackgroundProperty, value);
        }

        public static Brush GetFocusRowBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusRowBackgroundProperty);
        }

        // 标题栏行高
        public static readonly DependencyProperty HeadRowHeightProperty =
            DependencyProperty.RegisterAttached("HeadRowHeight", typeof(double), typeof(DataGridHelper));

        public static void SetHeadRowHeight(DependencyObject element, double value)
        {
            element.SetValue(HeadRowHeightProperty, value);
        }

        public static double GetHeadRowHeight(DependencyObject element)
        {
            return (double)element.GetValue(HeadRowHeightProperty);
        }

        // 单元格行高
        public static readonly DependencyProperty CellRowHeightProperty =
            DependencyProperty.RegisterAttached("CellRowHeight", typeof(double), typeof(DataGridHelper));

        public static void SetCellRowHeight(DependencyObject element, double value)
        {
            element.SetValue(CellRowHeightProperty, value);
        }

        public static double GetCellRowHeight(DependencyObject element)
        {
            return (double)element.GetValue(CellRowHeightProperty);
        }
    }
}
