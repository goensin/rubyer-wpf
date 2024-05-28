using Rubyer.Commons.KnownBoxes;
using Rubyer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// 弹出框
    /// </summary>
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    public class Popover : ContentControl
    {
        /// <summary>
        /// 弹出层名称
        /// </summary>
        public const string PopupPartName = "PART_Popup";

        private Popup popup;

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public static readonly RoutedEvent OpenedEvent =
            EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Popover));

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public event RoutedEventHandler Opened
        {
            add { AddHandler(OpenedEvent, value); }
            remove { RemoveHandler(OpenedEvent, value); }
        }

        public static readonly DependencyProperty TriggerModeProperty =
            DependencyProperty.Register("TriggerMode", typeof(PopoverTriggerMode), typeof(Popover), new PropertyMetadata(PopoverTriggerMode.Click, OnTriggerModeChanged));

        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(Popover), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(Popover), new PropertyMetadata(null));

        public static readonly DependencyProperty PopoverContentProperty =
           DependencyProperty.Register("PopoverContent", typeof(object), typeof(Popover), new PropertyMetadata(null));

        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(PopoverPlacementMode), typeof(Popover), new PropertyMetadata(PopoverPlacementMode.Top));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Popover), new PropertyMetadata(default(CornerRadius)));

        static Popover()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Popover), new FrameworkPropertyMetadata(typeof(Popover)));
        }

        /// <summary>
        /// 弹窗关闭
        /// </summary>
        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Popover));

        /// <summary>
        /// 弹窗关闭
        /// </summary>
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        /// <summary>
        /// 是否显示弹窗
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        /// <summary>
        /// 弹窗触发模式
        /// </summary>
        public PopoverTriggerMode TriggerMode
        {
            get { return (PopoverTriggerMode)GetValue(TriggerModeProperty); }
            set { SetValue(TriggerModeProperty, value); }
        }

        /// <summary>
        /// 弹窗标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 弹窗内容
        /// </summary>
        public object PopoverContent
        {
            get { return (object)GetValue(PopoverContentProperty); }
            set { SetValue(PopoverContentProperty, value); }
        }

        /// <summary>
        /// 弹窗位置
        /// </summary>
        public PopoverPlacementMode Placement
        {
            get { return (PopoverPlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            popup = GetTemplateChild(PopupPartName) as Popup;
            WeakEventManager<Popup, EventArgs>.AddHandler(popup, "Opened", Popup_Opened);
            WeakEventManager<Popup, EventArgs>.AddHandler(popup, "Closed", Popup_Closed);

            AddTriggerAction(TriggerMode);
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            popup.Placement = Placement switch
            {
                PopoverPlacementMode.Bottom => PlacementMode.Bottom,
                PopoverPlacementMode.Left => PlacementMode.Left,
                PopoverPlacementMode.Right => PlacementMode.Right,
                _ => PlacementMode.Top,
            };

            if (Content is FrameworkElement element && popup.Child is FrameworkElement popupElement)
            {
                element.Focus();

                switch (Placement)
                {
                    case PopoverPlacementMode.Top:
                    case PopoverPlacementMode.Bottom:
                    default:
                        var offset1 = popupElement.ActualWidth - element.ActualWidth;
                        popup.HorizontalOffset = Math.Abs(offset1 / 2) * (offset1 > 0 ? -1 : 1);
                        break;
                    case PopoverPlacementMode.Left:
                    case PopoverPlacementMode.Right:
                        var offset2 = popupElement.ActualHeight - element.ActualHeight;
                        popup.VerticalOffset = Math.Abs(offset2 / 2) * (offset2 > 0 ? -1 : 1);
                        break;
                }
            }

            var args = new RoutedEventArgs();
            args.RoutedEvent = OpenedEvent;
            RaiseEvent(args);
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            IsShow = false;

            var args = new RoutedEventArgs();
            args.RoutedEvent = ClosedEvent;
            RaiseEvent(args);
        }

        private async Task ShowPopup()
        {
            await Task.Delay(5);
            IsShow = true;
        }

        private async void Popover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => await ShowPopup();

        private async void Popover_MouseEnter(object sender, MouseEventArgs e) => await ShowPopup();

        private void Popover_MouseLeave(object sender, MouseEventArgs e) => IsShow = false;

        private async void Popover_GotFocus(object sender, RoutedEventArgs e) => await ShowPopup();

        private void Popover_LostFocus(object sender, RoutedEventArgs e) => IsShow = false;

        private async void Popover_MouseRightButtonDown(object sender, MouseButtonEventArgs e) => await ShowPopup();

        private void AddTriggerAction(PopoverTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case PopoverTriggerMode.None:
                    popup.StaysOpen = true;
                    break;
                case PopoverTriggerMode.Click:
                    popup.StaysOpen = false;
                    PreviewMouseLeftButtonUp += Popover_MouseLeftButtonDown;
                    break;
                case PopoverTriggerMode.Hover:
                    popup.StaysOpen = true;
                    MouseEnter += Popover_MouseEnter;
                    MouseLeave += Popover_MouseLeave;
                    break;
                case PopoverTriggerMode.Focus:
                    popup.StaysOpen = false;
                    GotFocus += Popover_GotFocus;
                    LostFocus += Popover_LostFocus;
                    break;
                case PopoverTriggerMode.ContextMenu:
                    popup.StaysOpen = false;
                    PreviewMouseRightButtonUp += Popover_MouseRightButtonDown;
                    break;
            }
        }

        private void RemoveTriggerAction(PopoverTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case PopoverTriggerMode.None:
                    break;
                case PopoverTriggerMode.Click:
                    PreviewMouseLeftButtonUp -= Popover_MouseLeftButtonDown;
                    break;
                case PopoverTriggerMode.Hover:
                    MouseEnter -= Popover_MouseEnter;
                    MouseLeave -= Popover_MouseLeave;
                    break;
                case PopoverTriggerMode.Focus:
                    GotFocus -= Popover_GotFocus;
                    LostFocus -= Popover_LostFocus;
                    break;
                case PopoverTriggerMode.ContextMenu:
                    PreviewMouseRightButtonUp -= Popover_MouseRightButtonDown;
                    break;
            }
        }

        private static void OnTriggerModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popover = (Popover)d;
            if (popover.IsLoaded)
            {
                popover.RemoveTriggerAction((PopoverTriggerMode)e.OldValue);
                popover.AddTriggerAction((PopoverTriggerMode)e.NewValue);
            }
        }
    }
}
