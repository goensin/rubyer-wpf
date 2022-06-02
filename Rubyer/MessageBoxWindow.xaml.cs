using System.Windows;
using System.Windows.Input;

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
            _ = messageBoxPanel.Child = card;
        }

        private void Card_ReturnResult(object sender, MessageBoxResultRoutedEventArgs e)
        {
            messageBoxPanel.Child = null;
            MessageBoxResult = e.Result;
            DialogResult = true;
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _ = Focus();
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus != null)
            {
                e.Handled = true;
            }
        }
    }
}
