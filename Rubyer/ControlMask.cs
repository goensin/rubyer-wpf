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

        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ControlMask), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}
