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
    /// TreeListView  GridViewRowPresenter 行数据显示
    /// </summary>
    public class TreeGridViewRowPresenter : GridViewRowPresenter
    {


        /// <inheritdoc/>
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    GridViewColumnCollection columns = base.Columns;
        //    if (columns == null)
        //    {
        //        return default(Size);
        //    }

        //    var column = columns.FirstOrDefault();
        //    var element = GetVisualChild(0) as FrameworkElement;
        //    //element.Width = 20;
        //    var size = base.MeasureOverride(constraint);
        //    size.Width -= size.Width / 2;
        //    return size;
        //}

    }
}
