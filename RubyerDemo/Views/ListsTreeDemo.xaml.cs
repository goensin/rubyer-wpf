using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using System.Windows.Controls;

namespace RubyerDemo.Views
{
    /// <summary>
    /// Lists.xaml 的交互逻辑
    /// </summary>
    public partial class ListsTreeDemo : UserControl
    {
        public ListsTreeDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<ListsViewModel>();
        }
    }
}
