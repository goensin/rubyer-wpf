﻿using Rubyer.Commons.KnownBoxes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    /// <summary>
    /// 消息框容器
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = RootGridName, Type = typeof(Grid))]
    public class MessageBoxContainer : ContentControl
    {
        const string TransitionPartName = "Path_Transition";
        const string RootGridName = "PART_RootGrid";

        private List<FrameworkElement> focusableElements; // Content 内 focusable 元素，用于打开弹窗使其失效
        private Grid rootGrid;

        static MessageBoxContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxContainer), new FrameworkPropertyMetadata(typeof(MessageBoxContainer)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TransitionPartName) is Transition transition)
            {
                transition.Showed += (sender, e) => IsClosed = false;
                transition.Closed += (sender, e) => IsClosed = true;
            }

            rootGrid = (Grid)GetTemplateChild(RootGridName);

            focusableElements = [];
        }

        /// <summary>
        /// 标识
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageBoxContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxContainer container)
            {
                string identify = e.NewValue.ToString();
                MessageBoxR.UpdateContainer(container, identify);
            }
        }

        /// <summary>
        /// 遮罩背景色
        /// </summary>
        public static readonly DependencyProperty MaskBackgroundProperty = DependencyProperty.Register(
            "MaskBackground", typeof(Brush), typeof(MessageBoxContainer), new PropertyMetadata(default(Brush)));

        /// <summary>
        ///  遮罩背景色
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            set { SetValue(MaskBackgroundProperty, value); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
           DependencyProperty.Register("IsShow", typeof(bool), typeof(MessageBoxContainer), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = d as MessageBoxContainer;

            if (container.IsShow)
            {
                var dialogContent = container.Content as FrameworkElement;
                dialogContent.ForEachVisualChild(x =>
                {
                    if (x is FrameworkElement element && element.Focusable)
                    {
                        element.Focusable = false;
                        container.focusableElements.Add(element);
                    }
                });
            }
            else
            {
                container.focusableElements.ForEach(x => x.Focusable = true);
                container.focusableElements.Clear();
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 是否关闭后
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty =
           DependencyProperty.Register("IsClosed", typeof(bool), typeof(MessageBoxContainer), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 是否关闭后
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 添加消息框卡片
        /// </summary>
        /// <param name="card">消息框卡片</param>
        public void AddCard(MessageBoxCard card)
        {
            card.Closing += Card_Closing;
            card.Closed += Card_Closed; ;
            rootGrid.Children.Add(card);
            IsShow = true;
        }

        private void Card_Closing(object sender, RoutedEventArgs e)
        {
            var card = (MessageBoxCard)sender;
            card.Closing -= Card_Closing;
            if (rootGrid.Children.Count <= 1)
            {
                IsShow = false;
            }
        }

        private void Card_Closed(object sender, RoutedEventArgs e)
        {
            var card = (MessageBoxCard)sender;
            card.Closed -= Card_Closed;
            rootGrid.Children.Remove(card);
        }
    }
}