using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyerDemo.Views
{
    /// <summary>
    /// SelectBox.xaml 的交互逻辑
    /// </summary>
    public partial class SelectBoxDemo : UserControl
    {
        public SelectBoxDemo()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                Color lightForegroundColor = (Color)App.Current.FindResource("LightForegroundColor");
                Color lightBackgroundColor = (Color)App.Current.FindResource("LightBackgroundColor");
                Color darkForegroundColor = (Color)App.Current.FindResource("DarkForegroundColor");
                Color darkBackgroundColor = (Color)App.Current.FindResource("DarkBackgroundColor");


                if (radio.Name == "day")
                {
                    App.Current.Resources["DefaultForeground"] = new SolidColorBrush(lightForegroundColor);
                    App.Current.Resources["DefaultBackground"] = new SolidColorBrush(lightBackgroundColor);
                   
                }
                else
                {
                    App.Current.Resources["DefaultForeground"] = new SolidColorBrush(darkForegroundColor);
                    App.Current.Resources["DefaultBackground"] = new SolidColorBrush(darkBackgroundColor);
                }
            }
        }
    }
}
