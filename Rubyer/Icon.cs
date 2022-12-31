using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 图标
    /// </summary>
    public class Icon : Control
    {
        private static readonly Lazy<Dictionary<IconType, IconInfo>> _codes = new Lazy<Dictionary<IconType, IconInfo>>(IconDatas.GetAll);

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
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(IconType), typeof(Icon), new PropertyMetadata(default(IconType), TypePropertyChangedCallBack));

        private static void TypePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Icon)d).UpdateIcon();

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType Type
        {
            get { return (IconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// 图标编码
        /// </summary>
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(Icon), new PropertyMetadata(""));

        /// <summary>
        /// 图标编码
        /// </summary>
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        /// <summary>
        /// 组
        /// </summary>
        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.Register("Group", typeof(string), typeof(Icon), new PropertyMetadata(""));

        /// <summary>
        /// 组
        /// </summary>
        public string Group
        {
            get { return (string)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }

        private void UpdateIcon()
        {
            var icon = new IconInfo(string.Empty, string.Empty);
            _codes.Value?.TryGetValue(Type, out icon);
            Group = icon.Group;
            Code = icon.Data;
        }

        /// <summary>
        /// 获取所有图标信息
        /// </summary>
        /// <returns>所有图标信息</returns>
        public static IEnumerable<IconInfo> GetAllIconInfo() => _codes.Value.Select(x => new IconInfo(x.Key, x.Value.Group, x.Value.Data));
    }

    /// <summary>
    /// 图标信息
    /// </summary>
    public class IconInfo
    {
        /// <summary>
        /// 图标组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 图标数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType Type { get; set; }

        /// <summary>
        /// 图标信息
        /// </summary>
        /// <param name="group">图标组</param>
        /// <param name="data">图标数据</param>
        public IconInfo(string group, string data)
        {
            Group = group;
            Data = data;
        }

        /// <summary>
        /// 图标信息
        /// </summary>
        /// <param name="type">图标类型</param>
        /// <param name="group">图标组</param>
        /// <param name="data">图标数据</param>
        public IconInfo(IconType type, string group, string data)
            : this(group, data)
        {
            Type = type;
        }
    }
}