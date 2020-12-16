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

        private void MessageBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("message");
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInfo("info");
        }

        private void WaringBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowWarning("warning");
        }

        private void SuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowSuccess("success");
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowError("erroooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor", 0);
        }

        private void MessageContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("MessageContainer", "message");
        }

        private void InfoContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInfo("MessageContainer", "info");
        }

        private void WaringContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowWarning("MessageContainer", "warning");
        }

        private void SuccessContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowSuccess("MessageContainer", "success");
        }

        private void ErrorContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowError("MessageContainer", "error");
        }

        private void ControlBtn_Click(object sender, RoutedEventArgs e)
        {
            string xaml = System.Windows.Markup.XamlWriter.Save(CustomContent);
            UIElement element = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
            Message.Show(element);
        }

        private void ControlContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            string xaml = System.Windows.Markup.XamlWriter.Save(CustomContent);
            UIElement element = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
            Message.Show("MessageContainer", element);
        }
    }
}
