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
            SelectedForeground = AllBrushes[8];
            SelectedBrush = AllBrushes[2];
            UnselectedBrush = AllBrushes[0];
        }

        public List<Brush> AllBrushes => new List<Brush>
        {
            Application.Current.Resources["Primary"] as Brush,
            Application.Current.Resources["Light"] as Brush,
            Application.Current.Resources["Dark"] as Brush,
            Application.Current.Resources["Accent"] as Brush,
            Application.Current.Resources["Success"] as Brush,
            Application.Current.Resources["Warning"] as Brush,
            Application.Current.Resources["Info"] as Brush,
            Application.Current.Resources["Error"] as Brush,
            Application.Current.Resources["WhiteForeground"] as Brush,
            Application.Current.Resources["BlackForeground"] as Brush,
        };

        [ObservableProperty]
        private Brush selectedForeground;

        [ObservableProperty]
        private Brush selectedBrush;

        [ObservableProperty]
        private Brush unselectedBrush;

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
