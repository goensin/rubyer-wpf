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
            for (int i = 0; i < Weathers.Count; i += 2)
            {
                Weathers[i].IsSelected = true;
            }

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
        /// 星期
        /// </summary>
        public IEnumerable<string> Weeks => ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"];

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