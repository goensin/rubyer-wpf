﻿using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using Rubyer;
using Rubyer.Enums;
using RubyerDemo.Utils;
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

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            controlSlider.Value = ((CornerRadius)App.Current.Resources["AllControlCornerRadius"]).TopLeft;
            contrainerSlider.Value = ((CornerRadius)App.Current.Resources["AllContainerCornerRadius"]).TopLeft;
            ThemeManager.SwitchThemeMode(ThemeMode.System);
            darkToggleButton.IsChecked = ThemeManager.GetIsAppDarkMode();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                var viewNames = (listBox.SelectedItem as ViewModels.MenuItem).Content.ToString().Split('.');
                string name = $"Views/{viewNames.Last()}.xaml";
                var uri = new Uri($"{name}", UriKind.Relative);
                var resourceInfo = Application.GetResourceStream(uri);
                var bamlTranslator = new BamlTranslator(resourceInfo.Stream);
                xamlTextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".xaml");
                xamlTextEditor.Encoding = Encoding.Default;
                xamlTextEditor.Text = bamlTranslator.ToString();
                Tab.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
            ThemeManager.SwitchThemeMode(darkToggleButton.IsChecked.GetValueOrDefault() ? ThemeMode.Black : ThemeMode.Light);
        }
    }
}