using Rubyer;
using System;
using System.Collections.Generic;
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
    /// Message.xaml 的交互逻辑
    /// </summary>
    public partial class MessageInfo : UserControl
    {
        public MessageInfo()
        {
            InitializeComponent();
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("info", 3000, true);
        }

        private void WaringBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show(MessageType.Warning, "warning", 3000, true);
        }

        private void SuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show(MessageType.Success, "success", 3000, true);
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show(MessageType.Error, "erroooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor", 0, true);
        }

        private void InfoContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("MainContainer", "info", 3000, true);
        }

        private void WaringContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("MainContainer", MessageType.Warning, "warning", 3000, true);
        }

        private void SuccessContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("MainContainer", MessageType.Success, "success", 3000, true);
        }

        private void ErrorContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("MainContainer", MessageType.Error, "error", 3000);
        }
    }
}
