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
using System.Threading;
using NAudio.Wave;
using System.Xml;
using System.IO;

namespace Music
{
    class Program
    {   
        [STAThread]
        static void Main(string[] args) => new Program().Start();

        public static DiscordClient _client;
        public static IAudioClient _audio;

        [STAThread]
        public void Start()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "Slightly above average bot";
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;

            });

            LoadConfig.Load("SetInfo", null);

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            _client.UsingAudio(x =>
            {
                x.Channels = 2;
                x.EnableEncryption = false;
                x.Bitrate = 128;
                x.Mode = AudioMode.Outgoing;
            });

            Audio audio = new Audio();

            audio.AudioStartup();

            CreateCommands();

            // Connect to any and all servers which the bot is assigned to
            _client.ExecuteAndWait(async () =>
            {
                string XML = Path.GetFullPath("Tokens.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(XML);
                string DiscordToken = doc.ChildNodes.Item(1).InnerText.ToString();

                try
                {
                    await _client.Connect(DiscordToken, TokenType.Bot);
                } catch (Exception)
                {
                    Console.WriteLine("Something went wrong most likely the token you are using is invalid. Silly Human");
                }

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

            CService.CreateCommand("Whatisthis")
                .Description("Do you want to know what i am you have come to the right place")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("I am a bot made by Hanzzi (ง •̀_•́)ง#4288 and i can do all kinds of stuff please refer to !help for all of my beautiful commands");
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

            CService.CreateCommand("MoarCatrito")
                .Alias("ENHANCEDCATS", "BRING THE KITTIES", "Catwrap")
                .Description("Enhanced version of the Catrito Command")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("cat.jpg");
                    await e.Channel.SendFile("cat.jpg");
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
                   string Text = Converters.ConvertToLeet(TextToLeet, e);
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
                    string Wolframify = Converters.WolframQueryHandler(Query);
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
                    await e.Channel.SendFile(Path.GetFullPath("cat.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat2.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat3.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat4.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat5.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat6.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat7.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat8.jpg"));
                    await e.Channel.SendFile(Path.GetFullPath("cat9.jpg"));

                });

            CService.CreateCommand("Snek")
                .Description("What is Snek? Nobody knows.")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("Snek is our lord and savior only surpassed by the almighty Hanzzi and his band of imaginary friends the twins Depression and Debugging.");
                   /*
                   Thread.Sleep(500);
                   ulong DankBot = 243638460663988225;
                   var msgs = (await e.Channel.DownloadMessages(1).ConfigureAwait(false)).Where(m => DankBot == DankBot)?.ToArray();
                   if (msgs == null || !msgs.Any())
                       return;
                   var toDelete = msgs as Message[] ?? msgs.ToArray();
                   await e.Channel.DeleteMessages(toDelete).ConfigureAwait(false);
                   */
               });
            
            CService.CreateCommand("Join")
                .Parameter("Url", ParameterType.Required)
                .Description("Joins a voice channel")
                .Do(async (e) =>
                {
                    try
                    {


                        var voiceChannel = e.Message.User.VoiceChannel;
                        Console.WriteLine("Channel:" + e.Message.User.VoiceChannel.ToString());

                        _audio = await _client.GetService<AudioService>()
                        .Join(voiceChannel);

                        Audio Audio = new Audio();
                        Audio.Download(e.GetArg("Url"));
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                });

            CService.CreateCommand("Leave")
                .Description("Leaves the voice channels")
                .Do(async (e) =>
                {
                    await _audio.Channel.LeaveAudio();
                });
                

            CService.CreateCommand("kick")
                .Alias("Pitythefool", "thispersonisannoying", "IAmSlightlyAnnoyedWithThisPerson")
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
                .Alias("CConsole", "Console", "CC", "Couldyoupleasecleartheconsolemygoodsir")
                .Description("Clears the Console so I can actually see whats happening")
                .Do((e) =>
                {
                    Console.Clear();
                    e.Channel.SendMessage("The Console has been cleared");
                });

            CService.CreateCommand("XD")
                .Alias("ecksdee")
                .Description("XD")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("https://www.youtube.com/watch?v=J7SDyPEjIp0");
                });

            CService.CreateCommand("Fix")
                .Alias("Issue", "Brokenaf", "CancerBot", "Bugs", "ReportBug", "Reportissue")
                .Description("Bug reports")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("Please forward any issues to the issues tab of my Github Repository: https://github.com/hanzzi/Discord-Bot/issues");
               });

            CService.CreateCommand("Bee")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Accordion to all known laws of aviation, there is no way that a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyways. Because bees don't care what humans think is impossible.");
                });

            CService.CreateCommand("Bolden")
                .Parameter("query", ParameterType.Unparsed)
                .Do((e) =>
                {
                    e.Channel.SendMessage(Converters.Embolden(e.GetArg("query"), e));
                });
        }

        

        

    }
}
