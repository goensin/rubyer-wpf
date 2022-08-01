using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 描述控件
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(DescriptionItem))]
    public class Description : ItemsControl
    {
        #region properties

        /// <summary>
        /// 列数
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(Description), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, OnColumnsChanged));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Description descriptionHost)
            {
                descriptionHost.Rows = descriptionHost.Items.Count / descriptionHost.Columns + (descriptionHost.Items.Count % descriptionHost.Columns > 0 ? 1 : 0);
            }
        }

        /// <summary>
        /// 列数
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// 行数
        /// </summary>
        internal static readonly DependencyPropertyKey RowsPropertyKey =
            DependencyProperty.RegisterReadOnly("Rows", typeof(int), typeof(Description), new PropertyMetadata(1));

        /// <summary>
        /// 行数
        /// </summary>
        public static readonly DependencyProperty RowsProperty = RowsPropertyKey.DependencyProperty;

        /// <summary>
        /// 行数
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            private set { SetValue(RowsPropertyKey, value); }
        }

        /// <summary>
        /// 描述背景色
        /// </summary>
        public static readonly DependencyProperty DescriptionBackgroundProperty =
            DependencyProperty.Register("DescriptionBackground", typeof(Brush), typeof(Description), new PropertyMetadata(default(Brush)));

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
            DependencyProperty.Register("DescriptionForeground", typeof(Brush), typeof(Description), new PropertyMetadata(default(Brush)));

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
            DependencyProperty.Register("DescriptionFontSize", typeof(double), typeof(Description), new FrameworkPropertyMetadata(SystemFonts.CaptionFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

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
            DependencyProperty.Register("DescriptionFontWeight", typeof(FontWeight), typeof(Description), new FrameworkPropertyMetadata(SystemFonts.CaptionFontWeight, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 描述字体加粗
        /// </summary>
        public FontWeight DescriptionFontWeight
        {
            get { return (FontWeight)GetValue(DescriptionFontWeightProperty); }
            set { SetValue(DescriptionFontWeightProperty, value); }
        }

        /// <summary>
        /// 描述水平对齐
        /// </summary>
        public static readonly DependencyProperty HorizontalDescriptionAlignmentProperty =
            DependencyProperty.Register("HorizontalDescriptionAlignment", typeof(HorizontalAlignment), typeof(Description), new FrameworkPropertyMetadata(default(HorizontalAlignment)));

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
            DependencyProperty.Register("VerticalDescriptionAlignment", typeof(VerticalAlignment), typeof(Description), new FrameworkPropertyMetadata(default(VerticalAlignment)));

        /// <summary>
        /// 描述垂直对齐
        /// </summary>
        public VerticalAlignment VerticalDescriptionAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalDescriptionAlignmentProperty); }
            set { SetValue(VerticalDescriptionAlignmentProperty, value); }
        }

        /// <summary>
        /// 显示描述路径
        /// </summary>
        public static readonly DependencyProperty DisplayDescriptionPathProperty =
            DependencyProperty.Register("DisplayDescriptionPath", typeof(string), typeof(Description), new FrameworkPropertyMetadata(string.Empty, OnDisplayDescriptionPathChanged));

        /// <summary>
        /// 显示描述路径
        /// </summary>
        public string DisplayDescriptionPath
        {
            get { return (string)GetValue(DisplayDescriptionPathProperty); }
            set { SetValue(DisplayDescriptionPathProperty, value); }
        }

        /// <summary>
        /// 描述字符串格式化
        /// </summary>
        public static readonly DependencyProperty DescriptionStringFormatProperty =
            DependencyProperty.Register("DescriptionStringFormat", typeof(string), typeof(Description), new FrameworkPropertyMetadata(null, OnDescriptionStringFormatChanged));

        /// <summary>
        /// 描述字符串格式化
        /// </summary>
        public string DescriptionStringFormat
        {
            get { return (string)GetValue(DescriptionStringFormatProperty); }
            set { SetValue(DescriptionStringFormatProperty, value); }
        }

        #endregion properties

        static Description()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Description), new FrameworkPropertyMetadata(typeof(Description)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DescriptionItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DescriptionItem();
        }

        private DescriptionItem GetDescriptionItem(int index)
        {
            DescriptionItem item;
            if (ItemsSource == null)
            {
                item = Items[index] as DescriptionItem;
            }
            else
            {
                item = ItemContainerGenerator.ContainerFromIndex(index) as DescriptionItem;
            }

            return item;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ChangeItemsColumnAndRow();
        }

        private void ChangeItemsColumnAndRow()
        {
            double thickness = BorderThickness.Left; // 边框厚度
            int rows = Items.Count / Columns + (Items.Count % Columns > 0 ? 1 : 0); // 总行数

            for (int i = 0; i < Items.Count; i++)
            {
                int column = i % Columns;
                int row = i / Columns;

                DescriptionItem item = GetDescriptionItem(i);

                if (item != null)
                {
                    // 设置 Grid Row 和 Column
                    Grid.SetRow(item, row);
                    Grid.SetColumn(item, column);

                    if (i == Items.Count - 1)
                    {
                        int columnSpan = Columns - column;
                        Grid.SetColumnSpan(item, columnSpan);
                    }

                    // 设置 item Border Thickness
                    if (row == rows - 1)
                    {
                        item.ContentBorderThickness = column == Columns - 1 || i == Items.Count - 1 ? new Thickness(0) : new Thickness(0, 0, thickness, 0);
                        item.DescriptionBorderThickness = new Thickness(0, 0, thickness, 0);
                    }
                    else
                    {
                        item.ContentBorderThickness = column == Columns - 1 ? new Thickness(0, 0, 0, thickness) : new Thickness(0, 0, thickness, thickness);
                        item.DescriptionBorderThickness = new Thickness(0, 0, thickness, thickness);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Rows = Items.Count / Columns + (Items.Count % Columns > 0 ? 1 : 0);
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsInitialized)
            {
                Rows = Items.Count / Columns + (Items.Count % Columns > 0 ? 1 : 0);
                ChangeItemsColumnAndRow();
            }
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            ChangeItemsColumnAndRow();

            CalculateDescriptionWidth();

            return base.MeasureOverride(constraint);
        }

        private void CalculateDescriptionWidth()
        {
            Size infinitySize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            double[] maxDescriptionWidths = Enumerable.Repeat(double.MinValue, Columns).ToArray();  // 计算宽度

            for (int i = 0; i < Items.Count; i++)
            {
                DescriptionItem item = GetDescriptionItem(i);

                if (item != null)
                {
                    // 计算每列 Description Border 的宽度
                    item.DescriptionWidth = GridLength.Auto;
                    item.Measure(infinitySize);
                    Border descriptionBorder = item.TryGetChildPartFromVisualTree<Border>("descriptionBorder");
                    if (descriptionBorder != null)
                    {
                        descriptionBorder.Measure(infinitySize); // 测量实际期望大小

                        int c = i % Columns; // 列
                        if (maxDescriptionWidths[c] < descriptionBorder.DesiredSize.Width)
                        {
                            maxDescriptionWidths[c] = descriptionBorder.DesiredSize.Width;
                        }
                    }
                }
            }

            // 设置列的 Description Border 宽度
            for (int i = 0; i < Items.Count; i++)
            {
                DescriptionItem item = GetDescriptionItem(i);
                int c = i % Columns; // 列

                if (item != null && maxDescriptionWidths.Length > c)
                {
                    item.DescriptionWidth = new GridLength(maxDescriptionWidths[c], GridUnitType.Pixel);
                }
            }
        }

        private static void OnDisplayDescriptionPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Description descriptionHost = (Description)d;

            if (descriptionHost.IsLoaded)
            {
                descriptionHost.UpdateDisplayMemberTemplateSelector();
            }
            else
            {
                descriptionHost.Loaded += (a, b) =>
                {
                    descriptionHost.UpdateDisplayMemberTemplateSelector();
                };
            }
        }

        private static void OnDescriptionStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Description descriptionHost = (Description)d;
            descriptionHost.UpdateDisplayMemberTemplateSelector();
        }

        private void UpdateDisplayMemberTemplateSelector()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                DescriptionItem item;
                if (ItemsSource == null)
                {
                    item = Items[i] as DescriptionItem;
                }
                else
                {
                    item = ItemContainerGenerator.ContainerFromIndex(i) as DescriptionItem;
                }

                if (item == null)
                {
                    return;
                }

                var descriptionContent = item.TryGetChildPartFromVisualTree<ContentControl>("descriptionContent");

                if (descriptionContent != null)
                {
                    if (!string.IsNullOrEmpty(DisplayDescriptionPath) || !string.IsNullOrEmpty(DescriptionStringFormat))
                    {
                        var contentTemplate = new ControlTemplate();
                        FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(TextBlock));
                        Binding binding2 = new Binding();
                        binding2.Path = new PropertyPath(DisplayDescriptionPath);
                        binding2.StringFormat = DescriptionStringFormat;
                        frameworkElementFactory2.SetBinding(TextBlock.TextProperty, binding2);
                        contentTemplate.VisualTree = frameworkElementFactory2;
                        contentTemplate.Seal();
                        descriptionContent.Template = contentTemplate;
                    }
                    else if (descriptionContent.Template is ControlTemplate)
                    {
                        descriptionContent.ClearValue(ContentControl.TemplateProperty);
                    }
                }
            }
        }
    }
}