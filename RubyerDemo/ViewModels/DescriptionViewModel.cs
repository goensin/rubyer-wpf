using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// The description view model.
    /// </summary>
    public class DescriptionViewModel : ViewModelBase
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

        private ObservableCollection<DescriptionModel> models;

        /// <summary>
        /// 所有步骤
        /// </summary>
        public ObservableCollection<DescriptionModel> Models
        {
            get => models;
            set
            {
                models = value;
                RaisePropertyChanged("Models");
            }
        }

        private int currentColumn;

        /// <summary>
        /// 当前列数
        /// </summary>
        public int CurrentColumn
        {
            get => currentColumn;
            set
            {
                currentColumn = value;
                RaisePropertyChanged("CurrentColumn");
            }
        }
    }

    /// <summary>
    /// 描述模型
    /// </summary>
    public class DescriptionModel
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}