using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class TicTacToe
    {
        private string DefaultBoard = $"~ 1 2 3{Environment.NewLine} 1 o x o {Environment.NewLine}2 x o x{Environment.NewLine}3 o x o";

        public enum Player
        {
            X,
            O
        }

        public enum PlayableCoords
        {
            Coord11,
            Coord12,
            Coord13,
            Coord21,
            Coord22,
            Coord23,
            Coord31,
            Coord32,
            Coord33
        }

        // Contains the coordinates for all possible positions of pieces
        public Dictionary<PlayableCoords, string> Coords;

        public void AddCoords()
        {
            Coords.Add(PlayableCoords.Coord11, null);
            Coords.Add(PlayableCoords.Coord12, null);
            Coords.Add(PlayableCoords.Coord13, null);
            Coords.Add(PlayableCoords.Coord21, null);
            Coords.Add(PlayableCoords.Coord22, null);
            Coords.Add(PlayableCoords.Coord23, null);
            Coords.Add(PlayableCoords.Coord31, null);
            Coords.Add(PlayableCoords.Coord32, null);
            Coords.Add(PlayableCoords.Coord33, null);
        }    

        public async Task TicTacToeStart(CommandEventArgs e, string UserX, string UserO)
        {
            Coords.Clear();

            AddCoords();

            await e.Channel.SendMessage($"~  1 2 3{Environment.NewLine} 1 o x o {Environment.NewLine}2 x o x{Environment.NewLine}3 o x o");

            await e.Channel.SendMessage("Who will begin? X/O");

            // Make subscribe method to listen for who will start
        }
    }
}
