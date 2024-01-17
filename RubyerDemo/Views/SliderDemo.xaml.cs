using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyerDemo.Views
{
    /// <summary>
    /// SliderDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SliderDemo : UserControl
    {
        public SliderDemo()
        {
            InitializeComponent();
        }

        private void renamer_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            Debug.WriteLine($"Renamer TextChanged: {e.NewValue}");
        }

        private void Slider_SelectionRangeChanged(object sender, RoutedEventArgs e)
        {
            var slider=(Slider)sender;
            Debug.WriteLine($"Slider SelectionRangeChanged: {slider.SelectionStart} - {slider.SelectionEnd}");
        }
    }
}
