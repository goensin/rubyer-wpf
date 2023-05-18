using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Rubyer
{
    /// <summary>
    /// 时钟
    /// </summary>
    [TemplatePart(Name = HourListPartName, Type = typeof(Selector))]
    [TemplatePart(Name = MinuteListPartName, Type = typeof(Selector))]
    [TemplatePart(Name = ConfirmPartName, Type = typeof(Button))]
    public class Clock : Control
    {
        /// <summary>
        /// 小时列表名称
        /// </summary>
        public const string HourListPartName = "PART_HourList";

        /// <summary>
        /// 分钟列表名称
        /// </summary>
        public const string MinuteListPartName = "PART_MinuteList";

        /// <summary>
        /// 确认按钮名称
        /// </summary>
        public const string ConfirmPartName = "PART_ConfirmButton";

        /// <summary>
        /// Initializes a new instance of the <see cref="Clock"/> class.
        /// </summary>
        static Clock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Clock), new FrameworkPropertyMetadata(typeof(Clock)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(ConfirmPartName) is Button confirmButton)
            {
                confirmButton.Click += ConfirmButton_Click;
            }

            Hours = new ObservableCollection<int>(Enumerable.Range(0, 24));
            Minutes = new ObservableCollection<int>(Enumerable.Range(0, 60));

            CurrentTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            Hour = CurrentTime.Value.Hour;
            Minute = CurrentTime.Value.Minute;
        }

        #region 路由事件

        /// <summary>
        /// 选择时间改变事件
        /// </summary>
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(Clock));

        /// <summary>
        /// 选择时间改变事件处理
        /// </summary>
        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        /// <summary>
        /// 当前事件改变事件
        /// </summary>
        public static readonly RoutedEvent CurrentTimeChangedEvent = EventManager.RegisterRoutedEvent(
            "CurrentTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(Clock));

        /// <summary>
        /// 当前事件改变事件处理
        /// </summary>
        public event RoutedPropertyChangedEventHandler<DateTime?> CurrentTimeChanged
        {
            add { AddHandler(CurrentTimeChangedEvent, value); }
            remove { RemoveHandler(CurrentTimeChangedEvent, value); }
        }
        #endregion

        #region 依赖属性

        /// <summary>
        /// 时集合
        /// </summary>
        internal static readonly DependencyProperty HoursProperty = DependencyProperty.Register(
           "Hours", typeof(ObservableCollection<int>), typeof(Clock), new PropertyMetadata(null));

        /// <summary>
        /// 时集合
        /// </summary>
        internal ObservableCollection<int> Hours
        {
            get { return (ObservableCollection<int>)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        /// <summary>
        /// 时
        /// </summary>
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
           "Hour", typeof(int), typeof(Clock), new PropertyMetadata(0, OnItemSeletedChanged));

        /// <summary>
        /// 时
        /// </summary>
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        /// <summary>
        /// 分集合
        /// </summary>
        internal static readonly DependencyProperty MinutesProperty = DependencyProperty.Register(
           "Minutes", typeof(ObservableCollection<int>), typeof(Clock), new PropertyMetadata(null));

        /// <summary>
        /// 分集合
        /// </summary>
        internal ObservableCollection<int> Minutes
        {
            get { return (ObservableCollection<int>)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        /// <summary>
        /// 分
        /// </summary>
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
           "Minute", typeof(int), typeof(Clock), new PropertyMetadata(0, OnItemSeletedChanged));

        /// <summary>
        /// 分
        /// </summary>
        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        /// <summary>
        /// 选中时间
        /// </summary>
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
          "SelectedTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime), OnSelectTimeChanged));

        private static void OnSelectTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = (Clock)d;
            if (clock.SelectedTime != null)
            {
                clock.Hour = clock.SelectedTime.Value.Hour;
                clock.Minute = clock.SelectedTime.Value.Minute;
            }
        }

        /// <summary>
        /// 选中时间
        /// </summary>
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime?)));

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime? CurrentTime
        {
            get { return (DateTime?)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        /// <summary>
        /// 是否显示确认按钮
        /// </summary>
        public static readonly DependencyProperty IsShowConfirmButtonProperty =
            DependencyProperty.Register("IsShowConfirmButton", typeof(bool), typeof(Clock), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// 是否显示确认按钮
        /// </summary>
        public bool IsShowConfirmButton
        {
            get { return (bool)GetValue(IsShowConfirmButtonProperty); }
            set { SetValue(IsShowConfirmButtonProperty, BooleanBoxes.Box(value)); }
        }

        #endregion

        /// <summary>
        /// 时间选择改变
        /// </summary>
        private static void OnItemSeletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = d as Clock;

            if (!clock.IsLoaded)
            {
                return;
            }

            clock.CurrentTime = Convert.ToDateTime($"{clock.Hour}:{clock.Minute}");

            if (e.NewValue != null)
            {
                var args = new RoutedPropertyChangedEventArgs<DateTime?>(DateTime.Now, (DateTime)clock.CurrentTime);
                args.RoutedEvent = CurrentTimeChangedEvent;
                clock.RaiseEvent(args);
            }
        }

        /// <summary>
        /// 确认时间
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var oldTime = SelectedTime;
            var newTime = CurrentTime;
            SelectedTime = CurrentTime;

            var args = new RoutedPropertyChangedEventArgs<DateTime?>(oldTime, newTime);
            args.RoutedEvent = SelectedTimeChangedEvent;
            RaiseEvent(args);
        }
    }
}
