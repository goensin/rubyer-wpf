using System.Collections.Specialized;
using System.Linq;
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
        /// <summary>
        /// 列集合
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
                DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(TreeListView), new PropertyMetadata(new GridViewColumnCollection()));

        static TreeListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(typeof(TreeListView)));
        }

        public TreeListView()
        {
            //Columns.CollectionChanged += Columns_CollectionChanged;
        }

        //private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    foreach (var column in Columns)
        //    {
        //        column.CellTemplate = null;
        //    }

        //    var firstColumn = Columns.FirstOrDefault();
        //    if (firstColumn != null)
        //    {
        //        firstColumn.CellTemplate = this.FindResource("RubyerTreeGridViewCellTemplate") as DataTemplate;
        //    }
        //}

        /// <summary>
        /// 列集合
        /// </summary>
        public GridViewColumnCollection Columns
        {
            get { return (GridViewColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
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


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
