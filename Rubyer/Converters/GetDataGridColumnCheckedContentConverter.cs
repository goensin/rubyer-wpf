using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Rubyer.Converters
{
    /// <summary>
    /// 获取 DataGridColumn 的 CheckedContent 属性
    /// </summary>
    public class GetDataGridColumnCheckedContentConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                throw new ArgumentException("the values length can't be less the 2.");
            }

            var dataGrid = ValidateArgument.NotNullOrEmptyCast<DataGrid>(values[0], "values[0]");
            var columnHeader = ValidateArgument.NotNullOrEmptyCast<DataGridColumnHeader>(values[1], "values[1]");
            foreach (var column in dataGrid.Columns)
            {
                if (columnHeader.Column == column)
                {
                    return ToggleButtonHelper.GetCheckedContent(column) ?? column.Header;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}