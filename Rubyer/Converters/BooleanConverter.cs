﻿using System;
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
            return (value is bool flag && flag) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T flag && EqualityComparer<T>.Default.Equals(flag, TrueValue);
        }
    }
}
