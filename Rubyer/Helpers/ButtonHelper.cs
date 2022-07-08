using Rubyer;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// Button 帮助类
    /// </summary>
    public class ButtonHelper
    {
        /// <summary>
        /// 显示阴影
        /// </summary>
        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.RegisterAttached(
            "ShowShadow", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Sets the show shadow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetShowShadow(DependencyObject element, bool value)
        {
            element.SetValue(ShowShadowProperty, value);
        }

        /// <summary>
        /// Gets the show shadow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowShadow(DependencyObject element)
        {
            return (bool)element.GetValue(ShowShadowProperty);
        }

        /// <summary>
        /// 形状
        /// </summary>
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.RegisterAttached(
            "Shape", typeof(ButtonShape), typeof(ButtonHelper), new PropertyMetadata(default(ButtonShape)));

        /// <summary>
        /// Sets the shape.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetShape(DependencyObject element, ButtonShape value)
        {
            element.SetValue(ShapeProperty, value);
        }

        /// <summary>
        /// Gets the shape.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A ButtonShape.</returns>
        public static ButtonShape GetShape(DependencyObject element)
        {
            return (ButtonShape)element.GetValue(ShapeProperty);
        }

        /// <summary>
        /// 圆形直径
        /// </summary>
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.RegisterAttached(
            "CircleDiameter", typeof(double), typeof(ButtonHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the circle diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetCircleDiameter(DependencyObject element, double value)
        {
            element.SetValue(CircleDiameterProperty, value);
        }

        /// <summary>
        /// Gets the circle diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetCircleDiameter(DependencyObject element)
        {
            return (double)element.GetValue(CircleDiameterProperty);
        }

        /// <summary>
        /// 加载中
        /// </summary>
        public static readonly DependencyProperty LoadingProperty = DependencyProperty.RegisterAttached(
            "Loading", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Sets the loading.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetLoading(DependencyObject element, bool value)
        {
            element.SetValue(LoadingProperty, value);
        }

        /// <summary>
        /// Gets the loading.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetLoading(DependencyObject element)
        {
            return (bool)element.GetValue(LoadingProperty);
        }
    }
}
