using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.backend.participants
{
    class Bot : Participant
    {
        internal override ConsoleColor Color { get; set; } = GetRandomCharacterColor();
        internal enum DifficultyLevel { Easy, Medium, Hard }
        static readonly string[] Names = {"bot_Noober",
        "bot_theLegend27", "bot_noobMaster69", "bot_TheRadasik",
        "bot_DanceLover", "bot_Majsneros", "bot_AnimeLover",
        "bot_Skillz", "bot_WoolRocker", "bot_Kirito", "bot_Luxar",
        "bot_Schiwi", "bot_NoobsReaper", "bot_Elissee"};
        internal override string Name { get; set; } = Names[new Random().Next(0, Names.Length)];
        internal DifficultyLevel Difficulty { get; set; }
        internal override char Character { get; set; }


        private List<string> _usedFields = new List<string>();
        internal override void ResetUsedFields() => _usedFields.Clear();
        internal override IReadOnlyList<string> UsedFields { get => _usedFields; }
        internal override string LastUsedField { get => UsedFields.DefaultIfEmpty(string.Empty).Last(); }



        internal override byte Wins { get; private protected set; }
        internal override byte DefinitiveWins => (byte)(Wins / 5);
        internal override bool HasDefinitiveWin => Wins % 5 == 0;
        internal override void IncreaseWins() => ++Wins;
        internal override void ResetWins() => Wins = 0;

        internal override void Move(ref GameArea gameArea, string opponentLastUsedField, char _)
        {
            System.Threading.Thread.Sleep(2000);
            string chosenFieldStr = ChooseField(new List<string>(gameArea.UsedFields), opponentLastUsedField);
            backend.utils.ioutils.Sound.Play(Properties.Resources.click);
            gameArea.FillField(chosenFieldStr, Character, Color);
            _usedFields.Add(chosenFieldStr);
        }

        internal string ChooseField(List<string> UsedFields, string opponentLastUsedField)
        {
            string rootField = ChooseRootField(opponentLastUsedField);
            List<string> fieldsAroundRoot = getFieldsAroundField(rootField);
            fieldsAroundRoot = Fields.RemoveUnplayableFields(fieldsAroundRoot, UsedFields);

            if (fieldsAroundRoot.Count == 0)
                fieldsAroundRoot = Fields.RemoveUnplayableFields(new List<string>(Fields.AllFields), UsedFields);

            return fieldsAroundRoot[new Random().Next(fieldsAroundRoot.Count)];
        }

        private string ChooseRootField(string opponentLastUsedField)
        {
            if (string.IsNullOrEmpty(LastUsedField))
                return string.IsNullOrEmpty(opponentLastUsedField) ? "D5" : opponentLastUsedField;
            else
                return LastUsedField;
        }

        private List<string> getFieldsAroundField(string rootField)
        {
            sbyte followingField = 1; sbyte prevField = -1; sbyte currField = 0;

            int followingNum = Fields.getNumRelativeTo(followingField, rootField);
            int currNum = Fields.getNumRelativeTo(currField, rootField);
            int prevNum = Fields.getNumRelativeTo(prevField, rootField);

            string followingLetter = Fields.GetLetterRelativeTo(followingField, rootField);
            string currLetter = Fields.GetLetterRelativeTo(currField, rootField);
            string prevLetter = Fields.GetLetterRelativeTo(prevField, rootField);

            return new List<string> {
                currLetter + followingNum, currLetter + prevNum,
                followingLetter + currNum, prevLetter + currNum
            };
        }
    }
}
