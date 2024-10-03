using System.Linq;
using System.Windows;

namespace Rubyer.Commons
{
    /// <summary>
    /// Window 帮助类
    /// </summary>
    public class WindowHelper
    {
        /// <summary>
        /// 获取当前 Window, 找不到 Active Window 时返回 MainWindow，再找不到返回第一个 Window
        /// </summary>
        /// <returns>当前 Window</returns>
        public static Window GetCurrentWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) ??
                         Application.Current.MainWindow ??
                         Application.Current.Windows.OfType<Window>().FirstOrDefault();
            return window;
        }
    }
}
