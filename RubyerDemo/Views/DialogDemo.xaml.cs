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
        }

        private void Dialog3_BeforeOpen(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("打开 3# 对话框前事件");
        }

        private void Dialog3_AfterClose(object sender, Rubyer.DialogResultRoutedEventArgs e)
        {
            var parameters = (IParameters)e.Result;
            if (parameters != null)
            {
                var user = parameters.GetValue<User>("User");
                Debug.WriteLine($"关闭 3# 对话框后事件，参数:{user.Name}:{user.Age}");
            }
        }
    }
}
