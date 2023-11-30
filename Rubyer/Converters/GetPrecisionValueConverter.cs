using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 获取精确度格式值
    /// </summary>
    public class GetPrecisionValueConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return Binding.DoNothing;

            var value = ValidateArgument.NotNullOrEmptyCast<double>(values[0], "values[0]");
            var precision = ValidateArgument.NotNullOrEmptyCast<int>(values[1], "values[1]");

            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            numberFormatInfo.NumberDecimalDigits = precision;
            return value.ToString("N", numberFormatInfo);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
