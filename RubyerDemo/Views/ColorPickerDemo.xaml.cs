using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace RubyerDemo.Views
{
    /// <summary>
    /// ColorPickerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerDemo : UserControl
    {
        public ColorPickerDemo()
        {
            InitializeComponent();

            colorPalette.OptionalColors = [
                Colors.White,
                Colors.Black,
                Colors.Red,
                Colors.Blue,
                Colors.Green,
                Colors.Purple,
                Colors.Orange,
                Colors.Pink,
                Color.FromArgb(200, 255, 0, 0),
                Color.FromArgb(160, 0, 255, 100),
                Color.FromArgb(120, 100, 50, 0),
                Color.FromArgb(80, 0, 50, 100),
                Color.FromArgb(40, 50, 50, 50)];
        }

        private void colorPalette_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Debug.WriteLine($"color: {e.NewValue}");
        }
    }
}
