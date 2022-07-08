using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 图标
    /// </summary>
    public class Icon : Control
    {
        private static readonly Lazy<Dictionary<IconType, string>> _codes = new Lazy<Dictionary<IconType, string>>(IconDatas.GetAll);

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateIcon();
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType Type
        {
            get { return (IconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(IconType), typeof(Icon), new PropertyMetadata(default(IconType), TypePropertyChangedCallBack));

        private static void TypePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Icon)d).UpdateIcon();
        }

        /// <summary>
        /// 图标编码
        /// </summary>
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        /// <summary>
        /// 图标编码
        /// </summary>
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(Icon), new PropertyMetadata(""));

        private void UpdateIcon()
        {
            string code = null;
            _codes.Value?.TryGetValue(Type, out code);
            Code = code;
        }
    }
}