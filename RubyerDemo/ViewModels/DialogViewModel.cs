using Rubyer;
using RubyerDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        public DialogViewModel()
        {
            User = new User();
            Users = new ObservableCollection<User>
            {
               new User{ Name="张三",Age=18 },
               new User{ Name="李四",Age=19 }
            };
        }

        private bool dialog1IsShow;
        public bool Dialog1IsShow
        {
            get => dialog1IsShow;
            set
            {
                dialog1IsShow = value;
                RaisePropertyChanged("Dialog1IsShow");
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                RaisePropertyChanged("Users");
            }
        }


        private RelayCommand openDialog1;
        public RelayCommand OpenDialog1 => openDialog1 ?? (openDialog1 = new RelayCommand(OpenDialog1Execute));
        // 打开 1# 对话框
        private void OpenDialog1Execute(object obj)
        {
            Dialog1IsShow = true;
        }

        private RelayCommand closeDialog1;
        public RelayCommand CloseDialog1 => closeDialog1 ?? (closeDialog1 = new RelayCommand(CloseDialog1Execute));
        // 关闭 1# 对话框
        private void CloseDialog1Execute(object obj)
        {
            Dialog1IsShow = false;
        }

        private RelayCommand beforeDialog3Open;
        public RelayCommand BeforeDialog3Open => beforeDialog3Open ?? (beforeDialog3Open = new RelayCommand(BeforeDialog3OpenExecute));
        // 2# 对话框打开前
        private void BeforeDialog3OpenExecute(object obj)
        {
            Message.ShowInContainer("MessageContainer", "打开 3# 对话框");
        }

        private RelayCommand afterDialog3Close;
        public RelayCommand AfterDialog3Close => afterDialog3Close ?? (afterDialog3Close = new RelayCommand(AfterDialog3CloseExecute));
        // 2# 对话框关闭后
        private void AfterDialog3CloseExecute(object obj)
        {
            if (obj is User user)
            {
                Users.Add((User)user.Clone());
            }
        }

        private RelayCommand openDialog4;
        public RelayCommand OpenDialog4 => openDialog4 ?? (openDialog4 = new RelayCommand(OpenDialog4Execute));
        // 打开 4# 对话框
        private void OpenDialog4Execute(object obj)
        {
            var content = new DialogContent();
            DialogBox.Show("MainDialog", content, "登录", BeforeDialog4Open, AfterDialog4Close);
        }

        private void AfterDialog4Close(DialogBox dialog, object arg)
        {
            Debug.WriteLine($"4# 对话框关闭参数:{arg}");
        }

        private void BeforeDialog4Open(DialogBox dialog)
        {
            Debug.WriteLine("4# 对话框打开");
        }
    }

    public class User : ICloneable
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
