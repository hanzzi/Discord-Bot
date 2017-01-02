using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using WolframAlphaNET;
using WolframAlphaNET.Objects;
using System.Threading;
using NAudio.Wave;
using System.Xml;
using System.IO;
using System.Diagnostics;
using WeatherNet.Clients;
using WeatherNet.Model;
using WeatherNet.Util;
using WeatherNet;
using System.Text.RegularExpressions;

namespace Music
{
    class Program
    {
        [STAThread]
        static void Main(string[] args) => new Program().Start();

        public static DiscordClient _client;
        public static IAudioClient _audio;
        public static bool SoundStopCall = false;
        public static bool ChangeStation = false;

        [STAThread]
        public void Start()
        {

            _client = new DiscordClient(x =>
            {
                x.AppName = "Slightly above average bot";
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;
            });


            Rejseplanen Rejseplanen = new Rejseplanen();

            LoadConfig.Load("SetInfo", null);

            // WeatherClient settings, simple af wrapper ftw
            ClientSettings.ApiUrl = "http://api.openweathermap.org/data/2.5";
            ClientSettings.ApiKey = Config.WeatherAPIToken;

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '?';
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
                try
                {
                    await _client.Connect(Config.DiscordToken, TokenType.Bot);
                }
                catch (Exception)
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
            CommandService CService = _client.GetService<CommandService>();

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
                   string Text = QueryHandlers.ConvertToLeet(TextToLeet, e);
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
                    string Wolframify = QueryHandlers.WolframQueryHandler(Query);
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
               });

            CService.CreateCommand("Join")
                .Description("Joins a voice channel")
                .Do(async (e) =>
                {
                    try
                    {
                        var voiceChannel = e.Message.User.VoiceChannel;
                        Console.WriteLine("Channel:" + e.Message.User.VoiceChannel.ToString());

                        _audio = await _client.GetService<AudioService>()
                        .Join(voiceChannel);

                    } catch (Exception ex)
                    {
                        Console.WriteLine($"DATA: {ex.Data}");
                        Console.WriteLine($"INNER EXCEPTION: {ex.InnerException}");
                        Console.WriteLine($"MESSAGE: {ex.Message}");
                        Console.WriteLine($"SOURCE: {ex.Source}");
                        Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
                    }

                });

            CService.CreateCommand("Leave")
                .Description("Leaves the voice channels")
                .Do((e) =>
                {
                    SoundStopCall = true;
                });

            CService.CreateCommand("Add")
                .Parameter("Url", ParameterType.Required)
                .Description("Adds a song to the queue")
                .Do(async (e) =>
                {
                    Audio Audio = new Audio();
                    await Audio.Download(e.GetArg("Url"), e);
                });

            CService.CreateCommand("Play")
                .Description("Plays the Current Music queue")
                .Do(async (e) =>
                {
                    await QueueHandler.NextSong(e);

                });

            CService.CreateCommand("Radio")
                .Parameter("Url")
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
                .Do(async (e) =>
               {
                   Process[] Processes = Process.GetProcessesByName("ffmpeg");
                   if (Processes.Length != 0)
                       ChangeStation = true;

                   Dictionary<string, string> Stations = LoadConfig.GetRadioStations();

                   foreach (var Node in Stations.Keys)
                   {
                       if (Node.Contains(e.GetArg("Url")))
                       {
                           string StationLink = Stations[e.GetArg("Url")];
                           Audio Audio = new Audio();
                           Audio.RadioStations(StationLink, e);
                       }
                   }
               });
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

            CService.CreateCommand("Stations")
                .Description("Displays the Radiostations available")
                .Do(async (e) =>
                {
                    Dictionary<string, string> Stations = LoadConfig.GetRadioStations();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("```");
                    foreach (string Node in Stations.Keys)
                    {
                        sb.Append(Node + Environment.NewLine);
                    }
                    sb.Append("```");
                    await e.Channel.SendMessage(sb.ToString());
                });

            CService.CreateCommand("kick")
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
                            await e.Channel.SendMessage(User + " Got sent to the rice fields");
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
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("LOL JK this feature has not been implemented");
                });

            CService.CreateCommand("Purge")
                .Parameter("Messages", ParameterType.Required)
                .Description("Clears the Console so I can actually see whats happening")
                .Do(async (e) =>
                {
                    int PruneMessages = 0;

                    if (Regex.IsMatch(e.GetArg("Messages"), @"-?\d+(\.\d+)?"))
                    {
                        if (e.GetArg("Messages") != null)
                            PruneMessages = Convert.ToInt32(e.GetArg("Messages"));

                        if (e.User.ServerPermissions.ManageMessages)
                        {
                            PruneMessages++;
                            Message[] Messages = await e.Channel.DownloadMessages(PruneMessages);
                            await e.Channel.DeleteMessages(Messages);
                        }
                        else
                        {
                            await e.Channel.SendMessage("You do not have the required permissions");
                        }
                    }
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
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(QueryHandlers.Embolden(e.GetArg("query"), e));
                });

            CService.CreateCommand("TheSmoke")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("I'll have two number 9s, a number 9 large, a number 6 with extra dip, a number 7, two number 45s, one with cheese, and a large soda.");
                    await e.Channel.SendFile("The Smoke.jpg");
                });

            CService.CreateCommand("Forecast")
                .Description("Gives the forecast for a city")
                .Parameter("City", ParameterType.Required)
                .Parameter("Country", ParameterType.Required)
                .Parameter("Iterations", ParameterType.Required)
                .Do(async (e) =>
                {
                    QueryHandlers Handler = new QueryHandlers();
                    await Handler.Forecast(e.GetArg("City"), e.GetArg("Country"), e, e.GetArg("Iterations"));
                });

            CService.CreateCommand("Weather")
                .Description("Gets the current weather for area")
                .Parameter("City", ParameterType.Required)
                .Parameter("Country", ParameterType.Required)
                .Do(async (e) =>
                {
                    QueryHandlers Query = new QueryHandlers();
                    await Query.Weather(e.GetArg("City"), e.GetArg("Country"), e);
                });

            CService.CreateCommand("Color")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("IT'S CALLED FUCKING COLOUR YOU FUCKING IMBECILE");
                });

            CService.CreateCommand("ZeroWidthSpace")
                .Description("Sends a zero width space")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Between these <​> there is a zero width space");
                });

            CService.CreateCommand("CelciusToFahrenheit")
                .Description("Converts Celcius to Fahrenheit")
                .Alias("CToF")
                .Parameter("Temperature", ParameterType.Required)
                .Do(async (e) =>
                {
                    QueryHandlers Conversion = new QueryHandlers();
                    await Conversion.CelciustoFahrenheit(e);
                });

            CService.CreateCommand("FahrenheitToCelcius")
                .Parameter("Temperature", ParameterType.Required)
                .Alias("FToC")
                .Description("Converts Fahrenheit to Celcius")
                .Do(async (e) =>
                {
                    QueryHandlers Handler = new QueryHandlers();
                    await Handler.FahrenheitToCelcius(e);
                });

            CService.CreateCommand("CelciusToKelvin")
                .Description("Converts Celcius to Kelvin")
                .Alias("CToK")
                .Parameter("Temperature", ParameterType.Required)
                .Do(async (e) =>
                {
                    QueryHandlers Handler = new QueryHandlers();
                    await Handler.CelciusToKelvin(e);
                });

            CService.CreateCommand("FPS")
                .Description("Displays the current FPS on the Bot (Not Really)")
                .Do(async (e) =>
                {
                    Random rnd = new Random();
                    await e.Channel.SendMessage($"The Current FPS is: {rnd.Next(59, 200)}");
                });

            CService.CreateCommand("GTFO")
                .Description("Tells the bot to get the fuck out of your server and never come back.")
                .Do( (e) =>
               {
                   EventHandler<MessageEventArgs> RecievedMessage = null;
                   RecievedMessage = async delegate (object s, MessageEventArgs m)
                   {

                       if (e.Message.User == m.Message.User)
                       {
                           if (m.Message.RawText == "y" | m.Message.RawText == "Y" | m.Message.RawText == "Yes" | m.Message.RawText == "yes")
                           {
                               await m.Channel.SendMessage("Urgh fine ill leave.");
                               _client.MessageReceived -= RecievedMessage;
                               Thread.Sleep(2000);
                               await e.Server.Leave();
                           }
                           if (m.Message.RawText == "n" | m.Message.RawText == "N" | m.Message.RawText == "No" | m.Message.RawText == "no")
                           {
                               await m.Channel.SendMessage("why are you so indecisive");
                               _client.MessageReceived -= RecievedMessage;
                           }
                       }
                       
                   };

                   if (e.User.ServerPermissions.Administrator)
                   {
                       e.Channel.SendMessage("Are you sure you want me gone? Y/N");
                       _client.MessageReceived += RecievedMessage;
                   }
               });

            // Rejseplanen Group Commands
            _client.GetService<CommandService>().CreateGroup("Trip", Trip =>
            {
                Trip.CreateCommand("Search")
                .Description("Searches for a stop or Address")
                .Parameter("Input", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    Rejseplanen Rp = new Rejseplanen();
                    await Rp.UserInputSearch(e.GetArg("Input"), e);
                });

                // More Advanced Trip planning but utterly unuserfriendly but for future additions it might be useful
                /*
                Trip.CreateCommand("AdvPlan")
                .Description("Plans a trip from Rejseplanen.dk Syntax: StartingPointID, Destination Coordinate X, Destination Coordinate Y, Date, Time, Destination name")
                .Parameter("Origin", ParameterType.Required)
                .Parameter("Destination", ParameterType.Required)
                .Do(async (e) =>
                {
                    string OriginID = e.GetArg("OriginID");
                    string DestCoordX = e.GetArg("DestCoordX");
                    string DestCoordY = e.GetArg("DestCoordY");
                    string Date = e.GetArg("Date");
                    string Time = e.GetArg("Time");
                    string DestCoordName = e.GetArg("DestCoordName");

                    Rejseplanen Rp = new Rejseplanen();
                    //await Rp.PlanTrip(OriginID, DestCoordX, DestCoordY, DestCoordName, Date, Time, e);
                });*/

                Trip.CreateCommand("Plan")
                .Parameter("Origin", ParameterType.Unparsed)
                .Description("Searches for a trip between two destinations seperated by a comma")
                //.Parameter("Destination", ParameterType.Required)
                .Do(async (e) =>
                {
                    string[] OriginAndDestination = e.GetArg("Origin").Split(',');
                    if (OriginAndDestination.Length == 3 | OriginAndDestination.Length != 0)
                    {
                        string Origin = OriginAndDestination[0].Replace(" ", string.Empty);
                        string Destination = OriginAndDestination[1].Replace(" ", string.Empty);
                        int Iterations = Convert.ToInt32(OriginAndDestination[2].Replace(" ", string.Empty));

                        Rejseplanen Travel = new Rejseplanen();
                        await Travel.GetOrigin(e, Origin, Destination, Iterations);
                    }
                    else
                        if (OriginAndDestination.Length == 0)
                    {
                        await e.Channel.SendMessage("Iterations cannot be 0");
                        await e.Channel.SendMessage("Format invalid Correct format is: Origin, Destination, Iterations");
                    }
                    else

                        await e.Channel.SendMessage("Format invalid Correct format is: Origin, Destination, Iterations");
                });
            });
        }
    }
}
