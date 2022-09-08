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
    /// 汉堡包菜单子项
    /// </summary>
    public class HamburgerMenuItem : ListBoxItem, ICommandSource
    {
        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Selected += (sender, e) => CommandHelpers.ExecuteCommandSource(this);
        }

        #region commands

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(HamburgerMenuItem), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HamburgerMenuItem)d).OnCommandChanged(e.OldValue as ICommand, e.NewValue as ICommand);
        }

        /// <summary>
        /// Gets or sets a command which will be executed if an item is clicked by the user.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter which will be passed by the Command.
        /// </summary>
        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            nameof(CommandTarget), typeof(IInputElement), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the element on which to raise the specified command.
        /// </summary>
        /// <returns>
        /// Element on which to raise a command.
        /// </returns>
        public IInputElement CommandTarget
        {
            get => (IInputElement)this.GetValue(CommandTargetProperty);
            set => this.SetValue(CommandTargetProperty, value);
        }

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                this.UnhookCommand(oldCommand);
            }

            if (newCommand != null)
            {
                this.HookCommand(newCommand);
            }
        }

        private void UnhookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(command, this.OnCanExecuteChanged);
            this.UpdateCanExecute();
        }

        private void HookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.AddHandler(command, this.OnCanExecuteChanged);
            this.UpdateCanExecute();
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            this.UpdateCanExecute();
        }

        private void UpdateCanExecute()
        {
            this.CanExecute = this.Command is null || CommandHelpers.CanExecuteCommandSource(this);
        }

        private bool canExecute = true;

        private bool CanExecute
        {
            get => this.canExecute;
            set
            {
                if (value == this.canExecute)
                {
                    return;
                }

                this.canExecute = value;
                this.CoerceValue(IsEnabledProperty);
            }
        }

        #endregion commands

        #region properties

        /// <summary>
        /// 图标
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// 图标
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType", typeof(IconType?), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType? IconType
        {
            get { return (IconType?)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// 标题
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion properties
    }
}