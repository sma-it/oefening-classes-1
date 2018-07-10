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
			menu.Start();
		}

		static void StartExercise1a()
		{
			if (!Utils.Object.DoesClassExist("First.Person"))
			{
				Console.WriteLine("Er bestaat geen class 'Person'.");
				return;
			}

			var person = new Utils.Object("First.Person");
			if (person.IsValid)
			{
				Console.Write("Je Voornaam: ");
				person.Prop("FirstName")?.Set(Console.ReadLine());

				Console.Write("Je Familienaam: ");
				person.Prop("LastName")?.Set(Console.ReadLine());

				Console.Write("Je naam is ");
				Console.Write(person.Prop("FirstName")?.Get() + " " + person.Prop("LastName")?.Get());
			}

		}

		static void StartExercise1b()
		{
			if (!Utils.Object.DoesClassExist("First.Person"))
			{
				Console.WriteLine("Er bestaat geen class 'Person'.");
				return;
			}

			var person = new Utils.Object("First.Person");
			if (person.IsValid)
			{
				Console.Write("Je Voornaam: ");
				person.Prop("FirstName")?.Set(Console.ReadLine());

				Console.Write("Je Familienaam: ");
				person.Prop("LastName")?.Set(Console.ReadLine());

				Console.Write("Je geboortedatum (dd/mm/yyyy): ");
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
					person.Prop("Name")?.Get()
					+ " is " + person.Method("Age")?.Invoke().ToString()
					+ " jaar oud.");
			}
		}

		static void StartExercise2()
		{
			if (!Utils.Object.DoesClassExist("First.ScoreBoard"))
			{
				Console.WriteLine("Er bestaat geen class 'ScoreBoard'.");
				return;
			}

			var board = new Utils.Object("First.ScoreBoard");
			if (board.IsValid)
			{
				var menu = new Menu();
				menu.AddOption('1', "Geef een punt aan Player 1",
					() =>
					{
						board.Method("PointToPlayer1")?.Invoke();
						Console.WriteLine("Player 1 heeft nu "
							+ board.Prop("Player1Score")?.Get()
							+ " punten.");
					}
				);

				menu.AddOption('2', "Geef een punt aan Player 2",
					() =>
					{
						board.Method("PointToPlayer2")?.Invoke();
						Console.WriteLine("Player 2 heeft nu "
							+ board.Prop("Player2Score")?.Get()
							+ " punten.");
					}
				);

				menu.AddOption('3', "Reset de scores",
					() =>
					{
						board.Method("Reset")?.Invoke();

						Console.WriteLine("Player 1 heeft nu "
							+ board.Prop("Player1Score")?.Get()
							+ " punten.");
						Console.WriteLine("Player 2 heeft nu "
							+ board.Prop("Player2Score")?.Get()
							+ " punten.");
					}
				);

				menu.AddOption('4', "Geef de spelers een naam",
					() =>
					{
						Console.Write("Naam voor speler 1: ");
						string player1 = Console.ReadLine();
						board.Prop("Player1Name")?.Set(player1);

						Console.Write("Naam voor speler 2: ");
						string player2 = Console.ReadLine();
						board.Prop("Player2Name")?.Set(player2);
					}
				);

				menu.AddOption('5', "Toon het scorebord",
					() =>
					{
						Console.WriteLine(
								board.Prop("Player1Name")?.Get()
								+ " heeft "
								+ board.Prop("Player1Score")?.Get()
								+ " punten."
							);
						Console.WriteLine(
								board.Prop("Player2Name")?.Get()
								+ " heeft "
								+ board.Prop("Player2Score")?.Get()
								+ " punten."
							);

						if ((bool)board.Method("IsPlayer1Winning")?.Invoke() == true)
						{
							Console.WriteLine(
								board.Prop("Player1Name")?.Get()
								+ " wint."
								);
						}
						else if ((bool)board.Method("IsPlayer2Winning")?.Invoke() == true)
						{
							Console.WriteLine(
								board.Prop("Player2Name")?.Get()
								+ " wint."
								);
						}
						else
						{
							Console.WriteLine("Gelijkspel!");
						}

						Console.WriteLine(
							"Het verschil bedraagt "
							+ board.Method("Distance")?.Invoke()
							+ " punten."
							);
					}
				);

				menu.Start();
			}
		}
	}
}
