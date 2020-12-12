using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rubyer
{
    public class MessageBoxR
    {
        private static readonly Brush infoBrush = (Brush)Application.Current.Resources["InfoBrush"];
        private static readonly Brush warningBrush = (Brush)Application.Current.Resources["WarningBrush"];
        private static readonly Brush successBrush = (Brush)Application.Current.Resources["SuccessBrush"];
        private static readonly Brush errorBrush = (Brush)Application.Current.Resources["ErrorBrush"];
        private static readonly Brush questionBrush = (Brush)Application.Current.Resources["QuestionBrush"];
        #region 全局
        public static MessageBoxResult Show(string message, string title, MessageBoxButton button = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBoxCard card = new MessageBoxCard
            {
                Content = message,
                Title = title,
                MessageBoxButton = button,
            };

            switch (icon)
            {
                case MessageBoxIcon.None:
                    card.IsShowIcon = false;
                    break;
                case MessageBoxIcon.Info:
                    card.IsShowIcon = true;
                    card.IconType = IconType.InformationFill;
                    card.ThemeColorBrush = infoBrush;
                    break;
                case MessageBoxIcon.Success:
                    card.IsShowIcon = true;
                    card.IconType = IconType.CheckboxCircleFill;
                    card.ThemeColorBrush = successBrush;
                    break;
                case MessageBoxIcon.Warining:
                    card.IsShowIcon = true;
                    card.IconType = IconType.ErrorWarningFill;
                    card.ThemeColorBrush = warningBrush;
                    break;
                case MessageBoxIcon.Error:
                    card.IsShowIcon = true;
                    card.IconType = IconType.CloseCircleFill;
                    card.ThemeColorBrush = errorBrush;
                    break;
                case MessageBoxIcon.Question:
                    card.IsShowIcon = true;
                    card.IconType = IconType.QuestionFill;
                    card.ThemeColorBrush = questionBrush;
                    break;
            }

            MessageBoxWindow window = new MessageBoxWindow();
            window.AddMessageBoxCard(card);

            var windows = Application.Current.Windows;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].IsActive)
                {
                    window.Owner = windows[i];
                } 
            }

            if (window.ShowDialog() == true)
            {
                return window.MessageBoxResult;
            }

            return MessageBoxResult.Cancel;
        }
        #endregion
    }

    public enum MessageBoxIcon
    {
        None = 0,
        Info,
        Success,
        Warining,
        Error,
        Question
    }
}
