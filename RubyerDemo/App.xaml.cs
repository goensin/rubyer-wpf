using RubyerDemo.ViewModels;
using ShowMeTheXAML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RubyerDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            XamlDisplay.Init();

            base.OnStartup(e);
            
            MainWindow window = new MainWindow { DataContext = new MainViewModel() };
            window.Show();
        }
    }
}