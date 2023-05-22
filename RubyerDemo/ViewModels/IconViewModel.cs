using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 图标
    /// </summary>
    public partial class IconViewModel : ObservableObject
    {
        public IconViewModel()
        {
            AllIcons = Icon.GetAllIconInfo();
            IconInfos = allIcons.GroupBy(x => x.Group);
        }

        [ObservableProperty]
        private IEnumerable<IconInfo> allIcons;

        [ObservableProperty]
        private IEnumerable<IGrouping<string, IconInfo>> iconInfos;

        [ObservableProperty]
        private IconInfo currentIcon;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private int iconTabIndex;

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IconInfos = allIcons.GroupBy(x => x.Group);
            }
            else
            {
                IconInfos = allIcons.Where(x => x.Type.ToString().Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)).GroupBy(x => x.Group);
            }

            IconTabIndex = 0;
        }

        [RelayCommand]
        private void SelectedItem(IconInfo icon)
        {
            CurrentIcon = icon;
        }
    }
}