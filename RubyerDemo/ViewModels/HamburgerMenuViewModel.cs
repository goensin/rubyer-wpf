using Rubyer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// The hamburger menu view model.
    /// </summary>
    public class HamburgerMenuViewModel : ViewModelBase
    {
        private RelayCommand selecteCommand;

        /// <summary>
        /// 子项集合
        /// </summary>
        public ObservableCollection<HamburgerMenuModel> Models { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        public ObservableCollection<HamburgerMenuModel> OptionsModels { get; set; }

        /// <summary>
        /// 下一步命令
        /// </summary>
        public RelayCommand SelecteCommand => selecteCommand ?? (selecteCommand = new RelayCommand(SelecteExecute));

        private void SelecteExecute(object message)
        {
            Debug.WriteLine($"HamburgerMenuItem Command: {message}");
        }

        public HamburgerMenuViewModel()
        {
            Models = new ObservableCollection<HamburgerMenuModel>
            {
                new HamburgerMenuModel("用户",IconType.UserLine),
                new HamburgerMenuModel("文档",IconType.FileLine),
                new HamburgerMenuModel("图片",IconType.ImageLine),
                new HamburgerMenuModel("设置",IconType.Settings2Line),
                new HamburgerMenuModel("自定义",IconType.UserLine),
                new HamburgerMenuModel("无图标",null),
                new HamburgerMenuModel("不可用",IconType.EmotionUnhappyLine,false),
            };

            OptionsModels = new ObservableCollection<HamburgerMenuModel>
            {
                new HamburgerMenuModel("睡眠",IconType.MoonFill),
                new HamburgerMenuModel("关机",IconType.ShutDownLine),
                new HamburgerMenuModel("重启",IconType.RestartLine),
            };
        }
    }

    /// <summary>
    /// 汉堡包菜单模型
    /// </summary>
    public class HamburgerMenuModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public IconType? Icon { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable { get; set; }

        public HamburgerMenuModel(string name, IconType? icon, bool isEnable = true)
        {
            Name = name;
            Icon = icon;
            IsEnable = isEnable;
        }
    }
}