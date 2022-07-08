using System.Windows;

namespace Rubyer.Converters
{
    /// <summary>
    /// bool - 显示状态转换器
    /// </summary>
    public class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToVisibilityConverter"/> class.
        /// </summary>
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
