using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class DataGridViewModel : ObservableObject
    {
        public DataGridViewModel()
        {
            var uri = new Uri("weather.json", UriKind.Relative);
            var streamResourceInfo = Application.GetResourceStream(uri);
            var json = new StreamReader(streamResourceInfo.Stream).ReadToEnd();
            Weathers = JsonSerializer.Deserialize<ObservableCollection<WeatherInfo>>(json);
        }



        /// <summary>
        /// 天气信息
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<WeatherInfo> weathers;

        [RelayCommand]
        private void Delete(WeatherInfo person)
        {
            //Persons.Remove(person);
        }
    }
}