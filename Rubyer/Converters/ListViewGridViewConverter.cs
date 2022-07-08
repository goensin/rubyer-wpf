using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// ListView 的 GridView Style 选择转换器
    /// </summary>
    public class ListViewGridViewConverter : IValueConverter
    {
        /// <summary>
        /// 默认 ListView 样式
        /// </summary>
        public Style DefaultStyle { get; set; }

        /// <summary>
        /// GridView 样式
        /// </summary>
        public Style ViewStyle { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ListView listView)
            {
                return listView.View != null ? ViewStyle : DefaultStyle;
            }

            return value is ViewBase ? ViewStyle : DefaultStyle;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}