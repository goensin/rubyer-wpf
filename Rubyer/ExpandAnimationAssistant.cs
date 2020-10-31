using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class ExpandAnimationAssistant : ContentControl
    {
        private FrameworkElement _element;

        static ExpandAnimationAssistant()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandAnimationAssistant), new FrameworkPropertyMetadata(typeof(ExpandAnimationAssistant)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var element = this.Content as FrameworkElement;
            if (element != null)
            {
                this._element = element;
                element.SizeChanged += Element_SizeChanged;
                element.Loaded += Element_Loaded;

                element.RenderTransformOrigin = new Point(0.5, 0.5);
                element.RenderTransform = new TransformGroup
                {
                    Children =
                    {
                        new ScaleTransform(),
                        new SkewTransform(),
                        new RotateTransform(),
                        new TranslateTransform()
                    }
                };
            }
        }



        public AnimationType AnimationType
        {
            get { return (AnimationType)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTypeProperty =
            DependencyProperty.Register("AnimationType", typeof(AnimationType), typeof(ExpandAnimationAssistant), new PropertyMetadata(AnimationType.FloatInDown));



        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            //var element = sender as FrameworkElement;
            //element.BeginStoryboard(floatInStoryBoard);
        }

        private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var element = sender as FrameworkElement;
            //element.BeginStoryboard(floatInStoryBoard);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            if (_element != null)
            {
                Storyboard storyBoard = this.FindResource(AnimationType.ToString()) as Storyboard;
                _element.BeginStoryboard(storyBoard);
            }

            base.OnChildDesiredSizeChanged(child);
        }
    }
}
