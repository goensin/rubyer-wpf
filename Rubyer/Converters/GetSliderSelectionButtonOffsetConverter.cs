using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 获取 Slider 范围选择按钮偏移
    /// </summary>
    public class GetSliderSelectionButtonOffsetConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3) return Binding.DoNothing;

            var value1 = ValidateArgument.NotNullOrEmptyCast<double>(values[0], "values[0]");
            var value2 = ValidateArgument.NotNullOrEmptyCast<double>(values[0], "values[0]");

            return (value1 - value2) - 2;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
