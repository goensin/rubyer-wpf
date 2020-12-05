using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class ExpandAnimationAssistant
    {
        static bool isChangeSize = false;
        // 圆角半径
        public static readonly DependencyProperty AnmimationControlProperty = DependencyProperty.RegisterAttached(
            "AnmimationControl", typeof(Control), typeof(ExpandAnimationAssistant), new PropertyMetadata(default(Control), OnControlChanged));

        private static void OnControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Expander expander = d as Expander;
            expander.Expanded += Expander_Expanded;
            expander.Collapsed += Expander_Collapsed;
            expander.SizeChanged += Expander_SizeChanged; ;
        }

        private static void Expander_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Height == 0 || isChangeSize == true)
            {
                return;
            }

            isChangeSize = true;
            Expander expander = sender as Expander;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = e.PreviousSize.Height;
            animation.To = e.NewSize.Height;
            animation.Duration = TimeSpan.FromMilliseconds(250);
            animation.Completed += (a, b) => 
            {
                Thread.Sleep(100);
                isChangeSize = false; 
            };
            expander.BeginAnimation(Expander.HeightProperty, animation);
        }


        private static void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            //Expander expander = sender as Expander;
            //FrameworkElement content = expander.Content as FrameworkElement;
            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = expander.ActualHeight;
            //animation.To = expander.ActualHeight - content.ActualHeight;
            //animation.Duration = TimeSpan.FromMilliseconds(250);

            //expander.BeginAnimation(Expander.HeightProperty, animation);
        }

        private static void Expander_Expanded(object sender, RoutedEventArgs e)
        {

            Expander expander = sender as Expander;
            //FrameworkElement content = expander.Content as FrameworkElement;
            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = expander.ActualHeight;
            //animation.To = expander.ActualHeight + content.ActualHeight;
            //animation.Duration = TimeSpan.FromMilliseconds(250);

            //expander.BeginAnimation(Expander.HeightProperty, animation);
        }

        public static void SetAnmimationControl(DependencyObject element, Control value)
        {
            element.SetValue(AnmimationControlProperty, value);
        }

        public static Control GetAnmimationControl(DependencyObject element)
        {
            return (Control)element.GetValue(AnmimationControlProperty);
        }

        //public AnimationType AnimationType
        //{
        //    get { return (AnimationType)GetValue(AnimationTypeProperty); }
        //    set { SetValue(AnimationTypeProperty, value); }
        //}

        //public static readonly DependencyProperty AnimationTypeProperty =
        //    DependencyProperty.Register("AnimationType", typeof(AnimationType), typeof(ExpandAnimationAssistant), new PropertyMetadata(AnimationType.FloatInDown));


        //protected override Size MeasureOverride(Size constraint)
        //{
        //    return base.MeasureOverride(constraint);
        //}

        //protected override void OnChildDesiredSizeChanged(UIElement child)
        //{
        //    if (_element != null)
        //    {
        //        Storyboard storyBoard = this.FindResource(AnimationType.ToString()) as Storyboard;
        //        _element.BeginStoryboard(storyBoard);
        //    }

        //    base.OnChildDesiredSizeChanged(child);
        //}
    }
}
