using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Modules;
using WolframAlphaNET;
using WolframAlphaNET.Objects;

namespace Music
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();

        private DiscordClient _client;
        private IAudioClient _audio;


        public void Start()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "DankBot";
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;

            });

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });



            CreateCommands();

            // Connect to any and all servers which the bot is assigned to
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect("MjQwNzUyMDgxOTI4MDYwOTI4.Cvy1UQ.wmqwZri4SqxkUSpcI4Evc099Ja0", TokenType.Bot);

            });


        }

        public void AudioConfig()
        {
            _client.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.Bitrate = 64000;
                x.EnableEncryption = true;
                
            });
        }

        // Log Functon
        public void Log(object sender, LogMessageEventArgs e)
        {
            if (e.Exception != null)
            {
                Console.WriteLine($"[{e.Severity}] [{e.Source}] [{e.Message}] [{e.Exception}]");
            } else
            {
                Console.WriteLine($"[{e.Severity}] [{e.Source}] [{e.Message}]");
            }
        }

        // Commands
        public void CreateCommands()
        {
            var CService = _client.GetService<CommandService>();

            // returns pong
            CService.CreateCommand("ping")
                .Description("Returns with an opposite but totally satisfactory answer.")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Pong!");
                });

            CService.CreateCommand("hello")
                .Description("Says hello to a user")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    var ToReturn = $"Hello {e.GetArg("user")}";
                    await e.Channel.SendMessage(ToReturn);
                });

            CService.CreateCommand("catrito")
                .Description("Sends file to a channel")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("cat.jpg");
                    
                });

            CService.CreateCommand("docs")
                .Description("Displays Documentation for Discord.NET")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("Here is the Documentation: https://discord.foxbot.me/docs/guides/voice.html");
               });

            CService.CreateCommand("coin")
                .Description("Flips a coin")
                .Do(async (e) =>
                {
                    Random rnd = new Random();
                    int Coin = rnd.Next(0, 2);

                    if (Coin == 0)
                    {
                        await e.Channel.SendMessage("Heads");
                    }

                    if (Coin == 1)
                    {
                        await e.Channel.SendMessage("Tails");
                    }

                });

            CService.CreateCommand("leet")
                .Description("leetify your messages with style!")
                .Parameter("Text", ParameterType.Unparsed)
                .Do(async (e) =>
               {
                   string TextToLeet = e.GetArg("Text");
                   string Text = ConvertToLeet(TextToLeet, e);
                   await e.Channel.SendMessage(Text);

               });

            CService.CreateCommand("whoishans")
                .Description("Tells you about the Creator of this bot")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Simply the most epic and awesome person to have ever lived. also according to UrbanDictionary 'he is consistently a badass mofo' here take a look: http://www.urbandictionary.com/define.php?term=Hans");
                });

            CService.CreateCommand("wolfram")
                .Description("Queries the Wolfram Alpha servers politely and the Wolfram Alpha delivers")
                .Parameter("query", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string Query = e.GetArg("query");
                    string Wolframify = WolframQueryHandler(Query);
                    await e.Channel.SendMessage(Wolframify);
                });

            CService.CreateCommand("D10")
                .Description("Rolls a D10")
                .Do(async (e) =>
                {
                    Random rnd = new Random();
                    int Roll = rnd.Next(1, 11);
                    await e.Channel.SendMessage("The Dice Landed on " + Roll.ToString());
                });

            CService.CreateCommand("D6")
                .Description("Rolls a D6")
                .Do(async (e) =>
                {
                    Random rnd = new Random();
                    int Roll = rnd.Next(1, 7);
                    await e.Channel.SendMessage("The Dice Landed on " + Roll.ToString());
                });

            CService.CreateCommand("D20")
                .Description("Rolls a D20")
                .Do(async (e) =>
                {
                    Random rnd = new Random();
                    int Roll = rnd.Next(1, 21);
                    await e.Channel.SendMessage("The Dice Landed on " + Roll.ToString());
                });

            CService.CreateCommand("CatBomb")
                .Description("Bombs the chat with cats")
                .Do(async (e) =>
                {
                    e.Channel.SendFile("cat.jpg");
                    e.Channel.SendFile("cat2.jpg");
                    e.Channel.SendFile("cat3.jpg");
                    e.Channel.SendFile("cat4.jpg");
                    e.Channel.SendFile("cat5.jpg");
                    e.Channel.SendFile("cat6.jpg");
                    e.Channel.SendFile("cat7.jpg");
                    e.Channel.SendFile("cat8.jpg");
                    e.Channel.SendFile("cat9.jpg");

                });

            CService.CreateCommand("Snek")
                .Description("What is Snek? Nobody knows.")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("Snek is our lord and savior only surpassed by the almighty Hanzzi and his band of imaginary friends the twins Depression and Debugging.");
               });

            CService.CreateCommand("Join")
                .Description("Joins a voice channel")
                .Do(async (e) =>
                {
                    //await e.Channel.SendMessage("Trying to join First Channel");
                    ulong User = e.User.Id;
                    Server Server = e.User.Server;
                    Channel Channel = e.User.VoiceChannel;
                    Channel Test = e.Server.GetChannel(243635934212521985);

                    await JoinChannel(User, Server, Test, _audio, Test, e);
                });


            CService.CreateCommand("kick")
                .Alias("Pity the fool", "this person is annoying", "I am slightly annoyed with this person")
                .Parameter("user")
                .Description("Kicks a User, what did you expect?")
                .Do(async (e) =>
                {
                    var User = e.GetArg("user");

                    if (e.User.ServerPermissions.KickMembers)
                    {
                        var usr = e.Server.FindUsers(User).FirstOrDefault();

                        if (usr == null)
                        {
                            await e.Channel.SendMessage("Error User: " + User + " Not Found");
                            return;
                        }
                        try
                        {
                            await usr.Kick().ConfigureAwait(false);
                            await e.Channel.SendMessage(User + " has been KICKED you should pity the fool.");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Something went wrong most likely that i do not have sufficient permissions");
                        }
                    }
                });

            
            CService.CreateCommand("Clear")
                .Description("Clear a specified amount of messages")
                .Parameter("Amount", ParameterType.Unparsed)
                .Do((e) =>
                {
                    e.Channel.SendMessage("LOL JK this feature has not been implemented");
                });

            CService.CreateCommand("ClearConsole")
                .Alias("CConsole", "Console", "CC", "Could you please clear the console my good sir")
                .Description("Clears the Console so I can actually see whats happening")
                .Do((e) =>
                {
                    Console.Clear();
                    e.Channel.SendMessage("The Console has been cleared");
                });
        }


        public async Task JoinChannel(ulong user, Server Server, Channel Channel, IAudioClient _audio, Channel Test, CommandEventArgs e)
        {
            DiscordClient _client = new DiscordClient();


            //var VoiceChannel = _client.FindServers(Server.ToString()).FirstOrDefault().VoiceChannels.FirstOrDefault();

            _client.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            try
            {

                var _vClient = await _client.GetService<AudioService>()
                    .Join(Channel);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

           



            //await _audio.Join(VoiceChannel);

            /*
            AudioService Audio = new AudioService();
            try
            {
                await Audio.Join(Channel);
            } catch (Exception ex)
            {
                Console.WriteLine("Audio Exception  " + ex.ToString());
            }
            */
            /*
            try
            {
                var _vClient = await _client.GetService<AudioService>()
                .Join(Channel);

                

                
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            */




        }
        


        // Converts a string to Lower LeetSpeak
        public static string ConvertToLeet(string ToLeet, CommandEventArgs e)
        {
            string[] carr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", " ", "[", "]", "Æ", "Ø", "Å" };
            string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", ".", " ", "[", "]", "æ", "ø", "å" };
            string[] larr = new string[] { "4", "8", "(", "d", "3", "f", "9", "#", "!", "j", "k", "1", "m", "~", "0", "p", "q", "r", "5", "7", "u", "v", "w", "*", "y", "2", ".", " ", "[", "]", "æ", "ø", "å" };

            string RESULT = "";
            if (ToLeet.Length <= 0)
            {
                e.Channel.SendMessage("Message must Contain characters to convert to leet speak");
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
            return RESULT;
        }
        
        public static string WolframQueryHandler(string query)
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
            return Value;
        } 
    }
}
