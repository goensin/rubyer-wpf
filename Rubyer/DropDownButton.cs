using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Rubyer
{
    /// <summary>
    /// 下拉按钮 
    /// </summary>
    [StyleTypedProperty(Property = "MenuItemContainerStyle", StyleTargetType = typeof(MenuItem))]
    [TemplatePart(Name = DropDownMenuItemName, Type = typeof(MenuItem))]
    public class DropDownButton : Button
    {
        const string DropDownMenuItemName = "PART_DropDownMenuItem";

        private MenuItem dropDownMenu;

        /// <summary>
        /// 是否打开下拉菜单
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否显示分割线
        /// </summary>
        public static readonly DependencyProperty IsShowSeparatorProperty =
            DependencyProperty.Register("IsShowSeparator", typeof(bool), typeof(DropDownButton), new PropertyMetadata(BooleanBoxes.FalseBox));

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }

        /// <summary>
        /// 是否打开下拉菜单
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否显示分割线
        /// </summary>
        public bool IsShowSeparator
        {
            get { return (bool)GetValue(IsShowSeparatorProperty); }
            set { SetValue(IsShowSeparatorProperty, BooleanBoxes.Box(value)); }
        }

        #region menu item

        /// <summary>Identifies the <see cref="MenuItemsSource"/> dependency property.</summary>
        public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(
            nameof(MenuItemsSource), typeof(IEnumerable), typeof(DropDownButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets an object source used to generate the content of the options.
        /// </summary>
        public IEnumerable MenuItemsSource
        {
            get => (IEnumerable)this.GetValue(MenuItemsSourceProperty);
            set => this.SetValue(MenuItemsSourceProperty, value);
        }

        /// <summary>Identifies the <see cref="MenuItemContainerStyle"/> dependency property.</summary>
        public static readonly DependencyProperty MenuItemContainerStyleProperty = DependencyProperty.Register(
            nameof(MenuItemContainerStyle), typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="Style"/> used for each item in the options.
        /// </summary>
        public Style MenuItemContainerStyle
        {
            get => (Style)this.GetValue(MenuItemContainerStyleProperty);
            set => this.SetValue(MenuItemContainerStyleProperty, value);
        }

        /// <summary>Identifies the <see cref="MenuItemTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty MenuItemTemplateProperty = DependencyProperty.Register(
            nameof(MenuItemTemplate), typeof(DataTemplate), typeof(DropDownButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display each item in the options.
        /// </summary>
        public DataTemplate MenuItemTemplate
        {
            get => (DataTemplate)this.GetValue(MenuItemTemplateProperty);
            set => this.SetValue(MenuItemTemplateProperty, value);
        }

        /// <summary>Identifies the <see cref="MenuItemTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty MenuItemTemplateSelectorProperty = DependencyProperty.Register(
            nameof(MenuItemTemplateSelector), typeof(DataTemplateSelector), typeof(DropDownButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> used to display each item in the options.
        /// </summary>
        public DataTemplateSelector MenuItemTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(MenuItemTemplateSelectorProperty);
            set => this.SetValue(MenuItemTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Gets the collection used to generate the content of the option list.
        /// </summary>
        /// <exception cref="Exception">
        /// Exception thrown if DropDownMenu is not yet defined.
        /// </exception>
        public ItemCollection MenuItems
        {
            get
            {
                if (this.dropDownMenu is null)
                {
                    throw new Exception("DropDownMenu is not defined yet. Please use MenuItemsSource instead.");
                }

                return this.dropDownMenu.Items;
            }
        }

        #endregion

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            dropDownMenu = GetTemplateChild(DropDownMenuItemName) as MenuItem;
        }
    }
}
