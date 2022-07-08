using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// items
    /// </summary>
    public class ItemsControlHelper
    {
        /// <summary>
        /// item margin
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.RegisterAttached(
            "ItemMargin", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// Gets the item margin.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Thickness.</returns>
        public static Thickness GetItemMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ItemMarginProperty);
        }

        /// <summary>
        /// Sets the item margin.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetItemMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// item padding
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.RegisterAttached(
            "ItemPadding", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// Gets the item padding.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Thickness.</returns>
        public static Thickness GetItemPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ItemPaddingProperty);
        }

        /// <summary>
        /// Sets the item padding.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetItemPadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(ItemPaddingProperty, value);
        }
    }
}