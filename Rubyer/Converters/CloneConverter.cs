using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Rubyer.Converters
{
    /// <summary>
    /// 克隆内容
    /// </summary>
    public class CloneConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            string xaml = XamlWriter.Save(value);
            var cloneValue = XamlReader.Parse(xaml);
            return cloneValue;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
