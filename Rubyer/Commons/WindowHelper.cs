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
        /// 获取当前 Window, 找不到 Active Window 时返回第一个 Window
        /// </summary>
        /// <returns>当前 Window</returns>
        public static Window GetCurrentWindow()
        {
            var windows = Application.Current.Windows.OfType<Window>().Where(w => !w.GetType().Name.Contains("AdornerWindow"));
            var window = windows.FirstOrDefault(x => x.IsActive) ??
                         windows.OfType<Window>().FirstOrDefault();
            return window;
        }
    }
}
