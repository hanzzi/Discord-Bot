using Discord.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class QueueHandler
    {
        private static Queue MusicQueue = new Queue();
        private static int CurrentIndex = 0;
        private Audio Audio = new Audio();

        // Adds an item to the queue
        public async Task AddItem(string FullName, byte[] bytes, CommandEventArgs e, string Name)
        {
            int Count = MusicQueue.Count;

            // Unassigned character is E022 used as a seperator this is chosen viewed results may vary
            string SaveItemName = $"{Count}{FullName}";

            MusicQueue.Enqueue(SaveItemName);
            File.WriteAllBytes(Config.MusicFolder + "\\" + SaveItemName, bytes);
            await e.Channel.SendMessage($"{Name} Has been added to the queue");

        }

        // Gets the next song in the queue
        public async Task NextSong(CommandEventArgs e)
        {
            object[] MusicArray = MusicQueue.ToArray();

            string CurrentItem = MusicArray.ElementAt(CurrentIndex).ToString();

            string FileDirectory = Config.MusicFolder + "\\" + CurrentItem;

            string NextSong = CurrentItem.Split('').Last().ToString();

            await e.Channel.SendMessage($"Now Playing {NextSong}");
            CurrentIndex++;

            await Audio.FFmpeg(FileDirectory, e);
            
        }

    }
}
