using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// The hamburger menu view model.
    /// </summary>
    public partial class HamburgerMenuViewModel : ObservableObject
    {
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
        [RelayCommand]
        private void Selecte(object message)
        {
            Debug.WriteLine($"HamburgerMenuItem Command: {message}");
        }

        public HamburgerMenuViewModel()
        {
            Models = new ObservableCollection<HamburgerMenuModel>
            {
                new HamburgerMenuModel("用户", IconType.UserLine, group: "组一"),
                new HamburgerMenuModel("文档", IconType.FileLine, group: "组一"),
                new HamburgerMenuModel("图片", IconType.ImageLine, group: "组一"),
                new HamburgerMenuModel("设置", IconType.Settings2Line, group: "组二"),
                new HamburgerMenuModel("睡眠", IconType.MoonFill, group: "组二"),
                new HamburgerMenuModel("不可用", IconType.EmotionUnhappyLine, false, group: "组二"),
            };

            OptionsModels = new ObservableCollection<HamburgerMenuModel>
            {
                new HamburgerMenuModel("关机", IconType.ShutDownLine),
                new HamburgerMenuModel("重启", IconType.RestartLine),
                new HamburgerMenuModel("更多", IconType.More2Line)
                {
                    Childs = new ObservableCollection<HamburgerMenuModel>
                    {
                        new HamburgerMenuModel("WiFi", IconType.WifiLine),
                        new HamburgerMenuModel("蓝牙", IconType.BluetoothLine),
                    }
                },
            };
        }
    }

    /// <summary>
    /// 汉堡包菜单模型
    /// </summary>
    public partial class HamburgerMenuModel : ObservableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        [ObservableProperty]
        private string name;

        /// <summary>
        /// 组
        /// </summary>
        [ObservableProperty]
        private string group;

        /// <summary>
        /// 图标
        /// </summary>
        [ObservableProperty]
        private IconType? icon;

        /// <summary>
        /// 是否可用
        /// </summary>
        [ObservableProperty]
        private bool isEnable;

        [ObservableProperty]
        private ObservableCollection<HamburgerMenuModel> childs;

        public HamburgerMenuModel(string name, IconType? icon, bool isEnable = true, string group = "")
        {
            Name = name;
            Group = group;
            Icon = icon;
            IsEnable = isEnable;
        }
    }
}