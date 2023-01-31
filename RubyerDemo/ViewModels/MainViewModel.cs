using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using Rubyer.Models;
using RubyerDemo.Consts;
using RubyerDemo.Views;
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

            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem("Button", "按钮", new ButtonDemo(), IconType.CheckboxBlankFill),
                new MenuItem("ToggleButton", "切换按钮", new ToggleButtonDemo(), IconType.ToggleLine),
                new MenuItem("TextBox", "文本框", new TextBoxDemo(), IconType.TBoxLine),
                new MenuItem("Password", "密码框", new PasswordBoxDemo(), IconType.LockPasswordLine),
                new MenuItem("NumericBox", "数值框", new NumericBoxDemo(), IconType.AddBoxLine),
                new MenuItem("ComboBox", "下拉框", new ComboBoxDemo(), IconType.ArrowDownSLine),
                new MenuItem("DateTimePicker", "日期时间选择", new DateTimePickerDemo(), IconType.TimeLine),
                new MenuItem("Renamer", "重命名", new RenamerDemo(), IconType.Edit2Line),
                new MenuItem("Slider", "滑动条", new SliderDemo(), IconType.GitCommitFill),
                new MenuItem("ProgressBar", "进度条", new ProgressBarDemo(), IconType.Loader4Line),
                new MenuItem("Icon", "图标", new IconDemo(), IconType.RemixiconLine),
                new MenuItem("Grid", "网格", new GridDemo(), IconType.GridLine),
                new MenuItem("GroupBox", "分组框", new GroupBoxDemo(), IconType.WindowFill),
                new MenuItem("Expander", "展开框", new ExpanderDemo(), IconType.LayoutTopLine),
                new MenuItem("ListBox", "列表框", new ListBoxDemo(), IconType.ListUnordered),
                new MenuItem("ListView", "列表视图", new ListViewDemo(), IconType.ListCheck2),
                new MenuItem("TreeView", "树形视图", new TreeViewDemo(), IconType.ListCheck2),
                new MenuItem("DataGrid", "数据表格", new DataGridDemo(), IconType.Table2),
                new MenuItem("TabControl", "选项卡", new TabControlDemo(), IconType.Layout4Line),
                new MenuItem("MenuBar", "菜单栏", new MenuBarDemo(), IconType.MenuLine),
                new MenuItem("TextBlock", "文本块", new TextBlockDemo(), IconType.Text),
                new MenuItem("PageBar", "页码条", new PageBarDemo(), IconType.MoreLine),
                new MenuItem("Message", "消息提示", new MessageDemo(), IconType.DiscussLine),
                new MenuItem("MessageBox", "消息框", new MessageBoxDemo(), IconType.ChatCheckLine),
                new MenuItem("Notification", "通知", new NotificationDemo(), IconType.QuestionAnswerLine),
                new MenuItem("Dialog", "对话框", new DialogDemo(), IconType.PictureInPictureLine),
                new MenuItem("Transition", "转换动画", new TransitionDemo(), IconType.ClockwiseLine),
                new MenuItem("BadgeTag", "标记标签", new BadgeTagDemo(), IconType.NotificationBadgeLine),
                new MenuItem("Loading", "加载中", new LoadingDemo(), IconType.Loader2Fill),
                new MenuItem("StepBar", "步骤条", new StepBarDemo(), IconType.ListOrdered),
                new MenuItem("Description", "描述列表", new DescriptionDemo(), IconType.ListCheck2),
                new MenuItem("HamburgerMenu", "汉堡包", new HamburgerMenuDemo(), IconType.MenuUnfoldLine),
            };

            CurrentMenuItem = MenuItems.First();

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
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private ObservableCollection<MenuItem> menuItems;

        [ObservableProperty]
        private MenuItem currentMenuItem;

        [ObservableProperty]
        private ObservableCollection<ThemeColorInfo> themeColors;

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
    }
}