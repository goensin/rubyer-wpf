using Rubyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class IconViewModel : ViewModelBase
    {
        public IEnumerable<IconInfo> AllIconInfo => Icon.GetAllIconInfo();

        private IEnumerable<IconInfo> iconInfos;

        public IEnumerable<IconInfo> IconInfos
        {
            get { return iconInfos ?? (iconInfos = AllIconInfo); }
            set
            {
                iconInfos = value;
                RaisePropertyChanged("IconTypes");
            }
        }

        private IconInfo currentIcon;

        /// <summary>
        /// 当前图标
        /// </summary>
        public IconInfo CurrentIcon
        {
            get => currentIcon;
            set
            {
                currentIcon = value;
                RaisePropertyChanged("CurrentIcon");
            }
        }

        private string searchText;

        /// <summary>
        /// 搜索文本
        /// </summary>
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                RaisePropertyChanged("SearchText");
            }
        }

        private RelayCommand search;

        /// <summary>
        /// 搜索命令
        /// </summary>
        public RelayCommand Search => search ?? (search = new RelayCommand(SearchExecute));

        private void SearchExecute(object obj)
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