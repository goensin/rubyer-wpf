using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
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
        private static readonly Brush primary = (Brush)Application.Current.Resources["Primary"];
        private static readonly Brush light = (Brush)Application.Current.Resources["Light"];
        private static readonly Brush dark = (Brush)Application.Current.Resources["Dark"];
        private static readonly Brush accent = (Brush)Application.Current.Resources["Accent"];

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

            ThemeColors = new ObservableCollection<ThemeColor>
            {
                new ThemeColor
                {
                    Name = "默认蓝",
                    Primary = primary,
                    Light = light,
                    Dark = dark,
                    Accent = accent,
                    IsSeleted =true
                },
                new ThemeColor
                {
                    Name = "酷安绿",
                    Primary = new SolidColorBrush(Color.FromRgb(0x0B,0xA3,0x61)),
                    Light = new SolidColorBrush(Color.FromRgb(0x56,0xD5,0x8F)),
                    Dark = new SolidColorBrush(Color.FromRgb(0x00,0x73,0x36)),
                    Accent = new SolidColorBrush(Color.FromRgb(0x79,0x55,0x48)),
                    IsSeleted =false
                },
                new ThemeColor
                {
                    Name = "网易红",
                    Primary = new SolidColorBrush(Color.FromRgb(0xE5,0x39,0x35)),
                    Light = new SolidColorBrush(Color.FromRgb(0xFF,0x6F,0x60)),
                    Dark = new SolidColorBrush(Color.FromRgb(0xAB,0x00,0x0D)),
                    Accent = new SolidColorBrush(Color.FromRgb(0x39,0x49,0xAB)),
                    IsSeleted =false
                },
                new ThemeColor
                {
                    Name = "基佬紫",
                    Primary = new SolidColorBrush(Color.FromRgb(0x6A,0x1B,0x9A)),
                    Light = new SolidColorBrush(Color.FromRgb(0x9C,0x4D,0xCC)),
                    Dark = new SolidColorBrush(Color.FromRgb(0x38,0x00,0x6B)),
                    Accent = new SolidColorBrush(Color.FromRgb(0xE6,0x51,0x00)),
                    IsSeleted =false
                },
                new ThemeColor
                {
                    Name = "哔哩粉",
                    Primary = new SolidColorBrush(Color.FromRgb(0xFB,0x72,0x99)),
                    Light = new SolidColorBrush(Color.FromRgb(0xFF,0xA4,0xCA)),
                    Dark = new SolidColorBrush(Color.FromRgb(0xC4,0x40,0x6B)),
                    Accent = new SolidColorBrush(Color.FromRgb(0x73,0xC9,0xE5)),
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
        private ObservableCollection<ThemeColor> themeColors;

        [RelayCommand]
        private void ChangeThemeColor(ThemeColor themeColor)
        {
            if (themeColor.IsSeleted)
            {
                return;
            }

            App.Current.Resources["Primary"] = themeColor.Primary;
            App.Current.Resources["Light"] = themeColor.Light;
            App.Current.Resources["Dark"] = themeColor.Dark;
            App.Current.Resources["Accent"] = themeColor.Accent;

            foreach (var item in ThemeColors)
            {
                item.IsSeleted = false;
            }

            themeColor.IsSeleted = true;
        }

        [RelayCommand]
        private async Task OpenAboutDialog()
        {
            var content = new About();
            await Dialog.Show(content, title: "关于");
        }
    }
}