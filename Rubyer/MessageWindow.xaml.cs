using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// 单例消息窗体
        /// </summary>
        private static MessageWindow messageWindow = null;

        private MessageWindow()
        {
            InitializeComponent();

            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                Close();
            };
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>MessageWindow 实例</returns>
        public static MessageWindow GetInstance()
        {
            if (messageWindow is null || !messageWindow.IsLoaded)
            {
                messageWindow = new MessageWindow();
            }

            return messageWindow;
        }

        /// <summary>
        /// 添加消息卡片
        /// </summary>
        /// <param name="messageCard">消息卡片</param>
        internal void Add(MessageCard messageCard)
        {
            messageStackPanel.Children.Add(messageCard);
        }

        /// <summary>
        /// 移除消息卡片
        /// </summary>
        /// <param name="messageCard">消息卡片</param>
        internal void Remove(MessageCard messageCard)
        {
            messageStackPanel.Children.Remove(messageCard);
            if (messageStackPanel.Children.Count == 0)
            {
                Close();
            }
        }

        /// <summary>
        /// 移除全部
        /// </summary>
        internal void ClearAll()
        {
            foreach (MessageCard card in messageStackPanel.Children)
            {
                card.IsShow = false;
            }
        }
    }
}