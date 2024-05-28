using System.ComponentModel;

namespace Rubyer.Enums
{
    /// <summary>
    /// 弹出框放置模式
    /// </summary>
    public enum PopoverPlacementMode
    {
        /// <summary>
        /// 上
        /// </summary>
        [Description("上")]
        Top = 0,

        /// <summary>
        /// 下
        /// </summary>
        [Description("下")]
        Bottom,

        /// <summary>
        /// 左
        /// </summary>
        [Description("左")]
        Left,

        /// <summary>
        /// 右
        /// </summary>
        [Description("右")]
        Right,
    }
}
