﻿using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rubyer
{
    /// <summary>
    /// 日期时间选择器
    /// </summary>
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = CurrentTextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    [TemplatePart(Name = ButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ClockPartName, Type = typeof(Clock))]
    [TemplatePart(Name = CalendarPartName, Type = typeof(Calendar))]
    [TemplatePart(Name = ConfirmButtonPartName, Type = typeof(Button))]
    public class DateTimePicker : Control
    {
        const string TextBoxPartName = "PART_TextBox";
        const string PopupPartName = "PART_Popup";
        const string ButtonPartName = "PART_Button";
        const string ClockPartName = "PART_Clock";
        const string CalendarPartName = "PART_Calendar";
        const string ConfirmButtonPartName = "PART_ConfirmButton";
        const string CurrentTextBoxPartName = "PART_CurrentTextBox";

        private TextBox _textBox;
        private Popup _popup;
        private Clock _clock;
        private Calendar _calendar;
        private Button _confirmButton;

        private bool isInternalSetting = true;
        private DateTime currentDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        /// <inheritdoc/>
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
            _textBox = textBox;

            Popup popup = GetTemplateChild(PopupPartName) as Popup;
            popup.Closed += Popup_Closed;
            popup.Opened += Popup_Opened;
            if (IsDropDownOpen)
            {
                _popup.IsOpen = true;
            }
            _popup = popup;

            Button button = GetTemplateChild(ButtonPartName) as Button;
            button.Click += Button_Click;

            Button confirmButton = GetTemplateChild(ConfirmButtonPartName) as Button;
            confirmButton.Click += ConfirmButton_Click;
            _confirmButton = confirmButton;

            currentDateTime = SelectedDateTime == null ? DateTime.Now : SelectedDateTime.Value;
        }

        #region 路由事件

        /// <summary>
        /// 选择时间改变事件
        /// </summary>
        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(DateTimePicker));

        /// <summary>
        /// 选择时间改变事件处理
        /// </summary>
        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        #endregion 路由事件

        #region 依赖属性

        /// <summary>
        /// 日历样式
        /// </summary>
        public static readonly DependencyProperty CalendarStyleProperty =
           DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(DateTimePicker));

        /// <summary>
        /// 日历样式
        /// </summary>
        public Style CalendarStyle
        {
            get => (Style)GetValue(CalendarStyleProperty);
            set => SetValue(CalendarStyleProperty, value);
        }

        /// <summary>
        /// 时钟样式
        /// </summary>
        public static readonly DependencyProperty ClockStyleProperty =
           DependencyProperty.Register("ClockStyle", typeof(Style), typeof(DateTimePicker));

        /// <summary>
        /// 时钟样式
        /// </summary>
        public Style ClockStyle
        {
            get => (Style)GetValue(ClockStyleProperty);
            set => SetValue(ClockStyleProperty, value);
        }

        /// <summary>
        /// 选择的日期时间
        /// </summary>
        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTime?), typeof(DateTimePicker), new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

        /// <summary>
        /// 选择的日期时间
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        /// <summary>
        /// 是否下拉打开
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDropDownOpenChanged));

        /// <summary>
        /// 是否下拉打开
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// Ons the is drop down open changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(DateTimePicker), new PropertyMetadata(null, OnTextChanged));

        /// <summary>
        /// 显示文本
        /// </summary>
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

            var oldDateTime = dateTimePicker.SelectedDateTime?.ToString(dateTimePicker.SelectedDateTimeFormat);
            if (oldDateTime != dateTimePicker.Text)
            {
                if (DateTime.TryParse(dateTimePicker.Text, out DateTime result))
                {
                    dateTimePicker.SelectedDateTime = result;
                    dateTimePicker.currentDateTime = result;
                }
                else if (string.IsNullOrEmpty(dateTimePicker.Text))
                {
                    dateTimePicker.SelectedDateTime = null;
                }
                else
                {
                    dateTimePicker.Text = oldDateTime;
                }
            }
        }

        /// <summary>
        /// 选择时间格式化
        /// </summary>
        public static readonly DependencyProperty SelectedDateTimeFormatProperty = DependencyProperty.Register(
            "SelectedDateTimeFormat", typeof(string), typeof(DateTimePicker), new PropertyMetadata(default(string), OnSelectedDateTimeFormatChanged));

        private static void OnSelectedDateTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker picker)
            {
                picker.Text = picker.SelectedDateTime?.ToString(picker.SelectedDateTimeFormat);
            }
        }

        /// <summary>
        /// 选择时间格式化
        /// </summary>
        public string SelectedDateTimeFormat
        {
            get { return (string)GetValue(SelectedDateTimeFormatProperty); }
            set { SetValue(SelectedDateTimeFormatProperty, value); }
        }

        #endregion 依赖属性

        #region 方法

        /// <summary>
        /// 文本鼠标点下
        /// </summary>
        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Focus();
        }

        /// <summary>
        /// 选择时间改变
        /// </summary>
        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker dateTimePicker)
            {
                dateTimePicker.Text = dateTimePicker.SelectedDateTime?.ToString(dateTimePicker.SelectedDateTimeFormat);

                var args = new RoutedPropertyChangedEventArgs<DateTime?>((DateTime?)e.OldValue, (DateTime?)e.NewValue);
                args.RoutedEvent = DateTimePicker.SelectedTimeChangedEvent;
                dateTimePicker.RaiseEvent(args);

                dateTimePicker.currentDateTime = dateTimePicker.SelectedDateTime == null ? DateTime.Now : dateTimePicker.SelectedDateTime.Value;

                dateTimePicker._textBox?.Focus();
                dateTimePicker._textBox?.SelectAll();
            }
        }

        /// <summary>
        /// 时钟的时间改变
        /// </summary>
        private void Clock_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (isInternalSetting)
            {
                return;
            }

            if (SelectedDateTime != null)
            {
                DateTime dateTime = (DateTime)SelectedDateTime;
                DateTime newDate = (DateTime)e.NewValue;
                currentDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, newDate.Hour, newDate.Minute, newDate.Second);
            }
            else
            {
                currentDateTime = e.NewValue.Value;
            }

        }

        /// <summary>
        /// 日期改变
        /// </summary>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInternalSetting)
            {
                return;
            }

            if (SelectedDateTime != null)
            {
                DateTime dateTime = (DateTime)SelectedDateTime;
                DateTime newTime = (DateTime)_calendar.SelectedDate;
                currentDateTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            }
            else
            {
                var time = _clock.CurrentTime == null ? TimeSpan.Zero : _clock.CurrentTime.Value.TimeOfDay;
                currentDateTime = (DateTime)_calendar.SelectedDate + time;
            }

            Mouse.Capture(null);
        }

        /// <summary>
        /// 点击时间按钮
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Text = _textBox.Text;
            IsDropDownOpen = !IsDropDownOpen;
        }

        /// <summary>
        /// popup 打开后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_Opened(object sender, EventArgs e)
        {
            isInternalSetting = true;
            _calendar.SelectedDate = currentDateTime.Date;
            _calendar.DisplayDate = currentDateTime.Date;
            _clock.SelectedTime = currentDateTime;
            isInternalSetting = false;
        }

        /// <summary>
        ///  popup 关闭
        /// </summary>
        private void Popup_Closed(object sender, EventArgs e)
        {
            if (_confirmButton.IsKeyboardFocusWithin)
            {
                _textBox.Focus();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = currentDateTime;
            IsDropDownOpen = false;

            var args = new RoutedPropertyChangedEventArgs<DateTime?>(SelectedDateTime, SelectedDateTime);
            args.RoutedEvent = DateTimePicker.SelectedTimeChangedEvent;
            RaiseEvent(args);
        }

        #endregion 方法
    }
}