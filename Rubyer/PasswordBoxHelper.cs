using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubyer
{
    public class PasswordBoxHelper
    {
        public static readonly DependencyProperty BindablePasswordProperty = DependencyProperty.RegisterAttached(
            "BindablePassword", typeof(string), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindablePasswordChanged));

        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        public static readonly DependencyProperty ShowSwitchButtonProperty = DependencyProperty.RegisterAttached(
            "ShowSwitchButton", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, SwitchPasswordShow));

        public static bool GetShowSwitchButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSwitchButtonProperty);
        }

        public static void SetShowSwitchButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSwitchButtonProperty, value);
        }

        public static readonly DependencyProperty ShowPasswordProperty = DependencyProperty.RegisterAttached(
            "ShowPassword", typeof(bool), typeof(PasswordBoxHelper));

        public static bool GetShowPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowPasswordProperty);
        }

        public static void SetShowPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowPasswordProperty, value);
        }

        private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if (passwordBox.Password == GetBindablePassword(passwordBox))
                {
                    return;
                }

                passwordBox.Password = GetBindablePassword(passwordBox);

                if (passwordBox.Password.Length > 0)
                {
                    _ = passwordBox.GetType()
                   .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                   .Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
                }
            }
        }

        private static void SwitchPasswordShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                MouseButtonEventHandler handleDown = (sender, args) => SetShowPassword(passwordBox, true);
                MouseButtonEventHandler handleUp = (sender, args) => SetShowPassword(passwordBox, false);

                passwordBox.Loaded += (sender, arg) =>
                {
                    if (passwordBox.Template.FindName("switchVisibilityButton", passwordBox) is Button switchButton)
                    {
                        switchButton.AddHandler(UIElement.MouseDownEvent, handleDown, true);
                        switchButton.AddHandler(UIElement.MouseUpEvent, handleUp, true);
                    }
                };

                passwordBox.Unloaded += (sender, arg) =>
                {
                    if (passwordBox.Template.FindName("switchVisibilityButton", passwordBox) is Button switchButton)
                    {
                        switchButton.RemoveHandler(UIElement.MouseDownEvent, handleDown);
                        switchButton.RemoveHandler(UIElement.MouseUpEvent, handleUp);
                    }
                };

                passwordBox.PasswordChanged += (sender, arg) =>
                {
                    SetBindablePassword(passwordBox, passwordBox.Password);
                };
            }
        }
    }
}
