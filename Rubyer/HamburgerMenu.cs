using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// 汉堡包菜单
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(HamburgerMenuItem))]
    [StyleTypedProperty(Property = "OptionsItemContainerStyle", StyleTargetType = typeof(HamburgerMenuOptionsItem))]
    [TemplatePart(Name = HamburgerButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = OptionsItemsControlPartName, Type = typeof(ItemsControl))]
    public class HamburgerMenu : TabControl
    {
        /// <summary>
        /// 汉堡包按钮
        /// </summary>
        public const string HamburgerButtonPartName = "PART_HamburgerButton";

        /// <summary>
        /// 选项集合控件
        /// </summary>
        public const string OptionsItemsControlPartName = "PART_OptionsItemsControl";

        #region fields

        private ItemsControl optionsItemsControl;

        #endregion fields

        #region properties

        /// <summary>
        /// 是否展开菜单
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否展开菜单
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标题
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 折叠宽度
        /// </summary>
        public static readonly DependencyProperty CollapsedWidthProperty = DependencyProperty.Register(
            "CollapsedWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        /// <summary>
        /// 折叠宽度
        /// </summary>
        public double CollapsedWidth
        {
            get { return (double)GetValue(CollapsedWidthProperty); }
            set { SetValue(CollapsedWidthProperty, value); }
        }

        /// <summary>
        /// 展开宽度
        /// </summary>
        public static readonly DependencyProperty ExpandedWidthProperty = DependencyProperty.Register(
            "ExpandedWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        /// <summary>
        /// 展开宽度
        /// </summary>
        public double ExpandedWidth
        {
            get { return (double)GetValue(ExpandedWidthProperty); }
            set { SetValue(ExpandedWidthProperty, value); }
        }

        /// <summary>
        /// 是否显示汉堡包按钮
        /// </summary>
        public static readonly DependencyProperty IsShowHamburgerButtonProperty = DependencyProperty.Register(
            "IsShowHamburgerButton", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(true));

        /// <summary>
        /// 是否显示汉堡包按钮
        /// </summary>
        public bool IsShowHamburgerButton
        {
            get { return (bool)GetValue(IsShowHamburgerButtonProperty); }
            set { SetValue(IsShowHamburgerButtonProperty, value); }
        }

        /// <summary>
        /// 是否显示小竖条
        /// </summary>
        public static readonly DependencyProperty IsShowLittleBarProperty = DependencyProperty.Register(
            "IsShowLittleBar", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(true));

        /// <summary>
        /// 是否显示小竖条
        /// </summary>
        public bool IsShowLittleBar
        {
            get { return (bool)GetValue(IsShowLittleBarProperty); }
            set { SetValue(IsShowLittleBarProperty, value); }
        }

        #endregion properties

        #region options item

        // form https://github.com/MahApps/MahApps.Metro
        // MahApps.Metro.Controls.HamburgerMenu

        /// <summary>Identifies the <see cref="OptionsItemsSource"/> dependency property.</summary>
        public static readonly DependencyProperty OptionsItemsSourceProperty = DependencyProperty.Register(
            nameof(OptionsItemsSource), typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets an object source used to generate the content of the options.
        /// </summary>
        public object OptionsItemsSource
        {
            get => this.GetValue(OptionsItemsSourceProperty);
            set => this.SetValue(OptionsItemsSourceProperty, value);
        }

        /// <summary>Identifies the <see cref="OptionsItemContainerStyle"/> dependency property.</summary>
        public static readonly DependencyProperty OptionsItemContainerStyleProperty = DependencyProperty.Register(
            nameof(OptionsItemContainerStyle), typeof(Style), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="Style"/> used for each item in the options.
        /// </summary>
        public Style OptionsItemContainerStyle
        {
            get => (Style)this.GetValue(OptionsItemContainerStyleProperty);
            set => this.SetValue(OptionsItemContainerStyleProperty, value);
        }

        /// <summary>Identifies the <see cref="OptionsItemTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty OptionsItemTemplateProperty = DependencyProperty.Register(
            nameof(OptionsItemTemplate), typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display each item in the options.
        /// </summary>
        public DataTemplate OptionsItemTemplate
        {
            get => (DataTemplate)this.GetValue(OptionsItemTemplateProperty);
            set => this.SetValue(OptionsItemTemplateProperty, value);
        }

        /// <summary>Identifies the <see cref="OptionsItemTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty OptionsItemTemplateSelectorProperty = DependencyProperty.Register(
            nameof(OptionsItemTemplateSelector), typeof(DataTemplateSelector), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> used to display each item in the options.
        /// </summary>
        public DataTemplateSelector OptionsItemTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(OptionsItemTemplateSelectorProperty);
            set => this.SetValue(OptionsItemTemplateSelectorProperty, value);
        }

        /// <summary>Identifies the <see cref="OptionsVisibility"/> dependency property.</summary>
        public static readonly DependencyProperty OptionsVisibilityProperty = DependencyProperty.Register(
            nameof(OptionsVisibility), typeof(Visibility), typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> of the options menu.
        /// </summary>
        public Visibility OptionsVisibility
        {
            get => (Visibility)this.GetValue(OptionsVisibilityProperty);
            set => this.SetValue(OptionsVisibilityProperty, value);
        }

        /// <summary>
        /// Gets the collection used to generate the content of the option list.
        /// </summary>
        /// <exception cref="Exception">
        /// Exception thrown if OptionsListView is not yet defined.
        /// </exception>
        public ItemCollection OptionsItems
        {
            get
            {
                if (this.optionsItemsControl is null)
                {
                    throw new Exception("OptionsListView is not defined yet. Please use OptionsItemsSource instead.");
                }

                return this.optionsItemsControl.Items;
            }
        }

        #endregion options item

        #region events

        /// <summary>
        /// 汉堡包按钮点击事件
        /// </summary>
        public static readonly RoutedEvent HamburgerButtonClickEvent = EventManager.RegisterRoutedEvent(nameof(HamburgerButtonClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HamburgerMenu));

        /// <summary>
        /// 汉堡包按钮点击
        /// </summary>
        public event RoutedEventHandler HamburgerButtonClick
        {
            add => this.AddHandler(HamburgerButtonClickEvent, value);
            remove => this.RemoveHandler(HamburgerButtonClickEvent, value);
        }

        #endregion events

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        #region methods

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button button = GetTemplateChild(HamburgerButtonPartName) as Button;
            button.Click += Button_Click;

            optionsItemsControl = GetTemplateChild(OptionsItemsControlPartName) as ItemsControl;
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is HamburgerMenuItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var args = new RoutedEventArgs(HamburgerButtonClickEvent, sender);
            this.RaiseEvent(args);

            if (!args.Handled)
            {
                IsExpanded = !IsExpanded;
            }
        }

        #endregion methods
    }
}