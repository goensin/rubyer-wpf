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
        /// <summary>
        /// 消息堆叠面板名称
        /// </summary>
        public const string MessageStackPanelName = "PART_MessageStackPanel";

        private StackPanel messageStackPanel;

        static MessageContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageContainer), new FrameworkPropertyMetadata(typeof(MessageContainer)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            messageStackPanel = GetTemplateChild(MessageStackPanelName) as StackPanel;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        /// <summary>
        /// 标识
        /// </summary>
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
        internal void AddCard(MessageCard messageCard)
        {
            _ = messageStackPanel.Children.Add(messageCard);
        }

        /// <summary>
        /// 移除消息卡片
        /// </summary>
        /// <param name="messageCard"></param>
        internal void RemoveCard(MessageCard messageCard)
        {
            messageStackPanel.Children.Remove(messageCard);
        }

        /// <summary>
        /// 移除所有卡片
        /// </summary>
        internal void ClearCards()
        {
            foreach (MessageCard card in messageStackPanel.Children)
            {
                card.IsShow = false;
            }
        }
    }
}