using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TicTacToe.frontend;
using TicTacToe.backend.utils.ioutils;

namespace TicTacToe.backend
{
    class GameArea
    {
        private const ConsoleColor AreaColor = ConsoleColor.Red;
        private List<string> _usedFields = new List<string>();
        internal IReadOnlyList<string> UsedFields { get => _usedFields; }
        private void AddUsedField(string field) => _usedFields.Add(field);
        //private string CurrentlyHighlightedField { get; set; } = string.Empty;
        internal string LastUsedField { get => UsedFields.DefaultIfEmpty(string.Empty).Last(); }

        internal const string Area = 
              "                                                                            \n"
            + "       1       2       3       4       5       6       7       8       9    \n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " A |       |       |       |       |       |       |       |       |       | A\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " B |       |       |       |       |       |       |       |       |       | B\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " C |       |       |       |       |       |       |       |       |       | C\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " D |       |       |       |       |       |       |       |       |       | D\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " E |       |       |       |       |       |       |       |       |       | E\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " F |       |       |       |       |       |       |       |       |       | F\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " G |       |       |       |       |       |       |       |       |       | G\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + " H |       |       |       |       |       |       |       |       |       | H\n"
            + "   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |   .   |\n"
            + "   |       |       |       |       |       |       |       |       |       |\n"
            + "   |-------|-------|-------|-------|-------|-------|-------|-------|-------|\n"
            + "       1       2       3       4       5       6       7       8       9    \n";

        internal void Draw() => Console.WriteLine(Area);


        internal void FillField(string field, char character, ConsoleColor color, bool save = true)
        {
            (byte fromLeft, byte fromTop) coordinations = Fields.ConvertFieldToConsoleCoordinations(field);
            Console.SetCursorPosition(coordinations.fromLeft - 1, coordinations.fromTop - 1);
            ConsoleUtils.WriteColorfully(character, color);
            if (save)
                AddUsedField(field);
        }

        internal bool IsFieldPlayable(string field) => Fields.AllFields.Contains(field) && !UsedFields.Contains(field);

        internal bool TryHighlightField(string fieldToHighlight, char character, ConsoleColor color,string oldField = "")
        {
            if (IsFieldPlayable(fieldToHighlight))
            {
                if (!string.IsNullOrEmpty(oldField))
                    FillField(oldField, '.', AreaColor, false);
                FillField(fieldToHighlight, character, color, false);

                return true;
            }
            else
            {
                backend.utils.ioutils.Sound.Play(Properties.Resources.error);
                return false;
            }
        }

        internal static bool IsFullRowFound(List<string> usedFields)
        {
            for (int i = 0; i < 9; ++i)
                if (FieldsCreateFullRow(usedFields, Fields.Columns[i], Fields.RowType.Column) || FieldsCreateFullRow(usedFields, Fields.Rows[i], Fields.RowType.Row))
                    return true;
            return false;
        }

        private static bool FieldsCreateFullRow(List<string> usedFields, string[] row, Fields.RowType rowTypeToFind)
        {
            List<string> usedFieldsInTheRow = usedFields.Where(field => row.Contains(field)).ToList();
            if (usedFieldsInTheRow.Count < Game.RowLengthToWin)
                return false;

            char[] theRow = new char[usedFieldsInTheRow.Count];

                for (int i = 0; i < theRow.Length; ++i)
                    theRow[i] = usedFieldsInTheRow[i][rowTypeToFind == Fields.RowType.Column ? 0 : 1];
                    
            return FindFullRow(theRow);
        }

        private static bool FindFullRow(char[] row)
        {
            Array.Sort(row);

            char nextCharInOrder = row[1];
            byte charsInOrder = 0;

            for (byte i = 1; i < row.Length; ++i)
            {
                if (row[i] == ++nextCharInOrder)
                {
                    if (++charsInOrder == Game.RowLengthToWin - 2)
                        return true;
                }
                else
                {
                    --nextCharInOrder;
                    charsInOrder = 0;
                }
            }
            return false;
        }
    }
}
