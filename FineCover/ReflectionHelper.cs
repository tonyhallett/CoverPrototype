using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FineCover
{
	public static class ReflectionHelper
	{
		public static PropertyInfo PROPERTY(Type type, string name)
		{
			return type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static FieldInfo FIELD(Type type, string name)
		{
			var field = default(FieldInfo);

			while (field == null && type != null)
			{
				field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				type = type.BaseType;
			}

			return field;
		}

		public static object GET(this object obj, string name)
		{
			if (obj == null)
			{
				return default;
			}

			var type = obj.GetType();

			// property

			var property = PROPERTY(type, name);

			if (property != null && property.CanRead)
			{
				return property.GetValue(obj, null);
			}

			// field

			var field = FIELD(type, name);

			if (field != null)
			{
				return field.GetValue(obj);
			}

			return default;
		}

		public static void SET(this object obj, string name, object value)
		{
			var type = obj.GetType();

			// property

			var property = PROPERTY(type, name);

			if (property != null && property.CanWrite)
			{
				property.SetValue(obj, value);
			}

			// field

			var field = FIELD(type, name);

			if (field != null)
			{
				field.SetValue(obj, value);
			}
		}
	}
}
