using System.Windows.Data;
using System.Windows;

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

                // 对话框关闭中
                dialogCard.Closing += (sender,e)=>
                {
                    dialogContext.OnDialogClosing(e);

                    if (dialogContext.CloseParameter is { })
                    {
                        e.Parameter = dialogContext.CloseParameter;
                    }
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
