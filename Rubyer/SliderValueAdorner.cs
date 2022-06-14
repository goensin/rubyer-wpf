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
    //public class SliderValueAdorner : Adorner
    //{
    //    private TextBlock valueTextBlock;
    //    private Card valueCard;

    //    public SliderValueAdorner(UIElement adornedElement, Slider slider)
    //      : base(adornedElement)
    //    {
    //        valueTextBlock = new TextBlock();

    //        var binding = new Binding();
    //        binding.Source = slider;
    //        binding.Path = new PropertyPath("Value");
    //        valueTextBlock.SetBinding(TextBlock.TextProperty, binding);

    //        //    < rubyer:Card x:Name = "valueBorder"
    //        //             Padding = "10 5"
    //        //             Background = "{DynamicResource DefaultForeground}"
    //        //             IsHitTestVisible = "False"
    //        //             TextBlock.Foreground = "{DynamicResource DefaultBackground}" >
    //        //    < TextBlock Text = "{Binding Value, RelativeSource={RelativeSource AncestorType=Slider}}" />
    //        //</ rubyer:Card >

    //        valueCard = new Card
    //        {
    //            //Background = (Brush)Application.Current.FindResource("DefaultBackground"),
    //            Padding = new Thickness(10, 5, 10, 5),
    //            Content = valueTextBlock,
    //            Margin = new Thickness(5, 5, 5, 5),
    //        };
    //        //valueTransition = (Transition)Application.Current.FindResource("SliderValueTransition");
    //        //var binding = new Binding();
    //        //binding.Source = adornedElement;
    //        //binding.Path = new PropertyPath(nameof(Thumb.IsMouseOverProperty));
    //        //valueTransition.SetBinding(Transition.IsShowProperty, binding);
    //    }

    //    protected override Visual GetVisualChild(int index) => valueCard;

    //    protected override int VisualChildrenCount => 1;

    //    protected override Size ArrangeOverride(Size finalSize)
    //    {
    //        valueTextBlock.Arrange(new Rect(new Point(0, -20), valueTextBlock.DesiredSize));
    //        return base.ArrangeOverride(finalSize);
    //    }
    //}

    public class SliderValueAdorner : Adorner
    {
        private TextBlock _textBlock;
        private Card valueCard;

        public SliderValueAdorner(UIElement adornedElement, Slider slider)
          : base(adornedElement)
        {
            _textBlock = new TextBlock();
            _textBlock.Text = "AdornerText";
            var binding = new Binding();
            binding.Source = slider;
            binding.Path = new PropertyPath("Value");
            _textBlock.SetBinding(TextBlock.TextProperty, binding);

            valueCard = new Card
            {
                //Background = (Brush)Application.Current.FindResource("DefaultBackground"),
                Padding = new Thickness(5),
                Content = _textBlock,
            };

            var binding2 = new Binding();
            binding2.Source = adornedElement;
            binding2.Path = new PropertyPath(nameof(IsMouseOver));
            binding2.Converter = new Converters.BooleanToVisibilityConverter();
            valueCard.SetBinding(Card.VisibilityProperty, binding2);
        }

        protected override Visual GetVisualChild(int index)
        {
            return valueCard;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            valueCard.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return base.MeasureOverride(valueCard.DesiredSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            valueCard.Arrange(new Rect(new Point(-50, -45), valueCard.DesiredSize));
            return base.ArrangeOverride(finalSize);
        }
    }
}
