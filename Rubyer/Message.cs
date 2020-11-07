using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rubyer
{
    public class Message
    {
        private static ResourceDictionary dictionary = (ResourceDictionary)Application.LoadComponent(new Uri("Theme\\MessageCard.xaml", UriKind.RelativeOrAbsolute));
        private static Style warningStyle = (Style)dictionary["WarningMessage"];
        private static Style successStyle = (Style)dictionary["SuccessMessage"];
        private static Style errorStyle = (Style)dictionary["ErrorMessage"];


        public void Show(MessageType type, string message, double? durationSeconds, bool isClearable)
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

            messageWindow.AddMessageCard(messageCard, durationSeconds);
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
