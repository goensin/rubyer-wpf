using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
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
    /// PageBar.xaml 的交互逻辑
    /// </summary>
    public partial class PageBarDemo : UserControl
    {
        public PageBarDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<PageBarViewModel>();
        }

        private void PageBar_PageIndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            Debug.WriteLine($"page index : {e.OldValue} => {e.NewValue}");
        }

        private void PageBar_PageSizeChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            Debug.WriteLine($"page size : {e.OldValue} => {e.NewValue}");
        }
    }
}
