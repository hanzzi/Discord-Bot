using Discord;
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
            O,
            Null
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
        public Dictionary<PlayableCoords, Player> Coords;

        public void AddCoords()
        {
            Coords.Add(PlayableCoords.Coord11, Player.Null);
            Coords.Add(PlayableCoords.Coord12, Player.Null);
            Coords.Add(PlayableCoords.Coord13, Player.Null);
            Coords.Add(PlayableCoords.Coord21, Player.Null);
            Coords.Add(PlayableCoords.Coord22, Player.Null);
            Coords.Add(PlayableCoords.Coord23, Player.Null);
            Coords.Add(PlayableCoords.Coord31, Player.Null);
            Coords.Add(PlayableCoords.Coord32, Player.Null);
            Coords.Add(PlayableCoords.Coord33, Player.Null);
        }

        public async Task TicTacToeStart(CommandEventArgs e, string UserX, string UserO, DiscordClient _client)
        {
            Coords.Clear();

            AddCoords();

            await e.Channel.SendMessage("Welcome to Tic Tac Toe");

            await e.Channel.SendMessage($"~  1 2 3{Environment.NewLine} 1 / / / {Environment.NewLine}2 / / /{Environment.NewLine}3 / / /");

            await e.Channel.SendMessage("O plays first");

            Player CurrentPlayer = Player.O;

            _client.MessageReceived += ((s, m) =>
            {
                PlayableCoords Message;

                //string[] Coords = null;

                //AddCoordsString(Coords);

                Enum.TryParse(e.Message.Text, out Message);

                if (Convert.ToChar(m.Message.Text) == Config.Prefix)
                {
                    if (CurrentPlayer == Player.O)
                    {
                        foreach (KeyValuePair<PlayableCoords, Player> Coord in Coords)
                        {
                            if (Message == Coord.Key)
                            {
                                if (Coord.Value != Player.X)
                                    Coords[Coord.Key] = CurrentPlayer;

                                Check(CurrentPlayer, e);
                            }
                        }
                    }
                    else if (CurrentPlayer == Player.X)
                    {
                        foreach (KeyValuePair<PlayableCoords, Player> Coord in Coords)
                        {
                            if (Message == Coord.Key)
                            {
                                if (Coord.Value != Player.O)
                                    Coords[Coord.Key] = CurrentPlayer;

                                bool CheckIfWon = Check(CurrentPlayer, e);

                                if (CheckIfWon == true)
                                {
                                    // UNSUBSCRIBE MESSAGERECIEVED
                                    // MAKE MESSAGERECIEVED A METHOD
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Channel.SendMessage("Something went wrong remember you can only enter X or O");
                    }
                }
            });
        }

        private bool Check(Player CurrentPlayer, CommandEventArgs e)
        {
            if (Coords[PlayableCoords.Coord11] == CurrentPlayer && Coords[PlayableCoords.Coord12] == CurrentPlayer && Coords[PlayableCoords.Coord13] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 11, 12, 13 and has won the game");
                return true;
            }

            if (Coords[PlayableCoords.Coord31] == CurrentPlayer && Coords[PlayableCoords.Coord32] == CurrentPlayer && Coords[PlayableCoords.Coord33] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 31, 32, 33 and has won the game");
                return true;
            }

            if (Coords[PlayableCoords.Coord11] == CurrentPlayer && Coords[PlayableCoords.Coord21] == CurrentPlayer && Coords[PlayableCoords.Coord31] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 11, 21, 31 and has won the game");
                return true;
            }

            if (Coords[PlayableCoords.Coord31] == CurrentPlayer && Coords[PlayableCoords.Coord32] == CurrentPlayer && Coords[PlayableCoords.Coord33] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 31, 32, 33 and has won the game");
                return true;
            }

            if (Coords[PlayableCoords.Coord11] == CurrentPlayer && Coords[PlayableCoords.Coord22] == CurrentPlayer && Coords[PlayableCoords.Coord33] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 11, 22, 33 and has won the game");
                return true;
            }

            if (Coords[PlayableCoords.Coord33] == CurrentPlayer && Coords[PlayableCoords.Coord22] == CurrentPlayer && Coords[PlayableCoords.Coord31] == CurrentPlayer)
            {
                e.Channel.SendMessage($"{CurrentPlayer} Has Taken Coordinates 33, 22, 31 and has won the game");
                return true;
            }
            return false;
        }

        private void AddCoordsString(string[] CoordArray)
        {
            CoordArray[0] = "11";
            CoordArray[1] = "12";
            CoordArray[2] = "13";
            CoordArray[3] = "21";
            CoordArray[4] = "22";
            CoordArray[5] = "23";
            CoordArray[6] = "31";
            CoordArray[7] = "32";
            CoordArray[8] = "33";
        }
        
    }
}
