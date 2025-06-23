using Rubyer.Commons;
using Rubyer.Commons.KnownBoxes;
using Rubyer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 页码条
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PageBarItem))]
    public class PageBar : ItemsControl
    {
        static PageBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageBar), new FrameworkPropertyMetadata(typeof(PageBar)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PageBarItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PageBarItem();
        }

        #region 命令

        /// <summary>
        /// 每页数量改变命令
        /// </summary>
        public static readonly DependencyProperty PageSizeChangedCommandProperty =
            DependencyProperty.Register("PageSizeChangedCommand", typeof(ICommand), typeof(PageBar), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// 每页数量改变命令
        /// </summary>
        public ICommand PageSizeChangedCommand
        {
            get { return (ICommand)GetValue(PageSizeChangedCommandProperty); }
            set { SetValue(PageSizeChangedCommandProperty, value); }
        }

        /// <summary>
        /// 当前页改变命令
        /// </summary>
        public static readonly DependencyProperty PageIndexChangedCommandProperty =
            DependencyProperty.Register("PageIndexChangedCommand", typeof(ICommand), typeof(PageBar), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// 当前页改变命令
        /// </summary>
        public ICommand PageIndexChangedCommand
        {
            get { return (ICommand)GetValue(PageIndexChangedCommandProperty); }
            set { SetValue(PageIndexChangedCommandProperty, value); }
        }

        #endregion 命令

        #region 事件

        /// <summary>
        /// 每页数量改变事件
        /// </summary>
        public static readonly RoutedEvent PageSizeChangedEvent =
            EventManager.RegisterRoutedEvent("PageSizeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PageBar));

        /// <summary>
        /// 每页数量改变事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> PageSizeChanged
        {
            add { AddHandler(PageSizeChangedEvent, value); }
            remove { RemoveHandler(PageSizeChangedEvent, value); }
        }

        /// <summary>
        /// 当前页改变事件
        /// </summary>
        public static readonly RoutedEvent PageIndexChangedEvent =
            EventManager.RegisterRoutedEvent("PageIndexChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PageBar));

        /// <summary>
        /// 当前页改变事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> PageIndexChanged
        {
            add { AddHandler(PageIndexChangedEvent, value); }
            remove { RemoveHandler(PageIndexChangedEvent, value); }
        }

        #endregion 事件

        #region 依赖属性

        /// <summary>
        /// 未选中颜色
        /// </summary>
        public static readonly DependencyProperty UnselectedBrushProperty =
            DependencyProperty.Register("UnselectedBrush", typeof(Brush), typeof(PageBar), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 未选中颜色
        /// </summary>
        public Brush UnselectedBrush
        {
            get { return (Brush)GetValue(UnselectedBrushProperty); }
            set { SetValue(UnselectedBrushProperty, value); }
        }

        /// <summary>
        /// 选中颜色
        /// </summary>
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PageBar), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 选中颜色
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        /// <summary>
        /// 当前前景色
        /// </summary>
        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.Register("SelectedForeground", typeof(Brush), typeof(PageBar), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 当前前景色
        /// </summary>
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageBar), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPageSizeChanged)));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            CheckPageSize(pageBar);

            pageBar.PageIndex = 1;
            pageBar.ReFreshPageBar();

            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue);
            args.RoutedEvent = PageBar.PageSizeChangedEvent;
            pageBar.RaiseEvent(args);
            pageBar.PageSizeChangedCommand?.Execute(newValue);
        }

        private static void CheckPageSize(PageBar pageBar)
        {
            if (!pageBar.PageSizeCollection.Contains(pageBar.PageSize))
            {
                pageBar.PageSize = pageBar.PageSizeCollection.FirstOrDefault();
            }
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 每页数量集合
        /// </summary>
        public static readonly DependencyProperty PageSizeCollectionProperty = DependencyProperty.Register(
            "PageSizeCollection",
            typeof(IEnumerable<int>),
            typeof(PageBar),
            new FrameworkPropertyMetadata(Enumerable.Range(1, 4).Select(x => x * 5), FrameworkPropertyMetadataOptions.AffectsParentMeasure, OnPageSizeCollectionChanged));

        private static void OnPageSizeCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckPageSize(d as PageBar);
        }

        /// <summary>
        /// 每页数量集合
        /// </summary>
        [TypeConverter(typeof(PageSizeCollectionConverter))]
        public IEnumerable<int> PageSizeCollection
        {
            get { return (IEnumerable<int>)GetValue(PageSizeCollectionProperty); }
            set { SetValue(PageSizeCollectionProperty, value); }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PageBar), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPageIndexChanged)));

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

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        /// <summary>
        /// 总数量
        /// </summary>
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(PageBar), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTotalChanged)));

        private static void OnTotalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            pageBar.ReFreshPageBar();
        }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        /// <summary>
        /// 是否显示总数量
        /// </summary>
        public static readonly DependencyProperty IsShowTotalProperty =
            DependencyProperty.Register("IsShowTotal", typeof(bool), typeof(PageBar), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否显示总数量
        /// </summary>
        public bool IsShowTotal
        {
            get { return (bool)GetValue(IsShowTotalProperty); }
            set { SetValue(IsShowTotalProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否显示每页数量
        /// </summary>
        public static readonly DependencyProperty IsShowPageSizeProperty =
            DependencyProperty.Register("IsShowPageSize", typeof(bool), typeof(PageBar), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否显示每页数量
        /// </summary>
        public bool IsShowPageSize
        {
            get { return (bool)GetValue(IsShowPageSizeProperty); }
            set { SetValue(IsShowPageSizeProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 子项停靠方向
        /// </summary>
        public static readonly DependencyProperty ItemsDockProperty =
            DependencyProperty.Register("ItemsDock", typeof(Dock), typeof(PageBar), new PropertyMetadata(Dock.Right));

        /// <summary>
        /// 子项停靠方向
        /// </summary>
        public Dock ItemsDock
        {
            get { return (Dock)GetValue(ItemsDockProperty); }
            set { SetValue(ItemsDockProperty, value); }
        }

        /// <summary>
        /// 子项内边距
        /// </summary>
        public static readonly DependencyProperty ItemsPaddingProperty =
            DependencyProperty.Register("ItemsPadding", typeof(Thickness), typeof(PageBar), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// 子项内边距
        /// </summary>
        public Thickness ItemsPadding
        {
            get { return (Thickness)GetValue(ItemsPaddingProperty); }
            set { SetValue(ItemsPaddingProperty, value); }
        }

        /// <summary>
        /// 是否圆角
        /// </summary>
        public static readonly DependencyProperty IsRoundProperty =
            DependencyProperty.Register("IsRound", typeof(bool), typeof(PageBar), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否圆角
        /// </summary>
        public bool IsRound
        {
            get { return (bool)GetValue(IsRoundProperty); }
            set { SetValue(IsRoundProperty, BooleanBoxes.Box(value)); }
        }

        #endregion 依赖属性

        #region 方法

        // 刷新页码条
        private void ReFreshPageBar()
        {
            foreach (var itemModel in Items.OfType<PageItemModel>())
            {
                itemModel.Command = null;
            }
            Items.Clear();

            if (PageSize == 0 || Total == 0)
            {
                return;
            }

            int pageCount = (int)Math.Ceiling(Total / (PageSize * 1.0));    // 总共多少页

            Items.Add(new PageItemModel
            {
                Content = "<",
                ToolTip = Application.Current.Resources["I18N_PageBar_PreviousPage"].ToString(),
                Value = PageIndex - 1,
                IsEnabled = PageIndex != 1 && pageCount != 1,
                Command = new RubyerCommand(PageNumberChanged)
            });

            Items.Add(new PageItemModel
            {
                Content = 1,
                Value = 1,
                IsEnabled = true,
                Command = new RubyerCommand(PageNumberChanged)
            });

            int begin;
            int end;
            begin = PageIndex >= 6 ? PageIndex - 3 : 2;                         // index 大于等于 6 页就从 index-3 开始，否则 2 开始
            end = PageIndex + 3 >= pageCount ? pageCount - 1 : PageIndex + 3;   // index+3 大于 total 就 total-1 结束，否则 index + 3

            // 补够按键数量
            if (end - begin < 6)
            {
                if (PageIndex - begin < 3)
                {
                    end += 3 - (PageIndex - begin);
                    end = end > pageCount - 1 ? pageCount - 1 : end;
                }
                else if (end - PageIndex < 3)
                {
                    begin -= 3 - (end - PageIndex);
                    begin = begin < 2 ? 2 : begin;
                }
            }

            for (int i = begin; i <= end; i++)
            {
                var model = new PageItemModel
                {
                    Value = i,
                    Content = i,
                    IsEnabled = true,
                    Command = new RubyerCommand(PageNumberChanged)
                };

                if (pageCount > 9)
                {
                    if (i == begin && PageIndex - begin >= 3 && PageIndex > 5)
                    {
                        model.Value = PageIndex - 5;
                        model.Content = "...";
                        model.ToolTip = Application.Current.Resources["I18N_PageBar_Forward5Pages"].ToString();
                    }
                    else if (i == end && end - PageIndex >= 3 && pageCount - PageIndex >= 5)
                    {
                        model.Value = PageIndex + 5;
                        model.Content = "...";
                        model.ToolTip = Application.Current.Resources["I18N_PageBar_Backwards5Pages"].ToString();
                    }
                }

                Items.Add(model);
            }

            // 最后一页
            if (pageCount > 1)
            {
                Items.Add(new PageItemModel
                {
                    Content = pageCount,
                    Value = pageCount,
                    IsEnabled = true,
                    Command = new RubyerCommand(PageNumberChanged)
                });
            }

            // 下一页
            Items.Add(new PageItemModel
            {
                Content = ">",
                ToolTip = Application.Current.Resources["I18N_PageBar_NextPage"].ToString(),
                Value = PageIndex + 1,
                IsEnabled = PageIndex != pageCount && pageCount != 1,
                Command = new RubyerCommand(PageNumberChanged)
            });
        }

        private void PageNumberChanged(object index)
        {
            PageIndex = (int)index;
        }

        #endregion 方法
    }
}