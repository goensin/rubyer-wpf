using System.Windows.Data;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// 对话框 DataContext 配置
    /// </summary>
    public class DialogDataContextConfiguration : IDialogDataContextConfiguration
    {
        private object openParameter;
        private IDialogDataContext dialogContext;
        private DialogContainer dialog;

        private void Dialog_BeforeOpen(object sender, RoutedEventArgs e)
        {
            dialogContext.OnDialogOpened(openParameter);
        }

        private void DialogContext_RequestClose(object parameter)
        {
            DialogContainer.CloseDialogCommand.Execute(parameter, dialog);
        }

        /// <inheritdoc/>
        public bool OnOpenAction(DialogContainer dialog, object content, object parameter)
        {
            if (content is FrameworkElement element && element.DataContext is IDialogDataContext dialogContext)
            {
                openParameter = parameter;
                this.dialogContext = dialogContext;
                this.dialog = dialog;

                // 绑定标题
                var binding = new Binding(nameof(dialog.Title))
                {
                    Source = dialogContext,
                    Mode = BindingMode.OneWay
                };

                dialog.SetBinding(DialogContainer.TitleProperty, binding);

                dialog.BeforeOpen += Dialog_BeforeOpen; // 传参到对话框打开 viewmodel

                dialogContext.RequestClose += DialogContext_RequestClose; // 设置关闭对话框委托

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void OnCloseAction()
        {
            if (dialog is { })
            {
                dialog.BeforeOpen -= Dialog_BeforeOpen;
            }

            if (dialogContext is { })
            {
                dialogContext.RequestClose -= DialogContext_RequestClose;
            }

            openParameter = null;
            dialogContext = null;
            dialog = null;
        }
    }
}
