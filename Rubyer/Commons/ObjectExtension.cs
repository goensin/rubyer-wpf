using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rubyer.Commons
{
    /// <summary>
    /// Object 扩展
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 强转为某个类型
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A T.</returns>
        public static T AssertCast<T>(this object value)
        {
            return (T)((object)value);
        }

        /// <summary>
        /// 强转为某个类型, 不为空
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A T.</returns>
        public static T AssertCastNotNull<T>(this object value)
        {
            return value.AssertCast<T>();
        }

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