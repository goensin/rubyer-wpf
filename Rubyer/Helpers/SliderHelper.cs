using Rubyer.Commons.KnownBoxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;

namespace Rubyer
{
    /// <summary>
    /// 范围按钮类型
    /// </summary>
    internal enum RangeButtonType
    {
        /// <summary>
        /// 开始按钮
        /// </summary>
        Start,

        /// <summary>
        /// 结束按钮
        /// </summary>
        End,

        /// <summary>
        /// 范围按钮
        /// </summary>
        Selection,
    }

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
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(startButton, "MouseLeave", RangeButton_MouseLeave);
            }

            if (slider.Template.FindName("EndRangeButton", slider) is Button endButton)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(endButton, "PreviewMouseLeftButtonDown", RangeButton_MouseDown);
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(endButton, "PreviewMouseLeftButtonUp", RangeButton_MouseUp);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(endButton, "PreviewMouseMove", RangeButton_MouseMove);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(endButton, "MouseEnter", RangeButton_MouseEnter);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(endButton, "MouseLeave", RangeButton_MouseLeave);
            }

            if (slider.Template.FindName("PART_SelectionRange", slider) is Button rangeButton)
            {
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(rangeButton, "PreviewMouseLeftButtonDown", RangeButton_MouseDown);
                WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(rangeButton, "PreviewMouseLeftButtonUp", RangeButton_MouseUp);
                WeakEventManager<UIElement, MouseEventArgs>.AddHandler(rangeButton, "PreviewMouseMove", RangeButton_MouseMove);
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
                    RangeButtonType buttonType = isStart ? RangeButtonType.Start : RangeButtonType.End;
                    UpdateRangeSelection(slider, position, buttonType);
                }
            }
        }

        private static void RangeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (element.TemplatedParent is Slider slider)
            {
                UpdateToolTipOffset(element, slider);
            }
        }

        private static void RangeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.ToolTip is ToolTip toolTip)
            {
                toolTip.IsOpen = false;
            }
        }

        private static void RangeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            SetIsRangeButtonPressed(element, true);

            var slider = element.TryGetParentFromVisualTree<Slider>();
            if (slider.Template.FindName("StartRangeButton", slider) is Button startButton && startButton.ToolTip is ToolTip startToolTip)
            {
                startToolTip.IsOpen = true;
            }

            if (slider.Template.FindName("EndRangeButton", slider) is Button endButton && endButton.ToolTip is ToolTip endToolTip)
            {
                endToolTip.IsOpen = true;
            }
        }

        private static void RangeButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            SetIsRangeButtonPressed(element, false);

            if (element is Button button && button.ToolTip is ToolTip toolTip)
            {
                toolTip.IsOpen = false;
            }

            var slider = element.TryGetParentFromVisualTree<Slider>();
            if (slider.Template.FindName("StartRangeButton", slider) is Button startButton && startButton.ToolTip is ToolTip startToolTip)
            {
                startToolTip.IsOpen = false;
            }

            if (slider.Template.FindName("EndRangeButton", slider) is Button endButton && endButton.ToolTip is ToolTip endToolTip)
            {
                endToolTip.IsOpen = false;
            }
        }

        // 更新 ToolTip
        private static void UpdateToolTipOffset(FrameworkElement element, Slider slider)
        {
            if (slider.AutoToolTipPlacement != AutoToolTipPlacement.None)
            {
                if (element.ToolTip is ToolTip toolTip)
                {
                    toolTip.PlacementTarget ??= slider;
                    toolTip.IsOpen = true;
                    var point = element.TranslatePoint(new Point(), slider);
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

        /// <summary>
        /// 保持 Thumb 在刻度上
        /// </summary>
        private static double GetTickValue(Slider slider, double value)
        {
            double tickValue = value;
            if (slider.IsSnapToTickEnabled && slider.TickFrequency > 0)
            {
                var num = slider.Minimum + Math.Round((value - slider.Minimum) / slider.TickFrequency) * slider.TickFrequency;
                var num2 = Math.Min(slider.Maximum, num + slider.TickFrequency);

                tickValue = value >= (num + num2) * 0.5 ? num2 : num;
            }

            return tickValue;
        }

        private static void UpdateRangeSelection(Slider slider, Point position, RangeButtonType buttonType)
        {
            var percent = slider.Orientation == Orientation.Horizontal ?
                          position.X / slider.ActualWidth :
                          (slider.ActualHeight - position.Y) / slider.ActualHeight;
            percent = Math.Min(Math.Max(percent, 0), 1);

            var value = percent * (slider.Maximum - slider.Minimum) + slider.Minimum;

            value = GetTickValue(slider, value);

            // 更新 SelectionStart 和 SelectionEnd 值
            bool hasChanged = false;
            switch (buttonType)
            {
                case RangeButtonType.Start:
                    var newStart = Math.Min(value, slider.SelectionEnd);
                    hasChanged = slider.SelectionStart != newStart;
                    slider.SelectionStart = newStart;
                    break;
                case RangeButtonType.End:
                    var newEnd = Math.Max(value, slider.SelectionStart);
                    hasChanged = slider.SelectionEnd != newEnd;
                    slider.SelectionEnd = newEnd;
                    break;
                case RangeButtonType.Selection:
                    var range = slider.SelectionEnd - slider.SelectionStart;
                    var halfRange = range / 2;
                    value = (value - halfRange) >= slider.Minimum ? value : slider.Minimum + halfRange;
                    value = (value + halfRange) <= slider.Maximum ? value : slider.Maximum - halfRange;
                    var start = GetTickValue(slider, Math.Min(value - halfRange, slider.SelectionEnd));
                    var end = GetTickValue(slider, Math.Max(value + halfRange, slider.SelectionStart));
                    if (start >= slider.Minimum && end <= slider.Maximum)
                    {
                        if (start < slider.SelectionStart)
                        {
                            // 向起始位置偏移
                            slider.SelectionStart = start;
                            slider.SelectionEnd = start + range;
                            hasChanged = true;
                        }
                        else if (end > slider.SelectionEnd)
                        {
                            // 向结束位置偏移
                            slider.SelectionEnd = end;
                            slider.SelectionStart = end - range;
                            hasChanged = true;
                        }
                    }
                    break;
            }

            if (hasChanged)
            {
                slider.RaiseEvent(new RoutedEventArgs(SelectionRangeChangedEvent));
            }
        }

        private static void RangeButton_MouseMove(object sender, MouseEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (GetIsRangeButtonPressed(element))
            {
                var slider = element.TryGetParentFromVisualTree<Slider>();
                var position = e.MouseDevice.GetPosition(slider);

                RangeButtonType buttonType;
                if (element.Name.Contains("Start"))
                {
                    buttonType = RangeButtonType.Start;
                }
                else if (element.Name.Contains("End"))
                {
                    buttonType = RangeButtonType.End;
                }
                else if (element.Name.Contains("Selection"))
                {
                    buttonType = RangeButtonType.Selection;
                }
                else
                {
                    return;
                }

                UpdateRangeSelection(slider, position, buttonType);


                if (slider.Template.FindName("StartRangeButton", slider) is Button startButton)
                {
                    UpdateToolTipOffset(startButton, slider);
                }

                if (slider.Template.FindName("EndRangeButton", slider) is Button endButton)
                {
                    UpdateToolTipOffset(endButton, slider);
                }
            }
        }
    }
}