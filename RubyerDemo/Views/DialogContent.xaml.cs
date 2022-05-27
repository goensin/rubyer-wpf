using Rubyer;
using RubyerDemo.Consts;
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
    /// DialogContent.xaml 的交互逻辑
    /// </summary>
    public partial class DialogContent : UserControl
    {
        public DialogContent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // DialogBox.CloseDialogCommand.Execute($"用户名:{userName.Text};密码:{password.Password}", null);

            Dialog.Close(ConstNames.MainDialogBox, $"用户名:{userName.Text};密码:{password.Password}");
        }
    }
}
