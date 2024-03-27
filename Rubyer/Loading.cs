using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    ///  加载中
    /// </summary>
    [TemplatePart(Name = MessageTextPartName, Type = typeof(TextBlock))]
    public class Loading : ContentControl
    {
        /// <summary>
        /// 消息文本名称
        /// </summary>
        public const string MessageTextPartName = "Path_MessageText";

        static Loading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
        }

        /// <summary>
        /// 边框厚度
        /// </summary>
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

        /// <summary>
        /// 虚线和间隙的值的集合
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof(DoubleCollection), typeof(Loading), new PropertyMetadata(default(DoubleCollection)));

        /// <summary>
        /// 虚线和间隙的值的集合
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        /// <summary>
        /// 聚焦颜色
        /// </summary>
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

        /// <summary>
        /// 进度
        /// </summary>
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
            loading.StrokeDashArray = new DoubleCollection(new List<double> { (loading.Diameter - loading.StrokeThickness) * Math.PI / loading.StrokeThickness * loading.Progress, double.MaxValue });
        }

        /// <summary>
        /// 直径
        /// </summary>
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.RegisterAttached(
            "Diameter", typeof(double), typeof(Loading), new PropertyMetadata(default(double)));

        /// <summary>
        /// 直径
        /// </summary>
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }
    }
}