using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// 颜色转字符串
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToStringConverter : IValueConverter
    {
        /// <inheritdoc/> 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return string.Empty;
        }

        /// <inheritdoc/> 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && ColorConverter.ConvertFromString(str) is Color color)
            {
                return color;
            }

            return Colors.Transparent;
        }
    }
}
