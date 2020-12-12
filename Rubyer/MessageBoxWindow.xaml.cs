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
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxResult MessageBoxResult { get; set; }

        public MessageBoxWindow()
        {
            InitializeComponent();
        }

        public void AddMessageBoxCard(MessageBoxCard card)
        {
            card.ReturnResult += Card_ReturnResult;

            messageBoxPanel.Children.Add(card);
        }

        private void Card_ReturnResult(object sender, MessageBoxResultRoutedEventArge e)
        {
            messageBoxPanel.Children.Remove(e.Card);
            DialogResult = true;
            MessageBoxResult = e.Result;
            this.Close();
        }
    }
}
