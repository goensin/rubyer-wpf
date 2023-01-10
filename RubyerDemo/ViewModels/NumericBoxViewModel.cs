using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 数值框
    /// </summary>
    public partial class NumericBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private int intValue = 1;

        [ObservableProperty]
        private double doubleValue = 0.1;
    }
}
