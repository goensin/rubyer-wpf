using ICSharpCode.AvalonEdit.Highlighting;
using Rubyer;
using RubyerDemo.Utils;
using System;
using System.Diagnostics;
using System.IO;
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
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                string name = $"Views/{(listBox.SelectedItem as ViewModels.MenuItem).Name.Split('-')[1]}.xaml";
                Uri uri = new Uri($"{name}", UriKind.Relative);
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

    }
}
