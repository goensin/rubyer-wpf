﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace RubyerDemo.ViewModels
{
    public partial class TabControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Poetry> poetries;
        [ObservableProperty]
        private Poetry selectedPoetry;

        [ObservableProperty]
        private int count = 1;

        public TabControlViewModel()
        {
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
}