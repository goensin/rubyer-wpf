using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Rubyer
{
    /// <summary>
    /// DataGrid 选择列
    /// </summary>
    public class DataGridSelectCheckBoxColumn : DataGridCheckBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridSelectCheckBoxColumn"/> class.
        /// </summary>
        static DataGridSelectCheckBoxColumn()
        {
            var elementStyle = Application.Current.FindResource("RubyerDataGridCheckBoxColumnEditting");
            DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridSelectCheckBoxColumn), new FrameworkPropertyMetadata(elementStyle));
            DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridSelectCheckBoxColumn), new FrameworkPropertyMetadata(elementStyle));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridSelectCheckBoxColumn"/> class.
        /// </summary>
        public DataGridSelectCheckBoxColumn()
        {
            var headStyle = (Style)Application.Current.FindResource("DataGridSelectCheckBoxColumnHeader");
            HeaderStyle = headStyle;
            CanUserSort = false;
        }
    }
}