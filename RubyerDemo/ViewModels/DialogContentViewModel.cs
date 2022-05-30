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

        public event Action<IParameters> RequestClose;

        public void OnDialogOpened(IParameters parameters)
        {
            User = parameters.GetValue<User>("User");
        }

        private RelayCommand login;
        public RelayCommand Login => login ?? (login = new RelayCommand(LoginExecute));

        private void LoginExecute(object obj)
        {
            var parameters = new Parameters { { "User", User.Clone() } };
            Dialog.Close(ConstNames.MainDialogBox, parameters);
        }
       
    }
}
