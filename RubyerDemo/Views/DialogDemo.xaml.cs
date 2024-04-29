using Microsoft.Extensions.DependencyInjection;
using Rubyer;
using Rubyer.Models;
using RubyerDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// DialogView.xaml 的交互逻辑
    /// </summary>
    public partial class DialogDemo : UserControl
    {
        public DialogDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<DialogViewModel>();
        }

        private void Dialog2_BeforeOpen(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("打开 2# 对话框前事件");
        }

        private void Dialog2_AfterClose(object sender, Rubyer.DialogResultRoutedEventArgs e)
        {
            Debug.WriteLine($"关闭 2# 对话框后事件");
        }
    }
}