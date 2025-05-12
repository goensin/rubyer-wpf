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

            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;
        }

        /// <inheritdoc/> 
        protected override void OnClosing(CancelEventArgs e)
        {
            MouseMove -= OnMouseMove;
            MouseDown -= OnMouseDown;
            KeyDown -= OnKeyDown;
        }

        private bool isUpdating = false;

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            visualHost.ClearVisuals();

            var position = e.GetPosition(this);
            var screenPoint = PointToScreen(position);

            var count = 7;
            var left = screenPoint.X - count / 2;
            var top = screenPoint.Y - count / 2;
            double x = screenPoint.X - count * 2;
            double y = screenPoint.Y - count * 2;

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)this.ActualWidth,
                (int)this.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            rtb.Render(this);

            var cb = new CroppedBitmap(rtb, new Int32Rect((int)x, (int)y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);

            Color color = Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);

            //var rect = new System.Drawing.Rectangle((int)left, (int)top, count, count);
            //using var bmp = new System.Drawing.Bitmap(rect.Width, rect.Height);
            //using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            //{
            //    g.CopyFromScreen(rect.Location, System.Drawing.Point.Empty, rect.Size);
            //}

            var visual = new DrawingVisual();
            using DrawingContext dc = visual.RenderOpen();
            //for (int i = 0; i < count; i++)
            //{
            //    for (int j = 0; j < count; j++)
            //    {
            //        var size = 10;
            //        System.Drawing.Color color = bmp.GetPixel(j, i);
            //        dc.DrawRectangle(
            //               new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B)),
            //               new Pen((Brush)Application.Current.FindResource("Border"), 1),
            //               new Rect(x + j * size, y + i * size, size, size));
            //    }
            //}

            //visualHost.AddVisual(visual);

            dc.DrawImage(cb, new Rect(x, y, count * 10, count * 10));
            visualHost.AddVisual(visual);

            //colorRect.Fill = new System.Windows.Media.SolidColorBrush(
            //    System.Windows.Media.Color.FromArgb(centerColor.A, centerColor.R, centerColor.G, centerColor.B)
            //);

            //var colors = ScreenColorPicker.GetColorsAt(new Point(left, top), 5, 5);

            //int w = 0;
            //int h = 0;
            //for (int i = 0; i < colors.Length; i++)
            //{
            //    var visual = new DrawingVisual();
            //    using DrawingContext dc = visual.RenderOpen();
            //    dc.DrawRectangle(
            //        new SolidColorBrush(colors[1]),
            //        new Pen((Brush)Application.Current.FindResource("Border"), 1),
            //        new Rect(x + w * size, y + h * size, size, size));

            //    if (i % 5 == 4)
            //    {
            //        h++;
            //        w = 0;
            //    }
            //    else
            //        w++;

            //    visualHost.AddVisual(visual);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        stopwatch.Start();


            //        Debug.WriteLine("1:" + stopwatch.ElapsedMilliseconds);



            //        //visualHost.AddVisual(visual);
            //        Debug.WriteLine("2:" + stopwatch.ElapsedMilliseconds);
            //    }
            //}
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close(true);
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
