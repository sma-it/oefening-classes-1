using NUnit.Framework;
using NUnitLite;
using System;

namespace Tests
{
	[TestFixture]
	public class Oef1a
	{
		// deze functie is nodig om de testen achteraf via de webinterface uit te voeren
		public static int Run(string resultPath)
		{
			string[] args = { "--work=" + resultPath };
			return new AutoRun().Execute(args);
		}

		[Test]
		public void Class()
		{
			Assert.That(Utils.Object.DoesClassExist("First.Person"), Is.True, "De Class Person bestaat niet.");

			var person = new Utils.Object("First.Person");
			person.AssertClass();			
		}

		[Test]
		public void FirstName()
		{
			var person = new Utils.Object("First.Person");
			if(person.AssertClass())
			{
				person.AssertProperty("FirstName", Utils.PropertyType.ReadWrite, typeof(string));
			}
		}

		[Test]
		public void LastName()
		{
			var person = new Utils.Object("First.Person");
			if (person.AssertClass())
			{
				person.AssertProperty("LastName", Utils.PropertyType.ReadWrite, typeof(string));
			}
		}
	}

	[TestFixture]
	public class Oef1b
	{
		[Test]
		public void DateOfBirth()
		{
			var person = new Utils.Object("First.Person");
			if (person.AssertClass())
			{
				person.AssertProperty("DateOfBirth", Utils.PropertyType.ReadWrite, typeof(DateTime));
			}
		}

		[Test]
		public void Age()
		{
			var person = new Utils.Object("First.Person");
			if(person.AssertClass())
			{
				person.AssertMethod("Age", typeof(int));
			}

			Random gen = new Random();
			for (int i = 0; i < 10; i++)
			{
				DateTime start = new DateTime(1900, 1, 1);
				int range = (DateTime.Today - start).Days;

				var myDate = start.AddDays(gen.Next(range));
				person.Prop("DateOfBirth")?.Set(myDate);

				int expectedAge = (DateTime.Now.Year - myDate.Year) - 1;
				if (DateTime.Now.Month > myDate.Month) expectedAge++;
				else if (DateTime.Now.Month == myDate.Month
					&& DateTime.Now.Day >= myDate.Day) expectedAge++;

				var calculatedAge = (int)person.Method("Age")?.Invoke();
				Assert.That(calculatedAge, Is.EqualTo(expectedAge), "De berekening van de leeftijd is niet juist");
				if (calculatedAge != expectedAge) break; // no need to continue after an error
			}

		}
	}

	[TestFixture]
	public class Oef2
	{
		[Test]
		public void Class()
		{
			Assert.That(Utils.Object.DoesClassExist("First.ScoreBoard"), Is.True, "De Class ScoreBoard bestaat niet.");

			var board = new Utils.Object("First.ScoreBoard");
			board.AssertClass();
		}

		[Test]
		public void Player1Score()
		{
			var board = new Utils.Object("First.ScoreBoard");
			if(board.AssertClass())
			{
				board.AssertProperty("Player1Score", Utils.PropertyType.ReadOnly, typeof(int));
			}
		}

		[Test]
		public void Player2Score()
		{
			var board = new Utils.Object("First.ScoreBoard");
			if (board.AssertClass())
			{
				board.AssertProperty("Player2Score", Utils.PropertyType.ReadOnly, typeof(int));
			}
		}

		[Test]
		public void Player1Name()
		{
			var board = new Utils.Object("First.ScoreBoard");
			if (board.AssertClass())
			{
				board.AssertProperty("Player1Name", Utils.PropertyType.ReadWrite, typeof(string));
			}
		}

		[Test]
		public void Player2Name()
		{
			var board = new Utils.Object("First.ScoreBoard");
			if (board.AssertClass())
			{
				board.AssertProperty("Player2Name", Utils.PropertyType.ReadWrite, typeof(string));
			}
		}

		[Test]
		public void PointToPlayer1()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("PointToPlayer1", typeof(void));
			
			board.Method("PointToPlayer1").Invoke();
			Assert.That(board.Prop("Player1Score")?.Get(), Is.EqualTo(1), "PointToPlayer1 verhoogt het aantal punten niet met 1");

			board.Method("PointToPlayer1").Invoke();
			Assert.That(board.Prop("Player1Score")?.Get(), Is.EqualTo(2), "PointToPlayer1 verhoogt het aantal punten niet met 1");
		}

		[Test]
		public void PointToPlayer2()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("PointToPlayer2", typeof(void));

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Prop("Player2Score").Get(), Is.EqualTo(1), "PointToPlayer1 verhoogt het aantal punten niet met 1");

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Prop("Player2Score").Get(), Is.EqualTo(2), "PointToPlayer1 verhoogt het aantal punten niet met 1");
		}

		[Test]
		public void Reset()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("Reset", typeof(void));

			// increase scores
			board.Method("PointToPlayer1").Invoke();
			board.Method("PointToPlayer2").Invoke();
			// reset and check if all are zero
			board.Method("Reset").Invoke();
			Assert.That(board.Prop("Player1Score")?.Get(), Is.EqualTo(0), "Reset does not reset all scores");
			Assert.That(board.Prop("Player2Score")?.Get(), Is.EqualTo(0), "Reset does not reset all scores");
		}

		[Test]
		public void IsPlayer1Winning()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("IsPlayer1Winning", typeof(bool));

			Assert.That(board.Method("IsPlayer1Winning").Invoke(), Is.False, "Player 1 should not be winning when both scores are zero");
			board.Method("PointToPlayer1").Invoke();
			Assert.That(board.Method("IsPlayer1Winning").Invoke(), Is.True, "Player 1 should be winning when she has more points");
		}

		[Test]
		public void IsPlayer2Winning()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("IsPlayer2Winning", typeof(bool));

			Assert.That(board.Method("IsPlayer2Winning").Invoke(), Is.False, "Player 2 should not be winning when both scores are zero");
			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Method("IsPlayer2Winning").Invoke(), Is.True, "Player 2 should be winning when she has more points");
		}

		[Test]
		public void Distance()
		{
			var board = new Utils.Object("First.ScoreBoard");
			board.AssertMethod("Distance", typeof(int));

			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(0), "When both players have zero points, the distance should be zero");

			board.Method("PointToPlayer1").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(1), "The distance between scores is not correctly calculated");

			board.Method("PointToPlayer1").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(2), "The distance between scores is not correctly calculated");

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(1), "The distance between scores is not correctly calculated");

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(0), "The distance between scores is not correctly calculated");

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(1), "The distance between scores is not correctly calculated");

			board.Method("PointToPlayer2").Invoke();
			Assert.That(board.Method("Distance").Invoke(), Is.EqualTo(2), "The distance between scores is not correctly calculated");
		}
	}
}
