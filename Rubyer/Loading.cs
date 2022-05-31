using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    ///  加载中
    /// </summary>
    [TemplatePart(Name = MessageTextPartName, Type = typeof(TextBlock))]
    public class Loading : Control
    {
        public const string MessageTextPartName = "Path_MessageText";
        static Loading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(MessageTextPartName) is TextBlock textBlock)
            {
                var binding = new Binding("Text");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                textBlock.SetBinding(TextBlock.TextProperty, binding);
            }
        }

        // 边框厚度
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(Loading), new PropertyMetadata(default(double)));

        /// <summary>
        /// 边框厚度
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof(DoubleCollection), typeof(Loading), new PropertyMetadata(default(DoubleCollection)));

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        // 聚焦颜色
        public static readonly DependencyProperty FocusedBrushProperty = DependencyProperty.RegisterAttached(
            "FocusedBrush", typeof(Brush), typeof(Loading), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 聚焦颜色
        /// </summary>
        public Brush FocusedBrush
        {
            get { return (Brush)GetValue(FocusedBrushProperty); }
            set { SetValue(FocusedBrushProperty, value); }
        }

        // 进度
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
            "Progress", typeof(double), typeof(Loading), new PropertyMetadata(default(double), OnProgressChanged));

        /// <summary>
        /// 进度
        /// </summary>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loading = d as Loading;
            loading.StrokeDashArray = new DoubleCollection(new List<double> { (loading.CircleDiameter - loading.StrokeThickness) * Math.PI / loading.StrokeThickness * loading.Progress, double.MaxValue });
        }

        // 文本
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Loading), new PropertyMetadata(default(string)));

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // 圆形直径
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.RegisterAttached(
            "CircleDiameter", typeof(double), typeof(Loading), new PropertyMetadata(default(double)));

        /// <summary>
        /// 圆形直径
        /// </summary>
        public double CircleDiameter
        {
            get { return (double)GetValue(CircleDiameterProperty); }
            set { SetValue(CircleDiameterProperty, value); }
        }
    }
}
