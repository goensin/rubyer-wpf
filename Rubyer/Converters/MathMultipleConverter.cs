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
    /// 数学计算多转换器
    /// </summary>
    public class MathMultipleConverter : IMultiValueConverter
    {
        /// <summary>
        /// 计算操作类型
        /// </summary>
        public enum MathOperation
        {
            /// <summary>
            /// 加
            /// </summary>
            Add,

            /// <summary>
            /// 减
            /// </summary>
            Subtract,

            /// <summary>
            /// 乘
            /// </summary>
            Multiply,

            /// <summary>
            /// 除
            /// </summary>
            Divide
        }

        /// <summary>
        /// 计算操作类型
        /// </summary>
        public MathOperation Operation { get; set; }

        /// <inheritdoc/>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Length < 2 || value[0] == null || value[1] == null) return Binding.DoNothing;

            if (!double.TryParse(value[0].ToString(), out double value1) || !double.TryParse(value[1].ToString(), out double value2))
                return 0;

            return Operation switch
            {
                MathOperation.Divide => value1 / value2,
                MathOperation.Multiply => value1 * value2,
                MathOperation.Subtract => value1 - value2,
                _ => (object)(value1 + value2),
            };
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}