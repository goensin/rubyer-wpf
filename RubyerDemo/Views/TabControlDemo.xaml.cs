using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
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
    }
}
