using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 密码框
    /// </summary>
    public partial class PasswordBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private string password = "123456";
    }
}
