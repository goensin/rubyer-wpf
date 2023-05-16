using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// 轮播图项尺度转换
    /// </summary>
    public class CarouselScaleConvereter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = ValidateArgument.NotNullOrEmptyCast<FlipViewItem>(values[0], nameof(values));
            var parent = ValidateArgument.NotNullOrEmptyCast<ScrollViewer>(values[1], nameof(values));

            var vector = VisualTreeHelper.GetOffset(item);
            var point = item.TranslatePoint(new Point(), parent);
            return parent.ViewportWidth * 0.8;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
