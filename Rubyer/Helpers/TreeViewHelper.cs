using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// TreeView 帮助类
    /// </summary>
    public class TreeViewHelper
    {
        /// <summary>
        /// 展开图标类型
        /// </summary>
        public static readonly DependencyProperty ExpandIconTypeProperty = DependencyProperty.RegisterAttached(
            "ExpandIconType", typeof(IconType), typeof(TreeViewHelper), new PropertyMetadata(IconType.ArrowRightSFill));

        public static void SetExpandIconType(DependencyObject element, IconType value)
        {
            element.SetValue(ExpandIconTypeProperty, value);
        }

        public static IconType GetExpandIconType(DependencyObject element)
        {
            return (IconType)element.GetValue(ExpandIconTypeProperty);
        }

        /// <summary>
        /// 选中时图标颜色
        /// </summary>
        public static readonly DependencyProperty IconFocusedBrushProperty = DependencyProperty.RegisterAttached(
            "IconFocusedBrush", typeof(Brush), typeof(TreeViewHelper), new PropertyMetadata(default(Brush)));

        public static void SetIconFocusedBrush(DependencyObject element, Brush value)
        {
            element.SetValue(IconFocusedBrushProperty, value);
        }

        public static Brush GetIconFocusedBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(IconFocusedBrushProperty);
        }

        /// <summary>
        /// 右键选中子项
        /// </summary>
        public static readonly DependencyProperty RightClickToSelectedProperty = DependencyProperty.RegisterAttached(
            "RightClickToSelected", typeof(bool), typeof(TreeViewHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnRightClickToSelectedChanged));

        public static void SetRightClickToSelected(DependencyObject element, bool value)
        {
            element.SetValue(RightClickToSelectedProperty, BooleanBoxes.Box(value));
        }

        public static bool GetRightClickToSelected(DependencyObject element)
        {
            return (bool)element.GetValue(RightClickToSelectedProperty);
        }

        private static void TreeView_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject dependencyObject)
            {
                var treeViewItem = dependencyObject.TryGetParentFromVisualTree<TreeViewItem>();
                if (treeViewItem != null)
                {
                    treeViewItem.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void OnRightClickToSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeView treeView)
            {
                if (GetRightClickToSelected(treeView))
                {
                    treeView.PreviewMouseRightButtonDown += TreeView_PreviewMouseRightButtonDown;
                }
                else
                {
                    treeView.PreviewMouseRightButtonDown -= TreeView_PreviewMouseRightButtonDown;
                }
            }
        }

        #region 定位到TreeViewItem方法

        /// <summary>
        /// 根据 DataContext 找到 TreeViewItem
        /// </summary>
        /// <param name="container">集合控件</param>
        /// <param name="item">子项 DataContext</param>
        /// <returns>TreeViewItem</returns>
        public static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if (container is TreeViewItem treeViewItem && !treeViewItem.IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.

                container.ApplyTemplate();

                ItemsPresenter itemsPresenter = (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();

                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    if (itemsHostPanel is RubyerVirtualizingStackPanel virtualizingPanel)
                    {
                        // Bring the item into view so
                        // that the container will be generated.
                        virtualizingPanel.BringIntoView(i);

                        subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                    }
                    else
                    {
                        subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);

                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        subContainer.BringIntoView();
                    }

                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 找到可视化子项
        /// </summary>
        /// <typeparam name="T">子项类型</typeparam>
        /// <param name="visual">可视化对象</param>
        /// <returns>可视化子项</returns>
        public static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    if (child is T correctlyTyped)
                    {
                        return correctlyTyped;
                    }

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
