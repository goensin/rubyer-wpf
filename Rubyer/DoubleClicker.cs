using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 双击转命令
    /// </summary>
    public class DoubleClicker : ContentControl
    {
        /// <summary>
        /// 命令
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(DoubleClicker), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 命令参数
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(DoubleClicker), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 是否是双击区域
        /// </summary>
        public static readonly DependencyProperty IsDoubleClickAreaProperty = DependencyProperty.RegisterAttached(
            "IsDoubleClickArea", typeof(bool), typeof(DoubleClicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));


        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// 是否是双击区域
        /// </summary>
        public static void SetIsDoubleClickArea(DependencyObject element, bool value)
        {
           element.SetValue(IsDoubleClickAreaProperty, value);
        }

        /// <summary>
        /// 是否是双击区域
        /// </summary>
        public static bool GetIsDoubleClickArea(DependencyObject element)
        {
           return (bool)element.GetValue(IsDoubleClickAreaProperty);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (Command != null && GetIsDoubleClickArea((DependencyObject)e.OriginalSource))
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
