using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    public class ExpanderResizer : ContentControl
    {
        private bool isAnimationing;
        private double? oldHeight;
        static ExpanderResizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderResizer), new FrameworkPropertyMetadata(typeof(ExpanderResizer)));
        }

        public override void OnApplyTemplate()
        {
            var element = this.Content as FrameworkElement;
            element.SizeChanged += Element_SizeChanged;
            base.OnApplyTemplate();
        }


        public bool HasExpander
        {
            get { return (bool)GetValue(HasExpanderProperty); }
            set { SetValue(HasExpanderProperty, value); }
        }

        public static readonly DependencyProperty HasExpanderProperty =
            DependencyProperty.Register("HasExpander", typeof(bool), typeof(ExpanderResizer), new PropertyMetadata(false, HasExpanderChanged));

        private static void HasExpanderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ExpanderResizer resizer = d as ExpanderResizer;
            //Expander expander = resizer.Parent as Expander;
            //expander.Expanded += Expander_Expanded;
            //expander.Collapsed += Expander_Collapsed;
        }



        public Expander ExpanderControl
        {
            get { return (Expander)GetValue(ExpanderControlProperty); }
            set { SetValue(ExpanderControlProperty, value); }
        }

        public static readonly DependencyProperty ExpanderControlProperty =
            DependencyProperty.Register("ExpanderControl", typeof(Expander), typeof(ExpanderResizer), new PropertyMetadata(null, OnExpanderControlChanged));

        private static void OnExpanderControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (e.NewValue is Expander expander)
            //{
            //    expander.Expanded += Expander_Expanded;
            //    expander.Collapsed += Expander_Collapsed;
            //    var element = VisualTreeHelper.GetChild(expander, 1) as FrameworkElement;
            //    element.SizeChanged += Content_SizeChanged;
            //}
        }

        private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Height != 0)
            {
                var element = sender as FrameworkElement;
                ExpanderResizer resizer = element.Parent as ExpanderResizer;
                if (!resizer.isAnimationing)
                {
                    resizer.isAnimationing = true;
                    DoubleAnimation animation = new DoubleAnimation();
                    animation.From = e.PreviousSize.Height;
                    animation.To = e.NewSize.Height;
                    animation.Duration = TimeSpan.FromMilliseconds(250);
                    animation.Completed += (a, b) =>
                    {
                        resizer.isAnimationing = false;
                        resizer.Height = double.NaN;
                    };

                    resizer.BeginAnimation(HeightProperty, animation);
                }
            }
        }

        private static void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            ExpanderResizer resizer = VisualTreeHelper.GetChild(expander, 0) as ExpanderResizer;

            resizer.oldHeight = expander.ActualHeight;
            //Expander expander = sender as Expander;
            //ExpanderResizer resizer = VisualTreeHelper.GetChild(expander, 0) as ExpanderResizer;

            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = expander.ActualHeight;
            //animation.To = 30;
            //animation.Duration = TimeSpan.FromMilliseconds(250);

            //resizer.BeginAnimation(HeightProperty, animation);
        }

        private static void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            ExpanderResizer resizer = VisualTreeHelper.GetChild(expander, 0) as ExpanderResizer;

            resizer.oldHeight = expander.ActualHeight;
            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = expander.ActualHeight;
            //animation.To = 109;
            //animation.Duration = TimeSpan.FromMilliseconds(250);

            //resizer.BeginAnimation(HeightProperty, animation);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //    if (oldHeight != null)
            //    {
            //        var content = this.Content as FrameworkElement;

            //        if (!isAnimationing && (Math.Abs((double)(content.DesiredSize.Height - ActualHeight))) > 0.1)
            //        {
            //            isAnimationing = true;

            //            DoubleAnimation animation = new DoubleAnimation();
            //            animation.From = oldHeight;
            //            animation.To = content.DesiredSize.Height;
            //            animation.Duration = TimeSpan.FromMilliseconds(250);

            //            animation.Completed += (sender, e) =>
            //            {
            //                Thread.Sleep(200);
            //                isAnimationing = false;
            //            };

            //            this.BeginAnimation(HeightProperty, animation);
            //        }
            //    }

            return base.MeasureOverride(constraint);
        }
    }
}
