using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumToBooleanConverter.ConvertEnumToBool(value, parameter);
        }

        internal static bool ConvertEnumToBool(object value, object parameter)
        {
            string text = ValidateArgument.NotNullOrEmptyCast<string>(parameter, "parameter");
            ValidateArgument.NotNull(value, "value");
            return text.Equals(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = ValidateArgument.NotNullOrEmptyCast<bool>(value, "value");
            string text = ValidateArgument.NotNullOrEmptyCast<string>(parameter, "paramater");
            if (flag)
            {
                return Enum.Parse(targetType, text, false);
            }

            return null;
        }
    }
}
