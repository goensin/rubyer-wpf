using System.Windows;
using System.Windows.Controls;

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
        /// 是否第一个
        /// </summary>
        public static readonly DependencyProperty IsFirstProperty =
            DependencyProperty.Register("IsFirst", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否第一个
        /// </summary>
        public bool IsFirst
        {
            get { return (bool)GetValue(IsFirstProperty); }
            set { SetValue(IsFirstProperty, value); }
        }

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public static readonly DependencyProperty IsLastProperty =
            DependencyProperty.Register("IsLast", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public bool IsLast
        {
            get { return (bool)GetValue(IsLastProperty); }
            set { SetValue(IsLastProperty, value); }
        }

        /// <summary>
        /// 是否等待状态
        /// </summary>
        public static readonly DependencyProperty IsWaitingProperty =
            DependencyProperty.Register("IsWaiting", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否等待状态
        /// </summary>
        public bool IsWaiting
        {
            get { return (bool)GetValue(IsWaitingProperty); }
            set { SetValue(IsWaitingProperty, value); }
        }

        /// <summary>
        /// 是否进行中状态
        /// </summary>
        public static readonly DependencyProperty IsUnderwayProperty =
            DependencyProperty.Register("IsUnderway", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否进行中状态
        /// </summary>
        public bool IsUnderway
        {
            get { return (bool)GetValue(IsUnderwayProperty); }
            set { SetValue(IsUnderwayProperty, value); }
        }

        /// <summary>
        /// 是否已完成状态
        /// </summary>
        public static readonly DependencyProperty IsCompletedProperty =
            DependencyProperty.Register("IsCompleted", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否已完成状态
        /// </summary>
        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, value); }
        }

        /// <summary>
        /// 是否结束状态
        /// </summary>
        public static readonly DependencyProperty IsFinishedProperty =
            DependencyProperty.Register("IsFinished", typeof(bool), typeof(StepBarItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否结束成状态
        /// </summary>
        public bool IsFinished
        {
            get { return (bool)GetValue(IsFinishedProperty); }
            set { SetValue(IsFinishedProperty, value); }
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