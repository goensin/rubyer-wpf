using System;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// 对话框适配接口
    /// </summary>
    public interface IDialogDataContext
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 对话框关闭参数
        /// </summary>
        object CloseParameter { get; set; }

        /// <summary>
        /// 请求关闭委托
        /// </summary>
        event Action<object> RequestClose;

        /// <summary>
        /// 当对话框打开完成方法
        /// </summary>
        /// <param name="parameters">参数</param>
        void OnDialogOpened(object parameters);

        /// <summary>
        /// 关闭中
        /// </summary>
        /// <param name="e"></param>
        void OnDialogClosing(RoutedEventArgs e);
    }
}
