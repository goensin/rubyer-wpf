using System;
using System.ComponentModel;
using System.Globalization;

namespace Rubyer.Commons
{
    /// <summary>
    /// Enum -> Description
    /// </summary>
    public class EnumDescriptionConverter : EnumConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescriptionConverter"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public EnumDescriptionConverter(Type type)
            : base(type)
        {
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var enumValue = value as Enum;
                return enumValue?.GetDescription();
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var enumValues = EnumType.GetEnumValues();
                for (int i = 0; i < enumValues.Length; i++)
                {
                    var enumValue = enumValues.GetValue(i);
                    if (value.Equals(((Enum)enumValue).GetDescription()))
                    {
                        return enumValue;
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}