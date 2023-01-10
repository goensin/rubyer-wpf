using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
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
    /// RenamerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RenamerDemo : UserControl
    {
        public RenamerDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<RenamerViewModel>();
        }

        private void renamer_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            Debug.WriteLine($"Renamer TextChanged: {e.NewValue}");
        }
    }
}
