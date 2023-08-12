using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using System;
using System.Collections.Generic;
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
    /// TreeListViewDemo.xaml 的交互逻辑
    /// </summary>
    public partial class TreeListViewDemo : UserControl
    {
        public TreeListViewDemo()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<TreeViewViewModel>();
        }
    }
}
