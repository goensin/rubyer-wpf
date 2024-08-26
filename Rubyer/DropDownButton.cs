using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// 是否打开下拉菜单
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDropDownOpenChanged));

        /// <summary>
        /// 是否显示分割线
        /// </summary>
        public static readonly DependencyProperty IsShowSeparatorProperty =
            DependencyProperty.Register("IsShowSeparator", typeof(bool), typeof(DropDownButton), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 下拉菜单
        /// </summary>
        public static readonly DependencyProperty DropDownMenuProperty =
            DependencyProperty.Register("DropDownMenu", typeof(ContextMenu), typeof(DropDownButton), new PropertyMetadata(null, OnDropDownMenuChanged));

        /// <summary>Identifies the <see cref="DropDownMenuStyle"/> dependency property.</summary>
        public static readonly DependencyProperty DropDownMenuStyleProperty = DependencyProperty.Register(
            nameof(DropDownMenuStyle), typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="Style"/> used for each item in the options.
        /// </summary>
        public Style DropDownMenuStyle
        {
            get => (Style)this.GetValue(DropDownMenuStyleProperty);
            set => this.SetValue(DropDownMenuStyleProperty, value);
        }

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

        /// <summary>
        /// 下拉菜单
        /// </summary>
        public ContextMenu DropDownMenu
        {
            get { return (ContextMenu)GetValue(DropDownMenuProperty); }
            set { SetValue(DropDownMenuProperty, value); }
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DropDownMenu is { })
            {
                DropDownMenu.Style = DropDownMenuStyle;
                DropDownMenu.PlacementTarget = this;
            }
        }

        private static void OnDropDownMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dropDownButton = (DropDownButton)d;
            if (dropDownButton.IsLoaded)
            {
                if (e.NewValue is ContextMenu newMenu)
                {
                    newMenu.Style = dropDownButton.DropDownMenuStyle;
                    newMenu.PlacementTarget = dropDownButton;
                }

                if (e.OldValue is ContextMenu oldMenu)
                {
                    oldMenu.Style = null;
                    oldMenu.PlacementTarget = null;
                }
            }
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dropDownButton = (DropDownButton)d;
            if (dropDownButton.DropDownMenu is { })
            {
                //dropDownButton.DropDownMenu.IsOpen = dropDownButton.IsDropDownOpen;
            }
        }
    }
}
