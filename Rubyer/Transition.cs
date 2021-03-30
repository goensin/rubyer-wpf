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

        #region 依赖属性
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(Transition), new PropertyMetadata(default(bool)));

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }


        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TransitionType), typeof(Transition), new PropertyMetadata(default(TransitionType)));

        public TransitionType Type
        {
            get { return (TransitionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion
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
