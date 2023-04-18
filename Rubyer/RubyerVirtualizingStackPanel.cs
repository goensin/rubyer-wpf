using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 公开 BringIntoView() 的 VirtualizingStackPanel
    /// 当使用 TreeViewHelper.RightClickToSelected 需要使用该虚拟化 StackPanel 作为 ItemsPanel
    /// </summary>
    public class RubyerVirtualizingStackPanel : VirtualizingStackPanel
    {
        /// <summary>
        /// Publically expose BringIndexIntoView.
        /// </summary>
        public void BringIntoView(int index)
        {
            this.BringIndexIntoView(index);
        }
    }
}
