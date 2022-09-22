using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo
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
                if (value != null)
                {
                    FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                    if (fieldInfo != null)
                    {
                        var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(inherit: false);
                        return ((attributes.Count() > 0) && (!string.IsNullOrEmpty(attributes.First().Description)))
                            ? attributes.First().Description
                            : value.ToString();
                    }
                }
            }

            return string.Empty;
        }
    }
}