using CommunityToolkit.Mvvm.ComponentModel;
using Rubyer;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 菜单项
    /// </summary>
    [INotifyPropertyChanged]
    public partial class MenuItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        [ObservableProperty]
        private string name;

        /// <summary>
        /// 描述
        /// </summary>
        [ObservableProperty]
        private string description;

        /// <summary>
        /// 图标类型
        /// </summary>
        [ObservableProperty]
        private IconType? iconType;

        /// <summary>
        /// 内容
        /// </summary>
        [ObservableProperty]
        private object content;

        public MenuItem(string name, string description, object content, IconType? iconType = null)
        {
            Name = name;
            Description = description;
            Content = content;
            IconType = iconType;
        }
    }
}
