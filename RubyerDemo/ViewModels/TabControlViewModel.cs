using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rubyer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RubyerDemo.ViewModels
{
    public partial class TabControlViewModel : ObservableObject
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
            Application.Current.Resources["HeaderBackground"] as Brush,
            Brushes.Transparent,
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

        public List<StyleResource> Styles => new List<StyleResource>
        {
            new StyleResource("RubyerTabControl", Application.Current.Resources["RubyerTabControl"] as Style),
            new StyleResource("LineTabControl", Application.Current.Resources["LineTabControl"] as Style),
            new StyleResource("CardTabControl", Application.Current.Resources["CardTabControl"] as Style),
        };

        [ObservableProperty]
        private HorizontalAlignment horizontalAlignment;

        [ObservableProperty]
        private VerticalAlignment verticalAlignment;

        [ObservableProperty]
        private Brush selectedForeground;

        [ObservableProperty]
        private Brush selectedBackground;

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

        [ObservableProperty]
        private StyleResource style;

        [ObservableProperty]
        private ObservableCollection<Poetry> poetries;

        [ObservableProperty]
        private Poetry selectedPoetry;

        private int count = 1;

        public TabControlViewModel()
        {
            SelectedForeground = AllBrushes[1];
            SelectedBackground = AllBrushes[2];
            Foreground = AllBrushes[0];
            ContentForeground = AllBrushes[0];
            Background = AllBrushes[11];
            ContentBackground = AllBrushes[1];
            FontWeight = FontWeights.Normal;
            FontFamily = AllFontFamilys.FirstOrDefault(x => x.Source.Contains("YaHei"));
            Style = Styles[0];

            Poetries = new ObservableCollection<Poetry>
            {
                new Poetry
                {
                    Title="虞美人 . 李煜",
                    Content="春花秋月何时了？往事知多少。\r\n"+
                            "小楼昨夜又东风，\r\n"+
                            "故国不堪回首月明中。\r\n"+
                            "雕栏玉砌应犹在，只是朱颜改。\r\n"+
                            "问君能有几多愁？恰似一江春水向东流。"
                },
                new Poetry
                {
                    Title="天净沙·秋思 . 马致远",
                    Content="枯藤老树昏鸦，\r\n"+
                            "小桥流水人家，\r\n"+
                            "古道西风瘦马。\r\n"+
                            "夕阳西下，断肠人在天涯。"
                },
                new Poetry
                {
                    Title="水调歌头 . 苏轼",
                    Content="明月几时有，把酒问青天。\r\n"+
                            "不知天上宫阙，今夕是何年。\r\n"+
                            "我欲乘风归去，又恐琼楼玉宇，高处不胜寒。\r\n"+
                            "起舞弄清影，何似在人间。\r\n"+
                            "转朱阁，低绮户，照无眠。\r\n"+
                            "不应有恨，何事长向别时圆？\r\n"+
                            "人有悲欢离合，月有阴晴圆缺，此事古难全。"
                }
            };

            SelectedPoetry = new Poetry();
        }

        [RelayCommand]
        private void AddTabItem(object items)
        {
            Poetry poetry = new Poetry
            {
                Title = $"TabItem {count}",
                Content = $"TabItem {count}"
            };

            count++;
            Poetries.Add(poetry);
            SelectedPoetry = poetry;
        }

        /// <summary>
        /// 移除命令
        /// </summary>
        [RelayCommand]
        private async Task RemoveItem(Poetry poetry)
        {
            if (MessageBoxResult.Yes == await MessageBoxR.Confirm("是否删除？"))
            {
                Poetries.Remove(poetry);
            }
        }
    }

    public partial class Poetry : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private bool isChecked;
    }

    public partial class StyleResource : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private Style style;

        public StyleResource(string name, Style style)
        {
            this.name = name;
            this.style = style;
        }
    }
}