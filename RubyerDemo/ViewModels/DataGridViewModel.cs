using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using RubyerDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// DataGrid
    /// </summary>
    public partial class DataGridViewModel : ObservableObject
    {
        public DataGridViewModel()
        {
            var uri = new Uri("weather.json", UriKind.Relative);
            var streamResourceInfo = Application.GetResourceStream(uri);
            var json = new StreamReader(streamResourceInfo.Stream).ReadToEnd();
            Weathers = JsonSerializer.Deserialize<ObservableCollection<WeatherInfo>>(json);

            uri = new Uri("city.json", UriKind.Relative);
            streamResourceInfo = Application.GetResourceStream(uri);
            json = new StreamReader(streamResourceInfo.Stream).ReadToEnd();
            Cities = JsonSerializer.Deserialize<List<City>>(json);
        }

        /// <summary>
        /// 天气信息
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<WeatherInfo> weathers;

        /// <summary>
        /// 省份城市
        /// </summary>
        public List<City> Cities { get; }

        [RelayCommand]
        private async Task Delete(WeatherInfo info)
        {
            var result = await MessageBoxR.Confirm("是否删除信息？");
            Message.Show(result);
        }
    }
}