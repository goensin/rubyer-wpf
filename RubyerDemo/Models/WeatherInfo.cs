using CommunityToolkit.Mvvm.ComponentModel;
using Rubyer.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RubyerDemo.Models
{
    /// <summary>
    /// 天气
    /// </summary>
    public class WeatherInfo : ObservableObject
    {
        [JsonPropertyName("days")]
        [Display(Name = "日期")]
        [ColumnWidth("100")]
        public string Day { get; set; }

        [JsonPropertyName("week")]
        [Display(Name = "星期")]
        [ColumnWidth("80")]
        public string Week { get; set; }

        [JsonPropertyName("temperature")]
        [Display(Name = "温度")]
        [ColumnWidth("120")]
        public string Temperature { get; set; }

        [JsonPropertyName("wind")]
        [Display(Name = "风向")]
        [ColumnWidth("100")]
        public string Wind { get; set; }

        [JsonPropertyName("winp")]
        [Display(Name = "风力")]
        [ColumnWidth("80")]
        public string Winp { get; set; }

        [JsonPropertyName("weather")]
        [Display(Name = "天气")]
        [ColumnWidth("120")]
        public string Weather { get; set; }

        private bool isSelected;

        /// <summary>
        /// 不自动生成字段
        /// </summary>
        [Display(Name = "选择", AutoGenerateField = false)]
        [ColumnWidth("50")]
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }
}
