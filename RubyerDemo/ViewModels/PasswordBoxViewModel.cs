using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 密码框
    /// </summary>
    public partial class PasswordBoxViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(10, ErrorMessage = "密码长度不能超过 {1}")]
        [Required]
        private string password = "123456";
    }
}
