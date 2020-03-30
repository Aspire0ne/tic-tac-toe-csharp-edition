using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.backend
{
	static class Launcher
	{

		static void Main(string[] args)
		{
			PrepareConsole();
			IntroduceGame();
			new frontend.OpponentSelection().PrepareParticipants();
		}

		private static void PrepareConsole()
		{
			Console.SetWindowSize(ConsoleSettings.WindowWidth, ConsoleSettings.WindowHeight);
			Console.SetBufferSize(ConsoleSettings.WindowWidth, ConsoleSettings.WindowHeight);
			Console.ForegroundColor = ConsoleSettings.Color;
			Console.Clear();
		}

		private static void IntroduceGame()
		{
			utils.ioutils.Sound.Play(TicTacToe.Properties.Resources.zacatek_hry);
			Console.WriteLine("Welcome to Tic-Tac-Toe!\n");
		}
	}

	internal static class ConsoleSettings
	{
		internal const byte WindowWidth = 80;
		internal const byte WindowHeight = 45;
		internal const ConsoleColor Color = ConsoleColor.Red;
	}
}