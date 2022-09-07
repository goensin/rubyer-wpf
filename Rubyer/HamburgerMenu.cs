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
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListBoxItem))]
    [TemplatePart(Name = HamburgerButtonPartName, Type = typeof(Button))]
    public class HamburgerMenu : ListBox
    {
        /// <summary>
        /// 汉堡包按钮
        /// </summary>
        public const string HamburgerButtonPartName = "PART_HamburgerButton";

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
            "Header", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
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
        /// 设置子项区域
        /// </summary>
        public static readonly DependencyProperty OptionContentProperty = DependencyProperty.Register(
            "OptionContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));

        /// <summary>
        /// 设置子项区域
        /// </summary>
        public object OptionContent
        {
            get { return (object)GetValue(OptionContentProperty); }
            set { SetValue(OptionContentProperty, value); }
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

        #endregion properties

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
            button.Click += Button_Click; ;
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