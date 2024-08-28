using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 可裁剪 Boder
    /// </summary>
    public class ClippingBorder : Border
    {
        /// <inheritdoc/>
        protected override void OnRender(DrawingContext dc)
        {
            if (Child is { })
            {
                var topLeft = CornerRadius.TopLeft - (Math.Max(BorderThickness.Top, BorderThickness.Left) / 2);
                var bottomLeft = CornerRadius.BottomLeft - (Math.Max(BorderThickness.Bottom, BorderThickness.Left) / 2);
                var topRight = CornerRadius.TopRight - (Math.Max(BorderThickness.Top, BorderThickness.Right) / 2);
                var bottomRight = CornerRadius.BottomRight - (Math.Max(BorderThickness.Bottom, BorderThickness.Right) / 2);
                if (topLeft < 0)
                    topLeft = 0;
                if (bottomLeft < 0)
                    bottomLeft = 0;
                if (topRight < 0)
                    topRight = 0;
                if (bottomRight < 0)
                    bottomRight = 0;
                var mask = new Border()
                {
                    CornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft),
                    Height = ActualHeight - BorderThickness.Top - BorderThickness.Bottom,
                    Width = ActualWidth - BorderThickness.Left - BorderThickness.Right,
                    Background = Brushes.Black,
                    SnapsToDevicePixels = true,
                };

                Child.OpacityMask = new VisualBrush(mask);
            }

            base.OnRender(dc);
        }
    }
}
