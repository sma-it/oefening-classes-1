using System;
using Utils;

namespace First
{
	class Program
	{
		static void Main(string[] args)
		{
			Menu menu = new Menu();
			menu.AddOption('a', "Start Oefening 1a", StartExercise1a);
			menu.AddOption('b', "Start Oefening 1b", StartExercise1b);
			menu.AddOption('2', "Start Oefening 2", StartExercise2);
			menu.AddOption('3', "Start Oefening 3", StartExercise3);

			menu.Start();
		}

		static void StartExercise1a()
		{
			if (!Utils.Object.DoesClassExist("First.Customer"))
			{
				Console.WriteLine("Er bestaat geen class 'Customer'.");
				return;
			}

			var person = new Utils.Object("First.Customer");
			if (person.IsValid)
			{
				Console.Write("Your First Name: ");
				person.Prop("FirstName")?.Set(Console.ReadLine());

				Console.Write("Your Last Name: ");
				person.Prop("LastName")?.Set(Console.ReadLine());

				Console.Write("Date of birth (dd/mm/yyyy): ");
				string date = Console.ReadLine();
				try
				{
					DateTime birth = Convert.ToDateTime(date);
					person.Prop("DateOfBirth")?.Set(birth);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				Console.Write("Your name is ");
				Console.WriteLine(person.Prop("FirstName")?.Get() + " " + person.Prop("LastName")?.Get());
				Console.Write("You are born on ");
				Console.WriteLine(person.Prop("DateOfBirth").Get().ToString());
			}
		}

		static void StartExercise1b()
		{
			if (!ValidateCustomerClass()) return;

			var person = new Utils.Object("First.Customer");
			if (person.IsValid)
			{
				Console.Write("Your first name: ");
				person.Prop("FirstName")?.Set(Console.ReadLine());

				Console.Write("Your last name: ");
				person.Prop("LastName")?.Set(Console.ReadLine());

				Console.Write("Date of birth(dd/mm/yyyy): ");
				string date = Console.ReadLine();
				try
				{
					DateTime birth = Convert.ToDateTime(date);
					person.Prop("DateOfBirth")?.Set(birth);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				Console.Write(
					person.Method("Name").Invoke()
					+ " is " + person.Method("Age").Invoke().ToString()
					+ " jaar oud.");
			}
		}

		static void StartExercise2()
		{
			if (!ValidateCustomerClass()) return;
			if (!ValidateBookClass()) return;
			Console.Clear();

			var book = new Utils.Object("First.Book");

			Console.Write("The Book's Title: ");
			book.Prop("Title")?.Set(Console.ReadLine());

			Console.Write("The Book's Author: ");
			book.Prop("Author")?.Set(Console.ReadLine());

			Console.Write("Required age to read: ");
			int age = Convert.ToInt32(Console.ReadLine());
			book.Prop("RequiredAge")?.Set(age);

			Console.WriteLine("Your Book Entry:");
			book.Method("Print").Invoke();

			var customer = new Utils.Object("First.Customer");
			customer.Prop("DateOfBirth").Set(new DateTime(DateTime.Now.Year - age, DateTime.Now.Month, DateTime.Now.Day));

			Console.Write("Customer 1 is " + (age) + " years old and is ");
			var allowed = (bool)book.Method("AllowedToRead", Utils.Object.GetClassType("First.Customer")).Invoke(customer.Instance);
			if(!allowed)
			{
				Console.WriteLine("not allowed to read this book.");
			} else
			{
				Console.WriteLine("allowed to read this book.");
			}

			customer.Prop("DateOfBirth").Set(new DateTime(DateTime.Now.Year - (age + 5), DateTime.Now.Month, DateTime.Now.Day));

			Console.Write("Customer 2 is " + (age + 5) + " years old and is ");
			allowed = (bool)book.Method("AllowedToRead", Utils.Object.GetClassType("First.Customer")).Invoke(customer.Instance);
			if (!allowed)
			{
				Console.WriteLine("not allowed to read this book.");
			}
			else
			{
				Console.WriteLine("allowed to read this book.");
			}

			customer.Prop("DateOfBirth").Set(new DateTime(DateTime.Now.Year - (age - 5), DateTime.Now.Month, DateTime.Now.Day));
			Console.Write("Customer 3 is " + (age - 5) + " years old and is ");
			allowed = (bool)book.Method("AllowedToRead", Utils.Object.GetClassType("First.Customer")).Invoke(customer.Instance);
			if (!allowed)
			{
				Console.WriteLine("not allowed to read this book.");
			}
			else
			{
				Console.WriteLine("allowed to read this book.");
			}
		}

		static void StartExercise3()
		{
			if (!ValidateCustomerClass()) return;
			if (!ValidateBookClass()) return;
			if (!ValidateTransactionClass()) return;
			Console.Clear();

			var transaction = new Utils.Object("First.Transaction");
			var book = new Utils.Object("First.Book");
			var customer = new Utils.Object("First.Customer");

			book.Prop("Title").Set("C# for Dummies");
			book.Prop("Author").Set("Microsoft");
			book.Prop("RequiredAge").Set(12);
			Console.WriteLine("Created a book:");
			book.Method("Print").Invoke();

			customer.Prop("FirstName").Set("Harry");
			customer.Prop("LastName").Set("Potter");
			customer.Prop("DateOfBirth").Set(new DateTime(DateTime.Now.Year - 20, 2, 12));
			Console.WriteLine("Created a customer:");
			customer.Method("Print").Invoke();

			Console.WriteLine("** Creating a transaction: **");
			transaction.Prop("Book").Set(book.Instance);
			transaction.Prop("Customer").Set(customer.Instance);

			Console.Write("Date for this transaction (dd/mm/yyyy): ");
			string date = Console.ReadLine();
			try
			{
				DateTime dt = Convert.ToDateTime(date);
				transaction.Prop("LoanDate")?.Set(dt);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("Printing the transaction: ");
			transaction.Method("Print").Invoke();

			if((bool)transaction.Method("IsLoanExpired").Invoke())
			{
				Console.WriteLine("This loan has expired.");
			} else
			{
				Console.WriteLine("This loan has not expired.");
			}
		}


		static bool ValidateCustomerClass()
		{
			var validator = new Utils.Validator("First.Customer");
			validator.AddProperty("FirstName", typeof(string));
			validator.AddProperty("LastName", typeof(string));
			validator.AddProperty("DateOfBirth", typeof(DateTime));

			validator.AddMethod("Age", typeof(int));
			validator.AddMethod("Name", typeof(string));
			validator.AddMethod("Print", typeof(void));

			return validator.Validate();
		}

		static bool ValidateBookClass()
		{
			var validator = new Validator("First.Book");
			validator.AddProperty("Title", typeof(string));
			validator.AddProperty("Author", typeof(string));
			validator.AddProperty("RequiredAge", typeof(int));

			validator.AddMethod("AllowedToRead", typeof(bool), new Type[] { Utils.Object.GetClassType("First.Customer") });
			validator.AddMethod("Print", typeof(void));

			return validator.Validate();
		}

		static bool ValidateTransactionClass()
		{
			var validator = new Validator("First.Transaction");
			validator.AddProperty("Book", Utils.Object.GetClassType("First.Book"));
			validator.AddProperty("Customer", Utils.Object.GetClassType("First.Customer"));
			validator.AddProperty("LoanDate", typeof(DateTime));

			validator.AddMethod("IsLoanExpired", typeof(bool));
			validator.AddMethod("Print", typeof(void));

			return validator.Validate();
		}

		static bool ValidateLibraryClass()
		{
			var validator = new Validator("First.Library");

			validator.AddMethod("ShowCustomers", typeof(void));
			validator.AddMethod("ShowTransactions", typeof(void), new Type[] { typeof(bool) });
			validator.AddMethod("IsBorrowed", typeof(bool), new Type[] { Utils.Object.GetClassType("First.Book") });
			validator.AddMethod("IsTransactionAllowed", typeof(bool), new Type[] { Utils.Object.GetClassType("First.Book"), Utils.Object.GetClassType("First.Customer") });

			return validator.Validate();
		}
	}
}
