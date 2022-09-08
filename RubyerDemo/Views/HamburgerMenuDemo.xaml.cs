using Rubyer;
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
    /// HamburgerMenuDemo.xaml 的交互逻辑
    /// </summary>
    public partial class HamburgerMenuDemo : UserControl
    {
        public HamburgerMenuDemo()
        {
            InitializeComponent();
        }

        private void HamburgerMenu_HamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Current HamburgerMenu IsExpanded:{(sender as HamburgerMenu).IsExpanded}");

            // e.Handled = true; // 取消切换
        }
    }
}