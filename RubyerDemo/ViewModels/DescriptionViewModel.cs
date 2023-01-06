using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public partial class DescriptionViewModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionViewModel"/> class.
        /// </summary>
        public DescriptionViewModel()
        {
            Models = new ObservableCollection<DescriptionModel>
            {
                new DescriptionModel{ Content = "Columns", Description = "列数"},
                new DescriptionModel{ Content = "HorizontalDescriptionAlignment", Description = "水平对齐"},
                new DescriptionModel{ Content = "VerticalDescriptionAlignment", Description = "垂直对齐"},
                new DescriptionModel{ Content = "DescriptionFontSize", Description = "字体大小"},
                new DescriptionModel{ Content = "DescriptionFontWeight, DescriptionBackground, DescriptionForeground, DisplayDescriptionPath, DescriptionStringFormat", Description = "其他"},
            };

            CurrentColumn = 2;
        }

        [ObservableProperty]
        private ObservableCollection<DescriptionModel> models;

        [ObservableProperty]
        private int currentColumn;
    }

    /// <summary>
    /// 描述模型
    /// </summary>
    public partial class DescriptionModel : ObservableObject
    {
        /// <summary>
        /// 描述
        /// </summary>
        [ObservableProperty]
        private string description;

        /// <summary>
        /// 内容
        /// </summary>
        [ObservableProperty]
        private string content;
    }
}