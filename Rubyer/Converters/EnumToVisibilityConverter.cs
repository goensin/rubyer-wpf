using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// enum => Visiblity
    /// </summary>
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// true 对应 Visibility
        /// </summary>
        public Visibility TrueVisibility { get; set; }

        /// <summary>
        /// false 对应 Visibility
        /// </summary>
        public Visibility FalseVisibility { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumToVisibilityConverter"/> class.
        /// </summary>
        public EnumToVisibilityConverter()
        {
            TrueVisibility = Visibility.Visible;
            FalseVisibility = Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumToVisibilityConverter.ConvertEnumToVisibility(value, parameter, TrueVisibility, FalseVisibility);
        }

        /// <summary>
        /// Converts the enum to visibility.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="trueVisibility">The true visibility.</param>
        /// <param name="falseVisibility">The false visibility.</param>
        /// <returns>An object.</returns>
        public static object ConvertEnumToVisibility(object value, object parameter, Visibility trueVisibility, Visibility falseVisibility)
        {
            bool flag = EnumToBooleanConverter.ConvertEnumToBool(value, parameter);
            return BooleanConverter<Visibility>.ConvertBooleanToType(flag, trueVisibility, falseVisibility);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}