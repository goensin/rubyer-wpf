using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// MenuItem 集合
    /// </summary>
    public class MenuItemCollection : FreezableCollection<Control>
    {
        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new MenuItemCollection();
        }
    }
}