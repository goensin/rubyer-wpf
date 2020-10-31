using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class IsLargeArcConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = System.Convert.ToDouble(values[0]);
            var min = System.Convert.ToDouble(values[1]);
            var max = System.Convert.ToDouble(values[2]);

            double percent = value / (max - min) > 1 ? 1 : value / (max - min);
            double angle = percent * 360;

            return angle > 180;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
