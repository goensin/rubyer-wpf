using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    [TemplatePart(Name = HourListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = MinuteListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SecondListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = ConfirmPartName, Type = typeof(Button))]
    [TemplatePart(Name = SelectTimePartName, Type = typeof(TextBlock))]
    public class Clock : Control
    {
        public const string HourListPartName = "PART_HourList";
        public const string MinuteListPartName = "PART_MinuteList";
        public const string SecondListPartName = "PART_SecondList";
        public const string ConfirmPartName = "PART_ConfirmButton";
        public const string SelectTimePartName = "PART_SelectTime";

        static Clock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Clock), new FrameworkPropertyMetadata(typeof(Clock)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBlock selectTimeText = GetTemplateChild(SelectTimePartName) as TextBlock;
            if (selectTimeText != null)
            {
                Binding binding = new Binding("DisplayTime");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                binding.StringFormat = "{0:HH : mm : ss}";
                selectTimeText.SetBinding(TextBlock.TextProperty, binding);
            }

            DateTime now = DateTime.Now;

            ListBox hourList = GetTemplateChild(HourListPartName) as ListBox;
            if (hourList != null)
            {
                Binding binding = new Binding("Hour");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                hourList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(hourList, 24, now.Hour);
            }

            ListBox minuteList = GetTemplateChild(MinuteListPartName) as ListBox;
            if (minuteList != null)
            {
                Binding binding = new Binding("Minute");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                minuteList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(minuteList, 60, now.Minute);
            }

            ListBox secondList = GetTemplateChild(SecondListPartName) as ListBox;
            if (secondList != null)
            {
                Binding binding = new Binding("Second");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                secondList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(secondList, 60, now.Second);
            }

            Button confirmButton = GetTemplateChild(ConfirmPartName) as Button;
            confirmButton.Click += ConfirmButton_Click;

            DisplayTime = DateTime.Now;
        }

        #region 路由事件
        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(Clock));

        public event RoutedPropertyChangedEventHandler<DateTime> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentTimeChangedEvent =
            EventManager.RegisterRoutedEvent("CurrentTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(Clock));

        public event RoutedPropertyChangedEventHandler<DateTime> CurrentTimeChanged
        {
            add { AddHandler(CurrentTimeChangedEvent, value); }
            remove { RemoveHandler(CurrentTimeChangedEvent, value); }
        }
        #endregion


        #region 依赖属性
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register("Hour", typeof(int), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));


        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register("Minute", typeof(int), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));


        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(int), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));


        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime)));


        public DateTime? DisplayTime
        {
            get { return (DateTime?)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        public static readonly DependencyProperty DisplayTimeProperty =
            DependencyProperty.Register("DisplayTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime)));


        public static readonly DependencyProperty IsShowConfirmButtonProperty =
            DependencyProperty.Register("IsShowConfirmButton", typeof(bool), typeof(Clock), new PropertyMetadata(default(bool)));

        public bool IsShowConfirmButton
        {
            get { return (bool)GetValue(IsShowConfirmButtonProperty); }
            set { SetValue(IsShowConfirmButtonProperty, value); }
        }

        #endregion

        // 时间选择改变
        private static void OnListSeletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Clock clock = d as Clock;
            clock.DisplayTime = Convert.ToDateTime($"{clock.Hour}:{clock.Minute}:{clock.Second}");

            RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>(DateTime.Now, (DateTime)clock.DisplayTime);
            args.RoutedEvent = Clock.CurrentTimeChangedEvent;
            clock.RaiseEvent(args);
        }

        // 添加子项
        private void AddItemSource(ListBox itemsControl, int count, int index)
        {
            string[] array = new string[count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i.ToString("D2");
            }
            itemsControl.ItemsSource = array;

            itemsControl.SelectedIndex = index;
            itemsControl.ScrollIntoView(((int)itemsControl.SelectedItem).ToString("D2"));
        }

        // 确认时间
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime oldTime = (DateTime)SelectedTime;
            DateTime newTime = (DateTime)DisplayTime;

            SelectedTime = DisplayTime;
            
            RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>(oldTime, newTime);
            args.RoutedEvent = Clock.SelectedTimeChangedEvent;
            this.RaiseEvent(args);
        }
    }
}
