using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 页码条
    /// </summary>
    public partial class PageBarViewModel : ObservableObject
    {
        public PageBarViewModel()
        {
            Total = 100;
            PageSize = 10;
            SelectedForegroundColor = Colors.White;
            SelectedColor = (Color)Application.Current.TryFindResource("LightDarkColor");
            UnselectedColor = (Color)Application.Current.TryFindResource("LightPrimaryColor");

            this.PropertyChanged += PageBarViewModel_PropertyChanged;
        }

        private void PageBarViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        [ObservableProperty]
        private Color selectedForegroundColor;

        [ObservableProperty]
        private Color selectedColor;

        [ObservableProperty]
        private Color unselectedColor;

        [ObservableProperty]
        private int total;

        [ObservableProperty]
        private int pageSize;

        [RelayCommand]
        private void PageIndexChanged(int index)
        {
            Debug.WriteLine($"当前页：{index}");
        }

        [RelayCommand]
        private void PageSizeChanged(int size)
        {
            Debug.WriteLine($"每页条数：{size}");
        }
    }
}
