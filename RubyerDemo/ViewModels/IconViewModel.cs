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
    public partial class IconViewModel : ObservableObject
    {
        public IEnumerable<IconInfo> AllIconInfo => Icon.GetAllIconInfo();

        private IEnumerable<IconInfo> iconInfos;

        public IEnumerable<IconInfo> IconInfos
        {
            get
            {
                if (iconInfos == null)
                {
                    iconInfos = AllIconInfo;
                }

                return iconInfos;
            }
            set
            {
                SetProperty(ref iconInfos, value);
            }
        }

        /// <summary>
        /// 当前图标
        /// </summary>
        [ObservableProperty]
        private IconInfo currentIcon;

        /// <summary>
        /// 搜索文本
        /// </summary>
        [ObservableProperty]
        private string searchText;

        /// <summary>
        /// 搜索命令
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IconInfos = AllIconInfo;
            }
            else
            {
                IconInfos = AllIconInfo.Where(i => i.Type.ToString().IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
        }
    }
}