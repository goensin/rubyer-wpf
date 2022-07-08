using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 4 个 double 计算 Arc 点
    /// </summary>
    public class GetArcPointConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4)
            {
                throw new ArgumentException("参数少于 4 个");
            }

            double value = System.Convert.ToDouble(values[0]);
            double min = System.Convert.ToDouble(values[1]);
            double max = System.Convert.ToDouble(values[2]);
            double radius = System.Convert.ToDouble(values[3]) / 2;

            double percent = (value - min) / (max - min) >= 1 ? 0.9999 : value / (max - min);
            double angle = percent * 360;

            double pointX = Math.Sin(angle * Math.PI / 180) * radius + radius;
            double pointY = (Math.Cos(angle * Math.PI / 180) * radius - radius) * -1;

            return new Point(pointX, pointY);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}