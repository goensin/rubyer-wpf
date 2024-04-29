using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// 对话框 DataContext 配置
    /// </summary>
    public class DialogDataContextConfiguration : IDialogDataContextConfiguration
    {
        /// <inheritdoc/>
        public bool OnOpenAction(DialogContainer container, DialogCard dialogCard, object content, object openParameter)
        {
            if (content is FrameworkElement element && element.DataContext is IDialogDataContext dialogContext)
            {
                // 绑定标题
                var binding = new Binding(nameof(dialogContext.Title))
                {
                    Source = dialogContext,
                    Mode = BindingMode.OneWay
                };

                dialogCard.SetBinding(DialogCard.TitleProperty, binding);

                // 传参到对话框打开 viewmodel
                dialogCard.BeforeOpenHandler += dialogCard =>
                {
                    dialogContext.OnDialogOpened(openParameter);
                };

                // 设置关闭对话框委托
                dialogContext.RequestClose += closeParameter =>
                {
                    DialogContainer.CloseDialogCommand.Execute(closeParameter, container);
                };

                return true;
            }

            return false;
        }
    }
}
