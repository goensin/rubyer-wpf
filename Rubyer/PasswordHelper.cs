using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubyer
{
    public static class PasswordHelper
    {
        // 密码内容
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new PropertyMetadata(""));

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // 是否显示切换按钮
        public static readonly DependencyProperty ShowSwitchButtonProperty =
           DependencyProperty.RegisterAttached("ShowSwitchButton", typeof(bool?), typeof(PasswordHelper), new PropertyMetadata(null, SwitchPasswordShow));

        public static bool? GetShowSwitchButton(DependencyObject obj)
        {
            return (bool?)obj.GetValue(ShowSwitchButtonProperty);
        }

        public static void SetShowSwitchButton(DependencyObject obj, bool? value)
        {
            obj.SetValue(ShowSwitchButtonProperty, value);
        }

        // 切换密码可见性
        internal static readonly DependencyProperty IsShowPasswordProperty =
            DependencyProperty.RegisterAttached("IsShowPassword", typeof(bool), typeof(PasswordHelper));

        internal static bool GetIsShowPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowPasswordProperty);
        }

        internal static void SetIsShowPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowPasswordProperty, value);
        }

        // 切换可见性
        private static void SwitchPasswordShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                MouseButtonEventHandler handleDown = (sender, args) =>
                {
                    SetIsShowPassword(passwordBox, true);
                };

                MouseButtonEventHandler handleUp = (sender, args) =>
                {
                    SetIsShowPassword(passwordBox, false);
                };

                passwordBox.Loaded += (sender, arg) =>
                {
                    if (passwordBox.Template.FindName("switchVisibilityButton", passwordBox) is Button switchButton)
                    {
                        switchButton.AddHandler(Button.MouseDownEvent, handleDown, true);
                        switchButton.AddHandler(Button.MouseUpEvent, handleUp, true);
                    }
                };

                passwordBox.Unloaded += (sender, arg) =>
                {
                    if (passwordBox.Template.FindName("switchVisibilityButton", passwordBox) is Button switchButton)
                    {
                        switchButton.RemoveHandler(Button.MouseDownEvent, handleDown);
                        switchButton.RemoveHandler(Button.MouseUpEvent, handleUp);
                    }
                };

                passwordBox.PasswordChanged += (sender, arg) =>
                {
                    SetPassword(passwordBox, passwordBox.Password);
                };
            }
        }
    }
}
