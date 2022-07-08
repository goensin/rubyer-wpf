using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息框容器
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    public class MessageBoxContainer : ContentControl
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionPartName = "Path_Transition";

        static MessageBoxContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxContainer), new FrameworkPropertyMetadata(typeof(MessageBoxContainer)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TransitionPartName) is Transition transition)
            {
                transition.Showed += (sender, e) => IsClosed = false;
                transition.Closed += (sender, e) => IsClosed = true;
            }
        }

        /// <summary>
        /// 标识
        /// </summary>
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

        /// <summary>
        /// 弹窗内容
        /// </summary>
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

        /// <summary>
        /// 遮罩背景色
        /// </summary>
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

        /// <summary>
        /// 是否显示
        /// </summary>
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

        /// <summary>
        /// 是否关闭后
        /// </summary>
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