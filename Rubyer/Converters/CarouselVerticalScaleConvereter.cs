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
    /// 轮播图垂直尺度转换
    /// </summary>
    public class CarouselVerticalScaleConvereter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var scaleTransform = values[2] as ScaleTransform;
            if (!(values[0] is FlipViewItem item) || !(values[1] is double))
            {
                return scaleTransform.ScaleY;
            }

            var scrollViewer = item.TryGetParentFromVisualTree<ScrollViewer>();
            if (scrollViewer == null || scrollViewer.Name != "PART_ContentScrollViewer")
            {
                return scaleTransform.ScaleY;
            }

            var point = new Point(0, -(scrollViewer.ViewportHeight - item.ActualHeight) / 2);
            var targetPosition = item.TransformToVisual(scrollViewer).Transform(point);
            var different = Math.Abs(targetPosition.Y);
            if (different > scrollViewer.ViewportHeight)
            {
                return 1D;
            }

            var percent = 1 - different / scrollViewer.ViewportHeight * 2;
            return 1 + percent;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
