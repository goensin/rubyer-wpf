using Rubyer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 头像
    /// </summary>
    public class Avatar : ContentControl
    {
        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            Border.CornerRadiusProperty.AddOwner(typeof(Avatar), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// 伸展方式
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            Viewbox.StretchProperty.AddOwner(typeof(Avatar), new FrameworkPropertyMetadata(default(Stretch), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// 头像形状
        /// </summary>
        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(AvatarShape), typeof(Avatar), new PropertyMetadata(default(AvatarShape)));


        static Avatar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Avatar), new FrameworkPropertyMetadata(typeof(Avatar)));
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// 伸展方式
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// 头像形状
        /// </summary>
        public AvatarShape Shape
        {
            get => (AvatarShape)GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }
    }
}
