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
    public class HamburgerMenuOptionsItem : MenuItem
    {
        static HamburgerMenuOptionsItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuOptionsItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuOptionsItem)));
        }

        #region properties

        ///// <summary>
        ///// 图标
        ///// </summary>
        //public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        //    "Icon", typeof(object), typeof(HamburgerMenuOptionsItem), new PropertyMetadata(null));

        ///// <summary>
        ///// 图标
        ///// </summary>
        //public object Icon
        //{
        //    get { return (object)GetValue(IconProperty); }
        //    set { SetValue(IconProperty, value); }
        //}

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
    }
}