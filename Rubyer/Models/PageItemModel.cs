using System.Windows.Input;

namespace Rubyer.Models
{
    /// <summary>
    /// 分页条子项模型
    /// </summary>
    public class PageItemModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string ToolTip { get; set; }

        //public PageItemModel(object content, int value, bool isEnabled, ICommand command, string toolTip)
        //{
        //    Content = content;
        //    Value = value;
        //    IsEnabled = isEnabled;
        //    Commnad = command;
        //    ToolTip = toolTip;
        //}
    }
}
