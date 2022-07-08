using Rubyer.Commons;
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
    /// Visibility -> bool
    /// </summary>
    public class VisibilityToBooleanConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return VisibilityToBooleanConverter.ConvertVisibilityToBool(value);
        }

        internal static bool ConvertVisibilityToBool(object value)
        {
            Visibility visibility = ValidateArgument.NotNullOrEmptyCast<Visibility>(value, "value");
            return visibility == Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = ValidateArgument.NotNullOrEmptyCast<bool>(value, "value");
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}