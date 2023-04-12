using System.ComponentModel;

namespace Rubyer.Enums
{
    /// <summary>
    /// 主题模式
    /// </summary>
    public enum ThemeMode
    {
        /// <summary>
        /// 亮色
        /// </summary>
        [Description("亮色")]
        Light = 0,

        /// <summary>
        /// 暗色
        /// </summary>
        [Description("暗色")]
        Dark,

        /// <summary>
        /// 跟随系统
        /// </summary>
        [Description("跟随系统")]
        System,
    }
}