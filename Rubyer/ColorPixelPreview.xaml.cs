using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Rubyer
{
    /// <summary>
    /// ColorPixelPreview.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPixelPreview : Window
    {
        /// <summary>
        /// 选中的颜色
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPixelPreview));

        /// <summary>
        /// 选中的颜色
        /// </summary>
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public ColorPixelPreview()
        {
            InitializeComponent();

            PreviewMouseMove += OnMouseMove;
            PreviewMouseDown += OnMouseDown;
            KeyDown += OnKeyDown;
        }

        /// <inheritdoc/> 
        protected override void OnClosing(CancelEventArgs e)
        {
            PreviewMouseMove -= OnMouseMove;
            PreviewMouseDown -= OnMouseDown;
            KeyDown -= OnKeyDown;
        }

        /// <inheritdoc/> 
        protected override void OnClosed(EventArgs e)
        {
            SelectedColor = ScreenColorPicker.GetColorAt(new Point(x, y));

            base.OnClosed(e);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            var screenPoint = PointToScreen(position);

            visualHost.ClearVisuals();

            SelectedColor = ScreenColorPicker.GetColorAt(new Point(screenPoint.X, screenPoint.Y));

            double size = 50;

            var visual = new DrawingVisual();
            using DrawingContext dc = visual.RenderOpen();
            dc.DrawRectangle(
                new SolidColorBrush(SelectedColor),
                new Pen((Brush)Application.Current.FindResource("Border"), 1),
                new Rect(position.X + 3, position.Y + 3, size, size));

            visualHost.AddVisual(visual);
        }

        double x;
        double y;

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            var screenPoint = PointToScreen(position);
            x = screenPoint.X;
            y = screenPoint.Y;

            Close(true);
            e.Handled = true;
        }

        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close(false);
            }
            else if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Close(true);
            }
        }

        private void Close(bool result)
        {
            DialogResult = result;
            Close();
        }
    }
}
