using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// The HamburgerMenuItemCollection provides typed collection of HamburgerMenuItemBase.
    /// form https://github.com/MahApps/MahApps.Metro
    /// MahApps.Metro.Controls.HamburgerMenu
    /// </summary>
    public class HamburgerMenuItemCollection : FreezableCollection<HamburgerMenuOptionsItem>
    {
        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new HamburgerMenuItemCollection();
        }
    }
}
