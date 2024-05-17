using Rubyer.Commons.KnownBoxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 步骤条子项
    /// </summary>
    [TemplateVisualState(Name = StateWaiting, GroupName = "ViewStates")]
    [TemplateVisualState(Name = StateUnderway, GroupName = "ViewStates")]
    [TemplateVisualState(Name = StateCompleted, GroupName = "ViewStates")]
    [TemplateVisualState(Name = StateFinished, GroupName = "ViewStates")]
    public class StepBarItem : ContentControl
    {
        /// <summary>
        /// 等待中状态
        /// </summary>
        public const string StateWaiting = "Waiting";

        /// <summary>
        /// 进行中状态
        /// </summary>
        public const string StateUnderway = "Underway";

        /// <summary>
        /// 完成状态
        /// </summary>
        public const string StateCompleted = "Completed";

        /// <summary>
        /// 结束状态
        /// </summary>
        public const string StateFinished = "Finished";

        #region properties

        /// <summary>
        /// 索引
        /// </summary>
        internal static readonly DependencyPropertyKey IndexPropertyKey =
            DependencyProperty.RegisterReadOnly("Index", typeof(int), typeof(StepBarItem), new PropertyMetadata(-1));

        /// <summary>
        /// 索引
        /// </summary>
        public static readonly DependencyProperty IndexProperty = IndexPropertyKey.DependencyProperty;

        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            internal set { SetValue(IndexPropertyKey, value); }
        }

        /// <summary>
        /// StepBar 当前索引
        /// </summary>
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(StepBarItem), new PropertyMetadata(-1, OnCurrentIndexChanged));

        /// <summary>
        /// StepBar 当前索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            internal set { SetValue(CurrentIndexProperty, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(StepBarItem), new PropertyMetadata(default(string)));

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public static readonly DependencyProperty DescriptionFontSizeProperty =
            DependencyProperty.Register("DescriptionFontSize", typeof(double), typeof(StepBarItem), new PropertyMetadata(12D));

        /// <summary>
        /// 描述字体大小
        /// </summary>
        public double DescriptionFontSize
        {
            get { return (double)GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        /// <summary>
        /// 是否第一个
        /// </summary>
        public static readonly DependencyProperty IsFirstProperty =
            DependencyProperty.Register("IsFirst", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否第一个
        /// </summary>
        public bool IsFirst
        {
            get { return (bool)GetValue(IsFirstProperty); }
            set { SetValue(IsFirstProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public static readonly DependencyProperty IsLastProperty =
            DependencyProperty.Register("IsLast", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public bool IsLast
        {
            get { return (bool)GetValue(IsLastProperty); }
            set { SetValue(IsLastProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否等待状态
        /// </summary>
        public static readonly DependencyProperty IsWaitingProperty =
            DependencyProperty.Register("IsWaiting", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否等待状态
        /// </summary>
        public bool IsWaiting
        {
            get { return (bool)GetValue(IsWaitingProperty); }
            set { SetValue(IsWaitingProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否进行中状态
        /// </summary>
        public static readonly DependencyProperty IsUnderwayProperty =
            DependencyProperty.Register("IsUnderway", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否进行中状态
        /// </summary>
        public bool IsUnderway
        {
            get { return (bool)GetValue(IsUnderwayProperty); }
            set { SetValue(IsUnderwayProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否已完成状态
        /// </summary>
        public static readonly DependencyProperty IsCompletedProperty =
            DependencyProperty.Register("IsCompleted", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否已完成状态
        /// </summary>
        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否结束状态
        /// </summary>
        public static readonly DependencyProperty IsFinishedProperty =
            DependencyProperty.Register("IsFinished", typeof(bool), typeof(StepBarItem), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否结束成状态
        /// </summary>
        public bool IsFinished
        {
            get { return (bool)GetValue(IsFinishedProperty); }
            set { SetValue(IsFinishedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否结束状态
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType", typeof(IconType?), typeof(StepBarItem), new PropertyMetadata(default(IconType?)));

        /// <summary>
        /// 是否结束成状态
        /// </summary>
        public IconType? IconType
        {
            get { return (IconType?)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        public static readonly DependencyProperty WaitingBrushProperty =
           DependencyProperty.Register("WaitingBrush", typeof(Brush), typeof(StepBarItem), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 未处理颜色
        /// </summary>
        public Brush WaitingBrush
        {
            get => (Brush)GetValue(WaitingBrushProperty);
            set => SetValue(WaitingBrushProperty, value);
        }

        /// <summary>
        /// 内容布局方向
        /// </summary>
        public static readonly DependencyProperty ContentOrientationProperty =
            DependencyProperty.Register("ContentOrientation", typeof(Orientation), typeof(StepBarItem), new FrameworkPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// 内容布局方向
        /// </summary>
        public Orientation ContentOrientation
        {
            get { return (Orientation)GetValue(ContentOrientationProperty); }
            set { SetValue(ContentOrientationProperty, value); }
        }

        #endregion properties

        static StepBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBarItem), new FrameworkPropertyMetadata(typeof(StepBarItem)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ChangeVisualState(this);
        }

        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stepBarItem = d as StepBarItem;

            ChangeVisualState(stepBarItem);
        }

        private static void ChangeVisualState(StepBarItem stepBarItem)
        {
            if (stepBarItem.CurrentIndex < stepBarItem.Index - 1)
            {
                VisualStateManager.GoToState(stepBarItem, StateWaiting, useTransitions: false);
                stepBarItem.IsWaiting = true;
                stepBarItem.IsUnderway = false;
                stepBarItem.IsCompleted = false;
                stepBarItem.IsFinished = false;
            }
            else if (stepBarItem.CurrentIndex == stepBarItem.Index - 1)
            {
                VisualStateManager.GoToState(stepBarItem, StateUnderway, useTransitions: false);
                stepBarItem.IsWaiting = false;
                stepBarItem.IsUnderway = true;
                stepBarItem.IsCompleted = false;
                stepBarItem.IsFinished = false;
            }
            else if (stepBarItem.CurrentIndex == stepBarItem.Index)
            {
                VisualStateManager.GoToState(stepBarItem, StateCompleted, useTransitions: false);
                stepBarItem.IsWaiting = false;
                stepBarItem.IsUnderway = false;
                stepBarItem.IsCompleted = true;
                stepBarItem.IsFinished = false;
            }
            else
            {
                VisualStateManager.GoToState(stepBarItem, StateFinished, useTransitions: false);
                stepBarItem.IsWaiting = false;
                stepBarItem.IsUnderway = false;
                stepBarItem.IsCompleted = false;
                stepBarItem.IsFinished = true;
            }
        }
    }
}