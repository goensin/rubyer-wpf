using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    public class MessageCard : ContentControl
    {
        static MessageCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        public static readonly DependencyProperty CornerRadiusProperty =
          DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessageCard), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        public static readonly DependencyProperty ThemeColorBrushProperty =
            DependencyProperty.Register("ThemeColorBrush", typeof(SolidColorBrush), typeof(MessageCard), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush ThemeColorBrush
        {
            get { return (SolidColorBrush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }


        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType", typeof(IconType), typeof(MessageCard), new PropertyMetadata(default(IconType)));

        public IconType IconType
        {
            get { return (IconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }


        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register("IsClearable", typeof(bool), typeof(MessageCard), new PropertyMetadata(default(bool)));

        public bool IsClearable
        {
            get { return (bool)GetValue(IsClearableProperty); }
            set { SetValue(IsClearableProperty, value); }
        }

    }
}
