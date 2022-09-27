using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// Menu 帮助类
    /// </summary>
    public class MenuHelper
    {
        /// <summary>
        /// 图标宽度
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.RegisterAttached(
            "IconWidth", typeof(double), typeof(MenuHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the IconWidth.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIconWidth(DependencyObject element, double value)
        {
            element.SetValue(IconWidthProperty, value);
        }

        /// <summary>
        /// Gets the IconWidth.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static double GetShowShadow(DependencyObject element)
        {
            return (double)element.GetValue(IconWidthProperty);
        }
    }
}