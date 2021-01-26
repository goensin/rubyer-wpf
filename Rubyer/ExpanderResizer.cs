using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class ExpanderResizer : ContentControl
    {
        private bool isWidthAnimationing;
        private bool isHeightAnimationing;

        static ExpanderResizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderResizer), new FrameworkPropertyMetadata(typeof(ExpanderResizer)));
        }

        public Expander ExpanderControl
        {
            get { return (Expander)GetValue(ExpanderControlProperty); }
            set { SetValue(ExpanderControlProperty, value); }
        }

        public static readonly DependencyProperty ExpanderControlProperty =
            DependencyProperty.Register("ExpanderControl", typeof(Expander), typeof(ExpanderResizer), new PropertyMetadata(null));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (ExpanderControl != null)
            {
                if (!isWidthAnimationing && !isHeightAnimationing)
                {
                    if (this.Content is UIElement element)
                    {
                        // 垂直方向
                        if (ExpanderControl.ExpandDirection == ExpandDirection.Down || ExpanderControl.ExpandDirection == ExpandDirection.Up)
                        {
                            if ((!ExpanderControl.IsExpanded && constraint.Height != 0)
                                || (ExpanderControl.IsExpanded && (ActualHeight == 0 || ActualHeight != element.DesiredSize.Height)))
                            {
                                isHeightAnimationing = true;
                                element.Measure(new Size(constraint.Width, double.PositiveInfinity));

                                DoubleAnimation heightAnimation = new DoubleAnimation
                                {
                                    From = ActualHeight,
                                    To = ExpanderControl.IsExpanded ? element.DesiredSize.Height : 0,
                                    Duration = TimeSpan.FromMilliseconds(250),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                heightAnimation.Completed += (sender, e) =>
                                {
                                    isHeightAnimationing = false;
                                };

                                this.BeginAnimation(HeightProperty, heightAnimation);
                            }
                        }
                        // 水平方向
                        else
                        {
                            if ((!ExpanderControl.IsExpanded && constraint.Width != 0)
                            || (ExpanderControl.IsExpanded && (ActualWidth == 0 || ActualWidth != element.DesiredSize.Width)))
                            {
                                isWidthAnimationing = true;

                                Panel panel = VisualTreeHelper.GetParent(this) as Panel;
                                Control control = VisualTreeHelper.GetChild(panel, 0) as Control;

                                element.Measure(new Size(ExpanderControl.MaxWidth - control.ActualWidth, constraint.Height));

                                DoubleAnimation widthAnimation = new DoubleAnimation
                                {
                                    From = ActualWidth,
                                    To = ExpanderControl.IsExpanded ? element.DesiredSize.Width : 0,
                                    Duration = TimeSpan.FromMilliseconds(250),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                widthAnimation.Completed += (sender, e) =>
                                {
                                    isWidthAnimationing = false;
                                };

                                this.BeginAnimation(WidthProperty, widthAnimation);
                            }
                        }
                    }
                }
            }

            return base.MeasureOverride(constraint);
        }

    }
}
