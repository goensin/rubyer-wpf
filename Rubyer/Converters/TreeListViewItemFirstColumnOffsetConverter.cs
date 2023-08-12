using Rubyer.Commons;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// TreeListViewItem 首列偏移距离转换器
    /// </summary>
    internal class TreeListViewItemFirstColumnOffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("values length less than 2.");
            }

            var element = ValidateArgument.NotNullOrEmptyCast<UIElement>(values[0], "value[0]");
            var treeListView = ValidateArgument.NotNullOrEmptyCast<TreeListView>(values[1], "value[1]");

            var point = element.TranslatePoint(new Point(), treeListView);


            int layerCount = 0;
            element.ForEachParent(parent =>
            {
                if (parent is TreeListViewItem)
                {
                    layerCount++;
                }
            }, parent => parent is TreeListView);

            var itemPadding = ItemsControlHelper.GetItemPadding(treeListView);
            return point.X;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
