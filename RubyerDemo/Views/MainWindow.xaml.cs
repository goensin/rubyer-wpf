using ICSharpCode.AvalonEdit.Highlighting;
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

    }
}
