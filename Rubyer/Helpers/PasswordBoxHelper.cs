﻿using Rubyer.Commons.KnownBoxes;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// PasswordBox 帮助类
    /// </summary>
    public class PasswordBoxHelper
    {
        /// <summary>
        /// 是否可以绑定密码
        /// </summary>
        public static readonly DependencyProperty IsBindableProperty = DependencyProperty.RegisterAttached(
            "IsBindable", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, OnIsBindableChanaged));

        /// <summary>
        /// Gets the is bindable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetIsBindable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBindableProperty);
        }

        /// <summary>
        /// Sets the is bindable.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetIsBindable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBindableProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 可绑定的密码
        /// </summary>
        public static readonly DependencyProperty BindablePasswordProperty = DependencyProperty.RegisterAttached(
            "BindablePassword", typeof(string), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindablePasswordChanged));

        /// <summary>
        /// Gets the bindable password.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A string.</returns>
        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        /// <summary>
        /// Sets the bindable password.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        /// <summary>
        /// 是否显示切换密码可见按钮
        /// </summary>
        public static readonly DependencyProperty ShowSwitchButtonProperty = DependencyProperty.RegisterAttached(
            "ShowSwitchButton", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox, SwitchPasswordShow));

        /// <summary>
        /// Gets the show switch button.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowSwitchButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSwitchButtonProperty);
        }

        /// <summary>
        /// Sets the show switch button.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        public static void SetShowSwitchButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSwitchButtonProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// 显示密码
        /// </summary>
        internal static readonly DependencyPropertyKey ShowPasswordPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "ShowPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(BooleanBoxes.FalseBox));

        /// <summary>
        /// 显示密码
        /// </summary>
        public static readonly DependencyProperty ShowPasswordProperty = ShowPasswordPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the show password.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A bool.</returns>
        public static bool GetShowPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowPasswordProperty);
        }

        /// <summary>
        /// Sets the show password.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">If true, value.</param>
        internal static void SetShowPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowPasswordPropertyKey, BooleanBoxes.Box(value));
        }

        private static void OnIsBindableChanaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                RoutedEventHandler handler = (a, b) =>
                {
                    SetBindablePassword(passwordBox, passwordBox.Password);
                };

                if (GetIsBindable(passwordBox))
                {
                    SetBindablePassword(passwordBox, passwordBox.Password);
                    passwordBox.PasswordChanged += handler;
                }
                else
                {
                    passwordBox.PasswordChanged -= handler;
                }
            }
        }

        private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if (GetIsBindable(passwordBox))
                {
                    if (passwordBox.Password.Equals(GetBindablePassword(passwordBox)))
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
        }

        private static void SetPasswordBoxEvents(PasswordBox passwordBox)
        {
            if (passwordBox.TryGetChildPartFromVisualTree<Button>("PART_SwitchButton") is Button switchButton)
            {
                if (GetShowSwitchButton(passwordBox))
                {
                    switchButton.PreviewMouseDown += SwitchButton_MouseDown;
                    switchButton.PreviewMouseUp += SwitchButton_MouseUp;
                }
                else
                {
                    switchButton.PreviewMouseDown -= SwitchButton_MouseDown;
                    switchButton.PreviewMouseUp -= SwitchButton_MouseUp;
                }
            }
        }

        private static void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            passwordBox.Loaded -= PasswordBox_Loaded;
            SetPasswordBoxEvents(passwordBox);
        }

        private static void SwitchButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            var passwordBox = button.TryGetParentFromVisualTree<PasswordBox>();
            SetShowPassword(passwordBox, true);
        }

        private static void SwitchButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            var passwordBox = button.TryGetParentFromVisualTree<PasswordBox>();
            SetShowPassword(passwordBox, false );
        }

        private static void SwitchPasswordShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if (passwordBox.IsLoaded)
                {
                    SetPasswordBoxEvents(passwordBox);
                }
                else
                {
                    passwordBox.Loaded += PasswordBox_Loaded;
                }
            }
        }
    }
}