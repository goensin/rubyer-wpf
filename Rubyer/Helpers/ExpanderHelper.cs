using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// Expander 帮助类
    /// </summary>
    public class ExpanderHelper
    {
        /// <summary>
        /// 展开图标类型
        /// </summary>
        public static readonly DependencyProperty ExpandIconTypeProperty = DependencyProperty.RegisterAttached(
            "ExpandIconType", typeof(IconType), typeof(ExpanderHelper), new PropertyMetadata(IconType.ArrowDownSLine));

        public static void SetExpandIconType(DependencyObject element, IconType value)
        {
            element.SetValue(ExpandIconTypeProperty, value);
        }

        public static IconType GetExpandIconType(DependencyObject element)
        {
            return (IconType)element.GetValue(ExpandIconTypeProperty);
        }

        /// <summary>
        /// 展开图标类型
        /// </summary>
        public static readonly DependencyProperty ExpandIconDockProperty = DependencyProperty.RegisterAttached(
            "ExpandIconDock", typeof(Dock), typeof(ExpanderHelper), new PropertyMetadata(Dock.Right));

        public static void SetExpandIconDock(DependencyObject element, Dock value)
        {
            element.SetValue(ExpandIconDockProperty, value);
        }

        public static Dock GetExpandIconDock(DependencyObject element)
        {
            return (Dock)element.GetValue(ExpandIconDockProperty);
        }
    }
}
