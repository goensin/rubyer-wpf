using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rubyer
{
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    [TemplatePart(Name = ButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ClockPartName, Type = typeof(Clock))]
    [TemplatePart(Name = CalendarPartName, Type = typeof(Calendar))]
    [TemplatePart(Name = ConfirmButtonPartName, Type = typeof(Button))]
    public class DateTimePicker : Control
    {
        public const string TextBoxPartName = "PART_TextBox";
        public const string PopupPartName = "PART_Popup";
        public const string ButtonPartName = "PART_Button";
        public const string ClockPartName = "PART_Clock";
        public const string CalendarPartName = "PART_Calendar";
        public const string ConfirmButtonPartName = "PART_ConfirmButton";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private TextBox _textBox;
        private Popup _popup;
        private Clock _clock;
        private Calendar _calendar;
        private Button _confirmButton;

        private bool isInited;

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Clock clock = GetTemplateChild(ClockPartName) as Clock;
            clock.CurrentTimeChanged += Clock_SelectedTimeChanged;
            this._clock = clock;

            Calendar calendar = GetTemplateChild(CalendarPartName) as Calendar;
            calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            this._calendar = calendar;

            TextBox textBox = GetTemplateChild(TextBoxPartName) as TextBox;
            Binding binding1 = new Binding("Text");
            binding1.Source = this;
            binding1.Mode = BindingMode.TwoWay;
            textBox.SetBinding(TextBox.TextProperty, binding1);
            this._textBox = textBox;

            Popup popup = GetTemplateChild(PopupPartName) as Popup;
            popup.Closed += Popup_Closed;
            popup.Opened += Popup_Opened;
            if (this.IsDropDownOpen)
            {
                this._popup.IsOpen = true;
            }
            this._popup = popup;

            Button button = GetTemplateChild(ButtonPartName) as Button;
            button.Click += Button_Click;

            Button confirmButton = GetTemplateChild(ConfirmButtonPartName) as Button;
            confirmButton.Click += ConfirmButton_Click;
            this._confirmButton = confirmButton;
        }

        #region 路由事件
        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(DateTimePicker));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }
        #endregion

        #region 依赖属性

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTime?), typeof(DateTimePicker), new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false, OnIsDropDownOpenChanged));

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker tp = d as DateTimePicker;
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

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(DateTimePicker), new PropertyMetadata(null, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 时间文本改变
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker dateTimePicker = (DateTimePicker)d;

            try
            {
                if (((DateTime)dateTimePicker.SelectedDateTime).ToString(DateTimeFormat) != dateTimePicker.Text)
                {
                    dateTimePicker.SelectedDateTime = Convert.ToDateTime(dateTimePicker.Text);
                }
            }
            catch (Exception)
            {
                dateTimePicker.Text = dateTimePicker.SelectedDateTime == null ? null : ((DateTime)dateTimePicker.SelectedDateTime).ToString(DateTimeFormat);
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 文本鼠标点下
        /// </summary>
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
            DateTimePicker dateTimePicker = (DateTimePicker)d;
            dateTimePicker.Text = ((DateTime)dateTimePicker.SelectedDateTime).ToString(DateTimeFormat);

            var args = new RoutedPropertyChangedEventArgs<DateTime?>((DateTime?)e.OldValue, (DateTime?)e.NewValue);
            args.RoutedEvent = DateTimePicker.SelectedTimeChangedEvent;
            dateTimePicker.RaiseEvent(args);

            dateTimePicker._textBox.Focus();
            dateTimePicker._textBox.SelectAll();
        }

        /// <summary>
        /// 时钟的时间改变
        /// </summary>
        private void Clock_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (!isInited)
            {
                return;
            }

            if (SelectedDateTime != null)
            {
                DateTime dateTime = (DateTime)SelectedDateTime;
                DateTime newDate = (DateTime)e.NewValue;
                SelectedDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, newDate.Hour, newDate.Minute, newDate.Second);
            }
            else
            {
                SelectedDateTime = e.NewValue;
            }
        }

        /// <summary>
        /// 日期改变
        /// </summary>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInited)
            {
                return;
            }

            if (SelectedDateTime != null)
            {
                DateTime dateTime = (DateTime)SelectedDateTime;
                DateTime newTime = (DateTime)_calendar.SelectedDate;
                SelectedDateTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            }
            else
            {
                SelectedDateTime = (DateTime)_calendar.SelectedDate + _clock.DisplayTime?.TimeOfDay;
            }

            Mouse.Capture(null);
            this._textBox.Focus();
        }


        /// <summary>
        /// 点击时间按钮
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e) => IsDropDownOpen = !IsDropDownOpen;

        /// <summary>
        /// popup 打开后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_Opened(object sender, EventArgs e) => isInited = true;

        /// <summary>
        ///  popup 关闭
        /// </summary>
        private void Popup_Closed(object sender, EventArgs e)
        {
            if (this._confirmButton.IsKeyboardFocusWithin)
            {
                this._textBox.Focus();
            }
            else
            {
                this.IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = false;

            var args = new RoutedPropertyChangedEventArgs<DateTime>((DateTime)SelectedDateTime, (DateTime)SelectedDateTime);
            args.RoutedEvent = DateTimePicker.SelectedTimeChangedEvent;
            this.RaiseEvent(args);
        }
        #endregion

    }
}
