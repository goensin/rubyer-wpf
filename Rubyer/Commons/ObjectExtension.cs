using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubyer.Commons
{
	public static class ObjectExtension
	{
		public static T AssertCast<T>(this object value)
		{
			return (T)((object)value);
		}

		public static T AssertCastNotNull<T>(this object value)
		{
			return value.AssertCast<T>();
		}
	}
}
