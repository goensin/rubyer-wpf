using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 汉堡包菜单选项
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(HamburgerMenuOptionsItem))]
    public class HamburgerMenuOptionsItem : MenuItem
    {
        #region properties

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType", typeof(IconType?), typeof(HamburgerMenuOptionsItem), new PropertyMetadata(null));

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType? IconType
        {
            get { return (IconType?)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        #endregion properties
        static HamburgerMenuOptionsItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuOptionsItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuOptionsItem)));
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