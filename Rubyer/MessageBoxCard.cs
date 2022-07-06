using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息框卡片
    /// </summary>
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = OkButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = CancelButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = YesButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = NoButtonPartName, Type = typeof(Button))]
    public class MessageBoxCard : Control
    {
        public const string CloseButtonPartName = "PART_CloseButton";
        public const string OkButtonPartName = "PART_OkButton";
        public const string CancelButtonPartName = "PART_CancelButton";
        public const string YesButtonPartName = "PART_YesButton";
        public const string NoButtonPartName = "PART_NoButton";

        public delegate void MessageBoxResultRoutedEventHandler(object sender, MessageBoxResultRoutedEventArgs e);

        static MessageBoxCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxCard), new FrameworkPropertyMetadata(typeof(MessageBoxCard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                InternalReturnResult(this, MessageBoxResult.Cancel);
            };

            if (IsShowOk)
            {
                ButtonBase okButton = GetTemplateChild(OkButtonPartName) as ButtonBase;
                okButton.Click += (sender, args) =>
                {
                    InternalReturnResult(this, MessageBoxResult.OK);
                };
            }

            if (IsShowCancel)
            {
                ButtonBase cancelButton = GetTemplateChild(CancelButtonPartName) as ButtonBase;
                cancelButton.Click += (sender, args) =>
                {
                    InternalReturnResult(this, MessageBoxResult.Cancel);
                };
            }

            if (IsShowYes)
            {
                ButtonBase yesButton = GetTemplateChild(YesButtonPartName) as ButtonBase;
                yesButton.Click += (sender, args) =>
                {
                    InternalReturnResult(this, MessageBoxResult.Yes);
                };
            }

            if (IsShowNo)
            {
                ButtonBase noButton = GetTemplateChild(NoButtonPartName) as ButtonBase;
                noButton.Click += (sender, args) =>
                {
                    InternalReturnResult(this, MessageBoxResult.No);
                };
            }
        }

        #region 事件

        public static readonly RoutedEvent ReturnResultEvent = EventManager.RegisterRoutedEvent("ReturnResult", RoutingStrategy.Bubble, typeof(MessageBoxResultRoutedEventHandler), typeof(MessageBoxCard));

        public event MessageBoxResultRoutedEventHandler ReturnResult
        {
            add { AddHandler(ReturnResultEvent, value); }
            remove { RemoveHandler(ReturnResultEvent, value); }
        }

        #endregion

        #region 依赖属性

        public static readonly DependencyProperty IsShowProperty =
           DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(MessageBoxCard), new PropertyMetadata(default(string)));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(MessageBoxCard), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(MessageBoxCard), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty ThemeColorBrushProperty = DependencyProperty.Register(
            "ThemeColorBrush", typeof(Brush), typeof(MessageBoxCard), new PropertyMetadata(default(Brush)));

        public Brush ThemeColorBrush
        {
            get { return (Brush)GetValue(ThemeColorBrushProperty); }
            set { SetValue(ThemeColorBrushProperty, value); }
        }


        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType", typeof(IconType), typeof(MessageBoxCard), new PropertyMetadata(default(IconType)));

        public IconType IconType
        {
            get { return (IconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        public static readonly DependencyProperty IsShowIconProperty = DependencyProperty.Register(
            "IsShowIcon", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShowIcon
        {
            get { return (bool)GetValue(IsShowIconProperty); }
            set { SetValue(IsShowIconProperty, value); }
        }

        public static readonly DependencyProperty IsShowOkProperty = DependencyProperty.Register(
            "IsShowOk", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShowOk
        {
            get { return (bool)GetValue(IsShowOkProperty); }
            set { SetValue(IsShowOkProperty, value); }
        }

        public static readonly DependencyProperty IsShowNoProperty = DependencyProperty.Register(
            "IsShowNo", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShowNo
        {
            get { return (bool)GetValue(IsShowNoProperty); }
            set { SetValue(IsShowNoProperty, value); }
        }

        public static readonly DependencyProperty IsShowYesProperty = DependencyProperty.Register(
            "IsShowYes", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShowYes
        {
            get { return (bool)GetValue(IsShowYesProperty); }
            set { SetValue(IsShowYesProperty, value); }
        }

        public static readonly DependencyProperty IsShowCancelProperty = DependencyProperty.Register(
            "IsShowCancel", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool IsShowCancel
        {
            get { return (bool)GetValue(IsShowCancelProperty); }
            set { SetValue(IsShowCancelProperty, value); }
        }

        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.Register(
            "ShowShadow", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool)));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxButtonProperty = DependencyProperty.Register(
            "MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxCard), new PropertyMetadata(default(MessageBoxButton), OnMessageBoxButtonChanged));

        private static void OnMessageBoxButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxCard card)
            {
                switch ((MessageBoxButton)e.NewValue)
                {
                    case MessageBoxButton.OK:
                    default:
                        card.IsShowOk = true;
                        card.IsShowCancel = false;
                        card.IsShowYes = false;
                        card.IsShowNo = false;
                        break;
                    case MessageBoxButton.OKCancel:
                        card.IsShowOk = true;
                        card.IsShowCancel = true;
                        card.IsShowYes = false;
                        card.IsShowNo = false;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        card.IsShowOk = false;
                        card.IsShowCancel = true;
                        card.IsShowYes = true;
                        card.IsShowNo = true;
                        break;
                    case MessageBoxButton.YesNo:
                        card.IsShowOk = false;
                        card.IsShowCancel = false;
                        card.IsShowYes = true;
                        card.IsShowNo = true;
                        break;
                }
            }
        }

        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        #endregion

        private void InternalReturnResult(MessageBoxCard card, MessageBoxResult result)
        {
            IsShow = false;
            MessageBoxResultRoutedEventArgs eventArgs = new MessageBoxResultRoutedEventArgs(ReturnResultEvent, result, card);
            card.RaiseEvent(eventArgs);
        }
    }

    /// <summary>
    /// 消息框框结果路由参数
    /// </summary>
    public class MessageBoxResultRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 结果
        /// </summary>
        public MessageBoxResult Result { get; set; }

        /// <summary>
        /// 消息框
        /// </summary>
        public MessageBoxCard Card { get; set; }

        public MessageBoxResultRoutedEventArgs(RoutedEvent routedEvent, MessageBoxResult result, MessageBoxCard card) : base(routedEvent)
        {
            Result = result;
            Card = card;
        }
    }
}
