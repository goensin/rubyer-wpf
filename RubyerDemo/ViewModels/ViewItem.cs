using CommunityToolkit.Mvvm.ComponentModel;
using Rubyer;
using System;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 视图项
    /// </summary>
    [INotifyPropertyChanged]
    public partial class ViewItem
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

        /// <summary>
        /// 是否更新
        /// </summary>
        [ObservableProperty]
        private bool isNew;

        public ViewItem(string name, string description, object content, IconType? iconType = null, DateTime? updateTime = null)
        {
            Name = name;
            Description = description;
            Content = content;
            IconType = iconType;
            IsNew = updateTime is { } && DateTime.Now - updateTime < TimeSpan.FromDays(60);
        }
    }
}
