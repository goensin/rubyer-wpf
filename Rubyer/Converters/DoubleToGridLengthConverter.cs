using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// double -> GridLength
    /// </summary>
    public class DoubleToGridLengthConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = ValidateArgument.NotNullOrEmptyCast<double>(value, "value");
            return new GridLength(doubleValue);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gridLength = ValidateArgument.NotNullOrEmptyCast<GridLength>(value, "value");
            return gridLength.Value;
        }
    }
}