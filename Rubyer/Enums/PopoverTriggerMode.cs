using System.ComponentModel;

namespace Rubyer.Enums
{
    /// <summary>
    /// 弹出框触发模式
    /// </summary>
    public enum PopoverTriggerMode
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")] 
        None = 0,

        /// <summary>
        /// 点击
        /// </summary>
        [Description("点击")]
        Click,

        /// <summary>
        /// 悬浮
        /// </summary>
        [Description("悬浮")]
        Hover,

        /// <summary>
        /// 聚焦
        /// </summary>
        [Description("聚焦")]
        Focus,

        /// <summary>
        /// 右键菜单
        /// </summary>
        [Description("右键菜单")]
        ContextMenu,
    }
}
