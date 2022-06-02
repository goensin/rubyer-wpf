using Rubyer;
using Rubyer.Commons;
using RubyerDemo.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Dialog.Close(ConstNames.MainDialogBox, User.Clone());
        }
    }
}
