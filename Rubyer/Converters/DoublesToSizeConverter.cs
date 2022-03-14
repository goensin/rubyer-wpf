using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class DoublesToSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("参数少于 2 个");
            }

            double width = System.Convert.ToDouble(values[0]);
            double height = System.Convert.ToDouble(values[1]);

            return new Size(width, height);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
