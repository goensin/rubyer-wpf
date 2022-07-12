﻿using Rubyer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// 数值框
    /// </summary>
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = ButtonIncreasePartName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ButtonDecreasePartName, Type = typeof(ButtonBase))]
    public class NumericBox : Control
    {
        /// <summary>
        /// 文本框名称
        /// </summary>
        public const string TextBoxPartName = "PART_TextBox";

        /// <summary>
        /// 增加按钮名称
        /// </summary>
        public const string ButtonIncreasePartName = "PART_IncreaseButton";

        /// <summary>
        /// 减少按钮名称
        /// </summary>
        public const string ButtonDecreasePartName = "PART_DecreaseButton";

        /// <summary>
        /// 默认 int 类型正则匹配
        /// </summary>
        public const string DefaultIntPattern = "[0-9-]";

        /// <summary>
        /// 默认 double 类型正则匹配
        /// </summary>
        public const string DefaultDoublePattern = "[0-9-+Ee\\.]";

        private TextBox textBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox"/> class.
        /// </summary>
        static NumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericBox), new FrameworkPropertyMetadata(typeof(NumericBox)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TextBoxPartName) is TextBox textBox)
            {
                var textBinding = new Binding(nameof(Text));
                textBinding.Source = this;
                textBinding.Mode = BindingMode.TwoWay;
                textBox.SetBinding(TextBox.TextProperty, textBinding);
                textBox.TextChanged += TextBox_TextChanged;
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                textBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, new CanExecuteRoutedEventHandler(TextBox_CanExecutePaste)));
                this.textBox = textBox;
            }

            if (GetTemplateChild(ButtonIncreasePartName) is ButtonBase upButton)
            {
                upButton.Click += IncreaseButton_Click;
            }

            if (GetTemplateChild(ButtonDecreasePartName) is ButtonBase downButton)
            {
                downButton.Click += DecreaseButton_Click;
            }
        }

        #region events

        /// <summary>
        /// 值改变事件
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double?>), typeof(NumericBox));

        /// <summary>
        /// 值改变事件处理
        /// </summary>
        public event RoutedPropertyChangedEventHandler<double?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion events

        #region propteries

        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text", typeof(string), typeof(NumericBox), new PropertyMetadata(null, OnTextChanged));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 文本格式
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty = DependencyProperty.Register(
        "TextFormat", typeof(string), typeof(NumericBox), new PropertyMetadata(default(string)));

        /// <summary>
        /// 文本格式
        /// </summary>
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        /// <summary>
        /// 值
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(double?), typeof(NumericBox), new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        /// <summary>
        /// 值
        /// </summary>
        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
           "MinValue", typeof(double), typeof(NumericBox), new PropertyMetadata(double.MinValue));

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
           "MaxValue", typeof(double), typeof(NumericBox), new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// 是否显示增减按钮
        /// </summary>
        public static readonly DependencyProperty ShowButtonProperty = DependencyProperty.Register(
           "ShowButton", typeof(bool), typeof(NumericBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示增减按钮
        /// </summary>
        public bool ShowButton
        {
            get { return (bool)GetValue(ShowButtonProperty); }
            set { SetValue(ShowButtonProperty, value); }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
          "MaxLength", typeof(int), typeof(NumericBox), new PropertyMetadata(default(int)));

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// 增减间隔
        /// </summary>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
          "Interval", typeof(double), typeof(NumericBox), new PropertyMetadata(default(double)));

        /// <summary>
        /// 增减间隔
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// 数值类型
        /// </summary>
        public static readonly DependencyProperty NumericTypeProperty = DependencyProperty.Register(
          "NumericType", typeof(NumericType), typeof(NumericBox), new PropertyMetadata(default(NumericType)));

        /// <summary>
        /// 数值类型
        /// </summary>
        public NumericType NumericType
        {
            get { return (NumericType)GetValue(NumericTypeProperty); }
            set { SetValue(NumericTypeProperty, value); }
        }

        /// <summary>
        /// 数值输入正则匹配
        /// </summary>
        public static readonly DependencyProperty NumericPatternProperty = DependencyProperty.Register(
         "NumericPattern", typeof(string), typeof(NumericBox), new PropertyMetadata(default(string)));

        /// <summary>
        /// 数值输入正则匹配
        /// </summary>
        public string NumericPattern
        {
            get { return (string)GetValue(NumericPatternProperty); }
            set { SetValue(NumericPatternProperty, value); }
        }

        #endregion propteries

        #region methods

        private static double GetCalculatedValue(NumericBox numberBox, double value)
        {
            if (value > numberBox.MaxValue)
            {
                return numberBox.MaxValue;
            }
            else if (value < numberBox.MinValue)
            {
                return numberBox.MinValue;
            }
            else
            {
                return value;
            }
        }

        private void GetIntervalAndMin(out double interval, out double min)
        {
            interval = NumericType == NumericType.Double ? Interval : Math.Round(Interval);
            min = MinValue == double.MinValue ? 0 : MinValue;
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            GetIntervalAndMin(out double interval, out double min);
            Value = GetCalculatedValue(this, Value == null ? min + interval : Value.GetValueOrDefault() + interval);
            textBox.Focus();
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            GetIntervalAndMin(out double interval, out double min);
            Value = GetCalculatedValue(this, Value == null ? min : Value.GetValueOrDefault() - interval);
            textBox.Focus();
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numberBox = d as NumericBox;

            if (string.IsNullOrEmpty(numberBox.Text))
            {
                numberBox.Value = null;
                return;
            }
            else if (double.TryParse(numberBox.Text, out double value))
            {
                var newValue = GetCalculatedValue(numberBox, value);
                if (numberBox.Value != newValue)
                {
                    numberBox.Value = newValue;
                    return;
                }
            }

            numberBox.Text = numberBox.Value.GetValueOrDefault().ToString(numberBox.TextFormat);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numberBox = d as NumericBox;
            numberBox.Text = numberBox.Value == null ? null : numberBox.Value.GetValueOrDefault().ToString(numberBox.TextFormat);
            numberBox.textBox?.Select(numberBox.textBox.Text.Length, 1);

            var args = new RoutedPropertyChangedEventArgs<double?>((double?)e.OldValue, (double?)e.NewValue);
            args.RoutedEvent = NumericBox.ValueChangedEvent;
            numberBox.RaiseEvent(args);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 限制与数值无关输入
            string pattern = string.IsNullOrEmpty(NumericPattern) ? (NumericType == NumericType.Int ? DefaultIntPattern : DefaultDoublePattern) : NumericPattern;
            var regex = new Regex(pattern);
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 限制空格输入
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TextBox_CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
        {
            // 限制粘贴
            e.CanExecute = false;
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (double.TryParse(textBox.Text, out double value))
            {
                if (value < MinValue || value > MaxValue)
                {
                    var newValue = GetCalculatedValue(this, value);
                    if (Value != newValue)
                    {
                        Value = newValue;
                    }
                    else
                    {
                        textBox.Text = newValue.ToString(TextFormat);
                        textBox.Select(textBox.Text.Length, 0);
                    }
                }
            }
            else if (string.IsNullOrEmpty(textBox.Text))
            {
                Value = null;
            }
        }

        #endregion methods
    }
}