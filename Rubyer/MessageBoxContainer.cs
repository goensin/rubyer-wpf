using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        // 标识
        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageBoxContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        // 弹窗内容
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(MessageBoxContainer), new PropertyMetadata(default(object)));

        /// <summary>
        /// 弹窗内容
        /// </summary>
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

        // 是否显示
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

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // 遮罩背景色
        public static readonly DependencyProperty MaskBackgroundProperty = DependencyProperty.Register(
            "MaskBackground", typeof(Brush), typeof(MessageBoxContainer), new PropertyMetadata(default(Brush)));

        /// <summary>
        ///  遮罩背景色
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            set { SetValue(MaskBackgroundProperty, value); }
        }
    }
}
