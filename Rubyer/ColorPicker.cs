using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        private ColorPalette colorPalette; // 调色板
        private Popup popup; // 弹出框

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
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(default(Color)));

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

            popup = (Popup)GetTemplateChild(PopupPartName);
            popup.Opened += Popup_Opened;
            popup.Closed += Popup_Closed;
            if (IsDropDownOpen)
            {
                popup.IsOpen = true;
            }
        }

        private void ColorPalette_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color = e.NewValue;
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
