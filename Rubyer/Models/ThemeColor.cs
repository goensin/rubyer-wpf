using System.Windows.Media;

namespace Rubyer.Models
{
    /// <summary>
    /// 主题配色
    /// </summary>
    public class ThemeColor
    {
        /// <summary>
        /// 主色调
        /// </summary>
        public Brush Primary { get; set; }

        /// <summary>
        /// 亮色调
        /// </summary>
        public Brush Light { get; set; }

        /// <summary>
        /// 暗色调
        /// </summary>
        public Brush Dark { get; set; }

        /// <summary>
        /// 强调色调
        /// </summary>
        public Brush Accent { get; set; }
    }
}
