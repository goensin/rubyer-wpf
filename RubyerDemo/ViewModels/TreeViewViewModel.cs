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
    public class TreeViewViewModel : ObservableObject
    {
        /// <summary>
        /// 所有城市
        /// </summary>
        public List<City> Cities { get; }

        public TreeViewViewModel()
        {
            var json = File.ReadAllText("city.json");
            Cities = JsonSerializer.Deserialize<List<City>>(json);
        }
    }
}
