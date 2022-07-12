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
        }

        private void NumericBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            Debug.WriteLine($"NumericBox ValueChanged: {e.NewValue}");
        }
    }
}