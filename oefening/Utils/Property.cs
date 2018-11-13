using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
	public class Property
	{
		private PropertyInfo property;
		private object instance;

		public Property(PropertyInfo property, object instance)
		{
			this.property = property;
			this.instance = instance;
		}

		public bool HasType(Type type)
		{
			return property.PropertyType == type;
		}

		public new Type GetType()
		{
			return property.PropertyType;
		}

		public bool IsReadOnly()
		{
			return property.CanRead && !property.CanWrite;
		}

		public bool IsWriteOnly()
		{
			return property.CanWrite && !property.CanRead;
		}

		public bool IsReadWrite()
		{
			return property.CanRead && property.CanWrite;
		}

		public void Set(object value)
		{
			try
			{
				property.SetValue(instance, value);
			}
			catch (ArgumentException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Using your property "
					+ property.Name
					+ " resulted in this error: \n\t"
					+ e.Message
					+ "\nPlease check that the type of this property is an "
					+ property.PropertyType
					+ "\n");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}

		public object Get()
		{
			return property.GetValue(instance);
		}
	}
}