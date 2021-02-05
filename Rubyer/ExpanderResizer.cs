using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class ExpanderResizer : ContentControl
    {
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
            DependencyProperty.Register("ExpanderControl", typeof(Expander), typeof(ExpanderResizer), new PropertyMetadata(null, OnExpenderContentChanged));

        private static void OnExpenderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Expander expander)
            {
                expander.Expanded += Expander_Expanded;
                expander.Collapsed += Expander_Collapsed;

                ExpanderResizer resizer = d as ExpanderResizer;

                if (expander.ExpandDirection == ExpandDirection.Up || expander.ExpandDirection == ExpandDirection.Down)
                {
                    resizer.Height = 0;
                }
                else
                {
                    resizer.Width = 0;
                }
            }
        }


        private static void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            var child1 = VisualTreeHelper.GetChild(expander, 0);
            var child2 = VisualTreeHelper.GetChild(child1, 0);
            Control control = VisualTreeHelper.GetChild(child2, 0) as Control;
            ExpanderResizer resizer = VisualTreeHelper.GetChild(child2, 1) as ExpanderResizer;
            UIElement element = resizer.Content as UIElement;

            if (expander.ExpandDirection == ExpandDirection.Up || expander.ExpandDirection == ExpandDirection.Down)
            {
                element.Measure(new Size(resizer.ActualWidth, double.PositiveInfinity));

                DoubleAnimation animation = new DoubleAnimation
                {
                    To = element.DesiredSize.Height,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                resizer.BeginAnimation(HeightProperty, animation);
            }
            else
            {
                element.Measure(new Size(expander.MaxWidth - control.ActualWidth, resizer.ActualHeight));

                DoubleAnimation animation = new DoubleAnimation
                {
                    To = element.DesiredSize.Width,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                resizer.BeginAnimation(WidthProperty, animation);
            }
        }


        private static void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            var child1 = VisualTreeHelper.GetChild(expander, 0);
            var child2 = VisualTreeHelper.GetChild(child1, 0);
            ExpanderResizer resizer = VisualTreeHelper.GetChild(child2, 1) as ExpanderResizer;

            DoubleAnimation animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            if (expander.ExpandDirection == ExpandDirection.Up || expander.ExpandDirection == ExpandDirection.Down)
            {
                resizer.BeginAnimation(HeightProperty, animation);
            }
            else
            {
                resizer.BeginAnimation(WidthProperty, animation);
            }
        }
    }
}
