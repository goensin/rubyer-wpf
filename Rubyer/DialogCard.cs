using Rubyer.Commons.KnownBoxes;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Rubyer
{
    /// <summary>
    /// 对话框卡片
    /// </summary>
    [TemplatePart(Name = BackgroundBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = DragThumbPartName, Type = typeof(Thumb))]
    public class DialogCard : ContentControl
    {
        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string BackgroundBorderPartName = "PART_BackgroundBorder";

        /// <summary>
        /// 转换动画名称
        /// </summary>
        public const string TransitionPartName = "Path_Transition";

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public const string CloseButtonPartName = "PART_CloseButton";

        /// <summary>
        /// 拖多块
        /// </summary>
        public const string DragThumbPartName = "PART_DragThumb";

        /// <summary>
        /// 卡片背景
        /// </summary>
        public const string CardBorderPartName = "PART_CardBorder";

        /// <summary>
        /// 关闭参数
        /// </summary>
        internal object CloseParameter { get; private set; }

        Transition transition;
        Thumb dragThumb;
        Border cardBorder;

        /// <summary>
        /// 消息框结果事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DialogResultRoutedEventHandler(object sender, DialogResultRoutedEventArgs e);

        static DialogCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogCard), new FrameworkPropertyMetadata(typeof(DialogCard)));
        }

        #region 事件

        /// <summary>
        /// 打开对话框前事件处理
        /// </summary>
        public Action<DialogCard> BeforeOpenHandler;

        /// <summary>
        /// 打开对话框后事件处理
        /// </summary>
        public Action<DialogCard, object> AfterCloseHandler;

        /// <summary>
        /// 关闭中消息事件
        /// </summary>
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DialogCard));

        /// <summary>
        /// 关闭中消息事件处理
        /// </summary>
        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        /// <summary>
        /// 关闭后消息事件
        /// </summary>
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Direct, typeof(DialogResultRoutedEventHandler), typeof(DialogCard));

        /// <summary>
        /// 关闭后消息事件处理
        /// </summary>
        public event DialogResultRoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        #endregion 事件

        #region 依赖属性

        /// <summary>
        /// 遮罩背景色
        /// </summary>
        public static readonly DependencyProperty MaskBackgroundProperty = DependencyProperty.Register("MaskBackground", typeof(Brush), typeof(DialogCard), new PropertyMetadata(default(Brush)));

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
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register("IsShow", typeof(bool), typeof(DialogCard), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DialogCard), new PropertyMetadata(default(string)));

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
        public static readonly DependencyProperty CornerRadiusProperty =
            Border.CornerRadiusProperty.AddOwner(typeof(DialogCard), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty =
            DependencyProperty.Register("TransitionType", typeof(TransitionType), typeof(DialogCard), new PropertyMetadata(default(TransitionType)));

        /// <summary>
        /// 转换类型
        /// </summary>
        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public static readonly DependencyProperty TransitionInitialScaleProperty =
            DependencyProperty.Register("TransitionInitialScale", typeof(double), typeof(DialogCard), new PropertyMetadata(default(double)));

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public double TransitionInitialScale
        {
            get { return (double)GetValue(TransitionInitialScaleProperty); }
            set { SetValue(TransitionInitialScaleProperty, value); }
        }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public static readonly DependencyProperty IsShowCloseButtonProperty = DependencyProperty.Register(
            "IsShowCloseButton", typeof(bool), typeof(DialogCard), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
            "IsEscKeyToClose", typeof(bool), typeof(DialogCard), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否 esc 键关闭弹窗 (点击空白关闭)
        /// </summary>
        public bool IsEscKeyToClose
        {
            get { return (bool)GetValue(IsEscKeyToCloseProperty); }
            set { SetValue(IsEscKeyToCloseProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否可拖动
        /// </summary>
        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register(
            "IsDraggable", typeof(bool), typeof(DialogCard), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDraggableChanged));

        /// <summary>
        /// 是否可拖动
        /// </summary>
        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 关闭完成 Task 源
        /// </summary>
        public TaskCompletionSource<object> CloseTaskCompletionSource { get; private set; }

        #endregion

        public DialogCard()
        {
            CloseTaskCompletionSource = new TaskCompletionSource<object>();
            Loaded += DialogCard_Loaded;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            WeakEventManager<ButtonBase, RoutedEventArgs>.AddHandler(closeButton, "Click", (sender, e) => Close());
            if (GetTemplateChild(TransitionPartName) is Transition transition)
            {
                transition.Closed += Transition_Closed;
                this.transition = transition;
            }

            if (GetTemplateChild(BackgroundBorderPartName) is Border background)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(background, "PreviewMouseLeftButtonDown", Background_PreviewMouseDown);
            }

            dragThumb = (Thumb)GetTemplateChild(DragThumbPartName);
            if (IsDraggable)
            {
                dragThumb.DragDelta += DragThumb_DragDelta;
            }

            cardBorder = (Border)GetTemplateChild(CardBorderPartName);
        }

        private void Background_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEscKeyToClose && e.OriginalSource is Border border && sender.Equals(border))
            {
                Close();
            }
        }

        private void Transition_Closed(object sender, RoutedEventArgs e)
        {
            transition.Closed -= Transition_Closed;
            var eventArgs = new DialogResultRoutedEventArgs(ClosedEvent, CloseParameter, this);
            RaiseEvent(eventArgs);
            CloseTaskCompletionSource.TrySetResult(CloseParameter);
            AfterCloseHandler?.Invoke(this, CloseParameter);
            BeforeOpenHandler = null;
            AfterCloseHandler = null;
        }

        private void DialogCard_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DialogCard_Loaded;
            BeforeOpenHandler?.Invoke(this);
            Focus();
        }

        /// <summary>
        /// 关闭对话框卡片
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Close(object parameter = null)
        {
            var args = new RoutedEventArgs(ClosingEvent, this);
            RaiseEvent(args);

            CloseParameter = parameter;
            IsShow = false;
        }

        internal void PressedEscToClose()
        {
            if (IsEscKeyToClose)
            {
                Close();
            }
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (cardBorder.RenderTransform is not TranslateTransform)
            {
                cardBorder.RenderTransform = new TranslateTransform();
            }

            if (cardBorder.RenderTransform is TranslateTransform translateTransform)
            {
                var point = cardBorder.TranslatePoint(new Point(), this);
                double x = e.HorizontalChange;
                double y = e.VerticalChange;

                if (x < 0 && x < -point.X)
                {
                    x = -point.X;
                }
                else if (x > 0 && x + point.X + cardBorder.ActualWidth > ActualWidth)
                {
                    x = ActualWidth - cardBorder.ActualWidth - point.X;
                }

                if (y < 0 && y < -point.Y)
                {
                    y = -point.Y;
                }
                else if (y > 0 && y + point.Y + cardBorder.ActualHeight > ActualHeight)
                {
                    y = ActualHeight - cardBorder.ActualHeight - point.Y;
                }

                translateTransform.X += x;
                translateTransform.Y += y;
            }
        }

        private static void OnIsDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialogCard = (DialogCard)d;
            if (dialogCard.dragThumb is { })
            {
                if (dialogCard.IsDraggable)
                {
                    dialogCard.dragThumb.DragDelta += dialogCard.DragThumb_DragDelta;
                }
                else
                {
                    dialogCard.dragThumb.DragDelta -= dialogCard.DragThumb_DragDelta;
                }
            }
        }
    }

    /// <summary>
    /// 对话框结果路由参数
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DialogResultRoutedEventArgs"/> class.
    /// </remarks>
    /// <param name="routedEvent">The routed event.</param>
    /// <param name="result">The result.</param>
    /// <param name="dialog">The dialog.</param>
    public class DialogResultRoutedEventArgs(RoutedEvent routedEvent, object result, DialogCard dialog) : RoutedEventArgs(routedEvent)
    {
        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; } = result;

        /// <summary>
        /// 对话框
        /// </summary>
        public DialogCard Dialog { get; set; } = dialog;
    }
}
