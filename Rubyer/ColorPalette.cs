using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 调色板
    /// </summary>
    [TemplatePart(Name = ColorSliderPartName, Type = typeof(Slider))]
    [TemplatePart(Name = OpacitySliderPartName, Type = typeof(Slider))]
    [TemplatePart(Name = DragThumbPartName, Type = typeof(Thumb))]
    [TemplatePart(Name = SlGridPartName, Type = typeof(Grid))]
    [TemplatePart(Name = RgbTextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = EyedropperButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = OptionalColorItemsControlPartName, Type = typeof(ItemsControl))]
    public class ColorPalette : Control
    {
        /// <summary>
        /// 颜色度滑块
        /// </summary>
        public const string ColorSliderPartName = "PART_ColorSlider";

        /// <summary>
        /// 透明度滑块
        /// </summary>
        public const string OpacitySliderPartName = "PART_OpacitySlider";

        /// <summary>
        /// SL 图拖多块
        /// </summary>
        public const string DragThumbPartName = "PART_DragSLThumb";

        /// <summary>
        /// SL 图面板
        /// </summary>
        public const string SlGridPartName = "PART_SlGrid";

        /// <summary>
        /// RGB 文本框
        /// </summary>
        public const string RgbTextBoxPartName = "PART_RgbTextBox";

        /// <summary>
        /// RGB 文本框
        /// </summary>
        public const string OptionalColorItemsControlPartName = "PART_OptionalColorItemsControl";

        /// <summary>
        /// 吸管按钮
        /// </summary>
        public const string EyedropperButtonPartName = "PART_EyedropperButton";

        private Slider colorSlider; // 颜色度滑块
        private Slider opacitySlider; // 透明度滑
        private Thumb dragSLThumb; // SL 图拖多块
        private Grid slGrid; // SL 图面板
        private TextBox rgbTextBox; // RGB 文本框
        private ItemsControl optionalColorItemsControl; // RGB 文本框

        static ColorPalette()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPalette), new FrameworkPropertyMetadata(typeof(ColorPalette)));
        }

        private bool isUpdating;
        private bool isHueUpdating;

        #region 事件

        /// <summary>
        /// 颜色改变
        /// </summary>
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent(
            "ColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPalette));

        /// <summary>
        /// 颜色改变
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }

        /// <summary>
        /// 开始提取颜色
        /// </summary>
        public static readonly RoutedEvent StartPickingEvent = EventManager.RegisterRoutedEvent(
            "StartPicking", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPalette));

        /// <summary>
        /// 开始提取颜色
        /// </summary>
        public event RoutedEventHandler StartPicking
        {
            add { AddHandler(StartPickingEvent, value); }
            remove { RemoveHandler(StartPickingEvent, value); }
        }

        /// <summary>
        /// 完成颜色提取
        /// </summary>
        public static readonly RoutedEvent CompletedPickedEvent = EventManager.RegisterRoutedEvent(
            "CompletedPicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPalette));

        /// <summary>
        /// 完成颜色提取
        /// </summary>
        public event RoutedEventHandler CompletedPicked
        {
            add { AddHandler(CompletedPickedEvent, value); }
            remove { RemoveHandler(CompletedPickedEvent, value); }
        }

        #endregion


        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ColorPalette), new PropertyMetadata(null));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 红
        /// </summary>
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(byte), typeof(ColorPalette), new PropertyMetadata((byte)0, OnRgbaChanged));

        /// <summary>
        /// 红
        /// </summary>
        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        /// <summary>
        /// 绿
        /// </summary>
        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register("Green", typeof(byte), typeof(ColorPalette), new PropertyMetadata((byte)0, OnRgbaChanged));

        /// <summary>
        /// 绿
        /// </summary>
        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        /// <summary>
        /// 蓝
        /// </summary>
        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPalette), new PropertyMetadata((byte)0, OnRgbaChanged));

        /// <summary>
        /// 蓝
        /// </summary>
        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        /// <summary>
        /// 透明度 
        /// </summary>
        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorPalette), new PropertyMetadata((byte)255, OnRgbaChanged));

        /// <summary>
        /// 透明度 
        /// </summary>
        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        /// <summary>
        /// 色相
        /// </summary>
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(ColorPalette), new PropertyMetadata(0.0, OnHslChanged));

        /// <summary>
        /// 色相
        /// </summary>
        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        /// <summary>
        /// 色相
        /// </summary>
        public static readonly DependencyProperty StaturationProperty =
            DependencyProperty.Register("Staturation", typeof(double), typeof(ColorPalette), new PropertyMetadata(0.0, OnHslChanged));

        /// <summary>
        /// 饱和度
        /// </summary>
        public double Staturation
        {
            get { return (double)GetValue(StaturationProperty); }
            set { SetValue(StaturationProperty, value); }
        }

        /// <summary>
        /// 饱和度
        /// </summary>
        public static readonly DependencyProperty LightnessProperty =
            DependencyProperty.Register("Lightness", typeof(double), typeof(ColorPalette), new PropertyMetadata(0.0, OnHslChanged));

        /// <summary>
        /// 亮度
        /// </summary>
        public double Lightness
        {
            get { return (double)GetValue(LightnessProperty); }
            set { SetValue(LightnessProperty, value); }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPalette), new PropertyMetadata(Colors.Red, OnColorChanged));

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// 颜色 - 没有透明度
        /// </summary>
        internal static readonly DependencyPropertyKey NoAlphaColorPropertyKey =
            DependencyProperty.RegisterReadOnly("NoAlphaColor", typeof(Color), typeof(ColorPalette), new PropertyMetadata(default(Color)));

        /// <summary>
        /// 颜色 - 没有透明度
        /// </summary>
        public static readonly DependencyProperty NoAlphaColorProperty = NoAlphaColorPropertyKey.DependencyProperty;

        /// <summary>
        /// 颜色 - 没有透明度
        /// </summary>
        public Color NoAlphaColor
        {
            get { return (Color)GetValue(NoAlphaColorProperty); }
            private set { SetValue(NoAlphaColorPropertyKey, value); }
        }

        /// <summary>
        /// 颜色 - 没有饱和度和亮度
        /// </summary>
        internal static readonly DependencyPropertyKey NoSlColorPropertyKey =
            DependencyProperty.RegisterReadOnly("NoSlColor", typeof(Color), typeof(ColorPalette), new PropertyMetadata(default(Color)));

        /// <summary>
        /// 颜色 - 没有饱和度和亮度
        /// </summary>
        public static readonly DependencyProperty NoSlColorProperty = NoSlColorPropertyKey.DependencyProperty;

        /// <summary>
        /// 颜色 - 没有饱和度和亮度
        /// </summary>
        public Color NoSlColor
        {
            get { return (Color)GetValue(NoSlColorProperty); }
            private set { SetValue(NoSlColorPropertyKey, value); }
        }

        /// <summary>
        /// 已复制颜色
        /// </summary>
        public static readonly DependencyProperty IsCopiedProperty =
            DependencyProperty.Register("IsCopied", typeof(bool), typeof(ColorPalette), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsCopiedChanged));

        /// <summary>
        /// 已复制颜色
        /// </summary>
        public bool IsCopied
        {
            get { return (bool)GetValue(IsCopiedProperty); }
            set { SetValue(IsCopiedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否启用透明度
        /// </summary>
        public static readonly DependencyProperty IsAlphaEnabledProperty =
            DependencyProperty.Register("IsAlphaEnabled", typeof(bool), typeof(ColorPalette), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsAlphaEnabledChanged));

        /// <summary>
        /// 是否启用透明度
        /// </summary>
        public bool IsAlphaEnabled
        {
            get { return (bool)GetValue(IsAlphaEnabledProperty); }
            set { SetValue(IsAlphaEnabledProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 显示 HSL
        /// </summary>
        public static readonly DependencyProperty IsShowHslProperty =
            DependencyProperty.Register("IsShowHsl", typeof(bool), typeof(ColorPalette), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 显示 HSL
        /// </summary>
        public bool IsShowHsl
        {
            get { return (bool)GetValue(IsShowHslProperty); }
            set { SetValue(IsShowHslProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否启用吸管工具
        /// </summary>
        public static readonly DependencyProperty IsEyedropperEnabledProperty =
            DependencyProperty.Register("IsEyedropperEnabled", typeof(bool), typeof(ColorPalette), new PropertyMetadata(BooleanBoxes.TrueBox));

        /// <summary>
        /// 是否启用吸管工具
        /// </summary>
        public bool IsEyedropperEnabled
        {
            get { return (bool)GetValue(IsEyedropperEnabledProperty); }
            set { SetValue(IsEyedropperEnabledProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 提取颜色中
        /// </summary>
        internal static readonly DependencyPropertyKey IsPickingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsPicking", typeof(bool), typeof(ColorPalette), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 提取颜色中
        /// </summary>
        public static readonly DependencyProperty IsPickingProperty = IsPickingPropertyKey.DependencyProperty;

        /// <summary>
        /// 提取颜色中
        /// </summary>
        public bool IsPicking
        {
            get { return (bool)GetValue(IsPickingProperty); }
            private set { SetValue(IsPickingPropertyKey, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 可选颜色集合
        /// </summary>
        public static readonly DependencyProperty OptionalColorsProperty =
            DependencyProperty.Register("OptionalColors", typeof(ObservableCollection<Color>), typeof(ColorPalette), new PropertyMetadata(new ObservableCollection<Color>()));

        /// <summary>
        /// 可选颜色集合
        /// </summary>
        public ObservableCollection<Color> OptionalColors
        {
            get { return (ObservableCollection<Color>)GetValue(OptionalColorsProperty); }
            set { SetValue(OptionalColorsProperty, value); }
        }

        public ColorPalette()
        {
            Color = Color.FromArgb(Alpha, Red, Green, Blue);
        }

        /// <inheritdoc/> 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            colorSlider = (Slider)GetTemplateChild(ColorSliderPartName);
            colorSlider.PreviewMouseDown += ColorSlider_PreviewMouseDown;
            colorSlider.ValueChanged += ColorSlider_ValueChanged;
            ColorToHsl(Color, out double hue, out _, out _);
            colorSlider.Value = hue;

            opacitySlider = (Slider)GetTemplateChild(OpacitySliderPartName);
            opacitySlider.PreviewMouseDown += OpacitySlider_PreviewMouseDown;

            dragSLThumb = (Thumb)GetTemplateChild(DragThumbPartName);
            dragSLThumb.DragDelta += DragThumb_DragDelta;

            slGrid = (Grid)GetTemplateChild(SlGridPartName);
            slGrid.PreviewMouseDown += SlGrid_PreviewMouseDown;

            rgbTextBox = (TextBox)GetTemplateChild(RgbTextBoxPartName);
            rgbTextBox.PreviewKeyDown += RgbTextBox_PreviewKeyDown;
            rgbTextBox.LostFocus += RgbTextBox_LostFocus;

            var eyedropperButton = (Button)GetTemplateChild(EyedropperButtonPartName);
            eyedropperButton.Click += EyedropperButton_Click;

            Loaded += ColorPalette_Loaded;

            optionalColorItemsControl = (ItemsControl)GetTemplateChild(OptionalColorItemsControlPartName);

            optionalColorItemsControl.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(CheckBox_Checked));
        }

        private void ColorPalette_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ColorPalette_Loaded;

            UpdateColor(Color);
            NoSlColor = HslToColor(colorSlider.Value);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is CheckBox checkBox && checkBox.DataContext is Color color)
            {
                Color = color;
                UpdateHslFromColor();
            }
        }

        private void RgbTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var binding = rgbTextBox.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();

                UpdateHslFromColor();
            }
        }

        private void RgbTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateHslFromColor();
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NoSlColor = HslToColor(colorSlider.Value);

            if (!isHueUpdating)
            {
                UpdateColorFromHsl();
            }
        }

        /// <summary>
        /// 根据 HSL 更新颜色
        /// </summary>
        private void UpdateColorFromHsl()
        {
            if (dragSLThumb is { } && slGrid is { })
            {
                double hue = colorSlider.Value;
                var thumbPoint = new Point(dragSLThumb.ActualWidth / 2, dragSLThumb.ActualHeight / 2);
                Point point = dragSLThumb.TranslatePoint(thumbPoint, slGrid);
                double saturation = point.X / slGrid.ActualWidth;
                double topL = 1.0 - 0.5 * saturation;
                double lightness = topL * (1.0 - (point.Y / slGrid.ActualHeight));
                var color = HslToColor(hue, saturation, lightness, Color.A);
                Color = color;
            }
        }

        /// <summary>
        /// 更新 HSL 滑块位置
        /// </summary>
        private void UpdateHslFromColor()
        {
            if (dragSLThumb is { } && slGrid is { })
            {
                var thumbPoint = new Point(dragSLThumb.ActualWidth / 2, dragSLThumb.ActualHeight / 2);
                Point point = dragSLThumb.TranslatePoint(thumbPoint, slGrid);

                ColorToHsl(Color, out double hue, out double saturation, out double lightness);
                double topL = 1.0 - 0.5 * saturation;
                double x = slGrid.ActualWidth * saturation;
                double y = (1.0 - lightness / topL) * slGrid.ActualHeight;
                UpdateDragThumkPosition(x - point.X, y - point.Y);

                if (Color != Colors.White && Color != Colors.Black)
                {
                    isHueUpdating = true;
                    colorSlider.Value = hue;
                    isHueUpdating = false;
                }
            }
        }

        private async void ColorSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(colorSlider);
            colorSlider.Value = (point.X / colorSlider.ActualWidth) * 360;
            if (colorSlider.Template.FindName("PART_Track", colorSlider) is Track track)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                if (!track.Thumb.IsDragging)
                {
                    var mouseDownEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                    {
                        RoutedEvent = Mouse.MouseDownEvent,
                        Source = e.Source
                    };

                    track.Thumb.RaiseEvent(mouseDownEvent);
                    this.Focus();
                }
            }
        }

        private async void OpacitySlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(opacitySlider);
            opacitySlider.Value = (point.X / opacitySlider.ActualWidth) * 255;
            if (opacitySlider.Template.FindName("PART_Track", opacitySlider) is Track track)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                if (!track.Thumb.IsDragging)
                {
                    var mouseDownEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                    {
                        RoutedEvent = Mouse.MouseDownEvent,
                        Source = e.Source
                    };

                    track.Thumb.RaiseEvent(mouseDownEvent);
                    this.Focus();
                }
            }
        }

        private void SlGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dragSLThumb is { } && slGrid is { })
            {
                var currentPoint = e.GetPosition(slGrid);
                var thumbPoint = new Point(dragSLThumb.ActualWidth / 2, dragSLThumb.ActualHeight / 2);
                Point point = dragSLThumb.TranslatePoint(thumbPoint, slGrid);
                UpdateDragThumkPosition(currentPoint.X - point.X, currentPoint.Y - point.Y);
                UpdateColorFromHsl();

                if (!dragSLThumb.IsDragging)
                {
                    var mouseDownEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                    {
                        RoutedEvent = Mouse.MouseDownEvent,
                        Source = e.Source
                    };

                    dragSLThumb.RaiseEvent(mouseDownEvent);
                    Focus();
                }

                e.Handled = true;
            }
        }

        private static void OnRgbaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPalette = (ColorPalette)d;
            if (!colorPalette.isUpdating)
            {
                colorPalette.Color = Color.FromArgb(colorPalette.Alpha, colorPalette.Red, colorPalette.Green, colorPalette.Blue);

                colorPalette.isHueUpdating = true;
                colorPalette.UpdateHslFromColor();
                colorPalette.isHueUpdating = false;
            }
        }


        private static void OnHslChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPalette = (ColorPalette)d;
            if (!colorPalette.isUpdating)
            {
                colorPalette.Color = HslToColor(colorPalette.Hue, colorPalette.Staturation, colorPalette.Lightness, colorPalette.Alpha);

                colorPalette.isHueUpdating = true;
                colorPalette.UpdateHslFromColor();
                colorPalette.isHueUpdating = false;
            }
        }


        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPalette = (ColorPalette)d;
            colorPalette.NoAlphaColor = Color.FromRgb(colorPalette.Color.R, colorPalette.Color.G, colorPalette.Color.B);
            colorPalette.IsCopied = false;

            colorPalette.isUpdating = true;

            colorPalette.Alpha = colorPalette.Color.A;
            colorPalette.Red = colorPalette.Color.R;
            colorPalette.Green = colorPalette.Color.G;
            colorPalette.Blue = colorPalette.Color.B;

            ColorToHsl(colorPalette.Color, out double h, out double s, out double l);
            colorPalette.Hue = h;
            colorPalette.Staturation = s;
            colorPalette.Lightness = l;

            colorPalette.RaiseEvent(new RoutedPropertyChangedEventArgs<Color>((Color)e.OldValue, (Color)e.NewValue)
            {
                RoutedEvent = ColorChangedEvent
            });

            colorPalette.isUpdating = false;
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UpdateDragThumkPosition(e.HorizontalChange, e.VerticalChange);

            UpdateColorFromHsl();
        }

        /// <summary>
        /// 更新 SL 滑块位置
        /// </summary>
        /// <param name="x">X 坐标更新</param>
        /// <param name="y">Y 坐标更新</param>
        private void UpdateDragThumkPosition(double x, double y)
        {
            if (dragSLThumb.RenderTransform is not TranslateTransform)
            {
                dragSLThumb.RenderTransform = new TranslateTransform();
            }

            if (dragSLThumb.RenderTransform is TranslateTransform translateTransform)
            {
                var point = dragSLThumb.TranslatePoint(new Point(), slGrid);
                var halfW = dragSLThumb.ActualWidth / 2;
                var halfH = dragSLThumb.ActualHeight / 2;

                if (x < 0 && x < -(point.X + halfW))
                {
                    x = -(point.X + halfW);
                }
                else if (x > 0 && x + point.X + halfW > slGrid.ActualWidth)
                {
                    x = slGrid.ActualWidth - halfW - point.X;
                }

                if (y < 0 && y < -(point.Y + halfH))
                {
                    y = -(point.Y + halfH);
                }
                else if (y > 0 && y + point.Y + halfH > slGrid.ActualHeight)
                {
                    y = slGrid.ActualHeight - halfH - point.Y;
                }

                translateTransform.X += x;
                translateTransform.Y += y;
            }
        }

        /// <summary>
        /// 通过 HSL 获取 Color
        /// </summary>
        /// <param name="hue">色相</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="lightness">亮度</param>
        /// <param name="alpha">透明度</param>
        /// <returns>Color</returns>
        public static Color HslToColor(double hue, double saturation = 1, double lightness = 0.5, byte alpha = 255)
        {
            // 规范化输入值（使用Math.Clamp确保范围正确）
            hue = Math.Min(Math.Max(hue % 360, 0), 360); // 色相0-360度
            if (hue < 0) hue += 360;

            saturation = Math.Min(Math.Max(saturation, 0), 1); // 饱和度0-1
            lightness = Math.Min(Math.Max(lightness, 0), 1);   // 亮度0-1

            // 无颜色情况（饱和度为0）
            if (saturation == 0)
            {
                byte l = (byte)(lightness * 255);
                return Color.FromArgb(alpha, l, l, l);
            }

            // 计算临时值q
            double q = lightness < 0.5
                ? lightness * (1 + saturation)
                : lightness + saturation - (lightness * saturation);

            double p = 2 * lightness - q;
            double hk = hue / 360.0;

            // 计算三个颜色通道
            double[] tc = { hk + 1.0 / 3, hk, hk - 1.0 / 3 };
            double[] colors = new double[3];

            for (int i = 0; i < 3; i++)
            {
                // 调整色相值到0-1范围
                if (tc[i] < 0) tc[i] += 1;
                if (tc[i] > 1) tc[i] -= 1;

                // 计算各通道颜色值
                if (tc[i] < 1.0 / 6)
                {
                    colors[i] = p + ((q - p) * 6 * tc[i]);
                }
                else if (tc[i] < 1.0 / 2)
                {
                    colors[i] = q;
                }
                else if (tc[i] < 2.0 / 3)
                {
                    colors[i] = p + ((q - p) * 6 * (2.0 / 3 - tc[i]));
                }
                else
                {
                    colors[i] = p;
                }

                // 确保最终值在0-1范围内（二次保护）
                colors[i] = Math.Min(Math.Max(colors[i], 0), 1);
            }

            // 转换为RGB并四舍五入
            return Color.FromArgb(
                alpha,
                (byte)(colors[0] * 255 + 0.5),
                (byte)(colors[1] * 255 + 0.5),
                (byte)(colors[2] * 255 + 0.5));
        }

        /// <summary>
        /// 通过 Color 获取 HSL
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="hue">色相</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="lightness">亮度</param>
        public static void ColorToHsl(Color color, out double hue, out double saturation, out double lightness)
        {
            // 归一化RGB值到0-1范围
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            // 计算最大值和最小值
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            // 计算亮度(L)
            lightness = (max + min) / 2;

            // 灰色情况（无饱和度）
            if (delta == 0)
            {
                hue = 0;
                saturation = 0;
                return;
            }

            // 计算饱和度(S)
            saturation = lightness > 0.5
                ? delta / (2 - max - min)
                : delta / (max + min);

            // 计算色相(H)
            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else // b == max
            {
                hue = 4 + (r - g) / delta;
            }

            hue *= 60; // 转换为度数

            // 规范化色相到0-360范围
            if (hue < 0) hue += 360;
        }

        /// <summary>
        /// 更新颜色
        /// </summary>
        /// <param name="color">颜色</param>
        internal void UpdateColor(Color color)
        {
            IsCopied = false;
            Color = color;
            UpdateHslFromColor();
        }

        /// <summary>
        /// 复制颜色到剪切板
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIsCopiedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPalette = (ColorPalette)d;
            if (colorPalette.IsCopied)
            {
                ApplicationCommands.Copy.Execute(colorPalette.rgbTextBox.Text, colorPalette.rgbTextBox);
            }
        }

        /// <summary>
        /// 启用 Alpha 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIsAlphaEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPalette = (ColorPalette)d;
            colorPalette.Alpha = 255;
        }

        /// <summary>
        /// 吸管按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EyedropperButton_Click(object sender, RoutedEventArgs e)
        {
            IsPicking = true;
            RaiseEvent(new RoutedEventArgs() { RoutedEvent = StartPickingEvent });

            var colorPixelPreview = new ColorPixelPreview();
            if (colorPixelPreview.ShowDialog() == true)
            {
                Color = colorPixelPreview.SelectedColor;
                UpdateHslFromColor();
            }

            IsPicking = false;
            RaiseEvent(new RoutedEventArgs() { RoutedEvent = CompletedPickedEvent });
        }
    }

    /// <summary>
    /// 屏幕取色器
    /// </summary>
    public class ScreenColorPicker
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int x, int y);

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>颜色</returns>
        public static Color GetColorAt(Point point)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, (int)point.X, (int)point.Y);
            ReleaseDC(IntPtr.Zero, hdc);

            return Color.FromRgb(
                (byte)(pixel & 0x000000FF),
                (byte)((pixel & 0x0000FF00) >> 8),
                (byte)((pixel & 0x00FF0000) >> 16));
        }
    }
}
