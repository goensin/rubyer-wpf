using Rubyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class IconViewModel : ViewModelBase
    {
        public IEnumerable<IconInfo> Types
        {
            get
            {
                var icons = Icon.GetAllIconInfo();
                return icons.Select(x => new IconInfo(x.type, x.group));
            }
        }

        private IEnumerable<IconInfo> iconTypes;

        public IEnumerable<IconInfo> IconTypes
        {
            get { return iconTypes ?? (iconTypes = Types); }
            set
            {
                iconTypes = value;
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
                IconTypes = Types;
            }
            else
            {
                IconTypes = Types.Where(i => i.Type.ToString().IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
        }

        /// <summary>
        /// 图标信息
        /// </summary>
        public class IconInfo
        {
            /// <summary>
            /// 图标类型
            /// </summary>
            public IconType Type { get; set; }

            /// <summary>
            /// 组
            /// </summary>
            public string Group { get; set; }

            public IconInfo(IconType type, string group)
            {
                Type = type;
                Group = group;
            }
        }
    }
}