using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// Color -> Hue
    /// </summary>
    public class ColorToHueConverter : IValueConverter
    {
        /// <inheritdoc/> 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = ValidateArgument.NotNullOrEmptyCast<Color>(value, nameof(value));
            //return GetHueFromColor(color);
            return Binding.DoNothing;
        }

        /// <inheritdoc/> 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hub = ValidateArgument.NotNullOrEmptyCast<double>(value, nameof(value));
            //var alpha = ValidateArgument.NotNullOrEmptyCast<byte>(value, nameof(parameter));
            return Binding.DoNothing;
        }
    }
}
