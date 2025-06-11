using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// Color -> Brush Converter
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = ValidateArgument.NotNullOrEmptyCast<Color>(value, nameof(value));
            return new SolidColorBrush(color);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var solidColorBrush = ValidateArgument.NotNullOrEmptyCast<SolidColorBrush>(value, nameof(value));
            return solidColorBrush.Color;
        }
    }
}
