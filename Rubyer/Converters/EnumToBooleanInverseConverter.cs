using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// enum -> inverse bool
    /// </summary>
    public class EnumToBooleanInverseConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !EnumToBooleanConverter.ConvertEnumToBool(value, parameter);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}