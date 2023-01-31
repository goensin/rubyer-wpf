using CommunityToolkit.Mvvm.ComponentModel;
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
        private bool isSeleted;

        [ObservableProperty]
        private string url;
    }
}
