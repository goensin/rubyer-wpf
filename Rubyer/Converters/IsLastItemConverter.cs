using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 最后一项转隐藏
    /// </summary>
    public class IsLastItemConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
            {
                return false;
            }

            var itemsControl = ValidateArgument.NotNullOrEmptyCast<ItemsControl>(values[0], "values[0]");
            var item = ValidateArgument.NotNullOrEmptyCast<Control>(values[1], "values[1]");

            return itemsControl.ItemContainerGenerator.IndexFromContainer(item) == itemsControl.Items.Count - 1;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return [value];
        }
    }
}
