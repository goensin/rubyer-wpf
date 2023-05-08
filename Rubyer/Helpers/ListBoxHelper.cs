using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// ListBox 帮助类
    /// </summary>
    public static class ListBoxHelper
    {
        /// <summary>
        /// 小滚动条
        /// </summary>
        public static readonly DependencyProperty IsShowLittleBarProperty =
            DependencyProperty.RegisterAttached("IsShowLittleBar", typeof(bool), typeof(ListBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Gets the is ShowLittleBar.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsShowLittleBar(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowLittleBarProperty);
        }

        /// <summary>
        /// Sets the is ShowLittleBar.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsShowLittleBar(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowLittleBarProperty, BooleanBoxes.Box(value));
        }
    }
}