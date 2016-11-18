using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using WolframAlphaNET;
using WolframAlphaNET.Objects;
using System.Runtime.InteropServices;
using Discord.Audio;
using System.Net;
using System.IO;
using System.Diagnostics;
using Discord.API;

namespace MusicBot_1._0
{
    public class MusicCommands : ModuleBase
    {
        private IAudioClient audio;

        [Command("join"), Summary("Joins Voice Channel")]
        public async Task JoinChannel(/*IUserMessage msg*/)
        {
            
            var User = Context.Message.Author as IGuildUser;


            var Channel = User.VoiceChannel;
            
            //channel = channel ?? (msg.Author as IGuildUser)?.VoiceChannel;
            //if (channel == null) { await msg.Channel.SendMessageAsync("User Must be in a voice channel, or a voice channel must be passed."); return; }

            audio = await Channel.ConnectAsync();
            Console.WriteLine("Maybe done it?");
        }
    }
}
