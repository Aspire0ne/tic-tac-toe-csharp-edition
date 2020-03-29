using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.backend
{
    class Launcher
    {
		private const byte WindowWidth = 80;
		private const byte WindowHeight = 45;

		static void Main(string[] args)
		{
			PrepareConsole();
			IntroduceGame();
			new frontend.OpponentSelection().PrepareParticipants();
		}

		private static void PrepareConsole()
		{
			Console.SetWindowSize(WindowWidth, WindowHeight);
			Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Clear();
		}

		private static void IntroduceGame()
		{
			utils.ioutils.Sound.Play(TicTacToe.Properties.Resources.zacatek_hry);
			Console.WriteLine("Welcome to Tic-Tac-Toe!\n");
		}
	}
}