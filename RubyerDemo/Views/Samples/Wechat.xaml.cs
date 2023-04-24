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
    /// Wechat.xaml 的交互逻辑
    /// </summary>
    public partial class Wechat : RubyerWindow
    {
        public Wechat()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetRequiredService<WechatViewModel>();
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            await Task.Delay(500);
            e.Cancel = true;
            this.Hide();
        }
    }
}
