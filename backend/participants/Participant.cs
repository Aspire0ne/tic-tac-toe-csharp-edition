using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.backend.participants
{
    public abstract class Participant
    {
        private protected static ConsoleColor GetRandomCharacterColor()
        {
            ConsoleColor randomColor = (ConsoleColor)new Random().Next(16);
            return randomColor == ConsoleSettings.Color ? randomColor + 1 : randomColor;
        }

        internal abstract ConsoleColor Color { get; set; }
        internal abstract void Move(ref GameArea gameArea, string opponentLastUsedField, char character);
        //characters
        internal static readonly char[] PossibleCharacters = { 'O', 'X' };

        internal abstract char Character { get; set; }
        internal abstract string Name { get; set; }
        //fields
        internal abstract string LastUsedField { get; }
        internal abstract IReadOnlyList<string> UsedFields { get; }
        internal abstract void ResetUsedFields();

        //wins
        internal abstract byte Wins { get; private protected set; }
        internal abstract bool HasDefinitiveWin { get; }
        internal abstract byte DefinitiveWins { get; }
        internal abstract void IncreaseWins();
        internal abstract void ResetWins();
    }
}