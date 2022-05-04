using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rubyer.Converters
{
    public class ListViewGridViewConverter : IValueConverter
    {
        public Style DefaultStyle { get; set; }

        public Style ViewStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ListView listView)
            {
                return listView.View != null ? ViewStyle : DefaultStyle;
            }

            return value is ViewBase ? ViewStyle : DefaultStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
