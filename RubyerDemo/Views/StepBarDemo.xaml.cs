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
    /// StepBarDemo.xaml 的交互逻辑
    /// </summary>
    public partial class StepBarDemo : UserControl
    {
        public StepBarDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<StepBarViewModel>();
        }

        private void StepBar_CurrentItemChanged(object sender, RoutedPropertyChangedEventArgs<Rubyer.StepBarItem> e)
        {
            Debug.WriteLine($"Old StepBarItem:{e.OldValue}, New StepBarItem:{e.NewValue}");
        }
    }
}
