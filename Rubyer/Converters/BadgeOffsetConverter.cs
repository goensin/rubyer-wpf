using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// Badge 偏移计算转换器
    /// </summary>
    public class BadgeOffsetConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double verOffset = ((double)values[0] - 10) * -1;
            double horOffset = ((double)values[1] - 10) * -1;
            return new Thickness(0, verOffset, horOffset, 0);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}