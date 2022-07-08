using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 是否为大弧形
    /// </summary>
    public class IsLargeArcConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
            {
                throw new ArgumentException("参数少于 3 个");
            }

            double value = System.Convert.ToDouble(values[0]);
            double min = System.Convert.ToDouble(values[1]);
            double max = System.Convert.ToDouble(values[2]);

            double percent = value / (max - min) > 1 ? 1 : value / (max - min);
            double angle = percent * 360;

            return angle > 180;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}