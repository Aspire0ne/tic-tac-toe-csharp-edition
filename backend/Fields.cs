using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TicTacToe.backend.utils.ioutils;

namespace TicTacToe.backend
{
	static class Fields
	{
		internal enum FieldCoordinationType { String, Console };
		internal enum RowType { Row, Column};
		internal static readonly ushort[] A = { 317, 325, 333, 341, 349, 357, 365, 373, 381 };
		internal static readonly ushort[] B = { 627, 635, 643, 651, 659, 667, 675, 683, 691 };
		internal static readonly ushort[] C = { 937, 945, 953, 961, 969, 977, 985, 993, 1001 };
		internal static readonly ushort[] D = { 1247, 1255, 1263, 1271, 1279, 1287, 1295, 1303, 1311 };
		internal static readonly ushort[] E = { 1557, 1565, 1573, 1581, 1589, 1597, 1605, 1613, 1621 };
		internal static readonly ushort[] F = { 1867, 1875, 1883, 1891, 1899, 1907, 1915, 1923, 1931 };
		internal static readonly ushort[] G = { 2177, 2185, 2193, 2201, 2209, 2217, 2225, 2233, 2241 };
		internal static readonly ushort[] H = { 2487, 2495, 2503, 2511, 2519, 2527, 2535, 2543, 2551 };

		private static readonly string[] Column1 = { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1" };
		private static readonly string[] Column2 = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2" };
		private static readonly string[] Column3 = { "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3" };
		private static readonly string[] Column4 = { "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4" };
		private static readonly string[] Column5 = { "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5" };
		private static readonly string[] Column6 = { "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6" };
		private static readonly string[] Column8 = { "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8" };
		private static readonly string[] Column7 = { "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7" };
		private static readonly string[] Column9 = { "A9", "B9", "C9", "D9", "E9", "F9", "G9", "H9" };

		private static readonly string[] Row1 = { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9" };
		private static readonly string[] Row2 = { "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9" };
		private static readonly string[] Row3 = { "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9" };
		private static readonly string[] Row4 = { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
		private static readonly string[] Row5 = { "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9" };
		private static readonly string[] Row6 = { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9" };
		private static readonly string[] Row7 = { "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9" };
		private static readonly string[] Row8 = { "H1", "H2", "H3", "H4", "H5", "H6", "H7", "H8", "H9" };

		internal static readonly string[][] Rows = { Row1, Row2, Row3, Row4, Row5, Row6, Row7, Row8, Row8 };
		internal static readonly string[][] Columns = { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9 };
		internal static readonly string[] AllFields = { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1",
		 "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2",
		 "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3",
		"A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4",
		"A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5",
		"A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6",
		"A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8",
		 "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7",
		"A9", "B9", "C9", "D9", "E9", "F9", "G9", "H9"};

		internal static ushort ConvertFieldToCoordination(string field)
		{
			byte num = byte.Parse(field[1].ToString());
				--num;
				return field[0] switch
				{
					'A' => A[num],
					'B' => B[num],
					'C' => C[num],
					'D' => D[num],
					'E' => E[num],
					'F' => F[num],
					'G' => G[num],
					'H' => H[num]
				};
		}

		internal static bool FieldExists(string field) => AllFields.Contains(field);

		internal static (byte fromLeft, byte fromTop) ConvertFieldToConsoleCoordinations(string field)
		{
			byte fieldNum = byte.Parse(field[1].ToString());
			byte fieldPosFromTop = (byte)(field[0] % 32);

			byte fromLeft = (byte)(fieldNum * 8);
			byte fromTop = (byte)(5 + 4 * --fieldPosFromTop);
			return (fromLeft, fromTop);
		}

		internal static List<string> RemoveUnplayableFields(List<string> options, List<string> usedFields)
		{
			var possibleFields = new List<string>();
			foreach (string[] field in Rows)
				possibleFields.AddRange(field);

			options.RemoveAll(s => !possibleFields.Contains(s) || usedFields.Contains(s));
			return options;
		}

		internal static string GetLetterRelativeTo(sbyte position, string rootField) => ((char)(rootField[0] + position)).ToString();

		internal static byte getNumRelativeTo(sbyte position, string rootField) => (byte)(char.GetNumericValue(rootField[1]) + position);

		internal static string getFieldRelativeTo(ConsoleUtils.MoveControl direction, string currField)
		{
			return direction switch
			{
				ConsoleUtils.MoveControl.Left => GetLetterRelativeTo(0, currField) + getNumRelativeTo(-1, currField),
				ConsoleUtils.MoveControl.Right => GetLetterRelativeTo(0, currField) + getNumRelativeTo(1, currField),
				ConsoleUtils.MoveControl.Down => GetLetterRelativeTo(1, currField) + getNumRelativeTo(0, currField),
				ConsoleUtils.MoveControl.Up => GetLetterRelativeTo(-1, currField) + getNumRelativeTo(0, currField),
				ConsoleUtils.MoveControl.Select => currField
			};
		}
	}
}
