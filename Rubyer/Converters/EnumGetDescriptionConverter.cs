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
    /// <summary>
    /// 枚举获取说明
    /// </summary>
    public class EnumGetDescriptionConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = ValidateArgument.NotNullOrEmptyCast<Enum>(value, "value");
            return enumValue.GetDescription();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}