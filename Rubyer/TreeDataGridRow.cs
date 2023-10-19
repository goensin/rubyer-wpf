using Rubyer.Commons.KnownBoxes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 树形数据表格行
    /// </summary>
    public class TreeDataGridRow : DataGridRow
    {
        #region 路由事件

        /// <summary>
        /// 展开事件
        /// </summary>
        public static readonly RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent(
            "Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridRow));

        /// <summary>
        /// 展开事件
        /// </summary>
        public event RoutedEventHandler Expanded
        {
            add { AddHandler(ExpandedEvent, value); }
            remove { RemoveHandler(ExpandedEvent, value); }
        }

        /// <summary>
        /// 展开
        /// </summary>
        public static readonly RoutedEvent CollapsedEvent = EventManager.RegisterRoutedEvent(
            "Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridRow));

        /// <summary>
        /// 展开
        /// </summary>
        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(CollapsedEvent, value); }
            remove { RemoveHandler(CollapsedEvent, value); }
        }

        #endregion

        #region 依赖属性

        /// <summary>
        /// 是否展开
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeDataGridRow), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsExpandedChanged));

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 表格行父级 Key
        /// </summary>
        internal static readonly DependencyPropertyKey ParentRowPropertyKey =
            DependencyProperty.RegisterReadOnly("ParentRow", typeof(TreeDataGridRow), typeof(TreeDataGridRow), new PropertyMetadata(null));

        /// <summary>
        /// 表格行父级
        /// </summary>
        public static readonly DependencyProperty ParentRowProperty = ParentRowPropertyKey.DependencyProperty;

        /// <summary>
        /// 表格行父级
        /// </summary>
        public TreeDataGridRow ParentRow
        {
            get { return (TreeDataGridRow)GetValue(ParentRowProperty); }
            internal set { SetValue(ParentRowPropertyKey, value); }
        }

        /// <summary>
        /// 是否有行子项 Key
        /// </summary>
        internal static readonly DependencyPropertyKey HasChildRowPropertyKey =
            DependencyProperty.RegisterReadOnly("HasChildRow", typeof(bool), typeof(TreeDataGridRow), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否有行子项
        /// </summary>
        public static readonly DependencyProperty HasChildRowProperty = HasChildRowPropertyKey.DependencyProperty;

        /// <summary>
        /// 是否有行子项
        /// </summary>
        public bool HasChildRow
        {
            get { return (bool)GetValue(HasChildRowProperty); }
            internal set { SetValue(HasChildRowPropertyKey, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 行子项集合 Key
        /// </summary>
        internal static readonly DependencyPropertyKey ChildRowsPropertyKey =
            DependencyProperty.RegisterReadOnly("ChildRows", typeof(IEnumerable<object>), typeof(TreeDataGridRow), new PropertyMetadata(null));

        /// <summary>
        /// 行子项集合
        /// </summary>
        public static readonly DependencyProperty ChildRowsProperty = ChildRowsPropertyKey.DependencyProperty;

        /// <summary>
        /// 行子项集合
        /// </summary>
        public IEnumerable<object> ChildRows
        {
            get { return (IEnumerable<object>)GetValue(ChildRowsProperty); }
            internal set { SetValue(ChildRowsPropertyKey, value); }
        }

        /// <summary>
        /// 所处节点等级 Key
        /// </summary>
        internal static readonly DependencyPropertyKey NodeLevelPropertyKey =
            DependencyProperty.RegisterReadOnly("NodeLevel", typeof(int), typeof(TreeDataGridRow), new PropertyMetadata(0));

        /// <summary>
        /// 所处节点等级
        /// </summary>
        public static readonly DependencyProperty NodeLevelProperty = NodeLevelPropertyKey.DependencyProperty;

        /// <summary>
        /// 所处节点等级
        /// </summary>
        public int NodeLevel
        {
            get { return (int)GetValue(NodeLevelProperty); }
            internal set { SetValue(NodeLevelPropertyKey, value); }
        }

        #endregion

        static TreeDataGridRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeDataGridRow), new FrameworkPropertyMetadata(typeof(TreeDataGridRow)));
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (TreeDataGridRow)d;
            if (row.IsExpanded)
            {
                var args = new RoutedEventArgs();
                args.RoutedEvent = ExpandedEvent;
                row.RaiseEvent(args);
            }
            else
            {
                var args = new RoutedEventArgs();
                args.RoutedEvent = CollapsedEvent;
                row.RaiseEvent(args);
            }
        }

    }
}
