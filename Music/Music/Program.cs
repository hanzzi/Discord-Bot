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
                x.LogLevel = LogSeverity.Info;
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

        // Log Functon
        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] [{e.Message}]");
        }

        // Commands
        public void CreateCommands()
        {
            var CService = _client.GetService<CommandService>();

            // returns pong
            CService.CreateCommand("ping!")
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
