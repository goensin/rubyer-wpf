using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Rubyer
{
    public class SliderValueAdorner : Adorner
    {
        TextBlock textBlock;
        Button button;
        public SliderValueAdorner(UIElement adornedElement) : base(adornedElement)
        {

            textBlock = new TextBlock();
            textBlock.Foreground = Brushes.Blue;
            textBlock.Width = 20;
            textBlock.Height = 30;
            textBlock.Text = "88";
            //Binding binding = new Binding();
            //binding.Source = slider;
            //binding.Path = new PropertyPath(nameof(slider.Value));
            //binding.Mode = BindingMode.OneWay;
            //textBlock.SetBinding(TextBlock.TextProperty, binding);

            button = new Button();
            button.Content = "123";
            button.Width = 80;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            textBlock.Arrange(new Rect(-5, -5, 30, 20));
            button.Arrange(new Rect(0, 0, 80, 28));
            return base.ArrangeOverride(finalSize);
        }
    }
}
