using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 描述控件子项
    /// </summary>
    public class DescriptionItem : ContentControl
    {
        static DescriptionItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DescriptionItem), new FrameworkPropertyMetadata(typeof(DescriptionItem)));
        }

        #region properties

        /// <summary>
        /// 描述
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(object), typeof(DescriptionItem), new PropertyMetadata(default(object)));

        /// <summary>
        /// 描述
        /// </summary>
        public object Description
        {
            get { return (object)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// 描述列宽
        /// </summary>
        public static readonly DependencyProperty DescriptionWidthProperty =
            DependencyProperty.Register("DescriptionWidth", typeof(GridLength), typeof(DescriptionItem), new PropertyMetadata(GridLength.Auto));

        /// <summary>
        /// 描述列宽
        /// </summary>
        public GridLength DescriptionWidth
        {
            get { return (GridLength)GetValue(DescriptionWidthProperty); }
            internal set { SetValue(DescriptionWidthProperty, value); }
        }

        /// <summary>
        /// 描述背景色
        /// </summary>
        public static readonly DependencyProperty DescriptionBackgroundProperty =
            DependencyProperty.Register("DescriptionBackground", typeof(Brush), typeof(DescriptionItem), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 描述背景色
        /// </summary>
        public Brush DescriptionBackground
        {
            get { return (Brush)GetValue(DescriptionBackgroundProperty); }
            set { SetValue(DescriptionBackgroundProperty, value); }
        }

        /// <summary>
        /// 描述前景色
        /// </summary>
        public static readonly DependencyProperty DescriptionForegroundProperty =
            DependencyProperty.Register("DescriptionForeground", typeof(Brush), typeof(DescriptionItem), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 描述前景色
        /// </summary>
        public Brush DescriptionForeground
        {
            get { return (Brush)GetValue(DescriptionForegroundProperty); }
            set { SetValue(DescriptionForegroundProperty, value); }
        }

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public static readonly DependencyProperty DescriptionFontSizeProperty =
            DependencyProperty.Register("DescriptionFontSize", typeof(double), typeof(DescriptionItem), new FrameworkPropertyMetadata(SystemFonts.CaptionFontSize, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public double DescriptionFontSize
        {
            get { return (double)GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        /// <summary>
        /// 描述字体加粗
        /// </summary>
        public static readonly DependencyProperty DescriptionFontWeightProperty =
            DependencyProperty.Register("DescriptionFontWeight", typeof(FontWeight), typeof(DescriptionItem), new FrameworkPropertyMetadata(SystemFonts.CaptionFontWeight, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// 描述字体加粗
        /// </summary>
        public FontWeight DescriptionFontWeight
        {
            get { return (FontWeight)GetValue(DescriptionFontWeightProperty); }
            set { SetValue(DescriptionFontWeightProperty, value); }
        }

        /// <summary>
        /// 描述格边框厚度
        /// </summary>
        public static readonly DependencyProperty DescriptionBorderThicknessProperty =
            DependencyProperty.Register("DescriptionBorderThickness", typeof(Thickness), typeof(DescriptionItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// 描述格边框厚度
        /// </summary>
        public Thickness DescriptionBorderThickness
        {
            get { return (Thickness)GetValue(DescriptionBorderThicknessProperty); }
            internal set { SetValue(DescriptionBorderThicknessProperty, value); }
        }

        /// <summary>
        /// 描述水平对齐
        /// </summary>
        public static readonly DependencyProperty HorizontalDescriptionAlignmentProperty =
            DependencyProperty.Register("HorizontalDescriptionAlignment", typeof(HorizontalAlignment), typeof(DescriptionItem), new FrameworkPropertyMetadata(default(HorizontalAlignment)));

        /// <summary>
        /// 描述水平对齐
        /// </summary>
        public HorizontalAlignment HorizontalDescriptionAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalDescriptionAlignmentProperty); }
            set { SetValue(HorizontalDescriptionAlignmentProperty, value); }
        }

        /// <summary>
        /// 描述垂直对齐
        /// </summary>
        public static readonly DependencyProperty VerticalDescriptionAlignmentProperty =
            DependencyProperty.Register("VerticalDescriptionAlignment", typeof(VerticalAlignment), typeof(DescriptionItem), new FrameworkPropertyMetadata(default(VerticalAlignment)));

        /// <summary>
        /// 描述垂直对齐
        /// </summary>
        public VerticalAlignment VerticalDescriptionAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalDescriptionAlignmentProperty); }
            set { SetValue(VerticalDescriptionAlignmentProperty, value); }
        }

        /// <summary>
        /// 内容格边框厚度
        /// </summary>
        public static readonly DependencyProperty ContentBorderThicknessProperty =
            DependencyProperty.Register("ContentBorderThickness", typeof(Thickness), typeof(DescriptionItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// 内容格边框厚度
        /// </summary>
        public Thickness ContentBorderThickness
        {
            get { return (Thickness)GetValue(ContentBorderThicknessProperty); }
            internal set { SetValue(ContentBorderThicknessProperty, value); }
        }

        /// <summary>
        /// 显示 Description 的字段
        /// </summary>
        public static readonly DependencyProperty DescriptionFieldProperty =
            DependencyProperty.Register(nameof(DescriptionField), typeof(object), typeof(DescriptionItem), new PropertyMetadata(null, OnDescriptionFieldChanged));

        /// <summary>
        /// 显示 Description 的字段，用来获取 DisplayName 显示dao Description 中
        /// </summary>
        public object DescriptionField
        {
            get { return (object)GetValue(DescriptionFieldProperty); }
            set { SetValue(DescriptionFieldProperty, value); }
        }

        #endregion properties

        private static void OnDescriptionFieldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DescriptionItem descriptionItem)
            {
                var binding = BindingOperations.GetBinding(descriptionItem, DescriptionFieldProperty);
                descriptionItem.LoadDisplayName(binding?.Path?.Path);
            }
        }

        private void LoadDisplayName(string propertyName)
        {
            if (DataContext == null || string.IsNullOrWhiteSpace(propertyName))
                return;

            var property = DataContext.GetType().GetProperty(propertyName);

            if (property?.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
            {
                Description = displayNameAttribute?.DisplayName;
            }
            else if (property?.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
            {
                Description = displayAttribute?.GetName();
            }
        }
    }
}