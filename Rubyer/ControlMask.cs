using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 控件遮罩
    /// 用于提供 MouseOver 和 Pressed 等效果
    /// </summary>
    public class ControlMask : Control
    {
        static ControlMask()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlMask), new FrameworkPropertyMetadata(typeof(ControlMask)));
        }

        // 是否激活
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(ControlMask), new PropertyMetadata(default(bool)));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ControlMask), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // 遮罩透明度
        public static readonly DependencyProperty MaskOpacityProperty = DependencyProperty.Register(
            "MaskOpacity", typeof(double), typeof(ControlMask), new PropertyMetadata(default(double)));

        public double MaskOpacity
        {
            get { return (double)GetValue(MaskOpacityProperty); }
            set { SetValue(MaskOpacityProperty, value); }
        }
    }
}
