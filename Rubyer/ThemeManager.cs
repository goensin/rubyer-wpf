﻿using Rubyer.Enums;
using System;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
using Rubyer.Models;

namespace Rubyer
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public class ThemeManager
    {
        private static bool themeApplying = false;

        /// <summary>
        /// 主题模式改变事件
        /// </summary>
        /// <param name="sender">发生对象</param>
        /// <param name="e">参数</param>
        public delegate void ThemeModeChangedEventHandler(object sender, ThemeModeChangedArgs e);

        /// <summary>
        /// 主题改变事件
        /// </summary>
        public static ThemeModeChangedEventHandler ThemeModeChanged;

        /// <summary>
        /// 获取当前应用模式是否为暗色
        /// </summary>
        /// <returns>是否为暗色</returns>
        public static bool GetIsAppDarkMode()
        {
            const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string RegistryValueName = "AppsUseLightTheme";
            // 这里也可能是LocalMachine(HKEY_LOCAL_MACHINE)
            // see "https://www.addictivetips.com/windows-tips/how-to-enable-the-dark-theme-in-windows-10/"
            object registryValueObject = Registry.CurrentUser.OpenSubKey(RegistryKeyPath)?.GetValue(RegistryValueName);
            if (registryValueObject is null) return false;
            return (int)registryValueObject <= 0;
        }

        private static void ApplyTheme(bool isDark)
        {
            Color lightForegroundColor = (Color)Application.Current.FindResource("LightForegroundColor");
            Color lightBackgroundColor = (Color)Application.Current.FindResource("LightBackgroundColor");
            Color darkForegroundColor = (Color)Application.Current.FindResource("DarkForegroundColor");
            Color darkBackgroundColor = (Color)Application.Current.FindResource("DarkBackgroundColor");

            if (!(Application.Current.Resources["DefaultBackground"] is Brush background) || !(Application.Current.Resources["DefaultForeground"] is Brush foreground))
            {
                return;
            }

            themeApplying = true;

            var backgroundAnimation = new ColorAnimation
            {
                Duration = TimeSpan.FromMilliseconds(600),
                To = isDark ? darkBackgroundColor : lightBackgroundColor,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            backgroundAnimation.Completed += (sender, e) => themeApplying = false;

            var foregroundAnimation = new ColorAnimation
            {
                Duration = TimeSpan.FromMilliseconds(600),
                To = isDark ? darkForegroundColor : lightForegroundColor,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnimation);
            foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundAnimation);

            Application.Current.Resources["DefaultForeground"] = foreground;
            Application.Current.Resources["DefaultBackground"] = background;

            var currentThemeColor = GetCurrentThemeColor(Application.Current);
            var args = new ThemeModeChangedArgs(currentThemeColor, isDark);
            ThemeModeChanged?.Invoke(Application.Current, args);
        }

        private static ThemeColor GetCurrentThemeColor(Application application)
        {
            return new ThemeColor
            {
                Primary = (Brush)application.Resources["Primary"],
                Light = (Brush)application.Resources["Light"],
                Dark = (Brush)application.Resources["Dark"],
                Accent = (Brush)application.Resources["Accent"],
            };
        }

        /// <summary>
        /// 切换主题模式
        /// </summary>
        /// <param name="mode">模式</param>
        public static void SwitchThemeMode(ThemeMode mode)
        {
            if (mode != ThemeMode.System)
            {
                SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            }

            bool isDark = false;
            switch (mode)
            {
                case ThemeMode.Light:
                default:
                    break;

                case ThemeMode.Black:
                    isDark = true;
                    break;

                case ThemeMode.System:
                    isDark = GetIsAppDarkMode();
                    SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
                    break;
            }

            ApplyTheme(isDark);
        }

        private static void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (!themeApplying)
            {
                var isDark = GetIsAppDarkMode();
                ApplyTheme(isDark);
            }
        }

        /// <summary>
        /// 切换容器圆角半径
        /// </summary>
        /// <param name="cornerRadius">圆角半径</param>
        public static void SwitchContainerCornerRadius(double cornerRadius)
        {
            Application.Current.Resources["AllContainerCornerRadius"] = new CornerRadius(cornerRadius);
            Application.Current.Resources["LeftContainerCornerRadius"] = new CornerRadius(cornerRadius, 0, 0, cornerRadius);
            Application.Current.Resources["RightContainerCornerRadius"] = new CornerRadius(0, cornerRadius, cornerRadius, 0);
            Application.Current.Resources["TopContainerCornerRadius"] = new CornerRadius(cornerRadius, cornerRadius, 0, 0);
            Application.Current.Resources["BottomContainerCornerRadius"] = new CornerRadius(0, 0, cornerRadius, cornerRadius);
        }

        /// <summary>
        /// 切换控件圆角半径
        /// </summary>
        /// <param name="cornerRadius">圆角半径</param>
        public static void SwitchControlCornerRadius(double cornerRadius)
        {
            Application.Current.Resources["AllControlCornerRadius"] = new CornerRadius(cornerRadius);
            Application.Current.Resources["LeftControlCornerRadius"] = new CornerRadius(cornerRadius, 0, 0, cornerRadius);
            Application.Current.Resources["RightControlCornerRadius"] = new CornerRadius(0, cornerRadius, cornerRadius, 0);
            Application.Current.Resources["TopControlCornerRadius"] = new CornerRadius(cornerRadius, cornerRadius, 0, 0);
            Application.Current.Resources["BottomControlCornerRadius"] = new CornerRadius(0, 0, cornerRadius, cornerRadius);
        }

        private static void SetBrush(Brush brush, string resourceName)
        {
            if (brush != null)
            {
                Application.Current.Resources[resourceName] = brush;
            }
        }

        private static bool CheckIsDark(Brush brush)
        {
            if (brush is SolidColorBrush color)
            {
                return (color.Color.R * 0.299 + color.Color.G * 0.578 + color.Color.B * 0.114) < 192;
            }

            return false;
        }

        /// <summary>
        /// 应用主题配色
        /// </summary>
        /// <param name="themeColor">主题配色</param>
        public static void ApplyThemeColor(ThemeColor themeColor)
        {
            SetBrush(themeColor.Primary, nameof(themeColor.Primary));
            SetBrush(themeColor.Light, nameof(themeColor.Light));
            SetBrush(themeColor.Dark, nameof(themeColor.Dark));
            SetBrush(themeColor.Accent, nameof(themeColor.Accent));

            var currentBackground = (Brush)Application.Current.Resources["DefaultBackground"];
            var currentThemeColor = GetCurrentThemeColor(Application.Current);
            var isDarkMode = CheckIsDark(currentBackground);
            var args = new ThemeModeChangedArgs(currentThemeColor, isDarkMode);
            ThemeModeChanged?.Invoke(Application.Current, args);
        }
    }

    /// <summary>
    /// 主题模式改变参数
    /// </summary>
    public class ThemeModeChangedArgs : EventArgs
    {
        /// <summary>
        /// 主题配色
        /// </summary>
        public ThemeColor ThemeColor { get; set; }

        /// <summary>
        /// 是否未暗色模式
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="themeColor">主题配色</param>
        /// <param name="isDarkMode">是否未暗色模式</param>
        public ThemeModeChangedArgs(ThemeColor themeColor, bool isDarkMode)
        {
            ThemeColor = themeColor;
            IsDarkMode = isDarkMode;
        }
    }
}