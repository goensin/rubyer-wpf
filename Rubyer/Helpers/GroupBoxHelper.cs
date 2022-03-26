using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public class GroupBoxHelper
    {
        // HeaderBackground
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderBackground", typeof(SolidColorBrush), typeof(GroupBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetHeaderBackground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(HeaderBackgroundProperty);
        }

        public static void SetHeaderBackground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(HeaderBackgroundProperty, value);
        }
    }
}
