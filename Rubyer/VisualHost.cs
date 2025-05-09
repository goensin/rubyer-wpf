using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace Rubyer
{
    public class VisualHost : FrameworkElement
    {
        protected override int VisualChildrenCount => _visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _visuals.Count)
                throw new ArgumentOutOfRangeException();

            return _visuals[index];
        }

        private readonly List<Visual> _visuals = new List<Visual>();

        /// <summary>
        /// 添加视觉元素
        /// </summary>
        /// <param name="visual"></param>
        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        /// <summary>
        /// 移除视觉元素
        /// </summary>
        /// <param name="visual"></param>
        public void RemoveVisual(Visual visual)
        {
            _visuals.Remove(visual);
            RemoveVisualChild(visual);
            RemoveLogicalChild(visual);
        }

        /// <summary>
        /// 清除视觉元素
        /// </summary>
        public void ClearVisuals()
        {
            foreach (var visual in _visuals)
            {
                RemoveVisualChild(visual);
                RemoveLogicalChild(visual);
            }
            _visuals.Clear();
        }
    }
}
