using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using TicTacToe.backend.utils.ioutils;

namespace TicTacToe.backend.participants
{
    internal class Player : Participant
    {

        internal override char Character { get; set; }
        //name
        internal override string Name { get; set; } = "Player_" + new Random().Next(0, 1000);
        //wins
        internal override byte Wins { get; private protected set; }
        internal override byte DefinitiveWins => (byte)(Wins / 5);
        internal override bool HasDefinitiveWin => Wins % 5 == 0;
        internal override void IncreaseWins() => ++Wins;
        internal override void ResetWins() => Wins = 0;

        //fields
        private List<string> _usedFields = new List<string>();
        internal override void ResetUsedFields() => _usedFields.Clear();
        internal override IReadOnlyList<string> UsedFields { get => _usedFields; }
        internal override string LastUsedField { get => UsedFields.LastOrDefault(); }

        internal override void Move(ref GameArea gameArea, string _, char character)
        {
            string chosenFieldStr = ChooseField(gameArea, character);
            gameArea.FillField(chosenFieldStr, Character);
            _usedFields.Add(chosenFieldStr);
        }

        private string ChooseField(GameArea gameArea, char character)
        {
            string currentlyHighlightedField = null;

            if (LastUsedField is null)
                gameArea.TryHighlightField("D5", character);

            while (true)
            {
                ConsoleUtils.MoveControl move = ConsoleUtils.InputMove();
                string fieldToMoveTo = Fields.getFieldRelativeTo(move, currentlyHighlightedField ?? LastUsedField ?? "D5");
                if (UsedFields.Contains(fieldToMoveTo))
                {
                    Sound.Play(Properties.Resources.error);
                    continue;
                }

                if (move != ConsoleUtils.MoveControl.Select)
                {
                    bool highlighted = gameArea.TryHighlightField(fieldToMoveTo, character, currentlyHighlightedField ?? (LastUsedField is null ? "D5" : null));
                    if (highlighted)
                        currentlyHighlightedField = fieldToMoveTo;
                }
                else
                    return fieldToMoveTo;
            }
        }

    }
}
