using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubyer.Commons
{
	/// <summary>
	/// 参数验证
	/// </summary>
	public sealed class ValidateArgument
	{
		/// <summary>
		/// 非 null
		/// </summary>
		/// <param name="obj">对象</param>
		/// <param name="parameterName">参数名称</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void NotNull(object obj, string parameterName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		/// <summary>
		/// 非 null 或空
		/// </summary>
		/// <param name="parameterValue">字符串值</param>
		/// <param name="parameterName">参数名称</param>
		/// <exception cref="ArgumentException"></exception>
		public static void NotNullOrEmpty(string parameterValue, string parameterName)
		{
			if (string.IsNullOrEmpty(parameterValue))
			{
				throw new ArgumentException(parameterName);
			}
		}

		/// <summary>
		/// 非 Null 或空类型转换
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parameterValue">参数值</param>
		/// <param name="parameterName">参数名称</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static T NotNullOrEmptyCast<T>(object parameterValue, string parameterName)
		{
			ValidateArgument.NotNull(parameterValue, parameterName);
			if (parameterValue is T)
			{
				return (T)((object)parameterValue);
			}
			throw new ArgumentException(parameterName);
		}

		/// <summary>
		/// 类型转换
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="parameterValue">参数值</param>
		/// <param name="parameterName">参数名称</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static T Cast<T>(object parameterValue, string parameterName)
		{
			if (parameterValue == null || parameterValue is T)
			{
				return (T)((object)parameterValue);
			}
			throw new ArgumentException(parameterName);
		}

		/// <summary>
		/// 是否在范围内
		/// </summary>
		/// <param name="checkedExpression">检查表达式</param>
		/// <param name="parameterName">参数名称</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static void Range(bool checkedExpression, string parameterName)
		{
			if (!checkedExpression)
			{
				throw new ArgumentOutOfRangeException(parameterName);
			}
		}

		/// <summary>
		/// 是否在范围内
		/// </summary>
		/// <param name="checkedExpression">检查表达式</param>
		/// <param name="parameterName">参数名称</param>
		/// <param name="message">错误信息</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static void Range(bool checkedExpression, string parameterName, string message)
		{
			if (!checkedExpression)
			{
				throw new ArgumentOutOfRangeException(parameterName, message);
			}
		}
	}
}
