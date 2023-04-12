﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace Rubyer
{
    /// <summary>
    /// 时钟
    /// </summary>
    [TemplatePart(Name = HourListPartName, Type = typeof(Selector))]
    [TemplatePart(Name = MinuteListPartName, Type = typeof(Selector))]
    [TemplatePart(Name = ConfirmPartName, Type = typeof(Button))]
    [TemplatePart(Name = SelectTimePartName, Type = typeof(TextBlock))]
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
        /// 时间文本名称
        /// </summary>
        public const string SelectTimePartName = "PART_SelectTime";

        private Selector _hourList;
        private Selector _minuteList;

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

            if (GetTemplateChild(SelectTimePartName) is TextBlock selectTimeText)
            {
                var binding = new Binding("CurrentTime");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                binding.StringFormat = "{0:HH : mm}";
                selectTimeText.SetBinding(TextBlock.TextProperty, binding);
            }
            var now = DateTime.Now;
            CurrentTime = Convert.ToDateTime($"{now.Hour}:{now.Minute}");

            if (GetTemplateChild(HourListPartName) is Selector hourList)
            {
                _hourList = hourList;

                var binding = new Binding("Hour");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                hourList.SetBinding(Selector.SelectedItemProperty, binding);

                AddItemsSource(hourList, 24, now.Hour);
            }

            if (GetTemplateChild(MinuteListPartName) is Selector minuteList)
            {
                _minuteList = minuteList;

                var binding = new Binding("Minute");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                minuteList.SetBinding(Selector.SelectedItemProperty, binding);

                AddItemsSource(minuteList, 60, now.Minute);
            }

            if (GetTemplateChild(ConfirmPartName) is Button confirmButton)
            {
                confirmButton.Click += ConfirmButton_Click;
            }

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
        /// 时
        /// </summary>
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
           "Hour", typeof(int), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));

        /// <summary>
        /// 时
        /// </summary>
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        /// <summary>
        /// 分
        /// </summary>
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
           "Minute", typeof(int), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));

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
            ((Clock)d).SetValueToClock();
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

        #endregion

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="itemsControl">列表控件</param>
        /// <param name="count">总数</param>
        /// <param name="index">当前索引</param>
        private void AddItemsSource(Selector itemsControl, int count, int index)
        {
            string[] array = new string[count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i.ToString("D2");
            }

            itemsControl.ItemsSource = array;
            itemsControl.SelectedIndex = index;
        }

        private void SetValueToClock()
        {
            if (SelectedTime != null)
            {
                _hourList.SelectedIndex =  SelectedTime.Value.Hour;
                _minuteList.SelectedIndex = SelectedTime.Value.Minute;
            }

            ScrollToCurrentItem();
        }

        private void ScrollToCurrentItem()
        {
            if (_hourList is ListBox hourList)
            {
                hourList.ScrollIntoView(hourList.Items[hourList.SelectedIndex]);
            }

            if (_minuteList is ListBox minuteList)
            {
                minuteList.ScrollIntoView(minuteList.Items[minuteList.SelectedIndex]);
            }
        }

        /// <summary>
        /// 时间选择改变
        /// </summary>
        private static void OnListSeletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = d as Clock;

            if (!clock.IsLoaded)
            {
                return;
            }

            clock.CurrentTime = Convert.ToDateTime($"{clock.Hour}:{clock.Minute}");
            clock.ScrollToCurrentItem();

            if (e.NewValue != null)
            {
                var args = new RoutedPropertyChangedEventArgs<DateTime?>(DateTime.Now, (DateTime)clock.CurrentTime);
                args.RoutedEvent = Clock.CurrentTimeChangedEvent;
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
            args.RoutedEvent = Clock.SelectedTimeChangedEvent;
            this.RaiseEvent(args);
        }
    }
}
