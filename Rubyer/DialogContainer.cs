using Rubyer.Commons.KnownBoxes;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System;

namespace Rubyer
{
    /// <summary>
    /// 消息框容器
    /// </summary>
    [TemplatePart(Name = TransitionPartName, Type = typeof(Transition))]
    [TemplatePart(Name = RootGridName, Type = typeof(Grid))]
    public class DialogContainer : ContentControl
    {
        const string TransitionPartName = "Path_BackgroundTransition";
        const string RootGridName = "PART_RootGrid";

        private List<FrameworkElement> focusableElements; // Content 内 focusable 元素，用于打开弹窗使其失效
        private Grid rootGrid;

        /// <summary>
        /// 消息框结果事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DialogResultRoutedEventHandler(object sender, DialogResultRoutedEventArgs e);

        static DialogContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContainer), new FrameworkPropertyMetadata(typeof(DialogContainer)));
        }

        #region 命令

        /// <summary>
        /// 关闭对话框命令
        /// </summary>
        public static RoutedCommand CloseDialogCommand = new RoutedCommand();

        /// <summary>
        /// 打开对话框命令
        /// </summary>
        public static RoutedCommand OpenDialogCommand = new RoutedCommand();

        /// <summary>
        /// 打开前命令
        /// </summary>
        public static readonly DependencyProperty BeforeOpenCommandProperty = DependencyProperty.Register(
            "BeforeOpenCommand", typeof(ICommand), typeof(DialogContainer));

        /// <summary>
        /// 打开前命令
        /// </summary>
        public ICommand BeforeOpenCommand
        {
            get { return (ICommand)GetValue(BeforeOpenCommandProperty); }
            set { SetValue(BeforeOpenCommandProperty, value); }
        }

        /// <summary>
        /// 关闭后命令
        /// </summary>
        public static readonly DependencyProperty AfterCloseCommandProperty = DependencyProperty.Register(
            "AfterCloseCommand", typeof(ICommand), typeof(DialogContainer));

        /// <summary>
        /// 关闭后命令
        /// </summary>
        public ICommand AfterCloseCommand
        {
            get { return (ICommand)GetValue(AfterCloseCommandProperty); }
            set { SetValue(AfterCloseCommandProperty, value); }
        }

        #endregion 命令

        #region 事件

        /// <summary>
        /// 打开前事件
        /// </summary>
        public static readonly RoutedEvent BeforeOpenEvent = EventManager.RegisterRoutedEvent("BeforeOpen", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DialogContainer));

        /// <summary>
        /// 打开前事件
        /// </summary>
        public event RoutedEventHandler BeforeOpen
        {
            add { AddHandler(BeforeOpenEvent, value); }
            remove { RemoveHandler(BeforeOpenEvent, value); }
        }

        /// <summary>
        /// 关闭后事件
        /// </summary>
        public static readonly RoutedEvent AfterCloseEvent = EventManager.RegisterRoutedEvent("AfterClose", RoutingStrategy.Direct, typeof(DialogResultRoutedEventHandler), typeof(DialogContainer));

        /// <summary>
        /// 关闭后事件
        /// </summary>
        public event DialogResultRoutedEventHandler AfterClose
        {
            add { AddHandler(AfterCloseEvent, value); }
            remove { RemoveHandler(AfterCloseEvent, value); }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 标识
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register("Identifier", typeof(string), typeof(DialogContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register("IsShow", typeof(bool), typeof(DialogContainer), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsShowChanged));

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, BooleanBoxes.Box(value)); }
        }

        #endregion

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(CloseDialogCommand, CloseDialogHandler));
            CommandBindings.Add(new CommandBinding(OpenDialogCommand, OpenDialogHandler));

            rootGrid = (Grid)GetTemplateChild(RootGridName);

            focusableElements = [];

            KeyDown += DialogContainer_KeyDown;
        }

        private void DialogContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                var dialogCard = GetLastDialogCard();
                dialogCard?.PressedEscToClose();
            }
        }

        private DialogCard GetLastDialogCard()
        {
            var count = rootGrid.Children.Count;
            if (count > 0 && rootGrid.Children[count - 1] is DialogCard dialogCard)
            {
                return dialogCard;
            }

            return null;
        }

        private void OpenDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            IsShow = true;
        }

        private void CloseDialogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogCard = GetLastDialogCard();
            dialogCard?.Close(e.Parameter);
        }

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogContainer dialog = (DialogContainer)d;
            Dialog.UpdateContainer(dialog, dialog.Identifier);
        }

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = d as DialogContainer;

            if (container.IsShow)
            {
                if (container.Content is FrameworkElement dialogContent)
                {
                    dialogContent.ForEachVisualChild(x =>
                    {
                        if (x is FrameworkElement element && element.Focusable)
                        {
                            element.Focusable = false;
                            container.focusableElements.Add(element);
                        }
                    });
                }
            }
            else
            {
                container.focusableElements.ForEach(x => x.Focusable = true);
                container.focusableElements.Clear();
            }
        }

        /// <summary>
        /// 添加消息框卡片
        /// </summary>
        /// <param name="dialogCard">消息框卡片</param>
        public void AddCard(DialogCard dialogCard)
        {
            dialogCard.BeforeOpenHandler += Card_Opened;
            dialogCard.Closing += Card_Closing;
            dialogCard.Closed += Card_Closed;
            rootGrid.Children.Add(dialogCard);
            IsShow = true;
        }

        private void Card_Opened(DialogCard card)
        {
            card.BeforeOpenHandler -= Card_Opened;
            RaiseEvent(new RoutedEventArgs(BeforeOpenEvent));
            BeforeOpenCommand?.Execute(null);
        }

        private void Card_Closing(object sender, DialogClosingRoutedEventArgs e)
        {
            var dialogCard = (DialogCard)sender;
            dialogCard.Closing -= Card_Closing;
            if (rootGrid.Children.Count <= 1)
            {
                IsShow = false;
            }
        }

        private void Card_Closed(object sender, DialogResultRoutedEventArgs e)
        {
            var dialogCard = (DialogCard)sender;
            dialogCard.Closed -= Card_Closed;

            var index = rootGrid.Children.IndexOf(dialogCard);
            if (index >= 0)
            {
                rootGrid.Children.RemoveAt(index);
            }

            var count = rootGrid.Children.Count;
            if (count > 0)
            {
                rootGrid.Children[count - 1].Focus();
            }

            AfterCloseCommand?.Execute(dialogCard.CloseParameter);
            RaiseEvent(new DialogResultRoutedEventArgs(AfterCloseEvent, dialogCard.CloseParameter, dialogCard));
        }
    }
}
