using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 面包屑子项
    /// </summary>
    public class BreadcrumbItem : Button
    {
        static BreadcrumbItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbItem), new FrameworkPropertyMetadata(typeof(BreadcrumbItem)));
        }

        #region 事件

        /// <summary>
        /// 导航
        /// </summary>
        public static readonly RoutedEvent NavigateEvent = EventManager.RegisterRoutedEvent(
            "Navigate", RoutingStrategy.Direct, typeof(RoutedPropertyChangedEventHandler<string>), typeof(BreadcrumbItem));

        /// <summary>
        /// 导航
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> Navigate
        {
            add { AddHandler(NavigateEvent, value); }
            remove { RemoveHandler(NavigateEvent, value); }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 链接
        /// </summary>
        public static readonly DependencyProperty HrefProperty =
            DependencyProperty.Register("Href", typeof(string), typeof(BreadcrumbItem), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        ///  链接
        /// </summary>
        public string Href
        {
            get { return (string)GetValue(HrefProperty); }
            set { SetValue(HrefProperty, value); }
        }

        /// <summary>
        /// 分隔符
        /// </summary>
        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(BreadcrumbItem), new FrameworkPropertyMetadata("/"));

        /// <summary>
        ///  分隔符
        /// </summary>
        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        /// <summary>
        /// 分隔符图标
        /// </summary>
        public static readonly DependencyProperty SeparatorIconProperty =
            DependencyProperty.Register("SeparatorIcon", typeof(IconType?), typeof(BreadcrumbItem), new FrameworkPropertyMetadata(null));

        /// <summary>
        ///  分隔符图标
        /// </summary>
        public IconType? SeparatorIcon
        {
            get { return (IconType?)GetValue(SeparatorIconProperty); }
            set { SetValue(SeparatorIconProperty, value); }
        }

        /// <summary>
        /// 分隔符颜色
        /// </summary>
        public static readonly DependencyProperty SeparatorBrushProperty =
            DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(BreadcrumbItem), new FrameworkPropertyMetadata(null));

        /// <summary>
        ///  分隔符颜色
        /// </summary>
        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        #endregion

        /// <inheritdoc/>
        protected override void OnClick()
        {
            base.OnClick();

            RaiseEvent(new RoutedPropertyChangedEventArgs<string>(null, Href) { RoutedEvent = NavigateEvent });
        }
    }
}
