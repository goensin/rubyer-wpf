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
    /// <summary>
    /// 标记
    /// </summary>
    public class Badge : ContentControl
    {
        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));
        }

        #region 属性

        /// <summary>
        /// 标记颜色
        /// </summary>
        public static readonly DependencyProperty BadgeBrushProperty =
            DependencyProperty.Register("BadgeBrush", typeof(Brush), typeof(Badge), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 标记颜色
        /// </summary>
        public Brush BadgeBrush
        {
            get { return (Brush)GetValue(BadgeBrushProperty); }
            set { SetValue(BadgeBrushProperty, value); }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Badge), new PropertyMetadata(default(string), OnTextChanged));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 标记尺寸
        /// </summary>
        public static readonly DependencyProperty BadgeSizeProperty =
            DependencyProperty.Register("BadgeSize", typeof(double), typeof(Badge), new PropertyMetadata(default(double)));

        /// <summary>
        /// 标记尺寸
        /// </summary>
        public double BadgeSize
        {
            get { return (double)GetValue(BadgeSizeProperty); }
            set { SetValue(BadgeSizeProperty, value); }
        }

        /// <summary>
        /// 标记字体尺寸
        /// </summary>
        public static readonly DependencyProperty BadgeFontSizeProperty =
            DependencyProperty.Register("BadgeFontSize", typeof(double), typeof(Badge), new PropertyMetadata(default(double)));

        /// <summary>
        /// 标记字体尺寸
        /// </summary>
        public double BadgeFontSize
        {
            get { return (double)GetValue(BadgeFontSizeProperty); }
            set { SetValue(BadgeFontSizeProperty, value); }
        }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public static readonly DependencyProperty IsHiddenProperty =
            DependencyProperty.Register("IsHidden", typeof(bool), typeof(Badge), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }

        #endregion 属性

        #region 事件

        /// <summary>
        /// 文本改变事件
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Direct, typeof(RoutedEventArgs), typeof(Badge));

        /// <summary>
        /// 文本改变事件
        /// </summary>处理
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        #endregion 事件

        #region 方法

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null)
            {
                return;
            }

            Badge badge = d as Badge;
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = TextChangedEvent;
            badge.RaiseEvent(args);
        }

        #endregion 方法
    }
}