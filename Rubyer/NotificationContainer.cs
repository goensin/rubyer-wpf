using System.Windows.Controls;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// 消通知容器
    /// </summary>
    [TemplatePart(Name = StackPanelName, Type = typeof(StackPanel))]
    public class NotificationContainer : ContentControl
    {
        /// <summary>
        /// 消息堆叠面板名称
        /// </summary>
        public const string StackPanelName = "PART_StackPanel";

        private StackPanel stackPanel;

        static NotificationContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationContainer), new FrameworkPropertyMetadata(typeof(NotificationContainer)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            stackPanel = GetTemplateChild(StackPanelName) as StackPanel;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(NotificationContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

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
            if (d is NotificationContainer container)
            {
                string identify = e.NewValue.ToString();
                Notification.UpdateContainer(container, identify);
            }
        }

        /// <summary>
        /// 添加卡片
        /// </summary>
        /// <param name="card">消息卡片</param>
        internal void AddCard(NotificationCard card)
        {
            stackPanel.Children.Insert(0, card);
        }

        /// <summary>
        /// 移除卡片
        /// </summary>
        /// <param name="card"></param>
        internal void RemoveCard(NotificationCard card)
        {
            stackPanel.Children.Remove(card);
        }

        /// <summary>
        /// 移除所有卡片
        /// </summary>
        internal void ClearCards()
        {
            foreach (NotificationCard card in stackPanel.Children)
            {
                card.IsShow = false;
            }
        }
    }
}