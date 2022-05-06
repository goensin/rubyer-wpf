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
