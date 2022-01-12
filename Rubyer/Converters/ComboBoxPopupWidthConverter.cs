using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class ComboBoxPopupWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)values[0];
            var marginLeft = (double)values[1];
            return width + (marginLeft * 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
