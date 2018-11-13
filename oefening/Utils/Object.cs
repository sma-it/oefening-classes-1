using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Utils
{
	public enum PropertyAccess
	{
		ReadOnly,
		WriteOnly,
		ReadWrite
	}

	public class Object
	{
		private Type type = null;
		private object instance = null;
		public object Instance => instance;

		private bool valid;
		public bool IsValid => valid;

		public static Type GetClassType(string className)
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetType(className, false);
		}

		public static bool DoesClassExist(string className)
		{
			var type = System.Reflection.Assembly.GetExecutingAssembly().GetType(className, false);
			return type != null;
		}

		public static bool HasConstructor(string className, Type[] argTypes)
		{
			var type = System.Reflection.Assembly.GetExecutingAssembly().GetType(className, false);
			if (type == null) return false;

			var constructor = type.GetConstructor(argTypes);
			return constructor != null;
		}

		public Object(string name) : this(name, new object[0]) { }

		public Object(string name, object[] arguments)
		{
			type = System.Reflection.Assembly.GetExecutingAssembly().GetType(name, false);
			if (type != null)
			{
				try
				{
					instance = Activator.CreateInstance(type, arguments);
				}
				catch (MissingMethodException e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Creating the class "
						+ name
						+ " resulted in this error: \n\t"
						+ e.Message
						+ "\nPlease check if your constructor meets the requirements."
						);
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
			valid = instance != null;
		}

		public bool HasProperty(string name)
		{
			var property = instance.GetType().GetProperty(name);
			return property != null;
		}

		public Property Prop(string name)
		{
			var property = instance.GetType().GetProperty(name);
			if (property == null) return null;

			return new Property(property, instance);
		}

		public bool HasMethod(string name, Type argType)
		{
			return HasMethod(name, new Type[] { argType });
		}

		public bool HasMethod(string name, Type[] argTypes = null)
		{
			var method =
				argTypes == null
				? instance.GetType().GetMethod(name)
				: instance.GetType().GetMethod(name, argTypes);
			return method != null;
		}

		public Method Method(string name, Type argType)
		{
			return Method(name, new Type[] { argType });
		}

		public Method Method(string name, Type[] argTypes = null)
		{
			var method =
				argTypes == null
				? instance.GetType().GetMethod(name)
				: instance.GetType().GetMethod(name, argTypes);
			if (method == null) return null;

			return new Method(method, instance);
		}

		/*
		 * These methods are intended to be called from inside 
		 * unit tests.
		 */
		public bool AssertClass()
		{
			Assert.That(IsValid, Is.True, "The class " + instance.ToString() + "is invalid");
			return IsValid;
		}

		public void AssertProperty(string name, PropertyAccess propertyType, Type type)
		{
			Assert.That(HasProperty(name), Is.True, "Property " + name + " does not exist");
			Assert.That(Prop(name)?.HasType(type), Is.True, "Property " + name + " does not have the correct type");
			switch (propertyType)
			{
				case PropertyAccess.ReadOnly:
					Assert.That(Prop(name)?.IsReadOnly(), Is.True, "Property " + name + " should be read only");
					break;
				case PropertyAccess.WriteOnly:
					Assert.That(Prop(name)?.IsWriteOnly(), Is.True, "Property " + name + " should be write only");
					break;
				case PropertyAccess.ReadWrite:
					Assert.That(Prop(name)?.IsReadWrite(), Is.True, "Property " + name + " should be read-write");
					break;
			}
		}

		public void AssertMethod(string name, Type returnType, Type[] args = null)
		{
			Assert.That(HasMethod(name, args), Is.True, "Method " + name + " does not exist, or has the wrong arguments");
			Assert.That(Method(name, args)?.HasReturnType(returnType), Is.True, "Method " + name + " does not have the correct return type");
		}
	}
}