using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rubyer
{
    public class DialogBox : ContentControl
    {
        public const string CloseButtonPartName = "PART_CloseButton";
        public const string RootBorderPartName = "PART_RootBorder";
        public const string CardBorderPartName = "PART_CardBorder";

        public static Dictionary<string, DialogBox> dialogs = new Dictionary<string, DialogBox>();

        private Action<DialogBox> beforeOpenHandler;
        private Action<DialogBox, object> afterCloseHandler;

        private static readonly Color containerBackgroun = Color.FromArgb(0xAA, 0, 0, 0);
        private Border rootBorder;
        private Border cardBorder;
        private object closeParameter;

        static DialogBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogBox), new FrameworkPropertyMetadata(typeof(DialogBox)));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(CloseDialogCommand, CloseDialogHandler));
            CommandBindings.Add(new CommandBinding(OpenDialogCommand, OpenDialogHandler));

            ButtonBase closeButton = GetTemplateChild(CloseButtonPartName) as ButtonBase;
            closeButton.Click += (sender, args) =>
            {
                closeParameter = null;
                IsShow = false;
            };

            rootBorder = GetTemplateChild(RootBorderPartName) as Border;
            cardBorder = GetTemplateChild(CardBorderPartName) as Border;
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

        #region 命令
        public static RoutedCommand CloseDialogCommand = new RoutedCommand();
        public static RoutedCommand OpenDialogCommand = new RoutedCommand();

        // 打开前命令
        public static readonly DependencyProperty BeforeOpenCommandProperty =
            DependencyProperty.Register("BeforeOpenCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand BeforeOpenCommand
        {
            get { return (ICommand)GetValue(BeforeOpenCommandProperty); }
            set { SetValue(BeforeOpenCommandProperty, value); }
        }

        // 关闭后命令
        public static readonly DependencyProperty AfterCloseCommandProperty =
            DependencyProperty.Register("AfterCloseCommand", typeof(ICommand), typeof(DialogBox));

        public ICommand AfterCloseCommand
        {
            get { return (ICommand)GetValue(AfterCloseCommandProperty); }
            set { SetValue(AfterCloseCommandProperty, value); }
        }
        #endregion

        #region 事件
        public static readonly RoutedEvent BeforeOpenEvent =
            EventManager.RegisterRoutedEvent("BeforeOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DialogBox));

        public event RoutedEventHandler BeforeOpen
        {
            add { AddHandler(BeforeOpenEvent, value); }
            remove { RemoveHandler(BeforeOpenEvent, value); }
        }


        public static readonly RoutedEvent AfterCloseEvent =
            EventManager.RegisterRoutedEvent("AfterClose", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(DialogBox));

        public event RoutedPropertyChangedEventHandler<object> AfterClose
        {
            add { AddHandler(AfterCloseEvent, value); }
            remove { RemoveHandler(AfterCloseEvent, value); }
        }
        #endregion

        #region 依赖属性

        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(DialogBox), new PropertyMetadata(default(string), OnIdentifierChanged));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox dialog = d as DialogBox;
            string identify = e.NewValue.ToString();

            if (dialogs.ContainsKey(identify))
            {
                dialogs.Remove(identify);
            }

            dialogs.Add(identify, dialog);
        }

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DialogBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DialogBox), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        public static readonly DependencyProperty IsShowCloseButtonProperty =
            DependencyProperty.Register("IsShowCloseButton", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsShowCloseButton
        {
            get { return (bool)GetValue(IsShowCloseButtonProperty); }
            set { SetValue(IsShowCloseButtonProperty, value); }
        }


        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(DialogBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowChanged));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogBox container = d as DialogBox;

            if ((bool)e.NewValue)
            {
                RoutedEventArgs args = new RoutedEventArgs(DialogBox.BeforeOpenEvent);
                container.RaiseEvent(args);
                container.BeforeOpenCommand?.Execute(null);
                container.beforeOpenHandler?.Invoke(container);

                // 卡片动画
                Storyboard cardStoryboard = new Storyboard();

                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                DoubleAnimation transformAnimation = new DoubleAnimation
                {
                    From = 50,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTargetProperty(transformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                cardStoryboard.Children.Add(opacityAnimation);
                cardStoryboard.Children.Add(transformAnimation);

                // 背景动画
                ColorAnimation backgroundAnimation = new ColorAnimation
                {
                    From = Colors.Transparent,
                    To = containerBackgroun,
                    Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                backgroundAnimation.Completed += (a, b) =>
                {
                    container.cardBorder.Visibility = Visibility.Visible;
                    container.cardBorder.BeginStoryboard(cardStoryboard);
                    container.cardBorder.Focus();
                };

                container.rootBorder.Visibility = Visibility.Visible;
                container.rootBorder.Background = new SolidColorBrush(Colors.Transparent);
                container.rootBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnimation);
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

        #endregion

        // 关闭对话框动作
        private void CloseAnimaton()
        {
            // 退出动画
            Storyboard exitStoryboard = new Storyboard();

            DoubleAnimation exitOpacityAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(exitOpacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));

            DoubleAnimation exitTransformAnimation = new DoubleAnimation
            {
                From = 0,
                To = 50,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTargetProperty(exitTransformAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            exitStoryboard.Children.Add(exitOpacityAnimation);
            exitStoryboard.Children.Add(exitTransformAnimation);

            // 背景动画
            ColorAnimation backgroundAnimation = new ColorAnimation
            {
                To = Colors.Transparent,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            // 退出动画完成
            exitStoryboard.Completed += (a, b) =>
            {
                rootBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnimation);
                cardBorder.Visibility = Visibility.Hidden;

                RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(null, this.closeParameter);
                args.RoutedEvent = DialogBox.AfterCloseEvent;
                this.RaiseEvent(args);
                this.AfterCloseCommand?.Execute(this.closeParameter);
                this.afterCloseHandler?.Invoke(this, this.closeParameter);

                this.closeParameter = null;
                this.beforeOpenHandler = null;
                this.afterCloseHandler = null;
            };

            // 背景动画完成
            backgroundAnimation.Completed += (a, b) =>
            {
                rootBorder.Visibility = Visibility.Hidden;
            };

            cardBorder.BeginStoryboard(exitStoryboard);    // 执行动画
        }

        /// <summary>
        /// 显示指定对话框
        /// </summary>
        /// <param name="identifier">DialogBox 标识</param>
        /// <param name="content">内容</param>
        /// <param name="openHandler">打开前处理程序</param>
        /// <param name="closeHandle">关闭后处理程序</param>
        /// <param name="title">标题</param>
        /// <param name="isShowCloseButton">是否显示默认关闭按钮</param>
        public static void Show(string identifier, object content, string title, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle, bool isShowCloseButton)
        {
            if (!dialogs.ContainsKey(identifier))
            {
                return;
            }

            DialogBox dialog = dialogs[identifier];

            dialog.Dispatcher.VerifyAccess();

            dialog.Content = content;
            dialog.Title = title;
            dialog.IsShowCloseButton = isShowCloseButton;
            dialog.beforeOpenHandler = openHandler;
            dialog.afterCloseHandler = closeHandle;
            dialog.IsShow = true;
        }

        public static void Show(string identifier, object content)
        {
            Show(identifier, content, "", null, null, true);
        }

        public static void Show(string identifier, object content, string title)
        {
            Show(identifier, content, title, null, null, true);
        }

        public static void Show(string identifier, object content, string title, bool isShowCloseButton)
        {
            Show(identifier, content, title, null, null, isShowCloseButton);
        }

        public static void Show(string identifier, object content, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, "", null, closeHandle, true);
        }

        public static void Show(string identifier, object content, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, "", openHandler, closeHandle, true);
        }

        public static void Show(string identifier, object content, string title, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, title, null, closeHandle, true);
        }

        public static void Show(string identifier, object content, string title, Action<DialogBox> openHandler, Action<DialogBox, object> closeHandle)
        {
            Show(identifier, content, title, openHandler, closeHandle, true);
        }
    }
}
