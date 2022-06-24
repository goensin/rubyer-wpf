using Rubyer;
using Rubyer.Commons;
using Rubyer.Models;
using System;

namespace RubyerDemo.ViewModels
{
    public class DialogContentViewModel : ViewModelBase, IDialogContext
    {
        public DialogContentViewModel()
        {

        }

        public string Title => "登录";

        public User User { get; set; }

        public event Action<object> RequestClose;

        public void OnDialogOpened(object parameter)
        {
            var parameters = (IParameters)parameter;
            User = parameters.GetValue<User>("User");
        }

        private RelayCommand login;
        public RelayCommand Login => login ?? (login = new RelayCommand(LoginExecute));

        private void LoginExecute(object obj)
        {
            var parameters = new Parameters();
            parameters.Add("User", User.Clone());
            RequestClose?.Invoke(parameters);
        }
    }
}
