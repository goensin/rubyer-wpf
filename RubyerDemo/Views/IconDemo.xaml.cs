using Rubyer;
using RubyerDemo.Consts;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace RubyerDemo.Views
{
    /// <summary>
    /// Icon.xaml 的交互逻辑
    /// </summary>
    public partial class IconDemo : UserControl
    {
        public IconDemo()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            string url = link.NavigateUri.AbsoluteUri.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            currentTextBox.SelectAll();
            ApplicationCommands.Copy.Execute(currentTextBox.Text, currentTextBox);
            Message.SuccessInContainer(ConstNames.MainMessageContainer, "复制成功");
        }
    }
}
