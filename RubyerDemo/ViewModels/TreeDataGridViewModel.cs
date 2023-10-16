using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using RubyerDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// DataGrid
    /// </summary>
    public partial class TreeDataGridViewModel : ObservableObject
    {
        public TreeDataGridViewModel()
        {
            UpdateCitiesSource();
        }

        [RelayCommand]
        private void UpdateCitiesSource()
        {
            var uri = new Uri("city.json", UriKind.Relative);
            var streamResourceInfo = Application.GetResourceStream(uri);
            var json = new StreamReader(streamResourceInfo.Stream).ReadToEnd();
            Cities = new ObservableCollection<City>(JsonSerializer.Deserialize<List<City>>(json));
        }

        [ObservableProperty]
        private ObservableCollection<City> cities;

        private void RemoveCity(City deleteCity, Collection<City> cities)
        {
            if (cities.Contains(deleteCity))
            {
                cities.Remove(deleteCity);
                return;
            }

            foreach (var city in cities)
            {
                if (city.Children is { })
                {
                    RemoveCity(deleteCity, city.Children);
                }
            }
        }

        [RelayCommand]
        private async Task Delete(City city)
        {
            var result = await MessageBoxR.Confirm("是否删除信息？");
            if (result == MessageBoxResult.Yes)
            {
                RemoveCity(city, Cities);
            }
        }

        [RelayCommand]
        private void Add(City city)
        {
            if (city.Children is null)
            {
                city.Children = new ObservableCollection<City>();
            }

            city.Children.Add(new City() { Code = city.Code + 1, Name = city.Name + 1 });
        }

        [RelayCommand]
        private void Move()
        {
            Cities.Move(1, 0);
            Notification.Info("移动索引 1 到 0");
        }

        private void ReplaceCity(City replaceCity, Collection<City> cities)
        {
            var index = cities.IndexOf(replaceCity);
            if (index < 0)
            {
                foreach (var city in cities)
                {
                    if (city.Children is { })
                    {
                        ReplaceCity(replaceCity, city.Children);
                    }
                }

                return;
            }

            cities[index] = new City() { Code = "新编号", Name = "新名称" };
        }

        [RelayCommand]
        private void Replace(City city)
        {
            ReplaceCity(city, Cities);

            Notification.Success("替换成功");
            DialogContainer.CloseDialogCommand.Execute(null, null);
        }

    }
}