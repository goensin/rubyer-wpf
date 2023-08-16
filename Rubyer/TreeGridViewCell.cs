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
    /// 树形列表单元
    /// </summary>
    public class TreeGridViewCell : ContentControl
    {
        static TreeGridViewCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeGridViewCell), new FrameworkPropertyMetadata(typeof(TreeGridViewCell)));
        }
    }
}
