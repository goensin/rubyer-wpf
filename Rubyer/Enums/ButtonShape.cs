using System.ComponentModel;

namespace Rubyer
{
    /// <summary>
    /// 按钮形状
    /// </summary>
    public enum ButtonShape
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角")]
        Round,

        /// <summary>
        /// 圆形
        /// </summary>
        [Description("圆形")]
        Circle
    }
}
