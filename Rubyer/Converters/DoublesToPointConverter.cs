using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 两个数 -> 点
    /// </summary>
    public class DoublesToPointConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("参数少于 2 个");
            }

            double x = System.Convert.ToDouble(values[0]);
            double y = System.Convert.ToDouble(values[1]);
            return new Point(x, y);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}