using System;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 消息框容器
    /// </summary>
    [TemplateVisualState(GroupName = "ShowStates", Name = ShowStateName)]
    [TemplateVisualState(GroupName = "ShowStates", Name = HideStateName)]
    public class MessageBoxContainer : ContentControl
    {
        public const string ShowStateName = "Show";
        public const string HideStateName = "Hide";

        static MessageBoxContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxContainer), new FrameworkPropertyMetadata(typeof(MessageBoxContainer)));
        }

        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageBoxContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(MessageBoxContainer), new PropertyMetadata(default(object)));

        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxContainer container)
            {
                string identify = e.NewValue.ToString();
                MessageBoxR.UpdateContainer(container, identify);
            }
        }

        public static readonly DependencyProperty IsShowProperty =
           DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageBoxContainer), new PropertyMetadata(default(bool), OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxContainer container)
            {
                if (container.IsShow)
                {
                    _ = VisualStateManager.GoToState(container, ShowStateName, true);
                }
                else
                {
                    _ = VisualStateManager.GoToState(container, HideStateName, true);
                }
            }
        }

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }
    }
}
