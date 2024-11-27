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
            var height = (double)values[0];
            var width = (double)values[1];
            double verOffset = (height / 2) * -1;
            double horOffset = verOffset;
            return new Thickness(0, verOffset, horOffset, 0);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}