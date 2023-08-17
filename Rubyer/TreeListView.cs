using Rubyer.Commons.KnownBoxes;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 树形列表
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeListViewItem))]
    public class TreeListView : TreeView
    {
        public static readonly DependencyProperty ColumnHeaderContainerStyleProperty =
            DependencyProperty.Register("ColumnHeaderContainerStyle", typeof(Style), typeof(TreeListView));

        public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty =
            DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(TreeListView), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ColumnHeaderStringFormatProperty =
            DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(TreeListView));

        public static readonly DependencyProperty AllowsColumnReorderProperty =
            DependencyProperty.Register("AllowsColumnReorder", typeof(bool), typeof(TreeListView), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

        public static readonly DependencyProperty ColumnHeaderContextMenuProperty =
            DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(TreeListView));

        public static readonly DependencyProperty ColumnHeaderToolTipProperty =
            DependencyProperty.Register("ColumnHeaderToolTip", typeof(object), typeof(TreeListView));

        /// <summary>
        /// 列集合
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(TreeListView), new PropertyMetadata(null));

        /// <summary>
        /// 网格线可见性
        /// </summary>
        public static readonly DependencyProperty GridLinesVisibilityProperty =
            DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(TreeListView), new PropertyMetadata(DataGridGridLinesVisibility.None));

        static TreeListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(typeof(TreeListView)));
        }

        public Style ColumnHeaderContainerStyle
        {
            get => (Style)GetValue(ColumnHeaderContainerStyleProperty);
            set => SetValue(ColumnHeaderContainerStyleProperty, value);
        }

        public DataTemplateSelector ColumnHeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ColumnHeaderTemplateSelectorProperty);
            set => SetValue(ColumnHeaderTemplateSelectorProperty, value);
        }

        public string ColumnHeaderStringFormat
        {
            get => (string)GetValue(ColumnHeaderStringFormatProperty);
            set => SetValue(ColumnHeaderStringFormatProperty, value);
        }

        public bool AllowsColumnReorder
        {
            get => (bool)GetValue(AllowsColumnReorderProperty);
            set => SetValue(AllowsColumnReorderProperty, value);
        }

        public ContextMenu ColumnHeaderContextMenu
        {
            get => (ContextMenu)GetValue(ColumnHeaderContextMenuProperty);
            set => SetValue(ColumnHeaderContextMenuProperty, value);
        }

        public object ColumnHeaderToolTip
        {
            get => GetValue(ColumnHeaderToolTipProperty);
            set => SetValue(ColumnHeaderToolTipProperty, value);
        }

        /// <summary>
        /// 列集合
        /// </summary>
        public GridViewColumnCollection Columns
        {
            get { return (GridViewColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        ///  网格线可见性
        /// </summary>
        public DataGridGridLinesVisibility GridLinesVisibility
        {
            get { return (DataGridGridLinesVisibility)GetValue(GridLinesVisibilityProperty); }
            set { SetValue(GridLinesVisibilityProperty, value); }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TreeListView()
        {
            Columns = new GridViewColumnCollection();
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }
    }
}
