using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Rubyer.Commons
{
    /// <summary>
    /// 每页大小集合转换器
    /// </summary>
    public class PageSizeCollectionConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => true;

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var strValue = value?.ToString();

            if (strValue != null)
            {
                var sizes = strValue.Split(',');

                return sizes.Select(x => int.Parse(x));
            }

            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", strValue, typeof(IEnumerable<int>)));
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is IEnumerable<int> sizes))
                throw new NotSupportedException();

            return string.Join(", ", sizes);
        }
    }
}
