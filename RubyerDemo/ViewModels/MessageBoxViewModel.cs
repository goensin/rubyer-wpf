using Rubyer;
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
            switch (obj.ToString())
            {
                case "1":
                    var result = MessageBoxR.Show("有消息消息消息消息", "有标题");
                    break;
                case "2":
                    MessageBox.Show("有消息", "有标题");
                    break;
                case "3":
                    MessageBox.Show("有消息,有按钮", "有标题", MessageBoxButton.YesNo);
                    break;
                case "4":
                    MessageBox.Show("有消息,有按钮,有图标", "有标题", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    break;
                case "5":
                    MessageBox.Show("有消息,有按钮,有图标,有默认结果", "有标题", MessageBoxButton.YesNo, MessageBoxImage.Stop, MessageBoxResult.OK);
                    break;
                default:
                    break;
            }

        }
    }
}
