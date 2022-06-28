using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using Rubyer;
using RubyerDemo.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            controlSlider.Value = ((CornerRadius)App.Current.Resources["AllControlCornerRadius"]).TopLeft;
            contrainerSlider.Value = ((CornerRadius)App.Current.Resources["AllContainerCornerRadius"]).TopLeft;
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
            var value = e.NewValue;
            App.Current.Resources["AllControlCornerRadius"] = new CornerRadius(value);
            App.Current.Resources["LeftControlCornerRadius"] = new CornerRadius(value, 0, 0, value);
            App.Current.Resources["RightControlCornerRadius"] = new CornerRadius(0, value, value, 0);
            App.Current.Resources["TopControlCornerRadius"] = new CornerRadius(value, value, 0, 0);
            App.Current.Resources["BottomControlCornerRadius"] = new CornerRadius(0, 0, value, value);
        }

        private void contrainerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var value = e.NewValue;
            App.Current.Resources["AllContainerCornerRadius"] = new CornerRadius(value);
            App.Current.Resources["LeftContainerCornerRadius"] = new CornerRadius(value, 0, 0, value);
            App.Current.Resources["RightContainerCornerRadius"] = new CornerRadius(0, value, value, 0);
            App.Current.Resources["TopContainerCornerRadius"] = new CornerRadius(value, value, 0, 0);
            App.Current.Resources["BottomContainerCornerRadius"] = new CornerRadius(0, 0, value, value);
        }
    }
}
