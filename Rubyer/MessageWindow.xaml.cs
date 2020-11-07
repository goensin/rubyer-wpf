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
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        private static MessageWindow messageWindow = null;

        private MessageWindow()
        {
            InitializeComponent();
        }

        public static MessageWindow GetInstance()
        {
            if (messageWindow != null)
            {
                messageWindow = new MessageWindow();
            }
            return messageWindow;
        }

        public void AddMessageCard(MessageCard messageCard, double? durationSeconds)
        {

        }
    }
}
