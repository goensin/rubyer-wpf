using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// TreeListView  GridViewRowPresenter 行数据显示
    /// </summary>
    public class TreeGridViewRowPresenter : GridViewRowPresenter
    {
        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            
            return base.MeasureOverride(constraint);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
