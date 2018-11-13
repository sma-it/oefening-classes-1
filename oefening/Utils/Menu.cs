using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
	public class Menu
	{
		struct Option
		{
			public string menuText;
			public Action funcPtr;
		}

		private Dictionary<char, Option> options = new Dictionary<char, Option>();

		public void AddOption(char key, string menuText, Action callback)
		{
			options.Add(key, new Option { menuText = menuText, funcPtr = callback });
		}

		public void Start()
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("Select an Option: ");
				foreach (var option in options)
				{
					Console.WriteLine("- " + option.Key + ": " + option.Value.menuText);
				}
				Console.WriteLine("- 0: Exit");

				while (!Console.KeyAvailable)
				{
					Thread.Sleep(50);
				}

				var key = Console.ReadKey(true);
				if (key.KeyChar.Equals('0')) break;

				if (options.ContainsKey(key.KeyChar))
				{
					Console.Clear();
					options[key.KeyChar].funcPtr();
					Console.WriteLine();
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey();
				}

			}
		}
	}
}