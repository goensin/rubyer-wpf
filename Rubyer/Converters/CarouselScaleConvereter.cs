using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer.Converters
{
    /// <summary>
    /// 轮播图水平尺度转换
    /// </summary>
    public class CarouselScaleConvereter : IMultiValueConverter
    {
        private static double HorizontalScale(object[] values, ScaleTransform scaleTransform)
        {
            if (!(values[0] is FlipViewItem item) || !(values[3] is double))
            {
                return scaleTransform.ScaleX;
            }

            var scrollViewer = item.TryGetParentFromVisualTree<ScrollViewer>();
            if (scrollViewer == null || scrollViewer.Name != "PART_ContentScrollViewer")
            {
                return scaleTransform.ScaleX;
            }

            var point = new Point(-(scrollViewer.ViewportWidth - item.ActualWidth) / 2, 0);
            var targetPosition = item.TransformToVisual(scrollViewer).Transform(point);
            var different = Math.Abs(targetPosition.X);
            if (different > scrollViewer.ViewportWidth)
            {
                return 1D;
            }

            var percent = 1 - different / scrollViewer.ViewportWidth * 2;
            return 1 + percent;
        }

        private static double VerticalScale(object[] values, ScaleTransform scaleTransform)
        {
            if (!(values[0] is FlipViewItem item) || !(values[4] is double))
            {
                return scaleTransform.ScaleX;
            }

            var scrollViewer = item.TryGetParentFromVisualTree<ScrollViewer>();
            if (scrollViewer == null || scrollViewer.Name != "PART_ContentScrollViewer")
            {
                return scaleTransform.ScaleX;
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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var scaleTransform = values[1] as ScaleTransform;
            var orientation = (Orientation)values[2];

            if (orientation == Orientation.Horizontal)
            {
                return HorizontalScale(values, scaleTransform);
            }
            else
            {
                return VerticalScale(values, scaleTransform);
            }
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
