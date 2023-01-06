using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public partial class InputBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private int? number;

        [ObservableProperty]
        private string testPassword;

        public List<int> Heights => Enumerable.Range(0, 100).ToList();


        private int intValue;

        public int IntValue
        {
            get => intValue;
            set
            {
                if (value > 5)
                {
                    SetProperty(ref intValue, 5);
                }
                else
                {
                    SetProperty(ref intValue, value);
                }
            }
        }

        [ObservableProperty]
        private int rangeValue = 20;

        [ObservableProperty]
        private int intervalValue;

        [ObservableProperty]
        private double? nullableValue;

        [ObservableProperty]
        private double doubleValue = 0.1;

        [ObservableProperty]
        private double exponentDoubleValue;

        [ObservableProperty]
        private FoodType currentFood;

        [ObservableProperty]
        private string name = "新建文本文档.txt";

        [RelayCommand]
        private void Renamer(object text)
        {
            Debug.WriteLine($"Renamer TextChangedCommand: {text}");
        }

        public InputBoxViewModel()
        {
            TestPassword = "123456";
        }
    }
}