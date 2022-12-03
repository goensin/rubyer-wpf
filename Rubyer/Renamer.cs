using Rubyer.Commons.KnownBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubyer
{
    /// <summary>
    /// 重命名工具
    /// </summary>
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    public class Renamer : ContentControl
    {
        /// <summary>
        /// 文本框名称
        /// </summary>
        public const string TextBoxPartName = "PART_TextBox";

        private TextBox textBox;

        #region commands

        /// <summary>
        /// 重命名命令
        /// </summary>
        public static RoutedCommand RenameCommand = new RoutedCommand();

        /// <summary>
        /// 重命名完成命令
        /// </summary>
        public static RoutedCommand RenameCompletedCommand = new RoutedCommand();

        /// <summary>
        /// 重命名取消命令
        /// </summary>
        public static RoutedCommand RenameCancelCommand = new RoutedCommand();

        #endregion commands

        #region events

        /// <summary>
        /// 文本改变事件
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(Renamer));

        /// <summary>
        /// 文本改变事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        #endregion events

        #region properties

        /// <summary>
        /// 文本改变命令
        /// </summary>
        public static readonly DependencyProperty TextChangedCommandProperty =
            DependencyProperty.Register("TextChangedCommand", typeof(ICommand), typeof(Renamer), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// 文本改变命令
        /// </summary>
        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        /// <summary>
        /// 是否重命名中
        /// </summary>
        public static readonly DependencyProperty IsRenamingProperty = DependencyProperty.Register(
            "IsRenaming", typeof(bool), typeof(Renamer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsRenamingChanged));

        /// <summary>
        /// 是否重命名中
        /// </summary>
        public bool IsRenaming
        {
            get { return (bool)GetValue(IsRenamingProperty); }
            set { SetValue(IsRenamingProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Renamer), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion properties

        static Renamer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Renamer), new FrameworkPropertyMetadata(typeof(Renamer)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(RenameCommand, RenameHandler));
            CommandBindings.Add(new CommandBinding(RenameCompletedCommand, RenameCompletedHandler));
            CommandBindings.Add(new CommandBinding(RenameCancelCommand, RenameCancelHandler));

            textBox = GetTemplateChild(TextBoxPartName) as TextBox;
            textBox.LostFocus += TextBox_LostFocus;

            var enterKeyBinding = new KeyBinding(RenameCompletedCommand, Key.Enter, ModifierKeys.None);
            var escKeyBinding = new KeyBinding(RenameCancelCommand, Key.Escape, ModifierKeys.None);
            var f2KeyBinding = new KeyBinding(RenameCommand, Key.F2, ModifierKeys.None);

            textBox.InputBindings.Add(enterKeyBinding);
            textBox.InputBindings.Add(escKeyBinding);
            this.InputBindings.Add(f2KeyBinding);

            this.PreviewMouseUp += (sender, e) =>
            {
                if (!IsRenaming)
                {
                    //e.Handled = true;
                    this.Focus();
                }
            };
        }

        private void RenameHandler(object sender, ExecutedRoutedEventArgs e)
        {
            IsRenaming = true;
        }

        private void RenameCompletedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            IsRenaming = false;
        }

        private void RenameCancelHandler(object sender, ExecutedRoutedEventArgs e)
        {
            textBox.Text = Text;
            IsRenaming = false;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsRenaming = false;
        }

        private static void OnIsRenamingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var renamer = d as Renamer;
            if (renamer.IsRenaming)
            {
                renamer.textBox.Visibility = Visibility.Visible;
                renamer.textBox.Focus();
                renamer.textBox.SelectAll();
            }
            else
            {
                renamer.textBox.Visibility = Visibility.Collapsed;
                renamer.Focus();
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var renamer = d as Renamer;
            var args = new RoutedPropertyChangedEventArgs<string>(e.OldValue.ToString(), e.NewValue.ToString());
            args.RoutedEvent = Renamer.TextChangedEvent;
            renamer.RaiseEvent(args);

            renamer.TextChangedCommand?.Execute(e.NewValue.ToString());
        }
    }
}