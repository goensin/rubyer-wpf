using Microsoft.Extensions.DependencyInjection;
using Rubyer;
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

namespace RubyerDemo.Views.Dialogs
{
    /// <summary>
    /// Dialog2View.xaml 的交互逻辑
    /// </summary>
    public partial class Dialog2View : UserControl
    {
        public Dialog2View()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetRequiredService<Dialog2ViewModel>();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialogCard = this.TryGetParentFromVisualTree<DialogCard>();

            var view = new Dialog2View();
            await Dialog.Show("Dialog2", view, parameters: dialogCard.Title + "~");
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            Dialog.Clear("Dialog2");
        }
    }
}
