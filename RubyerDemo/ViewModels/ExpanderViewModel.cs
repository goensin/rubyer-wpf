using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 扩展框
    /// </summary>
    public partial class ExpanderViewModel : ObservableObject
    {
        public List<Brush> AllBrushes => new List<Brush>
        {
            Application.Current.Resources["DefaultForeground"] as Brush,
            Application.Current.Resources["DefaultBackground"] as Brush,
            Application.Current.Resources["Primary"] as Brush,
            Application.Current.Resources["Light"] as Brush,
            Application.Current.Resources["Dark"] as Brush,
            Application.Current.Resources["Accent"] as Brush,
            Application.Current.Resources["Success"] as Brush,
            Application.Current.Resources["Warning"] as Brush,
            Application.Current.Resources["Info"] as Brush,
            Application.Current.Resources["Error"] as Brush,
        };

        public List<FontWeight> AllFontWeights => new List<FontWeight>
        {
           FontWeights.Black ,
           FontWeights.Bold ,
           FontWeights.DemiBold ,
           FontWeights.ExtraBlack ,
           FontWeights.ExtraBold ,
           FontWeights.ExtraLight ,
           FontWeights.Heavy ,
           FontWeights.Light ,
           FontWeights.Medium ,
           FontWeights.Regular ,
           FontWeights.SemiBold ,
           FontWeights.Thin ,
           FontWeights.UltraBlack ,
           FontWeights.UltraBold ,
           FontWeights.UltraLight ,
        };

        public List<FontFamily> AllFontFamilys => Fonts.SystemFontFamilies.ToList();

        [ObservableProperty]
        private HorizontalAlignment horizontalAlignment;

        [ObservableProperty]
        private VerticalAlignment verticalAlignment;

        [ObservableProperty]
        private Brush foreground;

        [ObservableProperty]
        private Brush background;

        [ObservableProperty]
        private Brush contentForeground;

        [ObservableProperty] 
        private Brush contentBackground;

        [ObservableProperty]
        private FontWeight fontWeight;

        [ObservableProperty]
        private FontFamily fontFamily;

        public ExpanderViewModel()
        {
            Foreground = AllBrushes[1];
            ContentForeground = AllBrushes[0];
            Background = AllBrushes[2];
            ContentBackground = AllBrushes[1];
            FontWeight = FontWeights.Normal;
            fontFamily = AllFontFamilys.FirstOrDefault(x => x.Source.Contains("YaHei"));
        }
    }
}
