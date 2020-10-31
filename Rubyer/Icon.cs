using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    public class Icon : Control
    {
        // 所有图标键值对
        private static readonly Lazy<Dictionary<IconType, string>> _codes = new Lazy<Dictionary<IconType, string>>(IconDatas.GetAll);

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateIcon();
        }

        // 图标类型
        public IconType Type
        {
            get { return (IconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(IconType), typeof(Icon), new PropertyMetadata(default(IconType), TypePropertyChangedCallBack));

        private static void TypePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Icon)d).UpdateIcon();
        }

        // 图标编码
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(Icon), new PropertyMetadata(""));


        // 更新图标
        private void UpdateIcon()
        {
            string code = null;
            _codes.Value?.TryGetValue(Type, out code);
            Code = code;
        }
    }
}
