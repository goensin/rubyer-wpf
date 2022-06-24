using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class EnumToBooleanInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !EnumToBooleanConverter.ConvertEnumToBool(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
