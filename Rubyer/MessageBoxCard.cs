using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer
{
    public delegate void MessageBoxResultRoutedEventHandler(object sender, MessageBoxResultRoutedEventArge e);

    /// <summary>
    /// 消息框卡片
    /// </summary>
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = OkButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = CancelButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = YesButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = NoButtonPartName, Type = typeof(Button))]
    [TemplateVisualState(GroupName = "ShowStates", Name = OpenStateName)]
    [TemplateVisualState(GroupName = "ShowStates", Name = CloseStateName)]
    public class MessageBoxCard : Control
    {
        public const string CloseButtonPartName = "PART_CloseButton";
        public const string OkButtonPartName = "PART_OkButton";
        public const string CancelButtonPartName = "PART_CancelButton";
        public const string YesButtonPartName = "PART_YesButton";
        public const string NoButtonPartName = "PART_NoButton";
        public const string OpenStateName = "Open";
        public const string CloseStateName = "Close";

        public delegate void MessageBoxResultRoutedEventHandler(object sender, MessageBoxResultRoutedEventArge e);
        public static readonly RoutedEvent ReturnResultEvent;
        private MessageBoxResult messageBoxResult;

        static MessageBoxCard()
        {
            ReturnResultEvent = EventManager.RegisterRoutedEvent("ReturnResult", RoutingStrategy.Bubble, typeof(MessageBoxResultRoutedEventHandler), typeof(MessageBoxCard));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxCard), new FrameworkPropertyMetadata(typeof(MessageBoxCard)));
        }

        public override void OnApplyTemplate()
        {
            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                messageBoxResult = MessageBoxResult.Cancel;
                GoToCloseState(this);
            };

            if (IsShowOk)
            {
                ButtonBase okButton = GetTemplateChild(OkButtonPartName) as ButtonBase;
                okButton.Click += (sender, args) =>
                {
                    messageBoxResult = MessageBoxResult.OK;
                    GoToCloseState(this);
                };
            }

            if (IsShowCancel)
            {
                ButtonBase cancelButton = GetTemplateChild(CancelButtonPartName) as ButtonBase;
                cancelButton.Click += (sender, args) =>
                {
                    messageBoxResult = MessageBoxResult.Cancel;
                    GoToCloseState(this);
                };
            }

            if (IsShowYes)
            {
                ButtonBase yesButton = GetTemplateChild(YesButtonPartName) as ButtonBase;
                yesButton.Click += (sender, args) =>
                {
                    messageBoxResult = MessageBoxResult.Yes;
                    GoToCloseState(this);
                };
            }

            if (IsShowNo)
            {
                ButtonBase noButton = GetTemplateChild(NoButtonPartName) as ButtonBase;
                noButton.Click += (sender, args) =>
                {
                    messageBoxResult = MessageBoxResult.No;
                    GoToCloseState(this);
                };
            }

            GoToOpenState(this);
            base.OnApplyTemplate();
        }

        #region 事件
        public event MessageBoxResultRoutedEventHandler ReturnResult
        {
            add { AddHandler(ReturnResultEvent, value); }
            remove { RemoveHandler(ReturnResultEvent, value); }
        }
        #endregion

        #region 依赖属性
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

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(MessageBoxCard), new PropertyMetadata(default(bool), OnIsClosedChanged));

        private static void OnIsClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxCard messageBoxCard)
            {
                if (messageBoxCard.IsClosed)
                {
                    MessageBoxResultRoutedEventArge eventArgs = new MessageBoxResultRoutedEventArge(ReturnResultEvent, messageBoxCard.messageBoxResult, messageBoxCard);
                    messageBoxCard.RaiseEvent(eventArgs);
                }
            }
        }

        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }
        #endregion

        private static void GoToOpenState(MessageBoxCard messageBoxCard)
        {
            _ = VisualStateManager.GoToState(messageBoxCard, OpenStateName, true);
            _ = messageBoxCard.Focus();
        }

        private static void GoToCloseState(MessageBoxCard messageBoxCard)
        {
            _ = VisualStateManager.GoToState(messageBoxCard, CloseStateName, true);
        }
    }

    public class MessageBoxResultRoutedEventArge : RoutedEventArgs
    {
        public MessageBoxResult Result { get; set; }
        public MessageBoxCard Card { get; set; }

        public MessageBoxResultRoutedEventArge(RoutedEvent routedEvent, MessageBoxResult result, MessageBoxCard card) : base(routedEvent)
        {
            Result = result;
            Card = card;
        }
    }
}
