using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 面包屑
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(BreadcrumbItem))]
    public class Breadcrumb : ItemsControl
    {
        #region 事件

        /// <summary>
        /// 导航
        /// </summary>
        public static readonly RoutedEvent NavigateEvent = EventManager.RegisterRoutedEvent(
            "Navigate", RoutingStrategy.Direct, typeof(RoutedPropertyChangedEventHandler<string>), typeof(Breadcrumb));

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
            DependencyProperty.Register("Href", typeof(string), typeof(Breadcrumb), new FrameworkPropertyMetadata(string.Empty));

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
            DependencyProperty.Register("Separator", typeof(string), typeof(Breadcrumb), new FrameworkPropertyMetadata("/"));

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
            DependencyProperty.Register("SeparatorIcon", typeof(IconType?), typeof(Breadcrumb), new FrameworkPropertyMetadata(null));

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
            DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(Breadcrumb), new FrameworkPropertyMetadata(null));

        /// <summary>
        ///  分隔符颜色
        /// </summary>
        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        #endregion

        static Breadcrumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(typeof(Breadcrumb)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BreadcrumbItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BreadcrumbItem();
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += Breadcrumb_Loaded;
        }

        private void Breadcrumb_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Breadcrumb_Loaded;

            for (int i = 0; i < Items.Count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is BreadcrumbItem breadcrumbItem)
                {
                    breadcrumbItem.Navigate += BreadcrumbItem_Navigate;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (e.OldItems is { })
            {
                foreach (var item in e.NewItems.OfType<BreadcrumbItem>())
                {
                    item.Navigate += BreadcrumbItem_Navigate;
                }

                foreach (var item in e.OldItems.OfType<BreadcrumbItem>())
                {
                    item.Navigate -= BreadcrumbItem_Navigate;
                }
            }
        }

        private void BreadcrumbItem_Navigate(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<string>(Href, e.NewValue) { RoutedEvent = NavigateEvent });

            Href = e.NewValue;
        }
    }
}
