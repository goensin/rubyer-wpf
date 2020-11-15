using ICSharpCode.AvalonEdit.Highlighting;
using Rubyer;
using System;
using System.IO;
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

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                int index = baseDir.IndexOf(PROJECT_NAME);

                string fileName = Path.Combine(baseDir.Substring(0, index + PROJECT_NAME.Length), name);

                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".xaml");
                textEditor.Load(fileName);

                Tab.SelectedIndex = 0;
            }
            catch (Exception) { }
        }
    }
}
