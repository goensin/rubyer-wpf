namespace Rubyer
{
    /// <summary>
    /// 对话框 DataContext 配置
    /// </summary>
    public interface IDialogDataContextConfiguration
    {
        /// <summary>
        /// 打开时动作
        /// </summary>
        /// <param name="dialog">对话框</param>
        /// <param name="content">对话框内容</param>
        /// <param name="parameter">打开参数</param>
        /// <returns>是否配置成功 IDialogDataContext</returns>
        bool OnOpenAction(DialogContainer dialog, object content, object parameter);

        /// <summary>
        /// 关闭时动作
        /// </summary>
        void OnCloseAction();
    }
}
