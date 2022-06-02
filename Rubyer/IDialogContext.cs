using Rubyer.Commons;
using System;

namespace Rubyer
{
    /// <summary>
    /// 对话框接口
    /// </summary>
    public interface IDialogContext
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 请求关闭委托
        /// </summary>
        event Action<object> RequestClose;

        /// <summary>
        /// 当对话框打开完成方法
        /// </summary>
        /// <param name="parameters">参数</param>
        void OnDialogOpened(object parameters);
    }
}
