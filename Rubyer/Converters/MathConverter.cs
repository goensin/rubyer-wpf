using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows.Data;
using static Rubyer.Converters.MathMultipleConverter;

namespace Rubyer.Converters
{
    /// <summary>
    /// 数学计算转换器
    /// </summary>
    public class MathConverter : IValueConverter
    {
        /// <summary>
        /// 计算操作类型
        /// </summary>
        public MathOperation Operation { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ValidateArgument.NotNull(value, nameof(value));
            ValidateArgument.NotNull(parameter, nameof(parameter));

            if (!double.TryParse(value.ToString(), out double num1) || !double.TryParse(parameter.ToString(), out double num2))
                return 0;

            switch (Operation)
            {
                case MathOperation.Add:
                default:
                    return num1 + num2;

                case MathOperation.Divide:
                    return num1 / num2;

                case MathOperation.Multiply:
                    return num1 * num2;

                case MathOperation.Subtract:
                    return num1 - num2;
            }
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
