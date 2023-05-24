using System.Collections.ObjectModel;
using System.Windows;

namespace Rubyer.Commons
{
    /// <summary>
    /// 归属元素的可通知集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// 所有者
        /// </summary>
        public UIElement Owner { get; }

        public ElementObservableCollection(UIElement owner)
        {
            Owner = owner;
        }
    }
}
