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

        private RelayCommand showedCommand;
        public RelayCommand ShowedCommand => showedCommand ?? (showedCommand = new RelayCommand(ShowedExecute));

        private void ShowedExecute(object obj)
        {
            Debug.WriteLine("transition showed command");
        }

        private RelayCommand closedCommand;
        public RelayCommand ClosedCommand => closedCommand ?? (closedCommand = new RelayCommand(ClosedExecute));

        private void ClosedExecute(object obj)
        {
            Debug.WriteLine("transition closed command");
        }
    }
}
