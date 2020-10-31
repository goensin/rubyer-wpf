using Rubyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class IconViewModel : ViewModelBase
    {
        private Lazy<IEnumerable<IconType>> _types;

        public IconViewModel()
        {
            _types = new Lazy<IEnumerable<IconType>>(() => Enum.GetValues(typeof(IconType)).OfType<IconType>().ToList());
        }

        private IEnumerable<IconType> iconTypes;
        public IEnumerable<IconType> IconTypes
        {
            get { return iconTypes ?? (iconTypes = _types.Value); }
            set
            {
                iconTypes = value;
                RaisePropertyChanged("IconTypes");
            }
        }

        private IconType currentIcon;
        public IconType CurrentIcon
        {
            get => currentIcon;
            set
            {
                currentIcon = value;
                RaisePropertyChanged("CurrentIcon");
            }
        }

        private string searchText;
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
        public RelayCommand Search => search ?? (search = new RelayCommand(SearchExecute));
        // 搜索
        private void SearchExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IconTypes = _types.Value;
            }
            else
            {
                IconTypes = _types.Value.Where(i => i.ToString().IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
        }
    }
}
