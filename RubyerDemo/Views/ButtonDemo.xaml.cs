using Rubyer;
using RubyerDemo.Consts;
using System;
using System.Collections.Generic;
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
    /// Button.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonDemo : UserControl
    {
        public ButtonDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonHelper.GetLoading(loadingButton))
            {
                ButtonHelper.SetLoading(loadingButton, false);
            }
            else
            {
                ButtonHelper.SetLoading(loadingButton, true);
            }
        }
    }
}