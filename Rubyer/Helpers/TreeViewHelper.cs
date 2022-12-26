using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// TreeView 帮助类
    /// </summary>
    public class TreeViewHelper
    {
        /// <summary>
        /// 展开图标类型
        /// </summary>
        public static readonly DependencyProperty ExpandIconTypeProperty = DependencyProperty.RegisterAttached(
            "ExpandIconType", typeof(IconType), typeof(TreeViewHelper), new PropertyMetadata(IconType.ArrowRightSFill));

        public static void SetExpandIconType(DependencyObject element, IconType value)
        {
            element.SetValue(ExpandIconTypeProperty, value);
        }

        public static IconType GetExpandIconType(DependencyObject element)
        {
            return (IconType)element.GetValue(ExpandIconTypeProperty);
        }
    }
}
