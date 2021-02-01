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
    public class PageBarItem : ContentControl
    {
        static PageBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageBarItem), new FrameworkPropertyMetadata(typeof(PageBarItem)));
        }

        #region 命令
        // 点击页码命令
        public static readonly DependencyProperty PageNumberCommandProperty =
            DependencyProperty.Register("PageNumberCommand", typeof(ICommand), typeof(PageBarItem));

        public ICommand PageNumberCommand
        {
            get { return (ICommand)GetValue(PageNumberCommandProperty); }
            set { SetValue(PageNumberCommandProperty, value); }
        }
        #endregion

        #region 依赖属性

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(PageBarItem), new PropertyMetadata(default(int)));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        public static readonly DependencyProperty CurrentBrushProperty =
            DependencyProperty.Register("CurrentBrush", typeof(Brush), typeof(PageBarItem), new PropertyMetadata(default(Brush)));

        public Brush CurrentBrush
        {
            get { return (Brush)GetValue(CurrentBrushProperty); }
            set { SetValue(CurrentBrushProperty, value); }
        }

        #endregion
    }
}
