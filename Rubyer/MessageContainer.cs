using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 消息容器
    /// </summary>
    [TemplatePart(Name = MessageStackPanelName, Type = typeof(StackPanel))]
    public class MessageContainer : ContentControl
    {
        public const string MessageStackPanelName = "PART_MessageStackPanel";

        private StackPanel messageStackPanel;

        static MessageContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageContainer), new FrameworkPropertyMetadata(typeof(MessageContainer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            messageStackPanel = GetTemplateChild(MessageStackPanelName) as StackPanel;
        }

        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageContainer container)
            {
                string identify = e.NewValue.ToString();
                Message.UpdateContainer(container, identify);
            }
        }

        /// <summary>
        /// 添加消息卡片
        /// </summary>
        /// <param name="messageCard">消息卡片</param>
        internal void AddMessageCard(MessageCard messageCard)
        {
            _ = messageStackPanel.Children.Add(messageCard);
        }

        /// <summary>
        /// 移除消息卡片
        /// </summary>
        /// <param name="messageCard"></param>
        internal void RemoveMessageCard(MessageCard messageCard)
        {
            messageStackPanel.Children.Remove(messageCard);
        }
    }
}
