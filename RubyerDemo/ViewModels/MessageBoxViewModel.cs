using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    public class MessageBoxViewModel : ViewModelBase
    {
        private RelayCommand messageBoxShow;
        public RelayCommand MessageBoxShow => messageBoxShow ?? (messageBoxShow = new RelayCommand(MessageBoxShowExecute));
        private void MessageBoxShowExecute(object obj)
        {
            MessageBox.Show("只有消息");
            MessageBox.Show("只有消息");
        }
    }
}
