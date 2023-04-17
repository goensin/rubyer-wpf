using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// string 不为 null 或者 empty
    /// </summary>
    public class NotNullOrEmptyConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !string.IsNullOrEmpty(value.ToString());
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
