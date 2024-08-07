﻿using Rubyer;
using Rubyer.Commons.KnownBoxes;
using System.Windows;

namespace Rubyer
{
    /// <summary>
    /// Button 帮助类
    /// </summary>
    public class ButtonHelper
    {
        /// <summary>
        /// 显示阴影
        /// </summary>
        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.RegisterAttached(
            "ShowShadow", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Sets the show shadow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetShowShadow(DependencyObject element, bool value)
        {
            element.SetValue(ShowShadowProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Gets the show shadow.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowShadow(DependencyObject element)
        {
            return (bool)element.GetValue(ShowShadowProperty);
        }

        /// <summary>
        /// 形状
        /// </summary>
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.RegisterAttached(
            "Shape", typeof(ButtonShape), typeof(ButtonHelper), new PropertyMetadata(default(ButtonShape)));

        /// <summary>
        /// Sets the shape.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetShape(DependencyObject element, ButtonShape value)
        {
            element.SetValue(ShapeProperty, value);
        }

        /// <summary>
        /// Gets the shape.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A ButtonShape.</returns>
        public static ButtonShape GetShape(DependencyObject element)
        {
            return (ButtonShape)element.GetValue(ShapeProperty);
        }

        /// <summary>
        /// 加载中
        /// </summary>
        public static readonly DependencyProperty LoadingProperty = DependencyProperty.RegisterAttached(
            "Loading", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// Sets the loading.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetLoading(DependencyObject element, bool value)
        {
            element.SetValue(LoadingProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Gets the loading.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static bool GetLoading(DependencyObject element)
        {
            return (bool)element.GetValue(LoadingProperty);
        }

        /// <summary>
        /// 加载中内容
        /// </summary>
        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.RegisterAttached(
            "LoadingContent", typeof(object), typeof(ButtonHelper), new PropertyMetadata(null));

        /// <summary>
        /// Sets the loading content.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">If true, value.</param>
        public static void SetLoadingContent(DependencyObject element, object value)
        {
            element.SetValue(LoadingContentProperty, value);
        }

        /// <summary>
        /// Gets the loading content.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A bool.</returns>
        public static object GetLoadingContent(DependencyObject element)
        {
            return (object)element.GetValue(LoadingContentProperty);
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.RegisterAttached(
            "IconType", typeof(IconType?), typeof(ButtonHelper), new PropertyMetadata(null));

        public static void SetIconType(DependencyObject element, IconType? value)
        {
            element.SetValue(IconTypeProperty, value);
        }

        public static IconType? GetIconType(DependencyObject element)
        {
            return (IconType?)element.GetValue(IconTypeProperty);
        }

    }
}