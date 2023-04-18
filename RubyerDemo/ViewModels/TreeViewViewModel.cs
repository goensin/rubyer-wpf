using CommunityToolkit.Mvvm.ComponentModel;
using RubyerDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 树形控件
    /// </summary>
    public partial class TreeViewViewModel : ObservableObject
    {
        public TreeViewViewModel()
        {
            var json = File.ReadAllText("city.json");
            Cities = JsonSerializer.Deserialize<List<City>>(json);
        }

        /// <summary>
        /// 省份城市
        /// </summary>
        public List<City> Cities { get; }

        /// <summary>
        /// 选中城市
        /// </summary>
        [ObservableProperty]
        private City selectedCity;
    }
}
