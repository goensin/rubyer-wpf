using System;
using System.Windows.Controls;

namespace Rubyer.DataAnnotations
{
    /// <summary>
    /// DataGrid 列宽
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnWidthAttribute : Attribute
    {
        /// <summary>
        /// 列宽
        /// </summary>
        public DataGridLength Width { get; }

        /// <summary>
        /// DataGrid 列宽
        /// </summary>
        /// <param name="width">列宽</param>
        public ColumnWidthAttribute(string width)
        {
            var converter = new DataGridLengthConverter();
            Width = (DataGridLength)converter.ConvertFromString(width);
        }
    }
}
