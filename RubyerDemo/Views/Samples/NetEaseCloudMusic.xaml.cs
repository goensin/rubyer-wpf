using Microsoft.Extensions.DependencyInjection;
using Rubyer;
using RubyerDemo.ViewModels.Samples;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace RubyerDemo.Views.Samples
{
    /// <summary>
    /// NetEaseCloudMusic.xaml 的交互逻辑
    /// </summary>
    public partial class NetEaseCloudMusic : RubyerWindow
    {
        public NetEaseCloudMusic()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetRequiredService<NetEaseCloudMusicViewModel>();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Notification.SuccessGlobal("hello rubyer ~~", millisecondTimeOut: 0);
        }
    }
}
