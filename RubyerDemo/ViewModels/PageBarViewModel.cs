using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public partial class PageBarViewModel : ObservableObject
    {
        public PageBarViewModel()
        {
            Total = 100;
            PageSize = 10;
        }

        [ObservableProperty]
        private int total;

        [ObservableProperty]
        private int pageSize;

        [ObservableProperty]
        private string pageBarMessage;

        [RelayCommand]
        private void PageIndexChanged(int index)
        {
            PageBarMessage = $"当前页：{index}";
        }

        [RelayCommand]
        private void PageSizeChanged(int size)
        {
            PageBarMessage = $"每页条数：{size}";
        }
    }
}
