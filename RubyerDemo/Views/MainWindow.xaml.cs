using ICSharpCode.AvalonEdit.Highlighting;
using Rubyer;
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

        const string PROJECT_NAME = "RubyerDemo";

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                string name = $"Views/{(listBox.SelectedItem as ViewModels.MenuItem).Name.Split('-')[1]}.xaml";
                Uri uri = new Uri($"/{PROJECT_NAME};component/{name}", UriKind.Relative);
                var resourceInfo = Application.GetResourceStream(uri);
                xamlTextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".xaml");
                xamlTextEditor.Encoding = Encoding.Default;
                xamlTextEditor.Load(resourceInfo.Stream);
                Tab.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
