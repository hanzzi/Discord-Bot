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
    
    public class BaseCommands : ModuleBase
    {
        DiscordSocketClient client = new DiscordSocketClient();
        private IAudioClient audio;

        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);

        }

        [Command("whatisthis"), Summary("Do you want to know what i am you have come to the right place")]
        public async Task Whatisthis()
        {
            await ReplyAsync("I am a bot made by Hanzzi (ง •̀_•́)ง#4288 and i can do all kinds of stuff please refer to !help for all of my beautiful commands");
        }

        [Command("catrito"), Summary("Catrito to take away")]
        public async Task Catrito()
        {
            await Context.Channel.SendFileAsync("cat.jpg");
        }

        [Command("MoarCatrito"), Summary("BRING THE KITTIES")]
        public async Task MoarCats()
        {

            await Context.Channel.SendFileAsync("cat.jpg");
            await Context.Channel.SendFileAsync("cat.jpg");
            await Context.Channel.SendFileAsync("cat.jpg");
        }

        [Command("docs"), Summary("Displays the Documentation for Discord.NET 1.0")]
        public async Task Docs()
        {
            await ReplyAsync("Here is the documentation for the API this Bot has been written with: https://discord.foxbot.me/docs/guides/intro.html");
        }

        [Command("coin"), Summary("Flips a coin")]
        public async Task Coin()
        {

            Random rnd = new Random();
            int Coin = rnd.Next(0, 2);

            if (Coin == 0)
            {
                await ReplyAsync("Heads");
            }

            if (Coin == 1)
            {
                await ReplyAsync("Tails");
            }

        }

        [Command("leet"), Summary("Leetify your messages with style!")]
        public async Task Leet(ulong ChannelId, string ToLeet)
        {
            string[] carr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", " ", "[", "]", "Æ", "Ø", "Å" };
            string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", ".", " ", "[", "]", "æ", "ø", "å" };
            string[] larr = new string[] { "4", "8", "(", "d", "3", "f", "9", "#", "!", "j", "k", "1", "m", "~", "0", "p", "q", "r", "5", "7", "u", "v", "w", "*", "y", "2", ".", " ", "[", "]", "æ", "ø", "å" };

            string RESULT = "";
            if (ToLeet.Length <= 0)
            {
                await ReplyAsync("Message must Contain characters to convert to leet speak");
            }
            else
            {
                foreach (char ch in ToLeet)
                {
                    if (Convert.ToInt32(ch) == Convert.ToInt32(ConsoleKey.Enter))
                    { RESULT = RESULT + "\r\n"; }
                    for (int c = 0; c < arr.Length; c++)
                    {
                        if (ch.ToString() == arr[c])
                            RESULT = RESULT + larr[c];
                    }
                    if (Convert.ToInt32(ch) == Convert.ToInt32(ConsoleKey.Enter))
                    { RESULT = RESULT + "\r\n"; }
                    for (int c = 0; c < carr.Length; c++)
                    {
                        if (ch.ToString() == carr[c])
                            RESULT = RESULT + larr[c];
                    }
                }
            }
            await ReplyAsync(RESULT);
        }

        [Command("whoishans"), Summary("Tells you about the creator of this bot")]
        public async Task WhoIsHans()
        {
            await ReplyAsync("Simply the most epic and awesome person to have ever lived. also according to UrbanDictionary 'he is consistently a badass mofo' here take a look: http://www.urbandictionary.com/define.php?term=Hans");
        }

        [Command("Wolfram"), Summary("Queries the Wolfram Alpha servers politely and the Wolfram Alpha delivers")]
        public async Task Wolfram([Summary("Wolfram Alpha Query")] string query)
        {
            WolframAlpha wolfram = new WolframAlpha("U23WH8-ALE9R832G2");
            StringBuilder sb = new StringBuilder();
            string EndResult = "";

            QueryResult results = wolfram.Query(query);

            if (results != null)
            {
                foreach (Pod pod in results.Pods)
                {
                    sb.Append(pod.Title);

                    if (pod.SubPods != null)
                    {
                        foreach (SubPod subPod in pod.SubPods)
                        {
                            sb.AppendLine(subPod.Title);
                            sb.AppendLine(subPod.Plaintext);
                        }
                    }
                }

            }
            string Value = sb.ToString();
            if (Value == "")
            {
                Value = "The Query was not accepted please try again";
            }
            await ReplyAsync(Value);
        }

        [Command("D10"), Summary("Rolls a D10")]
        public async Task D10()
        {
            Random rnd = new Random();
            int Roll = rnd.Next(1, 11);
            await ReplyAsync("The Dice Landed on " + Roll.ToString());
        }

        [Command("D6"), Summary("Rolls a D6")]
        public async Task D6()
        {
            Random rnd = new Random();
            int Roll = rnd.Next(1, 7);
            await ReplyAsync("The Dice Landed on " + Roll.ToString());
        }

        [Command("userinfo"), Summary("Gets user info")]
        [Alias("user", "whois")]
        public async Task UserInfo(IUser user = null)
        {
            var UserInfo = user ?? Context.Client.CurrentUser;

            if (UserInfo.Game != null)
            {
                await ReplyAsync($"{UserInfo.Username}#{UserInfo.Discriminator} Is Currently Playing {UserInfo.Game}, He/She is {UserInfo.Status}, It is {UserInfo.IsBot} that {UserInfo.Username} is a bot no matter what people tell you.");

            }
            else
            {
                await ReplyAsync($"{UserInfo.Username}#{UserInfo.Discriminator} Is Currently Playing Nothing, He/She is {UserInfo.Status}, It is {UserInfo.IsBot} that {UserInfo.Username} is a bot no matter what people tell you.");
            }
        }

        [Command("BotInfo"), Summary("Gets Info about the bot")]
        public async Task BotInfo()
        {
            var Bot = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" + 
                "- Author: " + Bot.Owner.Username + "(ID " + Bot.Owner.Id + ")\n" + 
                "- Made With: Discord.Net (" + DiscordConfig.Version + ")\n" +
                "- Uptime:" + GetUptime + "\n" + "- Amount of Guilds connected: " +
                (Context.Client as DiscordSocketClient).Guilds.Count() + "\n" +
                "- Amount of Users connected: " + (Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)
                );
                
            /*
            var bot = client.CurrentUser;

            await ReplyAsync(bot.CreatedAt + "/n" + bot.Discord + "/n" + bot.Id + "/n" + bot.Username + "/n" + bot.Status);
       */
        }

        private static string GetUptime
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        

        [Command("crust"), Summary("Crusty Goodness")]
        public async Task Crust()
        {
            await ReplyAsync("C R U S T Y G O O D N E S S" + System.Environment.NewLine + "R" + System.Environment.NewLine + "U" + System.Environment.NewLine + "S" + System.Environment.NewLine + "Y" + System.Environment.NewLine + "G" + System.Environment.NewLine + "O" + System.Environment.NewLine + "O" + System.Environment.NewLine + "D" + System.Environment.NewLine + "N" + System.Environment.NewLine + "E" + System.Environment.NewLine + "S" + System.Environment.NewLine + "S");



        }

        [Command("join"), Summary("Join a channel")]
        public async Task JoinChannel(IUserMessage msg, IVoiceChannel Channel = null)
        {
            Channel = Channel ?? (msg.Author as IGuildUser)?.VoiceChannel;
            if (Channel == null) { await msg.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            audio = await Channel.ConnectAsync();

            Console.WriteLine(audio.ConnectionState.ToString());
        }

        [Command("finddignity"), Summary("Finds dignity")]
        public async Task FindDignity()
        {
            await ReplyAsync("The server responded with error 404(Not Found): \"Nothing found with the suffix: Diginity\"");
        }

        [Command("randomemoji"), Summary("Gets a Random Emoji")]
        public async Task Emoji()
        {
            Random rnd = new Random();
            int Roll = rnd.Next(0, 11);
            switch (Roll)
            {
                case 0:

                    await ReplyAsync("");
                    break;
                case 1:
                    await ReplyAsync("Cancer 1");
                    break;
                case 2:
                    await ReplyAsync("Cancer 2");
                    break;
                case 3:
                    await ReplyAsync("cancer 3");
                    break;
                case 4:
                    await ReplyAsync("Cancer 4");
                    break;
                case 5:
                    await ReplyAsync("Cancer 5");
                    break;
                case 6:
                    await ReplyAsync("Cancer 6");
                    break;
                case 7:
                    await ReplyAsync("Cancer 7");
                    break;
                case 8:
                    await ReplyAsync("Cancer 8");
                    break;
                case 9:
                    await ReplyAsync("Cancer 9");
                    break;
                case 10:
                    await ReplyAsync("Cancer 10");
                    break;
            }

        }

         [Command("snek"), Summary("What is Snek? Nobody knows.")]
         public async Task Snek()
        {
            await ReplyAsync("Snek is our lord and savior only surpassed by the almighty Hanzzi and his band of imaginary friends the twins Depression and Debugging.");
        }

        [Command("forecast"), Summary("Gives you the forecast ")]
        public async Task ForeCast(float Latitude, float Longitude)
        {
            string Url = "https://api.weather.mg/forecast";

            WebRequest GETUrl = WebRequest.Create(Url + "location=" + Latitude + "," + Longitude + "?speedUnit=meters_per_second?temperatureUnit=degree?precipitationUnit=millimeter");
            Stream Stream = GETUrl.GetResponse().GetResponseStream();

            StreamReader Reader = new StreamReader(Stream);

            StringBuilder sb = new StringBuilder();
            string Line = "";
            int i = 0;

            while(Line != null)
            {
                i++;
                Line = Reader.ReadLine();
                if (Line != null)
                {
                    sb.Append(i + Line);
                }
            }
        }
    }

    
        
       
    


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
