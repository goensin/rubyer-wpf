using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 下拉框
    /// </summary>
    public partial class ComboBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> items = new List<string>
        {
            "选项一", "选项二", "选项三", "选项四"
        };

        [ObservableProperty]
        private FoodType currentFood;
    }
}
