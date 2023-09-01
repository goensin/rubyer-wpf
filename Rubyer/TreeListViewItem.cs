using Rubyer.Commons.KnownBoxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// 树形列表项
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeListViewItem))]
    public class TreeListViewItem : TreeViewItem
    {
        /// <summary>
        /// 加载中
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(TreeListViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 加载中
        /// </summary>
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, BooleanBoxes.Box(value));
        }

        static TreeListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListViewItem), new FrameworkPropertyMetadata(typeof(TreeListViewItem)));
        }

        public TreeListViewItem()
        {
            Expanded += TreeListViewItem_Expanded;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ExpanderToggleButton") is ToggleButton expendButton)
            {
                expendButton.PreviewMouseDown += ExpendButton_MouseDown;
                //expendButton.Click += ExpendButton_Click;
            }
        }

        private void ExpendButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;

            IsLoading = toggleButton.IsChecked.GetValueOrDefault(false);
        }

        private void TreeListViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            IsLoading = false;
        }

        private void ExpendButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsExpanded)
            {
                IsLoading = true;
                UpdateLayout();
            }
        }


        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
        }
    }
}
