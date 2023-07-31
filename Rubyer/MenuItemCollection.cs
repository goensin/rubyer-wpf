using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// The MenuItemCollection provides typed collection of HamburgerMenuItemBase.
    /// form https://github.com/MahApps/MahApps.Metro
    /// MahApps.Metro.Controls.HamburgerMenu
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