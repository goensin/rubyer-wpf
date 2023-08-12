using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using Rubyer.Models;
using RubyerDemo.Consts;
using RubyerDemo.Views;
using RubyerDemo.Views.Samples;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RubyerDemo.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            Title = "Rubyer UI";

            ViewItems = new ObservableCollection<ViewItem>
            {
                new ViewItem("Overview", "总览", new Overview(), IconType.Home2Line),
                new ViewItem("Button", "按钮", new ButtonDemo(), IconType.CheckboxBlankFill),
                new ViewItem("DropDownButton", "下拉按钮", new DropDownButtonDemo(), IconType.ArrowDownCircleLine),
                new ViewItem("ToggleButton", "切换按钮", new ToggleButtonDemo(), IconType.ToggleLine),
                new ViewItem("TextBox", "文本框", new TextBoxDemo(), IconType.TBoxLine),
                new ViewItem("Password", "密码框", new PasswordBoxDemo(), IconType.LockPasswordLine),
                new ViewItem("NumericBox", "数值框", new NumericBoxDemo(), IconType.AddBoxLine),
                new ViewItem("ComboBox", "下拉框", new ComboBoxDemo(), IconType.ArrowDownSLine),
                new ViewItem("DateTimePicker", "日期时间选择", new DateTimePickerDemo(), IconType.TimeLine),
                new ViewItem("Renamer", "重命名", new RenamerDemo(), IconType.Edit2Line),
                new ViewItem("Slider", "滑动条", new SliderDemo(), IconType.GitCommitFill),
                new ViewItem("ProgressBar", "进度条", new ProgressBarDemo(), IconType.Loader4Line),
                new ViewItem("Icon", "图标", new IconDemo(), IconType.RemixiconLine),
                new ViewItem("Grid", "网格", new GridDemo(), IconType.GridLine),
                new ViewItem("GroupBox", "分组框", new GroupBoxDemo(), IconType.WindowFill),
                new ViewItem("Expander", "展开框", new ExpanderDemo(), IconType.LayoutTopLine),
                new ViewItem("ListBox", "列表框", new ListBoxDemo(), IconType.ListUnordered),
                new ViewItem("ListView", "列表视图", new ListViewDemo(), IconType.ListCheck2),
                new ViewItem("TreeView", "树形视图", new TreeViewDemo(), IconType.NodeTree),
                new ViewItem("TreeListView", "树形列表视图", new TreeListViewDemo(), IconType.NodeTree),
                new ViewItem("DataGrid", "数据表格", new DataGridDemo(), IconType.Table2),
                new ViewItem("TabControl", "选项卡", new TabControlDemo(), IconType.Layout4Line),
                new ViewItem("MenuBar", "菜单栏", new MenuBarDemo(), IconType.MenuLine),
                new ViewItem("TextBlock", "文本块", new TextBlockDemo(), IconType.Text),
                new ViewItem("PageBar", "页码条", new PageBarDemo(), IconType.MoreLine),
                new ViewItem("Message", "消息提示", new MessageDemo(), IconType.DiscussLine),
                new ViewItem("MessageBox", "消息框", new MessageBoxDemo(), IconType.ChatCheckLine),
                new ViewItem("Notification", "通知", new NotificationDemo(), IconType.QuestionAnswerLine),
                new ViewItem("Dialog", "对话框", new DialogDemo(), IconType.PictureInPictureLine),
                new ViewItem("Transition", "转换动画", new TransitionDemo(), IconType.ClockwiseLine),
                new ViewItem("BadgeTag", "标记标签", new BadgeTagDemo(), IconType.NotificationBadgeLine),
                new ViewItem("Loading", "加载中", new LoadingDemo(), IconType.Loader2Fill),
                new ViewItem("StepBar", "步骤条", new StepBarDemo(), IconType.ListOrdered),
                new ViewItem("Description", "描述列表", new DescriptionDemo(), IconType.ListCheck2),
                new ViewItem("HamburgerMenu", "汉堡包", new HamburgerMenuDemo(), IconType.MenuUnfoldLine),
                new ViewItem("FlipView", "滑动视图", new FlipViewDemo(), IconType.ImageLine),
            };

            CurrentViewItem = ViewItems.First();

            ThemeColors = new ObservableCollection<ThemeColorInfo>
            {
                new ThemeColorInfo
                {
                    Name = "默认蓝",
                    Url = @"pack://application:,,,/RubyerDemo;component/Themes/BlueColor.xaml",
                    Primary = new SolidColorBrush(Color.FromRgb(0x21,0x96,0xF3)),
                    IsSeleted =true
                },
                new ThemeColorInfo
                {
                    Name = "酷安绿",
                    Url = @"pack://application:,,,/RubyerDemo;component/Themes/GreenColor.xaml",
                    Primary = new SolidColorBrush(Color.FromRgb(0x0B,0xA3,0x61)),
                    IsSeleted = false
                },
                new ThemeColorInfo
                {
                    Name = "网易红",
                    Primary = new SolidColorBrush(Color.FromRgb(0xE5,0x39,0x35)),
                    Url = @"pack://application:,,,/RubyerDemo;component/Themes/RedColor.xaml",
                    IsSeleted =false
                },
                new ThemeColorInfo
                {
                    Name = "基佬紫",
                    Primary =new SolidColorBrush( Color.FromRgb(0x6A,0x1B,0x9A)),
                    Url = @"pack://application:,,,/RubyerDemo;component/Themes/PurpleColor.xaml",
                    IsSeleted =false
                },
                new ThemeColorInfo
                {
                    Name = "哔哩粉",
                    Primary = new SolidColorBrush(Color.FromRgb(0xFB,0x72,0x99)),
                    Url = @"pack://application:,,,/RubyerDemo;component/Themes/PinkColor.xaml",
                    IsSeleted =false
                },
            };

            SampleItems = new ObservableCollection<ViewItem>
            {
                new ViewItem("微信", "微信", new Wechat(), IconType.WechatFill),
                new ViewItem("网易云音乐", "网易云音乐", new NetEaseCloudMusic(), IconType.NeteaseCloudMusicLine),
            };
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private ObservableCollection<ViewItem> viewItems;

        [ObservableProperty]
        private ViewItem currentViewItem;

        [ObservableProperty]
        private ObservableCollection<ThemeColorInfo> themeColors;

        [ObservableProperty]
        private ObservableCollection<ViewItem> sampleItems;

        [RelayCommand]
        private void ChangeThemeColor(ThemeColorInfo info)
        {
            if (info.IsSeleted)
            {
                return;
            }

            ThemeManager.ApplyThemeColor(info.Url);

            foreach (var item in ThemeColors)
            {
                item.IsSeleted = false;
            }

            info.IsSeleted = true;
        }

        [RelayCommand]
        private async Task OpenAboutDialog()
        {
            var content = new About();
            await Dialog.Show(content, title: "关于");
        }

        [RelayCommand]
        private void OpenSampleWindow(ViewItem item)
        {
            var window = item.Content as Window;
            window.Show();
            window.Activate();
        }
    }
}