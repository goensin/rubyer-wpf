using Microsoft.Extensions.DependencyInjection;
using Rubyer;
using RubyerDemo.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace RubyerDemo.Views
{
    /// <summary>
    /// ContainerControl.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlDemo : UserControl
    {
        public TabControlDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<TabControlViewModel>();
        }

        private void TabControl_CloseItem(object sender, CloseTabItemRoutedEventArgs e)
        {
            Debug.WriteLine("TabControl CloseItem");
            Debug.WriteLine($"子项：{e.OriginalSource}");
            Debug.WriteLine($"子项数据：{e.Item}");
            var result = MessageBox.Show("是否确认关闭子项？", "提示", button: MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
