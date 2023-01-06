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

namespace RubyerDemo.Controls
{
    /// <summary>
    /// 展示控件
    /// </summary>
    public class ControlDisplay : ContentControl
    {
        static ControlDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlDisplay), new FrameworkPropertyMetadata(typeof(ControlDisplay)));
        }

        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(ControlDisplay), new PropertyMetadata(null));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty DescriptionProperty =
          DependencyProperty.Register("Description", typeof(string), typeof(ControlDisplay), new PropertyMetadata(null));

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    }
}
