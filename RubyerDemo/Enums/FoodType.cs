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
        [Description("面条")]
        Noodle = 0,

        [Description("面包")]
        Bread,

        Hamburger,

        [Description("不自动生成")]
        [Display(AutoGenerateField = false)]
        Beef,
    }
}