using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    /// Transition.xaml 的交互逻辑
    /// </summary>
    [INotifyPropertyChanged]
    public partial class TransitionDemo : UserControl
    {
        public TransitionDemo()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Transition_Showed(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("transition showed event");
        }

        private void Transition_Closed(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("transition closed event");
        }

        [RelayCommand]
        private void Showed()
        {
            Debug.WriteLine("transition showed command");
        }

        [RelayCommand]
        private void Closed(object obj)
        {
            Debug.WriteLine("transition closed command");
        }
    }
}
