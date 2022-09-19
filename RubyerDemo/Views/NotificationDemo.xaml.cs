using Rubyer;
using RubyerDemo.Consts;
using System;
using System.Collections.Generic;
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
    /// NotificationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationDemo : UserControl
    {
        public NotificationDemo()
        {
            InitializeComponent();
        }

        private void NotificationBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.ShowGlobal("message", "title");
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.InfoGlobal("hello ~ \r\nrubyer ", "title");
        }

        private void WaringBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.WarningGlobal("warning", "title");
        }

        private void SuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.SuccessGlobal("success", "title");
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.ErrorGlobal("erroooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor", "title", millisecondTimeOut: 0);
        }

        private void NotificationContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Show(ConstNames.NotificationDemo, "message", "title");
        }

        private void InfoContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Info(ConstNames.NotificationDemo, "info", "title");
        }

        private void WaringContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Warning(ConstNames.NotificationDemo, "warning", "title");
        }

        private void SuccessContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Success(ConstNames.NotificationDemo, "success", "title");
        }

        private void ErrorContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Error(ConstNames.NotificationDemo, "error", "title", millisecondTimeOut: 0);
        }
    }
}