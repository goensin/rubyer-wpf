using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 列表框
    /// </summary>
    public partial class ListBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> items = new List<string>
        {
            "显示        ",
            "声音        ",
            "通知和操作  ",
            "电源和睡眠  ",
        };

        [ObservableProperty]
        private FoodType currentFood;
    }
}
