using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
	internal class PropertyValidator
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public PropertyAccess Access { get; set; }
	}

	internal class MethodValidator
	{
		public string Name { get; set; }
		public Type[] Args { get; set; }
		public Type ReturnType { get; set; }
	}

	public class Validator
	{
		List<PropertyValidator> Properties = new List<PropertyValidator>();
		List<MethodValidator> Methods = new List<MethodValidator>();
		string ClassName;

		public Validator(string name)
		{
			ClassName = name;
		}

		public void AddProperty(string name, Type type, PropertyAccess access = PropertyAccess.ReadWrite)
		{
			Properties.Add(new PropertyValidator
			{
				Name = name,
				Type = type,
				Access = access,
			});
		}

		public void AddMethod(string name, Type returnType, Type[] args = null)
		{
			Methods.Add(new MethodValidator
			{ 
				Name = name,
				Args = args == null ? new Type[] { } : args,
				ReturnType = returnType,
			});
		}

		public bool Validate()
		{
			Console.WriteLine("De class " + ClassName + " wordt gecontroleerd op fouten...");

			if (!Object.DoesClassExist(ClassName))
			{
				Console.WriteLine("De class " + ClassName + " bestaat niet.");
				return false;
			}

			var cl = new Object(ClassName);
			if(!cl.IsValid)
			{
				Console.WriteLine("De class " + ClassName + " is niet geldig.");
				return false;
			}

			foreach (var property in Properties)
			{
				if(!cl.HasProperty(property.Name))
				{
					Console.WriteLine("De property " + property.Name + " ontbreekt.");
					return false;
				}

				if(!cl.Prop(property.Name).HasType(property.Type))
				{
					Console.WriteLine("Het type van property " + property.Name + " is niet correct.");
					return false;
				}

				switch(property.Access)
				{
					case PropertyAccess.ReadOnly:
						if(!cl.Prop(property.Name).IsReadOnly())
						{
							Console.WriteLine("De property " + property.Name + " moet read-only zijn.");
							return false;
						}
						break;
					case PropertyAccess.ReadWrite:
						if(!cl.Prop(property.Name).IsReadWrite())
						{
							Console.WriteLine("De property " + property.Name + " moet read-write zijn.");
							return false;
						}
						break;
					case PropertyAccess.WriteOnly:
						if (!cl.Prop(property.Name).IsWriteOnly())
						{
							Console.WriteLine("De property " + property.Name + " moet write only zijn.");
							return false;
						}
						break;
				}
			}

			foreach(var method in Methods)
			{
				if(!cl.HasMethod(method.Name, method.Args))
				{
					Console.WriteLine("Er bestaat geen functie " + method.Name + " met de gevraagde argumenten.");
					return false;
				}

				if(!cl.Method(method.Name, method.Args).HasReturnType(method.ReturnType))
				{
					Console.WriteLine("De functie " + method.Name + " heeft niet het juiste return type.");
					return false;
				}
			}

			Console.WriteLine("Deze class voldoet aan de vereisten.");
			return true;
		}
	}
}
