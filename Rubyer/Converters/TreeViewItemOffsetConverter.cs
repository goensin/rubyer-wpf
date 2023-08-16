using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using Rubyer.Commons;
using System.Windows.Controls.Primitives;

namespace Rubyer.Converters
{
    /// <summary>
    /// TreeViewItem 偏移转换器
    /// </summary>
    public class TreeViewItemOffsetConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("values length less than 2.");
            }

            var element = ValidateArgument.NotNullOrEmptyCast<UIElement>(values[0], "value[0]");
            var toggleButton = ValidateArgument.NotNullOrEmptyCast<ToggleButton>(values[1], "value[1]");

            int layerCount = 0;
            element.ForEachParent(parent =>
            {
                if (parent is TreeListViewItem)
                {
                    layerCount++;
                }
            }, parent => parent is TreeListView);

            var offset = toggleButton.ActualWidth * (layerCount - 1);
            return offset;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
