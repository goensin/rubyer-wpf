using System.Collections;
using System.Collections.Generic;

namespace Rubyer.Models
{
    /// <summary>
    /// 传输参数接口
    /// </summary>
    public interface IParameters : IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        /// <summary>
        /// 集合数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 所有 key 集合
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        object this[string key] { get; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Add(string key, object value);

        /// <summary>
        /// 是否包含某个键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否包含</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T GetValue<T>(string key);

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        bool TryGetValue<T>(string key, out T value);
    }
}
