using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 步骤条
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StepBarItem))]
    public class StepBar : ItemsControl
    {
        #region fields

        private int generateIndex = 0;

        #endregion fields

        #region properties

        /// <summary>
        /// 当前索引
        /// </summary>
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(StepBar), new PropertyMetadata(-1));

        /// <summary>
        ///  当前索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        /// <summary>
        /// 显示描述路径
        /// </summary>
        public static readonly DependencyProperty DisplayDescriptionPathProperty =
            DependencyProperty.Register("DisplayDescriptionPath", typeof(string), typeof(StepBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, OnDisplayDescriptionPathChanged));

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
            DependencyProperty.Register("DescriptionStringFormat", typeof(string), typeof(StepBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnDescriptionStringFormatChanged));

        /// <summary>
        /// 描述字符串格式化
        /// </summary>
        public string DescriptionStringFormat
        {
            get { return (string)GetValue(DescriptionStringFormatProperty); }
            set { SetValue(DescriptionStringFormatProperty, value); }
        }

        /// <summary>
        /// 布局方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StepBar), new FrameworkPropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// 布局方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 内容布局方向
        /// </summary>
        public static readonly DependencyProperty ContentOrientationProperty =
            DependencyProperty.Register("ContentOrientation", typeof(Orientation), typeof(StepBar), new FrameworkPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// 内容布局方向
        /// </summary>
        public Orientation ContentOrientation
        {
            get { return (Orientation)GetValue(ContentOrientationProperty); }
            set { SetValue(ContentOrientationProperty, value); }
        }

        /// <summary>
        /// 未处理颜色
        /// </summary>
        public static readonly DependencyProperty WaitingBrushProperty =
            DependencyProperty.Register("WaitingBrush", typeof(Brush), typeof(StepBar), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 未处理颜色
        /// </summary>
        public Brush WaitingBrush
        {
            get => (Brush)GetValue(WaitingBrushProperty);
            set => SetValue(WaitingBrushProperty, value);
        }

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public static readonly DependencyProperty DescriptionFontSizeProperty =
            DependencyProperty.Register("DescriptionFontSize", typeof(double), typeof(StepBar), new PropertyMetadata(12D));

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public double DescriptionFontSize
        {
            get { return (double)GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        #endregion properties

        static StepBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBar), new FrameworkPropertyMetadata(typeof(StepBar)));
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is StepBarItem stepBarItem)
            {
                var index = this.ItemContainerGenerator.Items.IndexOf(stepBarItem);
                stepBarItem.Index = index + 1;
                SetFirstOrLast(stepBarItem);
                return true;
            }

            generateIndex = this.ItemContainerGenerator.Items.IndexOf(item) + 1;
            return false;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            var stepBarItem = new StepBarItem { Index = generateIndex };
            SetFirstOrLast(stepBarItem);
            return stepBarItem;
        }

        private void SetFirstOrLast(StepBarItem stepBarItem)
        {
            if (stepBarItem.Index == 1)
            {
                stepBarItem.IsFirst = true;
            }

            if (stepBarItem.Index == this.Items.Count)
            {
                stepBarItem.IsLast = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                for (int i = 0; i < this.ItemContainerGenerator.Items.Count; i++)
                {
                    var stepBarItem = this.ItemContainerGenerator.ContainerFromIndex(i) as StepBarItem;
                    stepBarItem.IsFirst = false;
                    stepBarItem.IsLast = false;
                    stepBarItem.Index = i + 1;
                    SetFirstOrLast(stepBarItem);
                }
            }
        }

        private static void OnDisplayDescriptionPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StepBar stepBar = (StepBar)d;

            if (stepBar.IsLoaded)
            {
                stepBar.UpdateDisplayMemberTemplateSelector();
            }
            else
            {
                stepBar.Loaded += (a, b) =>
                {
                    stepBar.UpdateDisplayMemberTemplateSelector();
                };
            }
        }

        private static void OnDescriptionStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StepBar stepBar = (StepBar)d;
            stepBar.UpdateDisplayMemberTemplateSelector();
        }

        private void UpdateDisplayMemberTemplateSelector()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                StepBarItem item;
                if (ItemsSource == null)
                {
                    item = Items[i] as StepBarItem;
                }
                else
                {
                    item = ItemContainerGenerator.ContainerFromIndex(i) as StepBarItem;
                }

                if (item == null)
                {
                    return;
                }

                var textBlock = item.TryGetChildPartFromVisualTree<TextBlock>("descriptionText");

                if (textBlock != null)
                {
                    if (!string.IsNullOrEmpty(DisplayDescriptionPath) || !string.IsNullOrEmpty(DescriptionStringFormat))
                    {
                        var binding = new Binding();
                        binding.Path = new PropertyPath(DisplayDescriptionPath);
                        binding.StringFormat = DescriptionStringFormat;
                        textBlock.SetBinding(TextBlock.TextProperty, binding);
                    }
                    else if (BindingOperations.GetBinding(textBlock, TextBlock.TextProperty) != null)
                    {
                        BindingOperations.ClearBinding(textBlock, TextBlock.TextProperty);
                    }
                }
            }
        }
    }
}