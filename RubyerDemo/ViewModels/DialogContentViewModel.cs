using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using Rubyer.Commons;
using Rubyer.Models;
using System;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    public partial class DialogContentViewModel : ObservableObject, IDialogDataContext
    {
        public string Title => "用户登录";

        public object CloseParameter { get; set; }

        [ObservableProperty]
        public User user;

        public event Action<object> RequestClose;

        public void OnDialogOpened(object parameter)
        {
            var parameters = (IParameters)parameter;
            User = parameters.GetValue<User>("User");
        }

        [RelayCommand]
        private void Login(object obj)
        {
            var parameters = new Parameters();
            parameters.Add("User", User.Clone());
            RequestClose?.Invoke(parameters);
        }

        public void OnDialogClosing(DialogClosingRoutedEventArgs e)
        {
            //e.Parameter = "hello";
        }
    }
}
