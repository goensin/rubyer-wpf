using Rubyer;
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
                    Message.ShowInContainer("MessageContainer", $"返回结果为 {result}");
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

        private async void MessageBoxShowContainerExecute(object obj)
        {
            switch (obj.ToString())
            {
                case "1":
                    await MessageBoxR.ShowInContainer("DialogContaioner", "有消息消息消息消息消息");
                    break;
                case "2":
                    var result = await MessageBoxR.ConfirmInContainer("DialogContaioner", "是否删除改数据?", "提示");
                    Message.ShowInContainer("MessageContainer", $"返回结果为 {result}");
                    break;
                case "3":
                    await MessageBoxR.InfoInContainer("DialogContaioner", "消息消息消息消息消息消息", "标题名称", MessageBoxButton.YesNo);
                    break;
                case "4":
                    await MessageBoxR.WaringInContainer("DialogContaioner", "警告警告警告警告警告警告", "标题名称", MessageBoxButton.YesNo);
                    break;
                case "5":
                    await MessageBoxR.SuccessInContainer("DialogContaioner", "成功成功成功成功成功成功", "标题名称", MessageBoxButton.OK);
                    break;
                case "6":
                    await MessageBoxR.ErrorInContainer("DialogContaioner", "错误错误错误错误错误错误", "标题名称", MessageBoxButton.YesNoCancel);
                    break;
                default:
                    break;
            }
        }
    }
}
