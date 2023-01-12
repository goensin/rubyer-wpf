using System.Windows;
using System.Windows.Media;

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

        /// <summary>
        /// 选中时图标颜色
        /// </summary>
        public static readonly DependencyProperty IconFocusedBrushProperty = DependencyProperty.RegisterAttached(
            "IconFocusedBrush", typeof(Brush), typeof(TreeViewHelper), new PropertyMetadata(default(Brush)));

        public static void SetIconFocusedBrush(DependencyObject element, Brush value)
        {
            element.SetValue(IconFocusedBrushProperty, value);
        }

        public static Brush GetIconFocusedBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(IconFocusedBrushProperty);
        }
    }
}
