﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubyer
{
    public static class ListBoxHelper
    {
        // 聚焦时颜色
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(SolidColorBrush), typeof(ListBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedBrushProperty);
        }

        public static void SetFocusedBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedBrushProperty, value);
        }

        // 聚焦前景色时颜色
        public static readonly DependencyProperty FocusedForegroundBrushProperty =
            DependencyProperty.RegisterAttached("FocusedForegroundBrush", typeof(SolidColorBrush), typeof(ListBoxHelper), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static SolidColorBrush GetFocusedForegroundBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(FocusedForegroundBrushProperty);
        }

        public static void SetFocusedForegroundBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(FocusedForegroundBrushProperty, value);
        }

        // 选中动画
        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.RegisterAttached("IsAnimation", typeof(bool), typeof(ListBoxHelper), new PropertyMetadata(false, OnIsAnimationChanged));

        private static void OnIsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.Loaded += (sender, arg) =>
                {
                    if (listBox.Template.FindName("currentRectangle", listBox) is Rectangle rectangle)
                    {
                        if (GetIsAnimation(listBox))
                        {
                            listBox.SelectionChanged += (a, b) =>
                            {
                                int index = listBox.SelectedIndex;
                                double top = GetListBoxItemTop(listBox, index);
                                ListBoxItem item = listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items[index]) as ListBoxItem;
                                rectangle.Height = item.ActualHeight;

                                DoubleAnimation topAnimation = new DoubleAnimation
                                {
                                    To = top,
                                    Duration = TimeSpan.FromMilliseconds(500),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                DoubleAnimation heightAnimation = new DoubleAnimation
                                {
                                    To = item.ActualHeight,
                                    Duration = TimeSpan.FromMilliseconds(500),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                rectangle.BeginAnimation(Canvas.TopProperty, topAnimation);
                                rectangle.BeginAnimation(FrameworkElement.HeightProperty, heightAnimation);
                            };

                            if (listBox.SelectedIndex >= 0)
                            {
                                int index = listBox.SelectedIndex;
                                double top = GetListBoxItemTop(listBox, index);
                                ListBoxItem item = listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items[index]) as ListBoxItem;
                                rectangle.Height = item.ActualHeight;
                                rectangle.SetValue(Canvas.TopProperty, top);
                            }
                        }
                    }
                };
            }
        }

        // 获取 item 所在纵向位置
        private static double GetListBoxItemTop(ListBox listBox, int index)
        {
            double top = 0;

            for (int i = 0; i < index; i++)
            {
                ListBoxItem item = listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items[i]) as ListBoxItem;
                top += item.ActualHeight;
            }

            return top;
        }

        public static bool GetIsAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAnimationProperty);
        }

        public static void SetIsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAnimationProperty, value);
        }
    }
}
