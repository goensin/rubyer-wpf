﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using Rubyer.Models;
using RubyerDemo.Views;
using RubyerDemo.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        // 打开 1# 对话框
        [RelayCommand]
        private async Task OpenDialog1()
        {
            var view = new Dialog1View();
            await Dialog.Show("Dialog1", view, title: "1#对话框");
        }

        // 打开 2# 对话框
        [RelayCommand]
        private async Task OpenDialog2()
        {
            var view = new Dialog2View();
            await Dialog.Show("Dialog2", view, parameters: "2#对话框");
        }

        //// 3# 对话框打开前
        //[RelayCommand]
        //private void BeforeDialog3Open(object obj)
        //{
        //    Message.Show("打开 3# 对话框");
        //}

        //// 3# 对话框关闭后
        //[RelayCommand]
        //private void AfterDialog3Close(object obj)
        //{
        //    if (obj is IParameters parameters)
        //    {
        //        if (parameters.TryGetValue("User", out User user))
        //        {
        //            Users.Add((User)user.Clone());
        //        }
        //    }
        //}

        // 打开 4# 对话框
        [RelayCommand]
        private async void OpenDialog4(object obj)
        {
            var content = new DialogContent();
            var parameters = new Parameters { { "User", new User { Name = "wu", Password = "123" } } };
            var para = await Dialog.Show(content, title: "登录", parameters: parameters, openHandler: BeforeDialog4Open, closeHandle: AfterDialog4Close);
            if (para is IParameters parameter)
            {
                if (parameter.TryGetValue("User", out User user))
                {
                    Debug.WriteLine($"4# 对话框关闭异步返回结果:name:{user.Name},password:{user.Password}");
                }
            }
        }

        private void BeforeDialog4Open(DialogCard dialog)
        {
            Debug.WriteLine("4# 对话框打开");
        }

        private void AfterDialog4Close(DialogCard dialog, object obj)
        {
            if (obj is IParameters parameters)
            {
                if (parameters.TryGetValue("User", out User user))
                {
                    Debug.WriteLine($"4# 对话框关闭事件接收结果:name:{user.Name},password:{user.Password}");
                }
            }
        }

        [RelayCommand]
        private async Task OpenDialog5()
        {
            var res = await Dialog.Show(new Loading { Content = "loading..." }, showCloseButton: false, openHandler: async dialog =>
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

        public object Clone() => MemberwiseClone();
    }
}
