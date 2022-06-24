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
    /// bool 转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BooleanConverter<T> : IValueConverter
    {
        /// <summary>
        /// bool 转换器
        /// </summary>
        /// <param name="trueValue">true 对应值</param>
        /// <param name="falseValue">false 对应值</param>
        protected BooleanConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        /// <summary>
        /// true 对应值
        /// </summary>
        public T TrueValue { get; set; }

        /// <summary>
        /// false 对应值
        /// </summary>
        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanConverter<T>.ConvertBooleanToType(value, TrueValue, FalseValue);
        }

        /// <summary>
        /// 转换 bool 为类型
        /// </summary>
        /// <param name="value">bool 值</param>
        /// <param name="trueValue">true 时的值</param>
        /// <param name="falseValue">false 时的值</param>
        /// <returns>类型的值</returns>
        internal static T ConvertBooleanToType(object value, T trueValue, T falseValue)
        {
            bool flag = ValidateArgument.NotNullOrEmptyCast<bool>(value, "value");
            return flag ? trueValue : falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T flag && EqualityComparer<T>.Default.Equals(flag, TrueValue);
        }
    }
}
