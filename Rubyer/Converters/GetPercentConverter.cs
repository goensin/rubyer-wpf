using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class GetPercentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = System.Convert.ToDouble(values[0]);
            var min = System.Convert.ToDouble(values[1]);
            var max = System.Convert.ToDouble(values[2]);

            var percent = (value - min) / (max - min);

            return $"{System.Convert.ToInt32(percent * 100)}%";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
