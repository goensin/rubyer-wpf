using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// TreeListView  GridViewRowPresenter 行数据显示
    /// </summary>
    public class TreeGridViewRowPresenter : GridViewRowPresenter
    {
        /// <summary>
        /// 首列偏移大小
        /// </summary>
        public static readonly DependencyProperty FirstColumnOffsetSizeProperty =
                DependencyProperty.Register("FirstColumnOffsetSize", typeof(double), typeof(TreeGridViewRowPresenter), new PropertyMetadata(default(double)));

        /// <summary>
        /// 列集合
        /// </summary>
        public double FirstColumnOffsetSize
        {
            get { return (double)GetValue(FirstColumnOffsetSizeProperty); }
            set { SetValue(FirstColumnOffsetSizeProperty, value); }
        }

        /// <summary>
        /// 子项内边距
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty =
                DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(TreeGridViewRowPresenter), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// 子项内边距
        /// </summary>
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            var horPadding = ItemPadding.Left + ItemPadding.Right;
            if (size.Width >= FirstColumnOffsetSize + horPadding)
            {
                size.Width -= FirstColumnOffsetSize + horPadding;
            }

            return size;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            GridViewColumnCollection columns = base.Columns;
            if (columns == null)
            {
                return arrangeSize;
            }

            List<UIElement> internalChildren = new List<UIElement>();
            var count = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < count; i++)
            {
                if (VisualTreeHelper.GetChild(this, i) is FrameworkElement element)
                {
                    internalChildren.Add(element);
                }
            }

            double num = 0.0;
            double num2 = arrangeSize.Width;
            int index = 0;
            foreach (GridViewColumn item in columns)
            {
                // 实在拿不到 Column 几个属性，需要使用反射
                var columnType = item.GetType();
                var actualIndexPropertyInfo = columnType.GetProperty("ActualIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var statePropertyInfo = columnType.GetProperty("State", BindingFlags.Instance | BindingFlags.NonPublic);
                var desiredWidthPropertyInfo = columnType.GetProperty("DesiredWidth", BindingFlags.Instance | BindingFlags.NonPublic);

                var actualIndex = (int)actualIndexPropertyInfo.GetValue(item);
                var state = (int)statePropertyInfo.GetValue(item);
                var desiredWidth = (double)desiredWidthPropertyInfo.GetValue(item);

                UIElement uIElement = internalChildren[actualIndex];
                if (uIElement != null)
                {
                    double num3 = Math.Min(num2, (state == 3) ? item.Width : desiredWidth);

                    if (index++ == 0)
                    {
                        num3 -= FirstColumnOffsetSize;
                    }

                    if (num3 < 0)
                    {
                        num3 = 0;
                    }
                    uIElement.Arrange(new Rect(num, 0.0, num3, arrangeSize.Height));
                    num2 -= num3;
                    num += num3;
                }
            }

            return arrangeSize;
        }
    }
}
