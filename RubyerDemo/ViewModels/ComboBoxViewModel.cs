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
        [ObservableProperty]
        private List<string> items = new List<string>
        {
            "选项一", "选项二", "选项三", "选项四"
        };

        public List<Food> Foods { get; }

        [ObservableProperty]
        private Food currentFoodType;

        [ObservableProperty]
        private ObservableCollection<Food> selectedFoods;

        public ComboBoxViewModel()
        {
            Foods = [new Food(1, FoodType.BeefMeatballs), new Food(2, FoodType.OysterOmelette), new Food(3, FoodType.KwayChap)];
            SelectedFoods = [Foods.First()];
        }

        [RelayCommand]
        private void ClearSelectedItems()
        {
            SelectedFoods?.Clear();
        }

        [RelayCommand]
        private void RemoveFirstItem()
        {
            if (SelectedFoods.Count > 0)
            {
                SelectedFoods.RemoveAt(0);
            }
        }

        [RelayCommand]
        private void ReplaceSelectedItems()
        {
            SelectedFoods = new ObservableCollection<Food>() { SelectedFoods.Last() };
        }
    }

    public class Food
    {
        public int Num { get; set; }
        public FoodType FoodType { get; set; }

        public Food(int num, FoodType foodType)
        {
            Num = num;
            FoodType = foodType;
        }
    }
}
