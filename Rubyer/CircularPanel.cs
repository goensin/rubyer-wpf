using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 圆形布局面板
    /// </summary>
    public class CircularPanel : Panel
    {
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(CircularPanel), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(CircularPanel), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(CircularPanel), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(double), typeof(CircularPanel), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty IsStretchItemsProperty =
            DependencyProperty.Register("IsStretchItems", typeof(bool), typeof(CircularPanel), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange));

        internal static readonly DependencyPropertyKey ItemSizePropertyKey =
            DependencyProperty.RegisterReadOnly("ItemSize", typeof(Size), typeof(Description), new PropertyMetadata(new Size()));

        /// <summary>
        /// 子项大小
        /// </summary>
        public static readonly DependencyProperty ItemSizeProperty = ItemSizePropertyKey.DependencyProperty;

        /// <summary>
        /// 布局起始角度
        /// </summary>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        /// <summary>
        /// 边框厚度
        /// </summary>
        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// 内边距
        /// </summary>
        public double Padding
        {
            get => (double)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// 是否拉伸子项控件
        /// 使子项大小拉伸为等分扇形内切圆的直径
        /// </summary>
        public bool IsStretchChildren
        {
            get => (bool)GetValue(IsStretchItemsProperty);
            set => SetValue(IsStretchItemsProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 子项大小
        /// </summary>
        public Size ItemSize
        {
            get { return (Size)GetValue(ItemSizeProperty); }
            private set { SetValue(ItemSizePropertyKey, value); }
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            double maxChildSize = Math.Min(availableSize.Width, availableSize.Height);

            foreach (UIElement child in Children)
            {
                child.Measure(new Size(maxChildSize, maxChildSize));
            }

            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count > 1)
            {
                double angleIncrement = 360.0 / Children.Count; // 扇形角度
                double bigCircleRadius = Math.Min(finalSize.Width - BorderThickness - Padding, finalSize.Height - BorderThickness - Padding) / 2; // 大圆半径
                var centralAngleRadians = angleIncrement * Math.PI / 180.0; // 弧度
                var radius = bigCircleRadius * (Math.Sin(centralAngleRadians / 2) / (1 + Math.Sin(centralAngleRadians / 2))); // 内切圆半径
                double diameter = radius * 2;     // 内切圆直径
                if (radius >= 0)
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        UIElement child = Children[i];

                        ItemSize = IsStretchChildren ? new(diameter, diameter) : child.DesiredSize; // 实际子项大小

                        // 计算每个子项所在位置
                        double angle = i * angleIncrement + StartAngle;
                        double radians = angle * (Math.PI / 180.0);
                        double x = (finalSize.Width - ItemSize.Width) / 2 + ((bigCircleRadius - ItemSize.Width / 2) * Math.Cos(radians));
                        double y = (finalSize.Height - ItemSize.Height) / 2 + ((bigCircleRadius - ItemSize.Height / 2) * Math.Sin(radians));

                        child.Arrange(new Rect(new Point(x, y), ItemSize));
                    }
                }
            }
            else if (Children.Count == 1)
            {
                Children[0].Arrange(new Rect(new Point(0, 0), finalSize));
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext dc)
        {
            var pen = new Pen(BorderBrush, BorderThickness);
            var center = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
            double radius = Math.Min(RenderSize.Width, RenderSize.Height) / 2;
            dc.DrawEllipse(Background, pen, center, radius, radius);
        }
    }
}
