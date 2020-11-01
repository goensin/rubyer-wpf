using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Rubyer
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    public class PageBar : Control
    {
        public const string ItemsControlPartName = "PART_ItemsControl";

        static PageBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageBar), new FrameworkPropertyMetadata(typeof(PageBar)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region 依赖属性
        // 选中颜色
        public static readonly DependencyProperty CurrentBrushProperty =
            DependencyProperty.Register("CurrentBrush", typeof(Brush), typeof(PageBar), new PropertyMetadata(default(Brush)));

        public Brush CurrentBrush
        {
            get { return (Brush)GetValue(CurrentBrushProperty); }
            set { SetValue(CurrentBrushProperty, value); }
        }


        // 页码条按钮
        public static readonly DependencyProperty PageInfosProperty =
            DependencyProperty.Register("PageInfos", typeof(ObservableCollection<PageInfo>), typeof(PageBar), new PropertyMetadata(default(ObservableCollection<PageInfo>)));

        public ObservableCollection<PageInfo> PageInfos
        {
            get { return (ObservableCollection<PageInfo>)GetValue(PageInfosProperty); }
            set { SetValue(PageInfosProperty, value); }
        }


        // 每页数量
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageBar), new PropertyMetadata(default(int), new PropertyChangedCallback(OnPageSizeChanged)));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageBar pageBar = (PageBar)d;
            pageBar.ReFreshPageBar();
            if (pageBar.PageSizeChangedCommand != null && pageBar.PageSizeChangedCommand.CanExecute(pageBar.PageSize))
            {
                pageBar.PageSizeChangedCommand.Execute(pageBar.PageSize);
            }
            pageBar.PageIndex = 1;
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
            if (pageBar.PageIndexChangedCommand != null && pageBar.PageIndexChangedCommand.CanExecute(pageBar.PageIndex))
            {
                pageBar.PageIndexChangedCommand.Execute(pageBar.PageIndex);
            }
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

        #region 方法

        // 刷新页码条
        private void ReFreshPageBar()
        {
            PageInfos = new ObservableCollection<PageInfo>();

            if (PageSize == 0 || Total == 0)
            {
                return;
            }

            int pageCount = (int)Math.Ceiling(Total / (PageSize * 1.0));    // 总共多少页

            PageInfos.Add(new PageInfo
            {
                Name = "<",
                ToolTip = "上一页",
                Value = PageIndex - 1,
                IsEnabled = PageIndex != 1 && pageCount != 1,
                IndexChangeCommand = new RelayCommand(IndexChanged),
                Foreground = Foreground,
                Background = Background
            });

            PageInfos.Add(new PageInfo
            {
                Name = "1",
                ToolTip = "1",
                Value = 1,
                IsEnabled = true,
                IndexChangeCommand = new RelayCommand(IndexChanged),
                Foreground = Foreground,
                Background = PageIndex == 1 ? CurrentBrush : Background
            });

            int begin = PageIndex >= 6 ? PageIndex - 3 : 2;                         // index 大于等于 6 页就从 index-3 开始，否则 2 开始
            int end = PageIndex + 3 >= pageCount ? pageCount - 1 : PageIndex + 3;   // index + 3 大于 total 就 total-1结束，否则 index + 3

            for (int i = begin; i <= end; i++)
            {
                PageInfo info = new PageInfo()
                {
                    Value = i,
                    Name = i.ToString(),
                    IsEnabled = true,
                    ToolTip = i.ToString(),
                    IndexChangeCommand = new RelayCommand(IndexChanged),
                    Foreground = Foreground,
                    Background = PageIndex == i ? CurrentBrush : Background
                };

                if (i == begin && PageIndex - begin >= 3 && PageIndex > 5)
                {
                    info.Value = PageIndex - 5;
                    info.Name = "...";
                    info.ToolTip = "向前 5 页";
                }
                else if (i == end && end - PageIndex >= 3 && pageCount - PageIndex >= 5)
                {
                    info.Value = PageIndex + 5;
                    info.Name = "...";
                    info.ToolTip = "向后 5 页";
                }

                PageInfos.Add(info);
            }

            // 最后一页
            if (pageCount > 1)
            {
                PageInfos.Add(new PageInfo()
                {
                    Name = pageCount.ToString(),
                    ToolTip = pageCount.ToString(),
                    Value = pageCount,
                    IsEnabled = true,
                    IndexChangeCommand = new RelayCommand(IndexChanged),
                    Foreground = Foreground,
                    Background = Background
                });
            }

            // 下一页
            PageInfos.Add(new PageInfo()
            {
                Name = ">",
                ToolTip = "下一页",
                Value = PageIndex + 1,
                IsEnabled = PageIndex != pageCount && pageCount != 1,
                IndexChangeCommand = new RelayCommand(IndexChanged),
                Foreground = Foreground,
                Background = Background
            });
        }

        private void IndexChanged(object obj)
        {
            int value = (int)obj;
            PageIndex = value;
        }
        #endregion
    }
}
