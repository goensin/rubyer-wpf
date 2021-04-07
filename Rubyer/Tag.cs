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
    public class Tag : ContentControl
    {
        static Tag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonDown += Tag_MouseLeftButtonDown;
        }

        #region 属性

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Tag), new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }


        public static readonly DependencyProperty HeadBackgroundProperty =
            DependencyProperty.Register("HeadBackground", typeof(Brush), typeof(Tag), new PropertyMetadata(default(Brush)));

        public Brush HeadBackground
        {
            get { return (Brush)GetValue(HeadBackgroundProperty); }
            set { SetValue(HeadBackgroundProperty, value); }
        }


        public static readonly DependencyProperty HeadForegroundProperty =
            DependencyProperty.Register("HeadForeground", typeof(Brush), typeof(Tag), new PropertyMetadata(default(Brush)));

        public Brush HeadForeground
        {
            get { return (Brush)GetValue(HeadForegroundProperty); }
            set { SetValue(HeadForegroundProperty, value); }
        }



        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(Tag), new PropertyMetadata(default(string)));


        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        #endregion


        #region 方法
        private void Tag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Url))
            {
                System.Diagnostics.Process.Start(this.Url);
            }
        }
        #endregion
    }
}
