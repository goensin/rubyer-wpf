using Rubyer;
using RubyerDemo.Consts;
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
    public partial class MessageDemo : UserControl
    {
        public MessageDemo()
        {
            InitializeComponent();
        }

        private void MessageBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("message");
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Info("hello ~ \r\nrubyer ");
        }

        private void WaringBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Warning("warning");
        }

        private void SuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Success("success");
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.Error("erroooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor", 0);
        }

        private void MessageContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInContainer(ConstNames.MessageDemo, "message");
        }

        private void InfoContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.InfoInContainer(ConstNames.MessageDemo, "info");
        }

        private void WaringContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.WarningInContainer(ConstNames.MessageDemo, "warning");
        }

        private void SuccessContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.SuccessInContainer(ConstNames.MessageDemo, "success");
        }

        private void ErrorContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            Message.ErrorInContainer(ConstNames.MessageDemo, "error");
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
            Message.ShowInContainer(ConstNames.MessageDemo, element);
        }
    }
}
