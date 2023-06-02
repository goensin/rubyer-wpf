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

            //Persons = new ObservableCollection<Person>
            //{
            //    new Person{ Id=1,Name="张三",Age=17,IsSelected=false,Gender=GenderType.Men},
            //    new Person{ Id=2,Name="李四",Age=18,IsSelected=true,Gender=GenderType.Men},
            //    new Person{ Id=3,Name="王五",Age=19,IsSelected=false,Gender=GenderType.Women},
            //    new Person{ Id=4,Name="赵六",Age=20,IsSelected=true,Gender=GenderType.Men},
            //    new Person{ Id=5,Name="孙七",Age=21,IsSelected=false,Gender=GenderType.Men},
            //    new Person{ Id=6,Name="周八",Age=22,IsSelected=true,Gender=GenderType.Women},
            //    new Person{ Id=7,Name="吴九",Age=23,IsSelected=false,Gender=GenderType.Men}
            //};
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