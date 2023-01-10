using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 重命名工具
    /// </summary>
    public partial class RenamerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string fileName = "新建文本文档.txt";

        [RelayCommand]
        private void Renamer(string text)
        {
            Debug.WriteLine($"Renamer TextChangedCommand: {text}");
        }
    }
}
