using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Rubyer;
using Rubyer.Enums;
using RubyerDemo.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RubyerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RubyerWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetRequiredService<MainViewModel>();

            Loaded += MainWindow_Loaded;
            ThemeManager.ThemeModeChanged += OnThemeModeChanged;
        }

        private void OnThemeModeChanged(object sender, ThemeModeChangedArgs e)
        {
            if (e.IsDarkMode)
            {
                this.BorderBrush = (Brush)Application.Current.Resources["Dark"];
            }
            else
            {
                this.BorderBrush = (Brush)Application.Current.Resources["Primary"];
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            controlSlider.Value = ((CornerRadius)App.Current.Resources["AllControlCornerRadius"]).TopLeft;
            contrainerSlider.Value = ((CornerRadius)App.Current.Resources["AllContainerCornerRadius"]).TopLeft;
            ThemeManager.SwitchThemeMode(ThemeMode.System);
            darkToggleButton.IsChecked = ThemeManager.GetIsAppDarkMode();
        }

        private void controlSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ThemeManager.SwitchControlCornerRadius(e.NewValue);
        }

        private void contrainerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ThemeManager.SwitchContainerCornerRadius(e.NewValue);
        }

        private void BlackSwitch_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SwitchThemeMode(darkToggleButton.IsChecked.GetValueOrDefault() ? ThemeMode.Dark : ThemeMode.Light);
        }
    }
}