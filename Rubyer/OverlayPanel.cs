using System;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 可覆盖面板
    /// </summary>
    public class OverlayPanel : Panel
    {
        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size();
            foreach (UIElement child in InternalChildren)
            {
                if (child == null) continue;
                child.Measure(availableSize);
                desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
                desiredSize.Height = Math.Max(desiredSize.Height, child.DesiredSize.Height);
            }
            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                if (child == null) continue;
                child.Arrange(new Rect(new Point(0, 0), finalSize)); // 所有控件都放在左上角重叠
            }
            return finalSize;
        }
    }
}
