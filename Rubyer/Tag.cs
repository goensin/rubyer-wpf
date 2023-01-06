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
    /// 标签
    /// </summary>
    public class Tag : ContentControl
    {
        static Tag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonDown += Tag_MouseLeftButtonDown;
        }

        #region 属性

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Tag), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标题
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 标题背景色
        /// </summary>
        public static readonly DependencyProperty HeadBackgroundProperty =
            DependencyProperty.Register("HeadBackground", typeof(Brush), typeof(Tag), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 标题背景色
        /// </summary>
        public Brush HeadBackground
        {
            get { return (Brush)GetValue(HeadBackgroundProperty); }
            set { SetValue(HeadBackgroundProperty, value); }
        }

        /// <summary>
        /// 标题前景色
        /// </summary>
        public static readonly DependencyProperty HeadForegroundProperty =
            DependencyProperty.Register("HeadForeground", typeof(Brush), typeof(Tag), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 标题前景色
        /// </summary>
        public Brush HeadForeground
        {
            get { return (Brush)GetValue(HeadForegroundProperty); }
            set { SetValue(HeadForegroundProperty, value); }
        }

        /// <summary>
        /// 链接
        /// </summary>
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(Tag), new PropertyMetadata(default(string)));

        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        #endregion 属性

        #region 方法

        private void Tag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(Url))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Url) { UseShellExecute = true });
            }
        }

        #endregion 方法
    }
}