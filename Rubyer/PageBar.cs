using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PageBarItem))]
    public class PageBar : ItemsControl
    {
        static PageBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageBar), new FrameworkPropertyMetadata(typeof(PageBar)));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PageBarItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PageBarItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        #region 命令
        // 每页数量改变
        public static readonly DependencyProperty PageSizeChangedCommandProperty =
            DependencyProperty.Register("PageSizeChangedCommand", typeof(ICommand), typeof(PageBar), new PropertyMetadata(default(ICommand)));

        public ICommand PageSizeChangedCommand
        {
            get { return (ICommand)GetValue(PageSizeChangedCommandProperty); }
            set { SetValue(PageSizeChangedCommandProperty, value); }
        }

        // 当前页改变
        public static readonly DependencyProperty PageIndexChangedCommandProperty =
            DependencyProperty.Register("PageIndexChangedCommand", typeof(ICommand), typeof(PageBar), new PropertyMetadata(default(ICommand)));

        public ICommand PageIndexChangedCommand
        {
            get { return (ICommand)GetValue(PageIndexChangedCommandProperty); }
            set { SetValue(PageIndexChangedCommandProperty, value); }
        }
        #endregion

        #region 事件
        public static readonly RoutedEvent PageSizeChangedEvent =
            EventManager.RegisterRoutedEvent("PageSizeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PageBar));

        public event RoutedPropertyChangedEventHandler<int> PageSizeChanged
        {
            add { AddHandler(PageSizeChangedEvent, value); }
            remove { RemoveHandler(PageSizeChangedEvent, value); }
        }

        public static readonly RoutedEvent PageIndexChangedEvent =
            EventManager.RegisterRoutedEvent("PageIndexChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PageBar));

        public event RoutedPropertyChangedEventHandler<int> PageIndexChanged
        {
            add { AddHandler(PageIndexChangedEvent, value); }
            remove { RemoveHandler(PageIndexChangedEvent, value); }
        }
        #endregion

        #region 依赖属性
        // 每页数量
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageBar), new PropertyMetadata(default(int), new PropertyChangedCallback(OnPageSizeChanged)));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            pageBar.PageIndex = 1;
            pageBar.ReFreshPageBar();


            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue);
            args.RoutedEvent = PageBar.PageSizeChangedEvent;
            pageBar.RaiseEvent(args);
            pageBar.PageSizeChangedCommand?.Execute(newValue);
        }

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // 当前页
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PageBar), new PropertyMetadata(1, new PropertyChangedCallback(OnPageIndexChanged)));

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            pageBar.ReFreshPageBar();

            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue);
            args.RoutedEvent = PageBar.PageIndexChangedEvent;
            pageBar.RaiseEvent(args);
            pageBar.PageIndexChangedCommand?.Execute(newValue);
        }

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        // 总数量
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(PageBar), new PropertyMetadata(default(int), new PropertyChangedCallback(OnTotalChanged)));

        private static void OnTotalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            pageBar.ReFreshPageBar();
        }

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }


        public static readonly DependencyProperty IsShowTotalProperty =
            DependencyProperty.Register("IsShowTotal", typeof(bool), typeof(PageBar), new PropertyMetadata(default(bool)));

        public bool IsShowTotal
        {
            get { return (bool)GetValue(IsShowTotalProperty); }
            set { SetValue(IsShowTotalProperty, value); }
        }


        public static readonly DependencyProperty IsShowPageSizeProperty =
            DependencyProperty.Register("IsShowPageSize", typeof(bool), typeof(PageBar), new PropertyMetadata(default(bool)));

        public bool IsShowPageSize
        {
            get { return (bool)GetValue(IsShowPageSizeProperty); }
            set { SetValue(IsShowPageSizeProperty, value); }
        }

        #endregion


        #region 方法

        // 刷新页码条
        private void ReFreshPageBar()
        {
            this.Items.Clear();

            if (PageSize == 0 || Total == 0)
            {
                return;
            }

            int pageCount = (int)Math.Ceiling(Total / (PageSize * 1.0));    // 总共多少页

            this.Items.Add(new PageBarItem
            {
                Content = "<",
                ToolTip = "上一页",
                Value = PageIndex - 1,
                IsEnabled = PageIndex != 1 && pageCount != 1,
                PageNumberCommand = new RubyerCommand(PageNumberChanged)
            });

            this.Items.Add(new PageBarItem
            {
                Content = "1",
                ToolTip = "1",
                Value = 1,
                IsEnabled = true,
                PageNumberCommand = new RubyerCommand(PageNumberChanged)
            });

            int begin = PageIndex >= 6 ? PageIndex - 3 : 2;                         // index 大于等于 6 页就从 index-3 开始，否则 2 开始
            int end = PageIndex + 3 >= pageCount ? pageCount - 1 : PageIndex + 3;   // index + 3 大于 total 就 total-1结束，否则 index + 3

            for (int i = begin; i <= end; i++)
            {
                PageBarItem info = new PageBarItem()
                {
                    Value = i,
                    Content = i.ToString(),
                    IsEnabled = true,
                    ToolTip = i.ToString(),
                    PageNumberCommand = new RubyerCommand(PageNumberChanged)
                };

                if (i == begin && PageIndex - begin >= 3 && PageIndex > 5)
                {
                    info.Value = PageIndex - 5;
                    info.Content = "...";
                    info.ToolTip = "向前 5 页";
                }
                else if (i == end && end - PageIndex >= 3 && pageCount - PageIndex >= 5)
                {
                    info.Value = PageIndex + 5;
                    info.Content = "...";
                    info.ToolTip = "向后 5 页";
                }

                this.Items.Add(info);
            }

            // 最后一页
            if (pageCount > 1)
            {
                this.Items.Add(new PageBarItem()
                {
                    Content = pageCount.ToString(),
                    ToolTip = pageCount.ToString(),
                    Value = pageCount,
                    IsEnabled = true,
                    PageNumberCommand = new RubyerCommand(PageNumberChanged)
                });
            }

            // 下一页
            this.Items.Add(new PageBarItem()
            {
                Content = ">",
                ToolTip = "下一页",
                Value = PageIndex + 1,
                IsEnabled = PageIndex != pageCount && pageCount != 1,
                PageNumberCommand = new RubyerCommand(PageNumberChanged)
            });
        }

        private void PageNumberChanged(object obj)
        {
            int num = (int)obj;
            this.PageIndex = num;
        }
        #endregion
    }
}
