using Rubyer.Commons;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 对话框
    /// </summary>
    [TemplatePart(Name = TransitionName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = BackgroundBorderPartName, Type = typeof(Border))]
    public class DialogBox : ContentControl
    {
        public const string TransitionName = "Path_Transition";
        public const string CloseButtonPartName = "PART_CloseButton";
        public const string BackgroundBorderPartName = "PART_BackgroundBorder";

        public delegate void DialogResultRoutedEventHandler(object sender, DialogResultRoutedEventArgs e);

        public Action<DialogBox> BeforeOpenHandler;
        public Action<DialogBox, object> AfterCloseHandler;

        private Border rootBorder;
        private object openParameter;
        private object closeParameter;

        static DialogBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogBox), new FrameworkPropertyMetadata(typeof(DialogBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(CloseDialogCommand, CloseDialogHandler));
            CommandBindings.Add(new CommandBinding(OpenDialogCommand, OpenDialogHandler));

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                closeParameter = null;
                IsShow = false;
            };

            rootBorder = GetTemplateChild(BackgroundBorderPartName) as Border;
            rootBorder.MouseLeftButtonDown += RootBorder_MouseLeftButtonDown;

            if (GetTemplateChild(TransitionName) is Transition transition)
            {
                transition.Showed += (sender, e) => IsClosed = false;
                transition.Closed += Closed; ;
            }
        }

        #region 命令
        public static RoutedCommand CloseDialogCommand = new RoutedCommand();
        public static RoutedCommand OpenDialogCommand = new RoutedCommand();

        // 打开前命令
        public static readonly DependencyProperty BeforeOpenCommandProperty = DependencyProperty.Register(
            "BeforeOpenCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand BeforeOpenCommand
        {
            get { return (ICommand)GetValue(BeforeOpenCommandProperty); }
            set { SetValue(BeforeOpenCommandProperty, value); }
        }

        // 关闭后命令
        public static readonly DependencyProperty AfterCloseCommandProperty = DependencyProperty.Register(
            "AfterCloseCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand AfterCloseCommand
        {
            get { return (ICommand)GetValue(AfterCloseCommandProperty); }
            set { SetValue(AfterCloseCommandProperty, value); }
        }
        #endregion

        #region 事件
        // 打开前事件
        public static readonly RoutedEvent BeforeOpenEvent = EventManager.RegisterRoutedEvent(
            "BeforeOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DialogBox));

        public event RoutedEventHandler BeforeOpen
        {
            add { AddHandler(BeforeOpenEvent, value); }
            remove { RemoveHandler(BeforeOpenEvent, value); }
        }

        // 关闭后事件
        public static readonly RoutedEvent AfterCloseEvent = EventManager.RegisterRoutedEvent(
            "AfterClose", RoutingStrategy.Bubble, typeof(DialogResultRoutedEventHandler), typeof(DialogBox));

        public event DialogResultRoutedEventHandler AfterClose
        {
            add { AddHandler(AfterCloseEvent, value); }
            remove { RemoveHandler(AfterCloseEvent, value); }
        }
        #endregion

        #region 依赖属性

        // 标识
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(
            "Identifier", typeof(string), typeof(DialogBox), new PropertyMetadata(default(string), OnIdentifierChanged));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox dialogBox = d as DialogBox;
            string identifier = e.NewValue.ToString();
            Dialog.AddDialogBox(identifier, dialogBox);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        // 对话框内容
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            "DialogContent", typeof(object), typeof(DialogBox), new PropertyMetadata(default(object)));

        /// <summary>
        /// 对话框内容
        /// </summary>
        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        // 标题
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DialogBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(DialogBox), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // 是否显示关闭按钮
        public static readonly DependencyProperty IsShowCloseButtonProperty = DependencyProperty.Register(
            "IsShowCloseButton", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton
        {
            get { return (bool)GetValue(IsShowCloseButtonProperty); }
            set { SetValue(IsShowCloseButtonProperty, value); }
        }

        // 是否点击背景关闭弹窗
        public static readonly DependencyProperty IsClickBackgroundToCloseProperty = DependencyProperty.Register(
            "IsClickBackgroundToClose", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否点击背景关闭弹窗
        /// </summary>
        public bool IsClickBackgroundToClose
        {
            get { return (bool)GetValue(IsClickBackgroundToCloseProperty); }
            set { SetValue(IsClickBackgroundToCloseProperty, value); }
        }

        // 是否显示
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(
            "IsShow", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox dialogBox = d as DialogBox;

            if ((bool)e.NewValue)
            {
                dialogBox.OpenAnimiation(dialogBox);
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
            "MaskBackground", typeof(Brush), typeof(DialogBox), new PropertyMetadata(default(Brush)));

        /// <summary>
        ///  遮罩背景色
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            set { SetValue(MaskBackgroundProperty, value); }
        }

        // 关闭完成
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(DialogBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 关闭完成
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }
        #endregion

        private void OpenDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            openParameter = e.Parameter;
            IsShow = true;
        }

        private void CloseDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            closeParameter = e.Parameter;
            IsShow = false;
        }

        private void RootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Border border)
            {
                if (IsClickBackgroundToClose && sender.Equals(border))
                {
                    closeParameter = null;
                    IsShow = false;
                }
            }
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Close(object parameter = null)
        {
            CloseDialogCommand.Execute(parameter, this);
        }

        // 打开对话框动作
        private void OpenAnimiation(DialogBox dialogBox)
        {
            RoutedEventArgs args = new RoutedEventArgs(BeforeOpenEvent);
            dialogBox.RaiseEvent(args);
            dialogBox.BeforeOpenCommand?.Execute(null);
            dialogBox.BeforeOpenHandler?.Invoke(dialogBox);

            _ = dialogBox.Focus();

            if (dialogBox.DialogContent is FrameworkElement element && element.DataContext is IDialogContext dialogContext)
            {
                dialogContext.OnDialogOpened(dialogBox.openParameter);
            }
        }

        private void Closed(object sender, RoutedEventArgs e)
        {
            IsClosed = true;

            var args = new DialogResultRoutedEventArgs(AfterCloseEvent, this.closeParameter, this);
            args.RoutedEvent = AfterCloseEvent;
            this.RaiseEvent(args);
            this.AfterCloseCommand?.Execute(this.closeParameter);
            this.AfterCloseHandler?.Invoke(this, this.closeParameter);

            this.closeParameter = null;
            this.BeforeOpenHandler = null;
            this.AfterCloseHandler = null;
        }
    }

    /// <summary>
    /// 对话框结果路由参数
    /// </summary>
    public class DialogResultRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 对话框
        /// </summary>
        public DialogBox Dialog { get; set; }

        public DialogResultRoutedEventArgs(RoutedEvent routedEvent, object result, DialogBox dialog)
            : base(routedEvent)
        {
            Result = result;
            Dialog = dialog;
        }
    }
}
