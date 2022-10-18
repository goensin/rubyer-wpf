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
    /// ColumnDefinitionCollection 类型转换
    /// from https://github.com/dotnet/maui
    /// Microsoft.Maui.Controls.ColumnDefinitionCollectionTypeConverter.cs
    /// </summary>
    public class ColumnDefinitionCollectionTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => destinationType == typeof(string);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var strValue = value?.ToString();

            if (strValue != null)
            {
                var lengths = strValue.Split(',');
                var converter = new GridLengthConverter();
                var definitions = new ColumnDefinition[lengths.Length];
                for (var i = 0; i < lengths.Length; i++)
                    definitions[i] = new ColumnDefinition { Width = (GridLength)converter.ConvertFromInvariantString(lengths[i]) };
                return definitions;
            }

            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", strValue, typeof(ColumnDefinitionCollection)));
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is ColumnDefinitionCollection cdc))
                throw new NotSupportedException();
            var converter = new GridLengthConverter();
            return string.Join(", ", cdc.Select(cd => converter.ConvertToInvariantString(cd.Width)));
        }
    }
}