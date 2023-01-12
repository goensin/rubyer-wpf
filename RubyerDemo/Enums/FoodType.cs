using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Rubyer.Commons;

namespace RubyerDemo
{
    /// <summary>
    /// 食物类型
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum FoodType
    {
        [Description("牛肉丸")]
        BeefMeatballs = 0,

        [Description("蚝烙")]
        OysterOmelette,

        KwayChap,

        [Description("不自动生成")]
        [Display(AutoGenerateField = false)]
        Chicken,
    }
}