﻿using Rubyer;
using RubyerDemo.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyerDemo.Views
{
    /// <summary>
    /// NotificationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationDemo : UserControl
    {
        public NotificationDemo()
        {
            InitializeComponent();
        }

        private void NotificationBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.ShowGlobal("message");
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.InfoGlobal("hello ~ \r\nrubyer ");
        }

        private void WaringBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.WarningGlobal("warning");
        }

        private void SuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.SuccessGlobal("success");
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.ErrorGlobal("erroooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor", millisecondTimeOut: 0);
        }

        private void NotificationContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Show(ConstNames.NotificationDemo, content: "message");
        }

        private void InfoContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Info(ConstNames.NotificationDemo, content: "info");
        }

        private void WaringContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Warning(ConstNames.NotificationDemo, content: "warning");
        }

        private void SuccessContainerBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Success(ConstNames.NotificationDemo, content: "success");
        }

        private void ErrorContaionBtn_Click(object sender, RoutedEventArgs e)
        {
            Notification.Error("error", millisecondTimeOut: 0);
        }
    }
}