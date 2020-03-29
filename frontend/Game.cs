using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.backend;
using TicTacToe.backend.participants;
using TicTacToe.backend.utils.ioutils;

namespace TicTacToe.frontend
{
    internal class Game
    {
		internal GameArea gameArea = new GameArea();

		internal const ushort DelayBetweenRules = 3500;
		internal const byte RowLengthToWin = 4;

		private readonly Participant participant1;
		private readonly Participant participant2;
		private bool restarted { get; set; }
		private byte gameNum { get; set; }




		internal Game(Participant participant1, Participant participant2)
		{
			this.participant1 = participant1;
			this.participant2 = participant2;
		}



		internal void PrepareGame()
		{
			++gameNum;
			if (!restarted)
			{
				Console.WriteLine($"Who gets {RowLengthToWin} characters in a row first wins.\nDo you wanna see a help to learn the rules? (y/n)");
				AskToShowHelp();
				Console.Clear();
			}

			IntroduceMatch(participant1, participant2);
			bool participant1Begins = DetermineIfParticipant1Begins();
			System.Threading.Thread.Sleep(3000);
			Console.WriteLine("Press any key whenever you're ready to start the game.");
			Console.ReadKey();
			Console.Clear();
			StartGame(participant1Begins);
		}

		private bool DetermineIfParticipant1Begins()
		{
			Console.WriteLine($"If the sum of 10 dice rolls is going to be even, {participant1.Name} begins. Otherwise {participant2.Name} begins.");
			System.Threading.Thread.Sleep(5000);

			int rollSum = RollDiceAndReturnSum(10);

			bool participant1Begins = (rollSum % 2 == 0 ? true : false);
			Console.WriteLine($"\n{(participant1Begins ? participant1.Name : participant2.Name)} begins.");

			return participant1Begins;
		}

		private int RollDiceAndReturnSum(int rolls)
		{
			Console.WriteLine("Rolling dice...");
			System.Threading.Thread.Sleep(1000);
			int rollSum = 0;

			string tossDivider = " - ";

			for (int i = rolls; i > 0; --i)
			{
				int num = new Random().Next(6) + 1;
				if (i == 1) tossDivider = "";
				Sound.Play(Properties.Resources.counter);
				Console.Write(num + tossDivider);
				rollSum += num;
				System.Threading.Thread.Sleep(350);
			}

			Sound.Play(Properties.Resources.wou);
			Console.WriteLine(" | Sum is " + rollSum);

			return rollSum;
		}

		private void AskToShowHelp()
		{
			if (ConsoleUtils.InputString(ConsoleUtils.inputYesNo).Equals("y"))
				ShowHelp();
		}

		private static void ShowHelp()
		{
			System.Threading.Thread.Sleep(200);
			new GameArea().Draw();
			Console.WriteLine("Here are the rules:");
			WriteRule("Above is game area."
			+ " Letter symbolizes row, number symbolizes position from left.");
			WriteRule("To select a field, write letter+position, e.g. A8, b2, e9, H1 ...");
			WriteRule("Who gets 4 fields in a row first wins. Diagonals don't count.");
			WriteRule("You can exit from the program whenever you want by writing \"exit\"");
			WriteRule("You can restart current game whenever you want by writing \"restart\""
			+ " (for instance if there's not enough space on playing area)");
			WriteRule("When you get 5 wins, you won definitively");

		}

		private static void WriteRule(string rule)
		{
			Sound.Play(Properties.Resources.cink);
			Console.WriteLine("\n" + rule);
			System.Threading.Thread.Sleep(DelayBetweenRules);
		}

		private void IntroduceMatch(Participant participant1, Participant participant2)
			=> Console.WriteLine($"{gameNum}. match: {participant1.Name} vs {participant2.Name}!");

		private void StartGame(bool participant1Begins)
		{
			Sound.Play(Properties.Resources.zacatek_hry);
			gameArea.Draw();

			if (participant1Begins)
				MakeFullMove(participant1, participant2.LastUsedField);

			while (true)
			{
				MakeFullMove(participant2, participant1.LastUsedField);
				MakeFullMove(participant1, participant2.LastUsedField);
			}
		}

		private void MakeFullMove(Participant participantToMove, string opponentLastUsedField)
		{
			writeWhosTurnItIs(participantToMove.Name);

			participantToMove.Move(ref gameArea, opponentLastUsedField, participantToMove.Character);
			if (GameArea.IsFullRowFound(new List<string>(participantToMove.UsedFields)))
			{
				Console.Clear();
				EndMatch(participantToMove);
			}
		}

		private void writeWhosTurnItIs(string participantToMoveName)
		{
			string strToWrite = $"It's {participantToMoveName}'s turn...";
			Console.SetCursorPosition(0, 37);
			for (int i = 0; i < Console.BufferWidth; ++i)
				Console.Write(' ');

			Console.SetCursorPosition(0, 37);
			for (int i = 0; i < Console.BufferWidth / 2 - strToWrite.Count() / 2; ++i)
				Console.Write(' ');

			Console.WriteLine(strToWrite);
		}

		private void EndMatch(Participant winner)
		{
			winner.IncreaseWins();

			Console.WriteLine($"End! {winner.Name} wins!");

			if (winner is Bot) {
				Sound.Play(Properties.Resources.fail);
				Console.Write(" | You lose!");
			} else
				Sound.Play(Properties.Resources.vyhra);

			Console.WriteLine("\nstats:");
			WriteStats(participant1);
			WriteStats(participant2);
			OfferNextRound(winner.HasDefinitiveWin);
		}

		private void OfferNextRound(bool hasDefinitiveWin)
		{
			Console.WriteLine(hasDefinitiveWin ? "Do you want to retiliate with this opponent? (y/n)" : "Do you want to play next round? (y/n)");

			switch (ConsoleUtils.InputString(ConsoleUtils.inputYesNo))
			{
				case "y":
					RestartGame(hasDefinitiveWin);
					break;
				case "n":
					new OpponentSelection().PrepareParticipants();
					break;
			}
		}

		private static void WriteStats(Participant participant)
			=> Console.WriteLine($"Amount of wins {participant.Name}: {participant.Wins} | definitive wins: {participant.DefinitiveWins}");

		private void RestartGame(bool retaliation)
		{
			Console.Clear();
			if (retaliation)
			{
				gameNum = 0;
				participant2.ResetWins();
				participant1.ResetWins();
			}
			gameArea = new GameArea();
			participant1.ResetUsedFields();
			participant2.ResetUsedFields();
			restarted = true;
			PrepareGame();
		}
	}
}
