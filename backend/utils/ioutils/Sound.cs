using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.backend.utils.ioutils
{
    static class Sound
    {
        private const string PlayingError = "[Sounds were turned off, because there was an error playing them!]";

        private static bool SoundsOff { get; set; }
        internal static void Play(System.IO.Stream audioFileStream)
        {
            if (!SoundsOff)
            {
                try
                {
                    new System.Media.SoundPlayer(audioFileStream).Play();
                }
                catch (Exception)
                {
                    SoundsOff = true;
                    ConsoleUtils.ShowError(PlayingError);
                }
            }
            
        }
    }
}
