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
    /// NotificationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationWindow : Window
    {
        /// <summary>
        /// 单例消息窗体
        /// </summary>
        private static NotificationWindow notificationWindow = null;

        private NotificationWindow()
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
        public static NotificationWindow GetInstance()
        {
            if (notificationWindow == null)
            {
                notificationWindow = new NotificationWindow();
            }
            else if (!notificationWindow.IsLoaded)
            {
                notificationWindow = new NotificationWindow();
            }
            return notificationWindow;
        }

        /// <summary>
        /// 添加通知卡片
        /// </summary>
        /// <param name="notificationCard">通知卡片</param>
        internal void AddMessageCard(NotificationCard notificationCard)
        {
            notificationStackPanel.Children.Insert(0, notificationCard);
        }

        /// <summary>
        /// 移除通知卡片
        /// </summary>
        /// <param name="notificationCard">通知卡片</param>
        internal void RemoveMessageCard(NotificationCard notificationCard)
        {
            notificationStackPanel.Children.Remove(notificationCard);
            if (notificationStackPanel.Children.Count == 0)
            {
                Close();
            }
        }
    }
}