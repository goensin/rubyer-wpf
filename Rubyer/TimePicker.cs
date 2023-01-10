using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace Rubyer
{
    /// <summary>
    /// 时间选择器
    /// </summary>
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    [TemplatePart(Name = ButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ClockPartName, Type = typeof(Clock))]
    public class TimePicker : Control
    {
        /// <summary>
        /// 文本框名称
        /// </summary>
        public const string TextBoxPartName = "PART_TextBox";

        /// <summary>
        /// 弹窗名称
        /// </summary>
        public const string PopupPartName = "PART_Popup";

        /// <summary>
        /// 选择按钮名称
        /// </summary>
        public const string ButtonPartName = "PART_Button";

        /// <summary>
        /// 时钟名称
        /// </summary>
        public const string ClockPartName = "PART_Clock";

        private TextBox _textBox;
        private Popup _popup;
        private Clock _clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Clock clock = GetTemplateChild(ClockPartName) as Clock;
            clock.CurrentTimeChanged += Clock_CurrentTimeChanged; ;
            clock.SelectedTimeChanged += Clock_SelectedTimeChanged;
            this._clock = clock;

            TextBox textBox = GetTemplateChild(TextBoxPartName) as TextBox;
            var binding1 = new Binding("Text");
            binding1.Source = this;
            binding1.Mode = BindingMode.TwoWay;
            textBox.SetBinding(TextBox.TextProperty, binding1);
            this._textBox = textBox;

            Popup popup = GetTemplateChild(PopupPartName) as Popup;
            popup.Closed += Popup_Closed;
            if (this.IsDropDownOpen)
            {
                this._popup.IsOpen = true;
            }
            this._popup = popup;

            Button button = GetTemplateChild(ButtonPartName) as Button;
            button.Click += Button_Click;
        }

        #region 路由事件

        /// <summary>
        /// 选择时间改变事件
        /// </summary>
        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(TimePicker));

        /// <summary>
        /// 选择时间改变事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        #endregion 路由事件

        #region 依赖属性

        /// <summary>
        /// 选中时间
        /// </summary>
        public static readonly DependencyProperty SeletedTimeProperty = DependencyProperty.Register(
            "SelectedTime", typeof(DateTime?), typeof(TimePicker), new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

        /// <summary>
        /// 选中时间
        /// </summary>
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SeletedTimeProperty); }
            set { SetValue(SeletedTimeProperty, value); }
        }

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(TimePicker), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDropDownOpenChanged));

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker tp = d as TimePicker;
            bool newValue = (bool)e.NewValue;
            if (tp._popup != null && tp._popup.IsOpen != newValue)
            {
                tp._popup.IsOpen = newValue;
                if (newValue)
                {
                    tp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate ()
                    {
                        tp._clock.Focus();
                    });
                }
            }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TimePicker), new PropertyMetadata(null, OnTextChanged));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = (TimePicker)d;
            var oldDateTime = timePicker.SelectedTime?.ToString(timePicker.SelectedTimeFormat);
            if (oldDateTime != timePicker.Text)
            {
                if (DateTime.TryParse(timePicker.Text, out DateTime result))
                {
                    timePicker.SelectedTime = result;
                }
                else if (string.IsNullOrEmpty(timePicker.Text))
                {
                    timePicker.SelectedTime = null;
                }
                else
                {
                    timePicker.Text = oldDateTime;
                }
            }
        }

        /// <summary>
        /// 选择时间格式化
        /// </summary>
        public static readonly DependencyProperty SelectedTimeFormatProperty = DependencyProperty.Register(
            "SelectedTimeFormat", typeof(string), typeof(TimePicker), new PropertyMetadata(default(string), OnSelectedTimeFormatChanged));

        private static void OnSelectedTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimePicker timePicker)
            {
                timePicker.Text = timePicker.SelectedTime?.ToString(timePicker.SelectedTimeFormat);
            }
        }

        /// <summary>
        /// 选择时间格式化
        /// </summary>
        public string SelectedTimeFormat
        {
            get { return (string)GetValue(SelectedTimeFormatProperty); }
            set { SetValue(SelectedTimeFormatProperty, value); }
        }

        #endregion 依赖属性

        #region 方法

        private void TextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Focus();
        }

        /// <summary>
        /// 选择时间改变
        /// </summary>
        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = (TimePicker)d;

            timePicker.Text = timePicker.SelectedTime?.ToString(timePicker.SelectedTimeFormat);

            RoutedPropertyChangedEventArgs<DateTime?> args = new RoutedPropertyChangedEventArgs<DateTime?>((DateTime?)e.OldValue, (DateTime?)e.NewValue);
            args.RoutedEvent = TimePicker.SelectedTimeChangedEvent;
            timePicker.RaiseEvent(args);
        }

        private void Clock_CurrentTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (SelectedTime != e.NewValue)
            {
                SelectedTime = e.NewValue;
            }
        }

        /// <summary>
        /// 时钟的时间改变
        /// </summary>
        private void Clock_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (SelectedTime != e.NewValue)
            {
                SelectedTime = e.NewValue;
            }

            IsDropDownOpen = false;

            _textBox.Focus();
            _textBox.SelectAll();
        }

        /// <summary>
        /// 点击时间按钮
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }

        /// <summary>
        /// popup 关闭
        /// </summary>
        private void Popup_Closed(object sender, EventArgs e)
        {
            if (this._clock.IsKeyboardFocusWithin)
            {
                this._textBox.Focus();
            }
            else
            {
                this.IsDropDownOpen = false;
            }
        }

        #endregion 方法
    }
}