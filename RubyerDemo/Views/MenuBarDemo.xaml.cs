using System;
using System.Collections.Generic;
using System.Text;
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
    /// MenuBar.xaml 的交互逻辑
    /// </summary>
    public partial class MenuBarDemo : UserControl
    {
        public MenuBarDemo()
        {
            InitializeComponent();

            DataContext = this;
        }


        public static readonly DependencyProperty IsItalicProperty =
            DependencyProperty.Register("IsItalic", typeof(bool), typeof(MenuBarDemo), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsItalic
        {
            get { return (bool)GetValue(IsItalicProperty); }
            set { SetValue(IsItalicProperty, value); }
        }


        public static readonly DependencyProperty IsUnderlineProperty =
            DependencyProperty.Register("IsUnderline", typeof(bool), typeof(MenuBarDemo), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsUnderline
        {
            get { return (bool)GetValue(IsUnderlineProperty); }
            set { SetValue(IsUnderlineProperty, value); }
        }


        public static readonly DependencyProperty IsBoldProperty =
            DependencyProperty.Register("IsBold", typeof(bool), typeof(MenuBarDemo), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsBold
        {
            get { return (bool)GetValue(IsBoldProperty); }
            set { SetValue(IsBoldProperty, value); }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            switch (radio.Tag.ToString())
            {
                case "left":
                    TestText.HorizontalContentAlignment = HorizontalAlignment.Left;
                    break;
                case "right":
                    TestText.HorizontalContentAlignment = HorizontalAlignment.Right;
                    break;
                case "center":
                    TestText.HorizontalContentAlignment = HorizontalAlignment.Center;
                    break;
            }
        }

        private void ItalicToggle_Checked(object sender, RoutedEventArgs e)
        {
            TestText.FontStyle = FontStyles.Italic;
        }

        private void ItalicToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            TestText.FontStyle = FontStyles.Normal;
        }

        private void UnderlineToggle_Checked(object sender, RoutedEventArgs e)
        {
            TestText.TextDecorations = TextDecorations.Underline;
        }

        private void UnderlineToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            TestText.TextDecorations = null;
        }

        private void BoldToggle_Checked(object sender, RoutedEventArgs e)
        {
            TestText.FontWeight = FontWeights.Bold;
        }

        private void BoldToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            TestText.FontWeight = FontWeights.Normal;
        }
    }
}
