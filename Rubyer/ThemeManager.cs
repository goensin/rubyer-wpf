using Rubyer.Enums;
using System;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;

namespace Rubyer
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public class ThemeManager
    {
        private static bool themeApplying = false;

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
            return (int)registryValueObject > 0 ? false : true;
        }

        private static void ApplyTheme(bool isDark)
        {
            Color lightForegroundColor = (Color)Application.Current.FindResource("LightForegroundColor");
            Color lightBackgroundColor = (Color)Application.Current.FindResource("LightBackgroundColor");
            Color darkForegroundColor = (Color)Application.Current.FindResource("DarkForegroundColor");
            Color darkBackgroundColor = (Color)Application.Current.FindResource("DarkBackgroundColor");

            var background = Application.Current.Resources["DefaultBackground"] as SolidColorBrush;
            var foreground = Application.Current.Resources["DefaultForeground"] as SolidColorBrush;

            if (background == null || foreground == null)
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
    }
}