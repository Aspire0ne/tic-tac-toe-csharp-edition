using System;
using System.Linq;
using TicTacToe.backend.participants;
using TicTacToe.backend.utils.ioutils;

namespace TicTacToe.frontend
{
	class OpponentSelection
	{
		private readonly Participant player1 = new Player();
		private Participant player2;
		private Bot bot;
		private enum OpponentMode { Bot = 0, Player = 1 }
		private OpponentMode Mode { get; set; }

		internal void PrepareParticipants()
		{
			Mode = ObtainMode();

			switch (Mode)
			{
				case OpponentMode.Bot:
					SetupBotMode();
					break;
				case OpponentMode.Player:
					SetupPlayerMode();
					break;
			}

			AssignCharacters(ObtainCharacters(), Mode);

			GoIntoGame(player1, Mode == OpponentMode.Player ? player2 : bot);
		}

		private void GoIntoGame(Participant participant1, Participant participant2)
			=> new Game(participant1, participant2).PrepareGame();

		private void SetupBotMode()
		{
			bot = new Bot();
			string chosenName = ObtainPlayerName(1);
			if (chosenName.Any())
				player1.Name = chosenName;

			Bot.DifficultyLevel difficulty = ObtainDifficulty();
			bot.Difficulty = difficulty;
		}
		private static Bot.DifficultyLevel ObtainDifficulty()
		{
			Console.WriteLine("Select bot difficulty:\n{0} - easy\n{1} - medium\n{2} - hard",
				(byte)Bot.DifficultyLevel.Easy, (byte)Bot.DifficultyLevel.Medium, (byte)Bot.DifficultyLevel.Hard);

			Bot.DifficultyLevel diff = (Bot.DifficultyLevel)ConsoleUtils.InputInt((byte)Bot.DifficultyLevel.Easy, (byte)Bot.DifficultyLevel.Medium, (byte)Bot.DifficultyLevel.Hard);
			Console.WriteLine("\n");
			return diff;
		}

		private void SetupPlayerMode()
		{
			player1.Name = ObtainPlayerName(1);
			player2 = new Player { Name = ObtainPlayerName(2) };
		}

		private static string ObtainPlayerName(int playerNum)
		{
			Console.WriteLine("Player" + playerNum + " - input name (0 or enter for random): ");
			string input = ConsoleUtils.InputString();
			Console.WriteLine("");
			return input.Equals("0") || !input.Any() ? "" : input;
		}

		private static OpponentMode ObtainMode()
		{
			Console.WriteLine($"Write {(byte)OpponentMode.Bot} to play against a bot\nWrite {(byte)OpponentMode.Player} to play against a friend");

			OpponentMode chosenMode = (OpponentMode)ConsoleUtils.InputInt(0, 1);
			switch (chosenMode)
			{
				case OpponentMode.Bot: Console.WriteLine("\nOkay, you're playing against a bot!");
					break;
				case OpponentMode.Player: Console.WriteLine("\nAll right, you're playing against a friend!");
					break;
			}

			Console.WriteLine("");
			return chosenMode;
		}

		private static char ObtainCharacters()
		{
			Console.WriteLine("Write X for crosses, O for circles:");
			char character = ConsoleUtils.InputString("X", "O").ElementAt(0);
			Console.WriteLine("\n");
			return character;
		}

		private void AssignCharacters(char participant1Character, OpponentMode mode)
		{
			player1.Character = participant1Character;
			char secondChar = Participant.PossibleCharacters[Array.IndexOf(Participant.PossibleCharacters, participant1Character) == 0 ? 1 : 0];
			if (mode == OpponentMode.Bot)
				bot.Character = secondChar;
			else if (mode == OpponentMode.Player)
				player2.Character = secondChar;
		}
	}
}
