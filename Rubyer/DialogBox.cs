﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// 对话框
    /// </summary>
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = RootBorderPartName, Type = typeof(Border))]
    [TemplateVisualState(GroupName = "ShowStates", Name = OpenStateName)]
    [TemplateVisualState(GroupName = "ShowStates", Name = CloseStateName)]
    public class DialogBox : ContentControl
    {
        public const string CloseButtonPartName = "PART_CloseButton";
        public const string RootBorderPartName = "PART_RootBorder";
        public const string OpenStateName = "Open";
        public const string CloseStateName = "Close";

        public Action<DialogBox> BeforeOpenHandler;
        public Action<DialogBox, object> AfterCloseHandler;

        private Border rootBorder;
        private object closeParameter;

        static DialogBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogBox), new FrameworkPropertyMetadata(typeof(DialogBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _ = CommandBindings.Add(new CommandBinding(CloseDialogCommand, CloseDialogHandler));
            _ = CommandBindings.Add(new CommandBinding(OpenDialogCommand, OpenDialogHandler));

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                closeParameter = null;
                IsShow = false;
            };

            rootBorder = GetTemplateChild(RootBorderPartName) as Border;
            rootBorder.MouseLeftButtonDown += RootBorder_MouseLeftButtonDown;
        }

        private void OpenDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            IsShow = true;
        }

        private void CloseDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            closeParameter = e.Parameter;
            IsShow = false;
        }

        private void RootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Border border)
            {
                if (IsClickBackgroundToClose && sender.Equals(border))
                {
                    closeParameter = null;
                    IsShow = false;
                }
            }
        }

        #region 命令
        public static RoutedCommand CloseDialogCommand = new RoutedCommand();
        public static RoutedCommand OpenDialogCommand = new RoutedCommand();

        // 打开前命令
        public static readonly DependencyProperty BeforeOpenCommandProperty = DependencyProperty.Register(
            "BeforeOpenCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand BeforeOpenCommand
        {
            get { return (ICommand)GetValue(BeforeOpenCommandProperty); }
            set { SetValue(BeforeOpenCommandProperty, value); }
        }

        // 关闭后命令
        public static readonly DependencyProperty AfterCloseCommandProperty = DependencyProperty.Register(
            "AfterCloseCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand AfterCloseCommand
        {
            get { return (ICommand)GetValue(AfterCloseCommandProperty); }
            set { SetValue(AfterCloseCommandProperty, value); }
        }
        #endregion

        #region 事件
        // 打开前事件
        public static readonly RoutedEvent BeforeOpenEvent = EventManager.RegisterRoutedEvent(
            "BeforeOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DialogBox));

        public event RoutedEventHandler BeforeOpen
        {
            add { AddHandler(BeforeOpenEvent, value); }
            remove { RemoveHandler(BeforeOpenEvent, value); }
        }

        // 关闭后事件
        public static readonly RoutedEvent AfterCloseEvent = EventManager.RegisterRoutedEvent(
            "AfterClose", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(DialogBox));

        public event RoutedPropertyChangedEventHandler<object> AfterClose
        {
            add { AddHandler(AfterCloseEvent, value); }
            remove { RemoveHandler(AfterCloseEvent, value); }
        }
        #endregion

        #region 依赖属性
        // 标识
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(
            "Identifier", typeof(string), typeof(DialogBox), new PropertyMetadata(default(string), OnIdentifierChanged));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox dialogBox = d as DialogBox;
            string identifier = e.NewValue.ToString();
            Dialog.AddDialogBox(identifier, dialogBox);
        }

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        // 对话框内容
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            "DialogContent", typeof(object), typeof(DialogBox), new PropertyMetadata(default(object)));

        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        // 标题
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DialogBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(DialogBox), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // 是否显示关闭按钮
        public static readonly DependencyProperty IsShowCloseButtonProperty = DependencyProperty.Register(
            "IsShowCloseButton", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsShowCloseButton
        {
            get { return (bool)GetValue(IsShowCloseButtonProperty); }
            set { SetValue(IsShowCloseButtonProperty, value); }
        }

        // 是否点击背景关闭弹窗
        public static readonly DependencyProperty IsClickBackgroundToCloseProperty = DependencyProperty.Register(
            "IsClickBackgroundToClose", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsClickBackgroundToClose
        {
            get { return (bool)GetValue(IsClickBackgroundToCloseProperty); }
            set { SetValue(IsClickBackgroundToCloseProperty, value); }
        }

        // 是否显示
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(
            "IsShow", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox container = d as DialogBox;

            if ((bool)e.NewValue)
            {
                container.OpenAnimiation(container);
            }
            else
            {
                container.CloseAnimaton();
            }
        }

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // 标题字体大小
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            "TitleFontSize", typeof(double), typeof(DialogBox), new PropertyMetadata(default(double)));

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }

        // 标题水平对齐
        public static readonly DependencyProperty TitleHorizontalAlignmentProperty = DependencyProperty.Register(
            "TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(DialogBox), new PropertyMetadata(default(HorizontalAlignment)));

        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set { SetValue(TitleHorizontalAlignmentProperty, value); }
        }

        // 关闭完成
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(DialogBox), new PropertyMetadata(default(bool), OnIsClosedChanged));

        private static void OnIsClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DialogBox dialog)
            {
                RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(null, dialog.closeParameter);
                args.RoutedEvent = AfterCloseEvent;
                dialog.RaiseEvent(args);
                dialog.AfterCloseCommand?.Execute(dialog.closeParameter);
                dialog.AfterCloseHandler?.Invoke(dialog, dialog.closeParameter);

                dialog.closeParameter = null;
                dialog.BeforeOpenHandler = null;
                dialog.AfterCloseHandler = null;
            }
        }

        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }
        #endregion

        // 打开对话框动作
        private void OpenAnimiation(DialogBox container)
        {
            RoutedEventArgs args = new RoutedEventArgs(BeforeOpenEvent);
            container.RaiseEvent(args);
            container.BeforeOpenCommand?.Execute(null);
            container.BeforeOpenHandler?.Invoke(container);

            GoToOpenState(container);
            _ = container.Focus();
        }

        // 关闭对话框动作
        private void CloseAnimaton()
        {
            GoToCloseState(this);
        }

        private static void GoToOpenState(FrameworkElement element)
        {
            _ = VisualStateManager.GoToState(element, OpenStateName, true);
        }

        private static void GoToCloseState(FrameworkElement element)
        {
            _ = VisualStateManager.GoToState(element, CloseStateName, true);
        }
    }
}
