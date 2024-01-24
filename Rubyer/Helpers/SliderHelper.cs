using Rubyer.Commons.KnownBoxes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// Slider 帮助类
    /// </summary>
    public class SliderHelper
    {
        /// <summary>
        /// 拖拽圆示直径
        /// </summary>
        public static readonly DependencyProperty GripDiameterProperty = DependencyProperty.RegisterAttached(
            "GripDiameter", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        /// <summary>
        /// Sets the grip diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetGripDiameter(DependencyObject element, double value)
        {
            element.SetValue(GripDiameterProperty, value);
        }

        /// <summary>
        /// Gets the grip diameter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A double.</returns>
        public static double GetGripDiameter(DependencyObject element)
        {
            return (double)element.GetValue(GripDiameterProperty);
        }

        /// <summary>
        /// 轨道厚度
        /// </summary>
        public static readonly DependencyProperty TrackThicknessProperty = DependencyProperty.RegisterAttached(
            "TrackThickness", typeof(double), typeof(SliderHelper), new PropertyMetadata(default(double)));

        public static void SetTrackThickness(DependencyObject element, double value)
        {
            element.SetValue(TrackThicknessProperty, value);
        }

        public static double GetTrackThickness(DependencyObject element)
        {
            return (double)element.GetValue(TrackThicknessProperty);
        }

        /// <summary>
        /// 是否选择范围
        /// </summary>
        public static readonly DependencyProperty IsSelectionRangeProperty =
            DependencyProperty.RegisterAttached("IsSelectionRange", typeof(bool), typeof(SliderHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSelectionRangeChangded));

        public static bool GetIsSelectionRange(DependencyObject obj) => (bool)obj.GetValue(IsSelectionRangeProperty);

        public static void SetIsSelectionRange(DependencyObject obj, bool value) => obj.SetValue(IsSelectionRangeProperty, BooleanBoxes.Box(value));

        /// <summary>
        /// 起始按钮按下
        /// </summary>
        internal static readonly DependencyPropertyKey IsRangeButtonPressedPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsRangeButtonPressed", typeof(bool), typeof(SliderHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 起始按钮按下
        /// </summary>
        public static readonly DependencyProperty IsRangeButtonPressedProperty = IsRangeButtonPressedPropertyKey.DependencyProperty;

        public static bool GetIsRangeButtonPressed(DependencyObject obj) => (bool)obj.GetValue(IsRangeButtonPressedProperty);

        internal static void SetIsRangeButtonPressed(DependencyObject obj, bool value) => obj.SetValue(IsRangeButtonPressedPropertyKey, BooleanBoxes.Box(value));

        // 选中访问改变事件
        public static readonly RoutedEvent SelectionRangeChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectionRangeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SliderHelper));

        public static void AddSelectionRangeChangedHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is Slider slider)
            {
                slider.AddHandler(SelectionRangeChangedEvent, handler);
            }
        }

        public static void RemoveSelectionRangeChangedHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is Slider slider)
            {
                slider.RemoveHandler(SelectionRangeChangedEvent, handler);
            }
        }

        private static void OnIsSelectionRangeChangded(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && GetIsSelectionRange(slider))
            {
                if (slider.IsLoaded)
                {
                    Slider_Loaded(slider, null);
                }
                else
                {
                    slider.Loaded += Slider_Loaded;
                }
            }
        }

        private static void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            var slider = (Slider)sender;
            slider.Loaded -= Slider_Loaded;

            if (slider.Template.FindName("StartRangeButton", slider) is Button startButton)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(startButton, "PreviewMouseLeftButtonDown", RangeButton_MouseDown);
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(startButton, "PreviewMouseLeftButtonUp", RangeButton_MouseUp);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(startButton, "PreviewMouseMove", RangeButton_MouseMove);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(startButton, "MouseEnter", RangeButton_MouseEnter);
            }

            if (slider.Template.FindName("EndRangeButton", slider) is Button endButton)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(endButton, "PreviewMouseLeftButtonDown", RangeButton_MouseDown);
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(endButton, "PreviewMouseLeftButtonUp", RangeButton_MouseUp);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(endButton, "PreviewMouseMove", RangeButton_MouseMove);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(endButton, "MouseEnter", RangeButton_MouseEnter);
            }

            if (slider.Template.FindName("TrackBackground", slider) is Border backBorder)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(backBorder, "PreviewMouseLeftButtonDown", TrackBackground_MouseDown);
            }
        }

        private static void TrackBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.Equals(e.OriginalSource))
            {
                var border = (Border)sender;
                if (border.TryGetParentFromVisualTree<Slider>() is Slider slider)
                {
                    var position = e.GetPosition(border);
                    var isStart = slider.Orientation == Orientation.Horizontal ?
                                  position.X / slider.ActualWidth < slider.SelectionStart / slider.Maximum :
                                  (slider.ActualHeight - position.Y) / slider.ActualHeight < slider.SelectionStart / slider.Maximum;
                    UpdateRangeSelection(slider, position, isStart);
                }
            }
        }

        private static void RangeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            if (button.TemplatedParent is Slider slider)
            {
                UpdateToolTipOffset(button, slider);
            }
        }

        private static void RangeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            SetIsRangeButtonPressed(button, true);
        }

        private static void RangeButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            SetIsRangeButtonPressed(button, false);

            if (button.ToolTip is ToolTip toolTip)
            {
                toolTip.IsOpen = false;
            }
        }

        // 更新 ToolTip
        private static void UpdateToolTipOffset(Button button, Slider slider)
        {
            if (slider.AutoToolTipPlacement != AutoToolTipPlacement.None)
            {
                if (button.ToolTip is ToolTip toolTip)
                {
                    toolTip.PlacementTarget ??= slider;
                    toolTip.IsOpen = true;
                    var point = button.TranslatePoint(new Point(), slider);
                    if (slider.Orientation == Orientation.Horizontal)
                    {
                        toolTip.HorizontalOffset = point.X;
                        toolTip.VerticalOffset = 0;
                    }
                    else
                    {
                        toolTip.HorizontalOffset = 0;
                        toolTip.VerticalOffset = point.Y;
                    }
                }
            }
        }


        private static void UpdateRangeSelection(Slider slider, Point position, bool isStart)
        {
            var percent = slider.Orientation == Orientation.Horizontal ?
                          position.X / slider.ActualWidth :
                          (slider.ActualHeight - position.Y) / slider.ActualHeight;
            percent = Math.Max(percent, 0);
            percent = Math.Min(percent, 1);

            var value = percent * (slider.Maximum - slider.Minimum) + slider.Minimum;

            // 保持 Thumb 在刻度上
            if (slider.IsSnapToTickEnabled && slider.TickFrequency > 0)
            {
                var num = slider.Minimum + Math.Round((value - slider.Minimum) / slider.TickFrequency) * slider.TickFrequency;
                var num2 = Math.Min(slider.Maximum, num + slider.TickFrequency);

                value = value >= (num + num2) * 0.5 ? num2 : num;
            }

            // 更新 SelectionStart 和 SelectionEnd 值
            bool hasChanged;
            if (isStart)
            {
                var newStart = Math.Min(value, slider.SelectionEnd);
                hasChanged = slider.SelectionStart != newStart;
                slider.SelectionStart = newStart;
            }
            else
            {
                var newEnd = Math.Max(value, slider.SelectionStart);
                hasChanged = slider.SelectionEnd != newEnd;
                slider.SelectionEnd = newEnd;
            }

            if (hasChanged)
            {
                slider.RaiseEvent(new RoutedEventArgs(SelectionRangeChangedEvent));
            }
        }

        private static void RangeButton_MouseMove(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            if (GetIsRangeButtonPressed(button))
            {
                var slider = button.TryGetParentFromVisualTree<Slider>();
                var position = e.MouseDevice.GetPosition(slider);

                UpdateRangeSelection(slider, position, button.Name.Contains("Start"));

                UpdateToolTipOffset(button, slider);
            }
        }
    }
}