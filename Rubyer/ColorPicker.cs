using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Rubyer
{
    /// <summary>
    /// 颜色选择器
    /// </summary>
    [TemplatePart(Name = ColorPalettePartName, Type = typeof(ColorPalette))]
    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    public class ColorPicker : Control
    {
        /// <summary>
        /// 调色板名称
        /// </summary>
        public const string ColorPalettePartName = "PART_ColorPalette";

        /// <summary>
        /// 弹窗名称
        /// </summary>
        public const string PopupPartName = "PART_Popup";

        private ColorPalette colorPalette; // 调色板
        private Popup popup; // 弹出框

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        #region 事件

        /// <summary>
        /// 颜色改变
        /// </summary>
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent(
            "ColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPicker));

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
            "StartPicking", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));

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
            "CompletedPicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));

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
            DependencyProperty.Register("Text", typeof(string), typeof(ColorPicker), new PropertyMetadata(null));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(ColorPicker), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDropDownOpenChanged));

        /// <summary>
        /// 弹窗打开
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 可选颜色集合
        /// </summary>
        public static readonly DependencyProperty OptionalColorsProperty =
            DependencyProperty.Register("OptionalColors", typeof(ObservableCollection<Color>), typeof(ColorPicker), new PropertyMetadata(new ObservableCollection<Color>(), null, CoerceOptionalColors));

        private static object CoerceOptionalColors(DependencyObject d, object baseValue)
        {
            return baseValue is ObservableCollection<Color> ? baseValue : new ObservableCollection<Color>();
        }

        /// <summary>
        /// 可选颜色集合
        /// </summary>
        public ObservableCollection<Color> OptionalColors
        {
            get { return (ObservableCollection<Color>)GetValue(OptionalColorsProperty); }
            set { SetValue(OptionalColorsProperty, value); }
        }

        /// <inheritdoc/> 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            colorPalette = (ColorPalette)GetTemplateChild(ColorPalettePartName);
            colorPalette.ColorChanged += ColorPalette_ColorChanged;
            colorPalette.StartPicking += ColorPalette_StartPicking;
            colorPalette.CompletedPicked += ColorPalette_CompletedPicked;

            popup = (Popup)GetTemplateChild(PopupPartName);
            popup.Opened += Popup_Opened;
            popup.Closed += Popup_Closed;
            if (IsDropDownOpen)
            {
                popup.IsOpen = true;
            }
        }

        /// <summary>
        /// 颜色改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPalette_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color = e.NewValue;

            RaiseEvent(new RoutedPropertyChangedEventArgs<Color>(e.OldValue, e.NewValue) { RoutedEvent = ColorChangedEvent });
        }

        /// <summary>
        /// 开始颜色拾取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPalette_StartPicking(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs() { RoutedEvent = StartPickingEvent, Source = e.Source });
        }

        /// <summary>
        /// 完成颜色拾取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ColorPalette_CompletedPicked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs() { RoutedEvent = CompletedPickedEvent, Source = e.Source });

            await Task.Delay(TimeSpan.FromSeconds(0.1));
            IsDropDownOpen = true;
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPicker)d;
            bool isOpen = (bool)e.NewValue;
            if (colorPicker.popup != null && colorPicker.popup.IsOpen != isOpen)
            {
                colorPicker.popup.IsOpen = isOpen;
                if (isOpen)
                {
                    colorPicker.Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
                    {
                        colorPicker.colorPalette.Focus();
                    });
                }
            }
        }

        /// <summary>
        /// popup 打开
        /// </summary>
        private void Popup_Opened(object sender, EventArgs e)
        {
            colorPalette.UpdateColor(Color);
            colorPalette.OptionalColors = OptionalColors;
        }

        /// <summary>
        /// popup 关闭
        /// </summary>
        private void Popup_Closed(object sender, EventArgs e)
        {
            if (colorPalette.IsKeyboardFocusWithin)
            {
                colorPalette.Focus();
            }

            IsDropDownOpen = false;
        }
    }
}
