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
    /// BreadcrumbDemo.xaml 的交互逻辑
    /// </summary>
    public partial class BreadcrumbDemo : UserControl
    {
        public List<BreadcrumbInfo> Infos => [
                                                new BreadcrumbInfo("设置", "settings"),
                                                new BreadcrumbInfo("用户设置", "user-settings"),
                                                new BreadcrumbInfo("通用设置", "general-settings"),
                                                new BreadcrumbInfo("手机设置", ""),
                                             ];

        public BreadcrumbDemo()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Breadcrumb_Navigate(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            Notification.Show(e.NewValue, title: "Navigate");
        }
    }

    public class BreadcrumbInfo
    {
        public string Text { get; set; }
        public string Href { get; set; }

        public BreadcrumbInfo(string text, string href)
        {
            Text = text;
            Href = href;
        }
    }
}
