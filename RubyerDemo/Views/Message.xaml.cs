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
    public partial class Message : UserControl
    {
        public Message()
        {
            InitializeComponent();
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow messageWindow = MessageWindow.GetInstance();
            messageWindow.Owner = App.Current.MainWindow;
            messageWindow.AddMessageCard(new MessageCard() { Content = "info", IsClearable = true }, 0);
            messageWindow.Show();
        }
    }
}
