using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using System.Windows.Controls;

namespace RubyerDemo.Views
{
    /// <summary>
    /// ListViewDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ListViewDemo : UserControl
    {
        public ListViewDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<ListViewModel>();
        }
    }
}
