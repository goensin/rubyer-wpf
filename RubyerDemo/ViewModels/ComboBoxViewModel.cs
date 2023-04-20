using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 下拉框
    /// </summary>
    public partial class ComboBoxViewModel : ObservableObject
    {
        public ComboBoxViewModel()
        {
            this.PropertyChanged += ComboBoxViewModel_PropertyChanged;
            SelectedItems = new ObservableCollection<FoodType>();
            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void ComboBoxViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        [ObservableProperty]
        private List<string> items = new List<string>
        {
            "选项一", "选项二", "选项三", "选项四"
        };

        [ObservableProperty]
        private FoodType currentFood;

        [ObservableProperty]
        private ObservableCollection<FoodType> selectedItems;

        [RelayCommand]
        private void ClearSelectedItems()
        {
            SelectedItems?.Clear();
        }

        [RelayCommand]
        private void RemoveFirstItem()
        {
            if (SelectedItems.Count > 0)
            {
                SelectedItems.RemoveAt(0);
            }
        }
    }
}
