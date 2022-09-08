using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 汉堡包菜单选项控件
    /// </summary>
    public class HamburgerMenuOptions : ItemsControl
    {
        static HamburgerMenuOptions()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuOptions), new FrameworkPropertyMetadata(typeof(ItemsControl)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is HamburgerMenuOptionsItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuOptionsItem();
        }
    }
}