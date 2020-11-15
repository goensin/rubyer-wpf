using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Rubyer
{
    public class Message
    {
        private static readonly Style warningStyle = (Style)Application.Current.Resources["WarningMessage"]; 
        private static readonly Style successStyle = (Style)Application.Current.Resources["SuccessMessage"];
        private static readonly Style errorStyle = (Style)Application.Current.Resources["ErrorMessage"]; 

        public static void Show(MessageType type, string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            MessageWindow messageWindow = MessageWindow.GetInstance();
            MessageCard messageCard;
            switch (type)
            {
                default:
                case MessageType.Info:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable
                    };
                    break;
                case MessageType.Warning:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = warningStyle
                    };
                    break;
                case MessageType.Success:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = successStyle
                    };
                    break;
                case MessageType.Error:
                    messageCard = new MessageCard
                    {
                        Content = message,
                        IsClearable = isClearable,
                        Style = errorStyle
                    };
                    break;
            }

            messageWindow.AddMessageCard(messageCard, millisecondTimeOut);
            messageWindow.Show();
        }

        public static void Show(string message, int millisecondTimeOut = 3000, bool isClearable = false)
        {
            Show(MessageType.Info, message, millisecondTimeOut, isClearable);
        }
    }

    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
