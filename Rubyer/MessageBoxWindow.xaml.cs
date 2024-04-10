using System.Windows;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        /// <summary>
        /// 消息框结果
        /// </summary>
        public MessageBoxResult MessageBoxResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxWindow"/> class.
        /// </summary>
        public MessageBoxWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加消息框
        /// </summary>
        /// <param name="card">消息框卡片</param>
        internal void AddMessageBoxCard(MessageBoxCard card)
        {
            card.Closed += Card_Close;
            messageBoxPanel.Child = card;
        }

        private void Card_Close(object sender, MessageBoxResultRoutedEventArgs e)
        {
            MessageBoxResult = e.Result;
            messageBoxPanel.Child = null;
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