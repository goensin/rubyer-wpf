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
        private bool isExpanded;

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
            if (!isWidthAnimationing && !isHeightAnimationing || ExpanderControl.IsExpanded)
            {
                if (ExpanderControl != null)
                {
                    if (this.Content is UIElement element)
                    {
                        if (ExpanderControl.ExpandDirection == ExpandDirection.Down || ExpanderControl.ExpandDirection == ExpandDirection.Up)
                        {
                            if (ExpanderControl.IsExpanded)
                            {
                                return base.MeasureOverride(new Size(constraint.Width, ExpanderControl.MaxHeight));
                            }
                            else
                            {
                                return base.MeasureOverride(new Size(constraint.Width, 0));
                            }
                        }
                        else
                        {
                            if (ExpanderControl.IsExpanded)
                            {
                                Panel panel = VisualTreeHelper.GetParent(this) as Panel;
                                Control control = VisualTreeHelper.GetChild(panel, 0) as Control;
                                return base.MeasureOverride(new Size(ExpanderControl.MaxWidth - control.ActualWidth, constraint.Height));
                            }
                            else
                            {
                                return base.MeasureOverride(new Size(0, constraint.Height));
                            }
                        }
                    }
                }
            }

            return base.MeasureOverride(new Size(constraint.Width, constraint.Height));
        }


        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (this.ActualHeight != 0 || this.ActualWidth != 0)
            {
                if ((this.ActualHeight != arrangeBounds.Height || this.ActualWidth != arrangeBounds.Width) || (!ExpanderControl.IsExpanded && arrangeBounds.Height != 0 && arrangeBounds.Width != 0))
                {
                    if (ExpanderControl.ExpandDirection == ExpandDirection.Down || ExpanderControl.ExpandDirection == ExpandDirection.Up)
                    {
                        if (!isHeightAnimationing || ExpanderControl.IsExpanded != isExpanded)
                        {
                            isHeightAnimationing = true;
                            isExpanded = ExpanderControl.IsExpanded;

                            DoubleAnimation heightAnimation = new DoubleAnimation
                            {
                                From = ActualHeight,
                                To = !ExpanderControl.IsExpanded ? 0 : arrangeBounds.Height,
                                Duration = TimeSpan.FromMilliseconds(300),
                                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                            };

                            heightAnimation.Completed += (sender, e) =>
                            {
                                isHeightAnimationing = false;
                            };

                            this.BeginAnimation(HeightProperty, heightAnimation);

                            return new Size(this.ActualWidth, this.ActualHeight);
                        }
                    }
                    else
                    {
                        if (!isWidthAnimationing || ExpanderControl.IsExpanded != isExpanded)
                        {
                            isWidthAnimationing = true;
                            isExpanded = ExpanderControl.IsExpanded;

                            DoubleAnimation widthAnimation = new DoubleAnimation
                            {
                                From = ActualWidth,
                                To = !ExpanderControl.IsExpanded ? 0 : arrangeBounds.Width,
                                Duration = TimeSpan.FromMilliseconds(300),
                                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                            };

                            widthAnimation.Completed += (sender, e) =>
                            {
                                isWidthAnimationing = false;
                            };

                            this.BeginAnimation(WidthProperty, widthAnimation);

                            return new Size(this.ActualWidth, this.ActualHeight);
                        }
                    }
                }
                return base.ArrangeOverride(arrangeBounds);
            }
            else
            {
                return base.ArrangeOverride(arrangeBounds);
            }
        }
    }
}
