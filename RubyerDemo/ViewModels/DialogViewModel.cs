using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using Rubyer.Models;
using RubyerDemo.Consts;
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
    public partial class DialogViewModel : ObservableObject
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

        [ObservableProperty]
        private bool dialog1IsShow;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private ObservableCollection<User> users;


        public IParameters Paramters
        {
            get
            {
                Parameters parameters = new Parameters();
                parameters.Add("User", User);
                return parameters;
            }
        }

        // 打开 2# 对话框
        [RelayCommand]
        private void OpenDialog1()
        {
            Dialog1IsShow = true;
        }

        // 关闭 2# 对话框
        [RelayCommand]
        private void CloseDialog1()
        {
            Dialog1IsShow = false;
        }

        // 3# 对话框打开前
        [RelayCommand]
        private void BeforeDialog3Open(object obj)
        {
            Message.Show("打开 3# 对话框");
        }

        // 3# 对话框关闭后
        [RelayCommand]
        private void AfterDialog3Close(object obj)
        {
            if (obj is IParameters parameters)
            {
                if (parameters.TryGetValue("User", out User user))
                {
                    Users.Add((User)user.Clone());
                }
            }
        }

        // 打开 4# 对话框
        [RelayCommand]
        private async void OpenDialog4(object obj)
        {
            var content = new DialogContent();
            var parameters = new Parameters();
            parameters.Add("User", new User { Name = "wu", Password = "123" });
            var para = await Dialog.Show(content, parameters: parameters, openHandler: BeforeDialog4Open, closeHandle: AfterDialog4Close);
            Debug.WriteLine($"4# 对话框关闭异步返回结果:name:{user.Name},password:{user.Password}");
        }

        private void AfterDialog4Close(DialogContainer dialog, object obj)
        {
            if (obj is IParameters parameters)
            {
                if (parameters.TryGetValue("User", out User user))
                {
                    Debug.WriteLine($"4# 对话框关闭事件接收结果:name:{user.Name},password:{user.Password}");
                }
            }

        }

        private void BeforeDialog4Open(DialogContainer dialog)
        {
            Debug.WriteLine("4# 对话框打开");
        }

        [RelayCommand]
        private async void OpenDialog5(object obj)
        {
            _ = await Dialog.Show(new Loading { Content = "loading..." }, showCloseButton: false, openHandler: async dialog =>
            {
                await Task.Delay(3000);
                dialog.Close();
            });
        }
    }

    public partial class User : ObservableObject, ICloneable
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private int age;

        public object Clone() => this.MemberwiseClone();
    }
}
