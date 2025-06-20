using Rubyer.Commons;
using System;
using System.Globalization;
using System.Windows;
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

            if (value is Thickness thickness)
            {
                value = thickness.Left;
            }
            else if (value is CornerRadius cornerRadius)
            {
                value = cornerRadius.TopLeft;
            }

            if (!double.TryParse(value.ToString(), out double num1) || !double.TryParse(parameter.ToString(), out double num2))
                return 0;

            return Operation switch
            {
                MathOperation.Divide => num1 / num2,
                MathOperation.Multiply => num1 * num2,
                MathOperation.Subtract => num1 - num2,
                _ => num1 + num2,
            };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
