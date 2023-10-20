using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Rubyer
{
    /// <summary>
    /// 垂直排列 UniformGrid
    /// </summary>
    public class VerticalUniformGrid : UniformGrid
    {
        private int _rows;

        private int _columns;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            UpdateComputedValues();
            Size availableSize = new Size(constraint.Width / (double)_columns, constraint.Height / (double)_rows);
            double num = 0.0;
            double num2 = 0.0;
            int i = 0;
            for (int count = base.InternalChildren.Count; i < count; i++)
            {
                UIElement uIElement = base.InternalChildren[i];
                uIElement.Measure(availableSize);
                Size desiredSize = uIElement.DesiredSize;
                if (num < desiredSize.Width)
                {
                    num = desiredSize.Width;
                }

                if (num2 < desiredSize.Height)
                {
                    num2 = desiredSize.Height;
                }
            }

            return new Size(num * (double)_columns, num2 * (double)_rows);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Rect finalRect = new Rect(0.0, 0.0, arrangeSize.Width / (double)_columns, arrangeSize.Height / (double)_rows);
            double num = arrangeSize.Height - 1.0;
            finalRect.X += finalRect.Width * (double)FirstColumn;
            foreach (UIElement internalChild in base.InternalChildren)
            {
                internalChild.Arrange(finalRect);
                if (internalChild.Visibility != Visibility.Collapsed)
                {
                    finalRect.Y += finalRect.Height;
                    if (finalRect.Y >= num)
                    {
                        finalRect.X += finalRect.Width;
                        finalRect.Y = 0.0;
                    }
                }
            }

            return arrangeSize;
        }

        private void UpdateComputedValues()
        {
            _columns = Columns;
            _rows = Rows;
            if (FirstColumn >= _columns)
            {
                FirstColumn = 0;
            }

            if (_rows != 0 && _columns != 0)
            {
                return;
            }

            int num = 0;
            int i = 0;
            for (int count = base.InternalChildren.Count; i < count; i++)
            {
                UIElement uIElement = base.InternalChildren[i];
                if (uIElement.Visibility != Visibility.Collapsed)
                {
                    num++;
                }
            }

            if (num == 0)
            {
                num = 1;
            }

            if (_rows == 0)
            {
                if (_columns > 0)
                {
                    _rows = (num + FirstColumn + (_columns - 1)) / _columns;
                    return;
                }

                _rows = (int)Math.Sqrt(num);
                if (_rows * _rows < num)
                {
                    _rows++;
                }

                _columns = _rows;
            }
            else if (_columns == 0)
            {
                _columns = (num + (_rows - 1)) / _rows;
            }
        }
    }
}
