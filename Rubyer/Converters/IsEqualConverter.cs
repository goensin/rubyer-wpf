using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 是否相等
    /// </summary>
    public class IsEqualConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0].Equals(values[1]);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}