using Microsoft.Win32;
using Rubyer.Enums;
using Rubyer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Rubyer
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public class ThemeManager
    {
        /// <summary>
        /// 所有颜色 key
        /// </summary>
        public static Dictionary<string, string> ColorKeys { get; private set; } = new Dictionary<string, string>
        {
            {"DefaultForeground", "默认前景色"},
            {"DefaultBackground", "默认背景色"},
            {"Primary", "主色"},
            {"Secondary", "次色"},
            {"SecondaryForeground", "次色前景"},
            {"Accent", "强调色"},
            {"Light", "亮色"},
            {"Dark", "暗色"},
            {"Border", "边框色"},
            {"BorderLight", "边框亮色"},
            {"BorderLighter", "边框最亮色"},
            {"SeconarydText", "次色文本"},
            {"WatermarkText", "水印文本"},
            {"Mask", "控件遮罩"},
            {"MaskDark", "控件遮罩暗色"},
            {"DialogBackground", "弹窗背景色"},
            {"HeaderBackground", "标题色"},
            {"FloatBackground", "弹出菜单色"},
            {"ControlBackground", "控件背景色"},
            {"ContainerBackground", "容器背景色"},
            {"ScrollBarBrush", "滚动条颜色"},
            {"WindowTitleBackground", "窗体标题背景色"},
            {"StatusBarBackground", "状态栏背景色"},
            {"PanelBackground", "面板背景色"},
            {"WhiteForeground", "白色前景色"},
            {"BlackForeground", "黑色前景色"},
            {"Error", "错误色"},
            {"Info", "信息色"},
            {"Warning", "警告色"},
            {"Success", "成功色"},
            {"Question", "确认色"},
        };

        /// <summary>
        /// 所有阴影 key
        /// </summary>
        public static Dictionary<string, string> EffectKeys { get; private set; } = new Dictionary<string, string>
        {
            {"AllDirectionEffect", "全方向阴影"},
            {"AllDirectionEffect2", "全方向阴影2"},
            {"AllDirectionEffect3", "全方向阴影3"},
            {"AllDirectionEffect4", "全方向阴影4"},
            {"AllDirectionEffect5", "全方向阴影5"},
            {"RightTopEffect", "右上阴影"},
            {"LeftTopEffect", "左上阴影"},
            {"RightBottomEffect", "右下阴影"},
            {"LeftBottomEffect", "左下阴影"},
            {"TopEffect", "上阴影"},
            {"BottomEffect", "下阴影"},
            {"LeftEffect", "左阴影"},
            {"RightEffect", "右阴影"},
        };

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

        public static string GetLightColor(Application application, string colorName)
        {
            return colorName.Contains("Effect") ? "LightEffectColor" : $"Light{colorName}Color";
        }

        public static string GetDarkColor(Application application, string colorName)
        {
            return colorName.Contains("Effect") ? "DarkEffectColor" : $"Dark{colorName}Color";
        }

        private static ColorAnimation GetColorAnimation(bool isDark, Color color)
        {
            themeApplying = true;

            var animation = new ColorAnimation
            {
                Duration = TimeSpan.FromMilliseconds(600),
                To = color,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            animation.Completed += (sender, e) => themeApplying = false;
            return animation;
        }

        private static void ApplyColor(bool isDark, string colorName)
        {
            var application = Application.Current;
            Color lightColor = (Color)application.FindResource(GetLightColor(application, colorName));
            Color darkColor = (Color)application.FindResource(GetDarkColor(application, colorName));

            if (application.Resources[colorName] is SolidColorBrush brush)
            {
                var color = isDark ? darkColor : lightColor;

                if (brush.Color == color)
                    return;

                ColorAnimation animation = GetColorAnimation(isDark, color);

                if (brush.IsFrozen)
                {
                    brush = brush.CloneCurrentValue();
                }

                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                application.Resources[colorName] = brush;
            }
        }

        private static void ApplyEffect(bool isDark, string effectName)
        {
            var application = Application.Current;
            Color lightColor = (Color)application.FindResource(GetLightColor(application, effectName));
            Color darkColor = (Color)application.FindResource(GetDarkColor(application, effectName));

            if (application.Resources[effectName] is DropShadowEffect effect)
            {
                if (effect.IsFrozen)
                {
                    effect = effect.CloneCurrentValue();
                }

                effect.Color = isDark ? darkColor : lightColor;
                application.Resources[effectName] = effect;
            }
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


        private static void ApplyTheme(bool isDark)
        {
            foreach (var colorKey in ColorKeys)
            {
                ApplyColor(isDark, colorKey.Key);
            }

            foreach (var effectKey in EffectKeys)
            {
                ApplyEffect(isDark, effectKey.Key);
            }

            var currentThemeColor = GetCurrentThemeColor(Application.Current);
            var args = new ThemeModeChangedArgs(currentThemeColor, isDark);
            ThemeModeChanged?.Invoke(Application.Current, args);
        }

        /// <summary>
        /// 切换主题模式
        /// </summary>
        /// <param name="mode">模式</param>
        public static void SwitchThemeMode(Rubyer.Enums.ThemeMode mode)
        {
            if (Application.Current is null)
            {
                return;
            }

            if (mode != Rubyer.Enums.ThemeMode.System)
            {
                SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            }

            bool isDark = false;
            switch (mode)
            {
                case Rubyer.Enums.ThemeMode.Light:
                default:
                    break;

                case Rubyer.Enums.ThemeMode.Dark:
                    isDark = true;
                    break;

                case Rubyer.Enums.ThemeMode.System:
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
            if (Application.Current is null)
            {
                return;
            }

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
            if (Application.Current is null)
            {
                return;
            }

            Application.Current.Resources["AllControlCornerRadius"] = new CornerRadius(cornerRadius);
            Application.Current.Resources["LeftControlCornerRadius"] = new CornerRadius(cornerRadius, 0, 0, cornerRadius);
            Application.Current.Resources["RightControlCornerRadius"] = new CornerRadius(0, cornerRadius, cornerRadius, 0);
            Application.Current.Resources["TopControlCornerRadius"] = new CornerRadius(cornerRadius, cornerRadius, 0, 0);
            Application.Current.Resources["BottomControlCornerRadius"] = new CornerRadius(0, 0, cornerRadius, cornerRadius);
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="cultureName">语言</param>
        public static void SwitchCulture(string cultureName)
        {
            if (Application.Current is null)
            {
                return;
            }

            string url = $"pack://application:,,,/Rubyer;component/Themes/Resources/I18N/{cultureName}.xaml";
            var resourceDictionaries = Application.Current.Resources.MergedDictionaries;
            var resourceDictionary = new ResourceDictionary { Source = new Uri(url, UriKind.RelativeOrAbsolute) };
            if (resourceDictionaries.FirstOrDefault(x => x.Source?.AbsoluteUri == resourceDictionary.Source.AbsoluteUri) is ResourceDictionary oldColorResource)
            {
                resourceDictionaries.Remove(oldColorResource);
            }

            resourceDictionaries.Add(resourceDictionary);
        }

        /// <summary>
        /// 设置默认文字大小
        /// </summary>
        /// <param name="fontSize">文字大小</param>
        public static void SwitchDefaultFontSize(double fontSize)
        {
            if (Application.Current is null)
            {
                return;
            }

            Application.Current.Resources["DefaultFontSize"] = fontSize;
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
        /// <param name="colorUrl">颜色配置文件路径</param>
        public static void ApplyThemeColor(string colorUrl)
        {
            if (Application.Current is null)
            {
                return;
            }

            var resourceDictionaries = Application.Current.Resources.MergedDictionaries;

            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(colorUrl, UriKind.RelativeOrAbsolute)
            };

            if (resourceDictionaries.FirstOrDefault(x => x.Source?.AbsoluteUri == resourceDictionary.Source.AbsoluteUri) is ResourceDictionary oldColorResource)
            {
                resourceDictionaries.Remove(oldColorResource);
            }

            resourceDictionaries.Add(resourceDictionary);

            var currentBackground = ResourceManager.TryGetResource<Brush>("DefaultBackground");
            var currentThemeColor = GetCurrentThemeColor(Application.Current);
            var isDarkMode = CheckIsDark(currentBackground);
            ApplyTheme(isDarkMode);

            var args = new ThemeModeChangedArgs(currentThemeColor, isDarkMode);
            ThemeModeChanged?.Invoke(Application.Current, args);
        }

        /// <summary>
        /// 添加颜色
        /// </summary>
        /// <param name="keys"></param>
        public static void AddColorKeys(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                ColorKeys.Add(keys[i], string.Empty);
            }
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