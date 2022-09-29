using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Rubyer.Commons
{
    /// <summary>
    /// bject 扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>描述</returns>
        public static string GetDescription(this object value)
        {
            if (value != null)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                if (fieldInfo != null)
                {
                    var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(inherit: false);
                    return ((attributes.Count() > 0) && (!string.IsNullOrEmpty(attributes.First().Description)))
                        ? attributes.First().Description
                        : value.ToString();
                }
            }

            return string.Empty;
        }
    }
}