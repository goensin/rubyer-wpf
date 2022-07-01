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
    public class DataGridSelectCheckBoxColumn : DataGridCheckBoxColumn
    {
        static DataGridSelectCheckBoxColumn()
        {
            var elementStyle = Application.Current.FindResource("RubyerDataGridCheckBoxColumnEditting");
            DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridSelectCheckBoxColumn), new FrameworkPropertyMetadata(elementStyle));
            DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridSelectCheckBoxColumn), new FrameworkPropertyMetadata(elementStyle));
        }

        public DataGridSelectCheckBoxColumn()
        {
            var headStyle = (Style)Application.Current.FindResource("DataGridSelectCheckBoxColumnHeader");
            HeaderStyle = headStyle;
            CanUserSort = false;
        }
    }
}
