using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Shapes;

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
            if (!isUpdating)
            {
                isUpdating = true;
                var position = e.GetPosition(this);
                var screenPoint = PointToScreen(position);
                SelectedColor = ScreenColorPicker.GetColorAt(screenPoint);

                visualHost.ClearVisuals();

                var left = position.X - 2;
                var top = position.Y - 2;
                var size = 10;
                double x = position.X - size * 2;
                double y = position.Y - size * 2;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var colors = ScreenColorPicker.GetColorsAt(new Point(left, top), 5, 5);
                Debug.WriteLine("1:" + stopwatch.ElapsedMilliseconds);
                int w = 0;
                int h = 0;
                for (int i = 0; i < colors.Length; i++)
                {
                    var visual = new DrawingVisual();
                    using DrawingContext dc = visual.RenderOpen();
                    dc.DrawRectangle(
                        new SolidColorBrush(colors[1]),
                        new Pen((Brush)Application.Current.FindResource("Border"), 1),
                        new Rect(x + w * size, y + h * size, size, size));

                    if (i % 5 == 4)
                    {
                        h++;
                        w = 0;
                    }
                    else
                        w++;

                    visualHost.AddVisual(visual);
                }
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

                isUpdating = false;
            }
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
