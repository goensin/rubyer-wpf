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
                    MessageBoxR.Show("有消息消息消息消息消息", "标题名称");
                    break;
                case "2":
                    var result = MessageBoxR.Confirm("是否删除改数据?", "提示");
                    Message.Show("MessageContainer", $"返回结果为 {result}");
                    break;
                case "3":
                    MessageBoxR.Info("消息消息消息消息消息消息", "标题名称", MessageBoxButton.YesNo);
                    break;
                case "4":
                    MessageBoxR.Waring("警告警告警告警告警告警告", "标题名称", MessageBoxButton.YesNo);
                    break;
                case "5":
                    MessageBoxR.Success("成功成功成功成功成功成功", "标题名称", MessageBoxButton.OK);
                    break;
                case "6":
                    MessageBoxR.Error("错误错误错误错误错误错误", "标题名称", MessageBoxButton.YesNoCancel);
                    break;
            }
        }

        private RelayCommand messageBoxShowContainer;
        public RelayCommand MessageBoxShowContainer => messageBoxShowContainer ?? (messageBoxShowContainer = new RelayCommand(MessageBoxShowContainerExecute));

        private void MessageBoxShowContainerExecute(object obj)
        {
            switch (obj.ToString())
            {
                case "1":
                    MessageBoxR.ShowInContainer("DialogContaioner", "有消息消息消息消息消息");
                    break;
                case "2":
                    MessageBoxR.ConfirmInContainer("DialogContaioner", "是否删除改数据?", "提示", ConfirmReturnResult);
                    break;
                case "3":
                    MessageBoxR.InfoInContainer("DialogContaioner", "消息消息消息消息消息消息", "标题名称", null, MessageBoxButton.YesNo);
                    break;
                case "4":
                    MessageBoxR.WaringInContainer("DialogContaioner", "警告警告警告警告警告警告", "标题名称", null, MessageBoxButton.YesNo);
                    break;
                case "5":
                    MessageBoxR.SuccessInContainer("DialogContaioner", "成功成功成功成功成功成功", "标题名称", null, MessageBoxButton.OK);
                    break;
                case "6":
                    MessageBoxR.ErrorInContainer("DialogContaioner", "错误错误错误错误错误错误", "标题名称", null, MessageBoxButton.YesNoCancel);
                    break;
                default:
                    break;
            }
        }

        private void ConfirmReturnResult(object sender, MessageBoxResultRoutedEventArge e)
        {
            Message.Show("MessageContainer", $"返回结果为 {e.Result}");
        }
    }
}
