using System;
using System.Linq;
using System.Collections.Generic;
using TicTacToe.Properties;
using System.Text;

namespace TicTacToe.backend.utils.ioutils
{
	internal static class ConsoleUtils
	{
		public static readonly string[] inputYesNo = { "y", "n" };
		internal enum AutoErrorType { IncorrectInput }
		internal enum MoveControl { Left, Right, Up, Down, Select }
		private static readonly Dictionary<string, MoveControl> moveControlsKeycode = new Dictionary<string, MoveControl>()
		{
			{ "RightArrow", MoveControl.Right},
			{ "LeftArrow" , MoveControl.Left},
			{ "Enter", MoveControl.Select},
			{ "DownArrow", MoveControl.Down},
			{ "UpArrow", MoveControl.Up}
		};

		internal static void WriteColorfully(string text, ConsoleColor color)
		{
			ConsoleColor colorBefore = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = colorBefore;
		}

		internal static void WriteColorfully(char text, ConsoleColor color) => WriteColorfully(text.ToString(), color);

		private static readonly Dictionary<AutoErrorType, string> AutoErrorMessages = new Dictionary<AutoErrorType, string>()
		{
			{ AutoErrorType.IncorrectInput, "Incorrect input." }
		};

		internal static MoveControl InputMove()
		{
			while (true)
			{
				string move = Console.ReadKey().Key.ToString();
				Console.Write("\b \b");
				if (MoveExists(move))
				{
					Sound.Play(Resources.click);
					return moveControlsKeycode[move];
				}

				Sound.Play(Resources.error);
			}
		}

		private static bool MoveExists(string move)
		{
			foreach (var pair in moveControlsKeycode)
				if (pair.Key.Equals(move))
					return true;
			return false;
		}


		internal static string InputString(params string[] options)
		{
			options = options.Select(s => s.ToLower()).ToArray();
			bool oneCharacterArray = Array.TrueForAll(options, s => s.Length == 1) && options.Any();
			
			while (true)
			{
				string input = oneCharacterArray ? Console.ReadKey().KeyChar.ToString() : Console.ReadLine();

				if (!options.Any() || options.Contains(input.ToLower()))
				{
					Sound.Play(Resources.click);
					return input;
				}

				Sound.Play(Resources.error);
				if (oneCharacterArray)
					Console.Write("\b \b");
				else
					ShowError(AutoErrorType.IncorrectInput);
					
			}
		}

		internal static int InputInt(params int[] options)
		{
			if (options.Any())
			{
				string[] optionsStr = new string[options.Length];
				for (int i = 0; i < optionsStr.Length; ++i)
					optionsStr[i] = options[i].ToString();
				return int.Parse(InputString(optionsStr));
			}

			while (true)
			{
				if (!int.TryParse(InputString(), out int num))
					ShowError(AutoErrorType.IncorrectInput);
				else
					return num;
			}
		}

		internal static void ShowError(string error)
		{
			Sound.Play(Resources.error);
			Console.WriteLine(error);
		}

		internal static void ShowError(AutoErrorType type)
		{
			AutoErrorMessages.TryGetValue(type, out string msg);
			Console.WriteLine(msg);
		}
	}
}
