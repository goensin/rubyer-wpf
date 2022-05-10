using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace Rubyer
{
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    [TemplatePart(Name = ButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ClockPartName, Type = typeof(Clock))]
    public class TimePicker : Control
    {
        public const string TextBoxPartName = "PART_TextBox";
        public const string PopupPartName = "PART_Popup";
        public const string ButtonPartName = "PART_Button";
        public const string ClockPartName = "PART_Clock";

        private TextBox _textBox;
        private Popup _popup;
        private Clock _clock;

        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Clock clock = GetTemplateChild(ClockPartName) as Clock;
            clock.SelectedTimeChanged += Clock_SelectedTimeChanged;
            this._clock = clock;

            TextBox textBox = GetTemplateChild(TextBoxPartName) as TextBox;
            Binding binding1 = new Binding("Text");
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



        private void TextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Focus();
        }


        #region 路由事件
        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(TimePicker));

        public event RoutedPropertyChangedEventHandler<DateTime> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }
        #endregion

        #region 依赖属性
        public DateTime? SeletedTime
        {
            get { return (DateTime?)GetValue(SeletedTimeProperty); }
            set { SetValue(SeletedTimeProperty, value); }
        }

        public static readonly DependencyProperty SeletedTimeProperty =
            DependencyProperty.Register("SeletedTime", typeof(DateTime?), typeof(TimePicker), new FrameworkPropertyMetadata(default(DateTime), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));


        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(TimePicker), new PropertyMetadata(false, OnIsDropDownOpenChanged));


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

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TimePicker), new PropertyMetadata(null, OnTextChanged));

        // 时间文本改变
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = (TimePicker)d;
            var oldDateTime = timePicker.SeletedTime.GetValueOrDefault().ToString("HH:mm:ss");
            if (oldDateTime != timePicker.Text)
            {
                if (DateTime.TryParse(timePicker.Text, out DateTime result))
                {
                    timePicker.SeletedTime = result;
                }
                else
                {
                    timePicker.Text = oldDateTime;
                }
            }
        }
        #endregion


        #region 方法



        // 选择时间改变
        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = (TimePicker)d;

            timePicker.Text = ((DateTime)timePicker.SeletedTime).ToString("HH:mm:ss");

            RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>((DateTime)e.OldValue, (DateTime)e.NewValue);
            args.RoutedEvent = TimePicker.SelectedTimeChangedEvent;
            timePicker.RaiseEvent(args);
        }

        // 时钟的时间改变
        private void Clock_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (SeletedTime != e.NewValue)
            {
                SeletedTime = e.NewValue;
            }

            IsDropDownOpen = false;

            _textBox.Focus();
            _textBox.SelectAll();
        }



        // 点击时间按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }

        // popup 关闭
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
        #endregion

    }

}
