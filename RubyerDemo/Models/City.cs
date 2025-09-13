using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace RubyerDemo.Models
{
    /// <summary>
    /// 城市
    /// </summary>
    public partial class City : ObservableObject, ICloneable
    {
        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<City> children = new ObservableCollection<City>();

        [ObservableProperty]
        private bool isExpanded = false;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
