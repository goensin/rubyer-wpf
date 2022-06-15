using Rubyer.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Rubyer
{

    public class SliderValueAdorner : Adorner
    {
        private TextBlock valueTextBlock;
        private Card card;
        private Dock placement;
        private double offset;

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

        protected override Visual GetVisualChild(int index) => card;

        protected override int VisualChildrenCount => 1;

        protected override Size MeasureOverride(Size constraint)
        {
            card.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return card.DesiredSize;
        }

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
