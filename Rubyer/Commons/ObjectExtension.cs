using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}