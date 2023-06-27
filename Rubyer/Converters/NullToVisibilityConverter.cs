using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// null -> Visiblity
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// null 对应 Visibility
        /// </summary>
        public Visibility NullVisibility { get; set; }

        /// <summary>
        /// 非 null 对应 Visibility    
        /// </summary>
        public Visibility NotNullVisibility { get; set; }

        public NullToVisibilityConverter()
        {
            NullVisibility = Visibility.Collapsed;
            NotNullVisibility = Visibility.Visible;
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? NullVisibility : NotNullVisibility;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
