using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class GetEndPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = System.Convert.ToDouble(values[0]);
            var min = System.Convert.ToDouble(values[1]);
            var max = System.Convert.ToDouble(values[2]);
            var radius = System.Convert.ToDouble(values[3]) / 2;

            double percent = (value-min) / (max - min) >= 1 ? 0.9999 : value / (max - min);
            double angle = percent * 360;

            double pointX = Math.Sin(angle * Math.PI / 180) * radius + radius;
            double pointY = (Math.Cos(angle * Math.PI / 180) * radius - radius) * -1;

            return new Point(pointX, pointY);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
