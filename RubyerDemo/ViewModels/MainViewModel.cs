﻿using Rubyer;
using RubyerDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RubyerDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
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
                new MenuItem{ Name = "按钮-Button",Content=new Button()},
                new MenuItem{ Name = "输入框-InputBox",Content=new InputBox() },
                new MenuItem{ Name = "选择框-SelectBox",Content=new SelectBox()},
                new MenuItem{ Name = "数据条-DataBar",Content=new DataBar()},
                new MenuItem{ Name = "图标-Icon",Content=new Views.Icon{ DataContext = new IconViewModel()} },
                new MenuItem{ Name = "分组框-GroupBox",Content=new GroupBox() },
                new MenuItem{ Name = "列表与树-ListsTree",Content=new ListsTree{ DataContext = new ListsViewModel()} },
                new MenuItem{ Name = "数据表格-DataGrid",Content=new DataGrid{ DataContext = new DataGridViewModel()} },
                new MenuItem{ Name = "选项卡-TabControl",Content=new TabControl{ DataContext = new TabControlViewModel()} },
                new MenuItem{ Name = "日期时间-DateTimeControl",Content=new DateTimeControl{} },
                new MenuItem{ Name = "菜单栏-MenuBar",Content=new MenuBar{} },
                new MenuItem{ Name = "文本块-TextBlock",Content=new TextBlock{} },
                new MenuItem{ Name = "页码条-PageBar",Content=new Views.PageBar{ DataContext = new PageBarViewModel()} },
                new MenuItem{ Name = "消息提示-MessageInfo",Content=new MessageInfo{} },
                new MenuItem{ Name = "消息框-MessageBoxView",Content = new MessageBoxView{ DataContext= new MessageBoxViewModel()} },
                new MenuItem{ Name = "对话框-DialogView",Content = new DialogView{ DataContext= new DialogViewModel()} },
                new MenuItem{ Name = "转换动画-TransitionView",Content = new TransitionView() },
                new MenuItem{ Name = "标记标签-BadgeTag",Content = new BadgeTag() }
            };

            CurrentMenuItem = MenuItems[0];

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
                    Name = "姨妈红",
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
                new ThemeColor
                {
                    Name = "高端黑",
                    Primary = new SolidColorBrush(Color.FromRgb(0x26,0x32,0x38)),
                    Light = new SolidColorBrush(Color.FromRgb(0x4F,0x5B,0x62)),
                    Dark = new SolidColorBrush(Color.FromRgb(0x00,0x0A,0x12)),
                    Accent = new SolidColorBrush(Color.FromRgb(0xAD,0x14,0x57)),
                    IsSeleted =false
                },
            };
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private ObservableCollection<MenuItem> menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        private MenuItem currentMenuItem;
        public MenuItem CurrentMenuItem
        {
            get => currentMenuItem;
            set
            {
                currentMenuItem = value;
                RaisePropertyChanged("CurrentMenuItem");
            }
        }

        private ObservableCollection<ThemeColor> themeColors;
        public ObservableCollection<ThemeColor> ThemeColors
        {
            get => themeColors;
            set
            {
                themeColors = value;
                RaisePropertyChanged("ThemeColors");
            }
        }


        private RelayCommand changeThemeColor;
        public RelayCommand ChangeThemeColor => changeThemeColor ?? (changeThemeColor = new RelayCommand(ChangeThemeColorExecute));

        private void ChangeThemeColorExecute(object obj)
        {
            ThemeColor themeColor = obj as ThemeColor;

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

        private RelayCommand openAboutDialog;
        public RelayCommand OpenAboutDialog => openAboutDialog ?? (openAboutDialog = new RelayCommand(OpenAboutDialogExecute));

        private void OpenAboutDialogExecute(object obj)
        {
            var content = new About();
            DialogBox.Show("MainDialog", content, "关于");
        }
    }
}
