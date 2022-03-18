using Rubyer.Enums;
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

        public static void SetShowShadow(DependencyObject element, bool value)
        {
            element.SetValue(ShowShadowProperty, value);
        }

        public static bool GetShowShadow(DependencyObject element)
        {
            return (bool)element.GetValue(ShowShadowProperty);
        }

        /// <summary>
        /// 形状
        /// </summary>
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.RegisterAttached(
            "Shape", typeof(ButtonShape), typeof(ButtonHelper), new PropertyMetadata(default(ButtonShape)));

        public static void SetShape(DependencyObject element, ButtonShape value)
        {
            element.SetValue(ShapeProperty, value);
        }

        public static ButtonShape GetShape(DependencyObject element)
        {
            return (ButtonShape)element.GetValue(ShapeProperty);
        }

        /// <summary>
        /// 圆形直径
        /// </summary>
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.RegisterAttached(
            "CircleDiameter", typeof(double), typeof(ButtonHelper), new PropertyMetadata(default(double)));

        public static void SetCircleDiameter(DependencyObject element, double value)
        {
            element.SetValue(CircleDiameterProperty, value);
        }

        public static double GetCircleDiameter(DependencyObject element)
        {
            return (double)element.GetValue(CircleDiameterProperty);
        }

        /// <summary>
        /// 加载中
        /// </summary>
        public static readonly DependencyProperty LoadingProperty = DependencyProperty.RegisterAttached(
            "Loading", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(default(bool)));

        public static void SetLoading(DependencyObject element, bool value)
        {
            element.SetValue(LoadingProperty, value);
        }

        public static bool GetLoading(DependencyObject element)
        {
            return (bool)element.GetValue(LoadingProperty);
        }
    }
}
