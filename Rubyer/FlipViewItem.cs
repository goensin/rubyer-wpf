using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 滑动视图子项
    /// </summary>
    public class FlipViewItem : ListBoxItem
    {
        static FlipViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipViewItem), new FrameworkPropertyMetadata(typeof(FlipViewItem)));
        }

        #region properties

        /// <summary>
        /// 索引
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(FlipViewItem), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///  索引
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }


        #endregion
    }
}
