using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using RubyerDemo.Consts;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    public partial class MessageBoxViewModel : ObservableObject
    {
        [RelayCommand]
        private void MessageBoxShow(string num)
        {
            switch (num)
            {
                case "1":
                    MessageBoxR.ShowGlobal("有消息消息消息消息消息", "标题名称");
                    break;

                case "2":
                    var result = MessageBoxR.ConfirmGlobal("是否删除改数据?", "提示");
                    Message.Show($"返回结果为 {result}");
                    break;

                case "3":
                    MessageBoxR.InfoGlobal("消息消息消息消息消息消息", "标题名称", MessageBoxButton.YesNo);
                    break;

                case "4":
                    MessageBoxR.WarningGlobal("警告警告警告警告警告警告", "标题名称", MessageBoxButton.YesNo);
                    break;

                case "5":
                    MessageBoxR.SuccessGlobal("成功成功成功成功成功成功", "标题名称", MessageBoxButton.OK);
                    break;

                case "6":
                    MessageBoxR.ErrorGlobal("错误错误错误错误错误错误", "标题名称", MessageBoxButton.YesNoCancel);
                    break;
            }
        }

        [RelayCommand]
        private async void MessageBoxShowContainer(string num)
        {
            switch (num)
            {
                case "1":
                    await MessageBoxR.Show("有消息消息消息消息消息");
                    break;

                case "2":
                    var result = await MessageBoxR.Confirm("是否删除改数据?");
                    Message.Show($"返回结果为 {result}");
                    break;

                case "3":
                    await MessageBoxR.Info("消息消息消息消息消息消息", button: MessageBoxButton.YesNo);
                    break;

                case "4":
                    await MessageBoxR.Warning("警告警告警告警告警告警告", button: MessageBoxButton.YesNo);
                    break;

                case "5":
                    await MessageBoxR.Success("成功成功成功成功成功成功", button: MessageBoxButton.OK);
                    break;

                case "6":
                    await MessageBoxR.Error("错误错误错误错误错误错误", button: MessageBoxButton.YesNoCancel);
                    break;

                default:
                    break;
            }
        }
    }
}