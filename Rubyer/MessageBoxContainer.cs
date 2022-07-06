using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息框容器
    /// </summary>
    [TemplatePart(Name = TransitionName, Type = typeof(Transition))]
    public class MessageBoxContainer : ContentControl
    {
        public const string TransitionName = "Path_Transition";

        static MessageBoxContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxContainer), new FrameworkPropertyMetadata(typeof(MessageBoxContainer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TransitionName) is Transition transition)
            {
                transition.Showed += (sender, e) => IsClosed = false;
                transition.Closed += (sender, e) => IsClosed = true;
            }
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

        // 是否显示
        public static readonly DependencyProperty IsShowProperty =
           DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageBoxContainer), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // 是否关闭后
        public static readonly DependencyProperty IsClosedProperty =
           DependencyProperty.Register("IsClosed", typeof(bool), typeof(MessageBoxContainer), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否关闭后
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }


    }
}
