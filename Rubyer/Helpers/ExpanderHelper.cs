using System.Windows;

namespace Rubyer
{
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
    }
}
