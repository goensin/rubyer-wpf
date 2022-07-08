using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// Slider 当前值装饰器
    /// </summary>
    public class SliderValueAdorner : Adorner
    {
        private readonly TextBlock valueTextBlock;
        private readonly Card card;
        private readonly Dock placement;
        private readonly double offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderValueAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">添加装饰器控件</param>
        /// <param name="slider">Slider 控件</param>
        /// <param name="placement">显示位置</param>
        /// <param name="offset">偏移</param>
        public SliderValueAdorner(UIElement adornedElement, Slider slider, Dock placement, double offset)
          : base(adornedElement)
        {
            this.placement = placement;
            this.offset = offset;

            var binding = new Binding
            {
                Source = slider,
                Path = new PropertyPath("Value")
            };

            valueTextBlock = new TextBlock
            {
                FontSize = 13,
                Foreground = (Brush)Application.Current.FindResource("DefaultBackground")
            };

            valueTextBlock.SetBinding(TextBlock.TextProperty, binding);

            card = new Card
            {
                Background = (Brush)Application.Current.FindResource("DefaultForeground"),
                Padding = new Thickness(10, 5, 10, 5),
                Content = valueTextBlock,
            };
        }

        /// <inheritdoc/>
        protected override Visual GetVisualChild(int index) => card;

        /// <inheritdoc/>
        protected override int VisualChildrenCount => 1;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            card.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return card.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var cardSize = card.DesiredSize;
            var elementSize = AdornedElement.RenderSize;
            double x = 0;
            double y = 0;

            switch (placement)
            {
                case Dock.Left:
                    x = cardSize.Width * -1 + offset;
                    y = cardSize.Height / -2 + elementSize.Height / 2;
                    break;

                case Dock.Top:
                    x = cardSize.Width / -2 + elementSize.Width / 2;
                    y = cardSize.Height * -1 + offset;
                    break;

                case Dock.Right:
                    x = elementSize.Width + offset;
                    y = cardSize.Height / -2 + elementSize.Height / 2;
                    break;

                case Dock.Bottom:
                    x = cardSize.Width / -2 + elementSize.Width / 2;
                    y = elementSize.Height + offset;
                    break;
            }

            var rect = new Rect(new Point(x, y), cardSize);
            card.Arrange(rect);
            return base.ArrangeOverride(finalSize);
        }
    }
}