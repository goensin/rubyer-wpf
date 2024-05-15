using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace Rubyer.Commons
{
    /// <summary>
    /// RowDefinitionCollection 类型转换
    /// from https://github.com/dotnet/maui
    /// Microsoft.Maui.Controls.RowDefinitionCollectionTypeConverter.cs
    /// </summary>
    public class RowDefinitionCollectionTypeConverter : TypeConverter
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
                var lengths = strValue.Split(',');
                var converter = new GridLengthConverter();
                var definitions = new RowDefinition[lengths.Length];
                for (var i = 0; i < lengths.Length; i++)
                    definitions[i] = new RowDefinition { Height = (GridLength)converter.ConvertFromInvariantString(lengths[i]) };
                return definitions;
            }

            return new RowDefinition[0];
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is RowDefinitionCollection rdc))
                throw new NotSupportedException();
            var converter = new GridLengthConverter();
            return string.Join(", ", rdc.Select(rd => converter.ConvertToInvariantString(rd.Height)));
        }
    }
}