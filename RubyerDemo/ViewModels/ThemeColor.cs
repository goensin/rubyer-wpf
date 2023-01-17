using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 主题颜色信息
    /// </summary>
    public partial class ThemeColorInfo : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private Brush primary;

        [ObservableProperty]
        private Brush light;

        [ObservableProperty]
        private Brush dark;

        [ObservableProperty]
        private Brush accent;

        [ObservableProperty]
        private bool isSeleted;
    }
}
