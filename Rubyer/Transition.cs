using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class Transition : ContentControl
    {
        static Transition()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Transition), new FrameworkPropertyMetadata(typeof(Transition)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += Transition_Loaded;
        }

        private void Transition_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsShow)
            {
                ShowAnimation(this);
            }
        }

        #region 依赖属性

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(Transition), new PropertyMetadata(default(bool), OnIsShwowChanged));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TransitionType), typeof(Transition), new PropertyMetadata(default(TransitionType)));

        /// <summary>
        /// 转换类型
        /// </summary>
        public TransitionType Type
        {
            get { return (TransitionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 动画时长
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(Transition), new PropertyMetadata(default(Duration)));

        /// <summary>
        /// 动画时长
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// 动画位置偏移
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(double), typeof(Transition), new PropertyMetadata(default(double)));

        /// <summary>
        /// 动画位置偏移
        /// </summary>
        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public static readonly DependencyProperty InitialScaleProperty =
            DependencyProperty.Register("InitialScale", typeof(double), typeof(Transition), new PropertyMetadata(default(double)));

        /// <summary>
        /// 动画初始缩放
        /// </summary>
        public double InitialScale
        {
            get { return (double)GetValue(InitialScaleProperty); }
            set { SetValue(InitialScaleProperty, value); }
        }

        #endregion

        private static void OnIsShwowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Transition transition && transition.IsLoaded)
            {
                if (transition.IsShow)
                {
                    ShowAnimation(transition);
                }
                else
                {
                    HideAnimation(transition);
                }
            }
        }

        private static void ShowAnimation(Transition transition)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation opacityAnimation = GetOpacityAnimation(transition, 0, 1);

            switch (transition.Type)
            {
                case TransitionType.Fade:
                case TransitionType.Collapse:
                case TransitionType.CollapseLeft:
                default:
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeLeft:
                    DoubleAnimation translateAnimation = GetTranslateXAnimation(transition, transition.Offset, 0);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeRight:
                    translateAnimation = GetTranslateXAnimation(transition, -transition.Offset, 0);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeUp:
                    translateAnimation = GetTranslateYAnimation(transition, transition.Offset, 0);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeDown:
                    translateAnimation = GetTranslateYAnimation(transition, -transition.Offset, 0);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.Zoom:
                    DoubleAnimation scaleXAnimation = GetScaleXAnimation(transition, transition.InitialScale, 1);
                    DoubleAnimation scaleYAnimation = GetScaleYAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomX:
                    scaleXAnimation = GetScaleXAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomY:
                    scaleYAnimation = GetScaleYAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomLeft:
                    transition.RenderTransformOrigin = new Point(0, 0.5);
                    scaleXAnimation = GetScaleXAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomRight:
                    transition.RenderTransformOrigin = new Point(1, 0.5);
                    scaleXAnimation = GetScaleXAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomUp:
                    transition.RenderTransformOrigin = new Point(0.5, 0);
                    scaleYAnimation = GetScaleYAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomDown:
                    transition.RenderTransformOrigin = new Point(0.5, 1);
                    scaleYAnimation = GetScaleYAnimation(transition, transition.InitialScale, 1);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
            }

            storyboard.Begin();
        }

        private static void HideAnimation(Transition transition)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation opacityAnimation = GetOpacityAnimation(transition, 1, 0);

            switch (transition.Type)
            {
                case TransitionType.Fade:
                case TransitionType.Collapse:
                case TransitionType.CollapseLeft:
                default:
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeLeft:
                    DoubleAnimation translateAnimation = GetTranslateXAnimation(transition, 0, transition.Offset);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeRight:
                    translateAnimation = GetTranslateXAnimation(transition, 0, -transition.Offset);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeUp:
                    translateAnimation = GetTranslateYAnimation(transition, 0, transition.Offset);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.FadeDown:
                    translateAnimation = GetTranslateYAnimation(transition, 0, -transition.Offset);
                    storyboard.Children.Add(translateAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;

                case TransitionType.Zoom:
                    DoubleAnimation scaleXAnimation = GetScaleXAnimation(transition, 1, transition.InitialScale);
                    DoubleAnimation scaleYAnimation = GetScaleYAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomX:
                    scaleXAnimation = GetScaleXAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomY:
                    scaleYAnimation = GetScaleYAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomLeft:
                    transition.RenderTransformOrigin = new Point(0, 0.5);
                    scaleXAnimation = GetScaleXAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomRight:
                    transition.RenderTransformOrigin = new Point(1, 0.5);
                    scaleXAnimation = GetScaleXAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomUp:
                    transition.RenderTransformOrigin = new Point(0.5, 0);
                    scaleYAnimation = GetScaleYAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
                case TransitionType.ZoomDown:
                    transition.RenderTransformOrigin = new Point(0.5, 1);
                    scaleYAnimation = GetScaleYAnimation(transition, 1, transition.InitialScale);
                    storyboard.Children.Add(scaleYAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    break;
            }

            storyboard.Begin();
        }

        private static DoubleAnimation GetOpacityAnimation(Transition transition, double from, double to)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = transition.Duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(opacityAnimation, transition);
            return opacityAnimation;
        }

        private static DoubleAnimation GetTranslateXAnimation(Transition transition, double from, double to)
        {
            DoubleAnimation transformAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = transition.Duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)"));
            Storyboard.SetTarget(transformAnimation, transition);
            return transformAnimation;
        }

        private static DoubleAnimation GetTranslateYAnimation(Transition transition, double from, double to)
        {
            DoubleAnimation transformAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = transition.Duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)"));
            Storyboard.SetTarget(transformAnimation, transition);
            return transformAnimation;
        }

        private static DoubleAnimation GetScaleXAnimation(Transition transition, double from, double to)
        {
            DoubleAnimation transformAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = transition.Duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
            Storyboard.SetTarget(transformAnimation, transition);
            return transformAnimation;
        }

        private static DoubleAnimation GetScaleYAnimation(Transition transition, double from, double to)
        {
            DoubleAnimation transformAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = transition.Duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
            Storyboard.SetTarget(transformAnimation, transition);
            return transformAnimation;
        }
    }

    public enum TransitionType
    {
        Fade = 0,
        FadeLeft,
        FadeRight,
        FadeUp,
        FadeDown,
        Zoom,
        ZoomX,
        ZoomY,
        ZoomLeft,
        ZoomRight,
        ZoomUp,
        ZoomDown,
        Collapse,
        CollapseLeft
    }
}
