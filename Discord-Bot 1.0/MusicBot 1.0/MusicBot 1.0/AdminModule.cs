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

namespace MusicBot_1._0
{

    [Group("admin")]
    public class Admin : ModuleBase
    {
        DiscordSocketClient client = new DiscordSocketClient();

        [Command("ban"), Summary("Bans a user")]
        [RequirePermission(GuildPermission.BanMembers)]
        [Alias("Ban")]
        public async Task Ban(IGuildUser User, string Reason)
        {
            await Context.Channel.SendMessageAsync(User + "Has been banned Reason:" + Reason);
            await Context.Guild.AddBanAsync(User, 1);
        }

        [Command("kick"), Summary("Kicks a user")]
        [RequirePermission(GuildPermission.KickMembers)]
        [Alias("Kick", "boot", "Boot", "CK", "Ck", "ck")]
        public async Task Kick(IGuildUser User, [Remainder] string Reason)
        {
            await User.KickAsync();
            await Context.Channel.SendMessageAsync(User + " Has been kicked. Reason: " + Reason);

        }

    }
}
