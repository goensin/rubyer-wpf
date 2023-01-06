using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace RubyerDemo.Views
{
    /// <summary>
    /// TextBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBoxDemo : UserControl
    {
        public InputBoxDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<InputBoxViewModel>();
        }

        private void NumericBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Debug.WriteLine($"NumericBox ValueChanged: {e.NewValue}");
        }

        private void renamer_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            Debug.WriteLine($"Renamer TextChanged: {e.NewValue}");
        }
    }
}