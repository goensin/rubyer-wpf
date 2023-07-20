using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.Generic;
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
    [TemplatePart(Name = ContentPresenterPartName, Type = typeof(ContentPresenter))]
    public class DialogContainer : ContentControl
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionName = "Path_Transition";

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public const string CloseButtonPartName = "PART_CloseButton";

        /// <summary>
        /// 背景 Border 名称
        /// </summary>
        public const string BackgroundBorderPartName = "PART_BackgroundBorder";

        /// <summary>
        /// Content 内容名称
        /// </summary>
        public const string ContentPresenterPartName = "PART_ContentPresenter";

        /// <summary>
        /// 对话框结果路由事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DialogResultRoutedEventHandler(object sender, DialogResultRoutedEventArgs e);

        /// <summary>
        /// 打开对话框前事件处理
        /// </summary>
        public Action<DialogContainer> BeforeOpenHandler;

        /// <summary>
        /// 打开对话框后事件处理
        /// </summary>
        public Action<DialogContainer, object> AfterCloseHandler;

        private Border rootBorder;
        private object closeParameter;
        private List<FrameworkElement> focusableElements; // Content 内 focusable 元素，用于打开弹窗使其失效

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogContainer"/> class.
        /// </summary>
        static DialogContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContainer), new FrameworkPropertyMetadata(typeof(DialogContainer)));
        }

        /// <inheritdoc/>
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
                transition.Closed += Closed;
            }

            if (GetTemplateChild(ContentPresenterPartName) is ContentPresenter contentPresenter)
            {
                contentPresenter.PreviewKeyDown += ContentPresenter_PreviewKeyDown;
            }

            focusableElements = new List<FrameworkElement>();
        }

        #region 命令

        /// <summary>
        /// 关闭对话框命令
        /// </summary>
        public static RoutedCommand CloseDialogCommand = new RoutedCommand();

        /// <summary>
        /// 打开对话框命令
        /// </summary>
        public static RoutedCommand OpenDialogCommand = new RoutedCommand();

        /// <summary>
        /// 打开前命令
        /// </summary>
        public static readonly DependencyProperty BeforeOpenCommandProperty = DependencyProperty.Register(
            "BeforeOpenCommand", typeof(ICommand), typeof(DialogContainer));

        /// <summary>
        /// 打开前命令
        /// </summary>
        public ICommand BeforeOpenCommand
        {
            get { return (ICommand)GetValue(BeforeOpenCommandProperty); }
            set { SetValue(BeforeOpenCommandProperty, value); }
        }

        /// <summary>
        /// 关闭后命令
        /// </summary>
        public static readonly DependencyProperty AfterCloseCommandProperty = DependencyProperty.Register(
            "AfterCloseCommand", typeof(ICommand), typeof(DialogContainer));

        /// <summary>
        /// 关闭后命令
        /// </summary>
        public ICommand AfterCloseCommand
        {
            get { return (ICommand)GetValue(AfterCloseCommandProperty); }
            set { SetValue(AfterCloseCommandProperty, value); }
        }

        #endregion 命令

        #region 事件

        /// <summary>
        /// 打开前事件
        /// </summary>
        public static readonly RoutedEvent BeforeOpenEvent = EventManager.RegisterRoutedEvent(
            "BeforeOpen", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DialogContainer));

        /// <summary>
        /// 打开前事件
        /// </summary>
        public event RoutedEventHandler BeforeOpen
        {
            add { AddHandler(BeforeOpenEvent, value); }
            remove { RemoveHandler(BeforeOpenEvent, value); }
        }

        /// <summary>
        /// 关闭后事件
        /// </summary>
        public static readonly RoutedEvent AfterCloseEvent = EventManager.RegisterRoutedEvent(
            "AfterClose", RoutingStrategy.Direct, typeof(DialogResultRoutedEventHandler), typeof(DialogContainer));

        /// <summary>
        /// 关闭后事件
        /// </summary>
        public event DialogResultRoutedEventHandler AfterClose
        {
            add { AddHandler(AfterCloseEvent, value); }
            remove { RemoveHandler(AfterCloseEvent, value); }
        }

        #endregion 事件

        #region 依赖属性

        /// <summary>
        /// 标识
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(
            "Identifier", typeof(string), typeof(DialogContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogContainer dialog = d as DialogContainer;
            string identifier = e.NewValue.ToString();
            Dialog.AddDialogContainer(identifier, dialog);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        /// <summary>
        /// 对话框内容
        /// </summary>
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            "DialogContent", typeof(object), typeof(DialogContainer), new PropertyMetadata(default(object)));

        /// <summary>
        /// 对话框内容
        /// </summary>
        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DialogContainer), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(DialogContainer), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public static readonly DependencyProperty IsShowCloseButtonProperty = DependencyProperty.Register(
            "IsShowCloseButton", typeof(bool), typeof(DialogContainer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton
        {
            get { return (bool)GetValue(IsShowCloseButtonProperty); }
            set { SetValue(IsShowCloseButtonProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否 esc 键关闭弹窗 (点击空白关闭)
        /// </summary>
        public static readonly DependencyProperty IsEscKeyToCloseProperty = DependencyProperty.Register(
            "IsEscKeyToClose", typeof(bool), typeof(DialogContainer), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsEscKeyToCloseChanged));

        private static void OnIsEscKeyToCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DialogContainer dialog)
            {
                if (dialog.IsEscKeyToClose)
                {
                    var escKeyBinding = new KeyBinding(DialogContainer.CloseDialogCommand, Key.Escape, ModifierKeys.None);
                    dialog.InputBindings.Add(escKeyBinding);
                }
                else
                {
                    foreach (var inputBinding in dialog.InputBindings)
                    {
                        if (inputBinding is KeyBinding keyBinding && keyBinding.Key == Key.Escape)
                        {
                            dialog.InputBindings.Remove(keyBinding);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 是否 esc 键关闭弹窗 (点击空白关闭)
        /// </summary>
        public bool IsEscKeyToClose
        {
            get { return (bool)GetValue(IsEscKeyToCloseProperty); }
            set { SetValue(IsEscKeyToCloseProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(
            "IsShow", typeof(bool), typeof(DialogContainer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogContainer dialog = d as DialogContainer;

            if (dialog.IsShow)
            {
                if (dialog.Content is FrameworkElement dialogContent)
                {
                    dialogContent.ForEachVisualChild(x =>
                    {
                        if (x is FrameworkElement element && element.Focusable)
                        {
                            element.Focusable = false;
                            dialog.focusableElements.Add(element);
                        }
                    });

                    dialog.OpenAnimiation(dialog);
                }
            }
            else
            {
                dialog.focusableElements.ForEach(x => x.Focusable = true);
                dialog.focusableElements.Clear();
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 遮罩背景色
        /// </summary>
        public static readonly DependencyProperty MaskBackgroundProperty = DependencyProperty.Register(
            "MaskBackground", typeof(Brush), typeof(DialogContainer), new PropertyMetadata(default(Brush)));

        /// <summary>
        ///  遮罩背景色
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            set { SetValue(MaskBackgroundProperty, value); }
        }

        /// <summary>
        /// 关闭完成
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(DialogContainer), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 关闭完成
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, BooleanBoxes.Box(value)); }
        }

        #endregion 依赖属性

        private void OpenDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
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
                if (IsEscKeyToClose && sender.Equals(border))
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
        private void OpenAnimiation(DialogContainer dialog)
        {
            RoutedEventArgs args = new RoutedEventArgs(BeforeOpenEvent);
            dialog.RaiseEvent(args);
            dialog.BeforeOpenCommand?.Execute(null);
            dialog.BeforeOpenHandler?.Invoke(dialog);

            _ = dialog.Focus();
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

        private void ContentPresenter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            if (key == Key.Tab && IsShow)
            {
                e.Handled = true;
            }
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
        public DialogContainer Dialog { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogResultRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="result">The result.</param>
        /// <param name="dialog">The dialog.</param>
        public DialogResultRoutedEventArgs(RoutedEvent routedEvent, object result, DialogContainer dialog)
            : base(routedEvent)
        {
            Result = result;
            Dialog = dialog;
        }
    }
}