using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// true 对应 Visibility
        /// </summary>
        public Visibility TrueVisibility { get; set; }

        /// <summary>
        /// false 对应 Visibility
        /// </summary>
        public Visibility FalseVisibility { get; set; }

        public EnumToVisibilityConverter()
        {
            TrueVisibility = Visibility.Visible;
            FalseVisibility = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumToVisibilityConverter.ConvertEnumToVisibility(value, parameter, TrueVisibility, FalseVisibility);
        }

        public static object ConvertEnumToVisibility(object value, object parameter, Visibility trueVisibility, Visibility falseVisibility)
        {
            bool flag = EnumToBooleanConverter.ConvertEnumToBool(value, parameter);
            return BooleanConverter<Visibility>.ConvertBooleanToType(flag, trueVisibility, falseVisibility);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
