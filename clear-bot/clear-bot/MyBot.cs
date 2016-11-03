using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace test_bot
{
    class MyBot
    {
        int torbins = 0;
        int russianstart = 0;
        string player1 = "";
        string player2 = "";
        string player3 = "";
        string player4 = "";
        string player5 = "";
        string player6 = "";
        string player1mem = "";
        string player2mem = "";
        string player3mem = "";
        string player4mem = "";
        string player5mem = "";
        string player6mem = "";
        int playernum = 0;
        DiscordClient discord;

        public MyBot()
        {

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.MessageReceived += Discord_MessageReceived;

            discord.UsingCommands(x =>
            {

                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;

            });

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("clear")
                .Do(async (e) =>
                {
                Role admin = e.Server.GetRole(240435153548214272);
                bool test = e.User.HasRole(admin);

                if (test == true)
                {
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(100);

                    await e.Channel.DeleteMessages(messageToDelete);
                  }
                });



            commands.CreateCommand("test")
                .Do(async (e) =>
                {

                    string test = e.Message.MentionedUsers.ToString();
                    
                    await e.Channel.SendMessage(test);
                   

                });


            commands.CreateCommand("cmd")
                .Do(async (e) =>
                {
                    

                    await e.Channel.SendMessage("Commands: !clear, !clearone, !test, !pm, !ruslet, !oh, !torbinstop, !torbinstart, !rusletcommands, !cmd, !bindec, !binenc");


                });

            commands.CreateCommand("rusletcommands")
                .Do(async (e) =>
                {


                    await e.Channel.SendMessage("Commands: !ruslet, !join, !start, !shoot, !rusletcommands");


                });

            commands.CreateCommand("clearone")
                .Do(async (e) =>
                {
                    Role admin = e.Server.GetRole(240435153548214272);
                    bool test = e.User.HasRole(admin);
                    
                    if (test == true) {
                        Message[] messageToDelete;
                        messageToDelete = await e.Channel.DownloadMessages(2);

                        await e.Channel.DeleteMessages(messageToDelete);
                    }
                });

            commands.CreateCommand("pm")
                .Do(async (e) =>
                {
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(1);

                    await e.Channel.DeleteMessages(messageToDelete);

                    Random ran = new Random();
                    int test = ran.Next(1, 7);

                    switch(test)
                    {
                        case 1:
                            await e.User.SendMessage("Your awesome");
                            break;
                        case 2:
                            await e.User.SendMessage("Fuck you");
                            break;
                        case 3:
                            await e.User.SendMessage("Dont even know");
                            break;
                        case 4:
                            await e.User.SendMessage("Do yoou like spam");
                            break;
                        case 5:
                            await e.User.SendMessage("hey");
                            break;
                        case 6:
                            int i = 1;
                            while (i <= 20)
                            {

                                await e.User.SendMessage("spamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspamspam");
                                System.Threading.Thread.Sleep(2000);
                                i++;
                            }
                            break;

                    }

                    

            });

            commands.CreateCommand("ruslet")
                .Do(async (e) =>
                {
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(1);

                    await e.Channel.DeleteMessages(messageToDelete);

                    if (russianstart == 0)
                    {

                        russianstart = 1;

                        await e.Channel.SendMessage("everyone");
                        await e.Channel.SendMessage("A game of russian roulette has started to join type ''!join'' ");

                    } else if (russianstart != 0)
                    {

                        await e.Channel.SendMessage("Wait for the last game to end before starting a new one!");

                    }
                });

            commands.CreateCommand("join")
                            .Do(async (e) =>
                            {
                                if (russianstart == 1 || russianstart == 2)
                                {

                                    
                                    switch (playernum)
                                    {
                                        case 0:
                                            playernum++;
                                            player1 = e.User.Name;
                                            player1mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            break;
                                        case 1:
                                            playernum++;
                                            player2 = e.User.Name;
                                            player2mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            russianstart = 2;
                                            break;
                                        case 2:
                                            playernum++;
                                            player3 = e.User.Name;
                                            player3mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            break;
                                        case 3:
                                            playernum++;
                                            player4 = e.User.Name;
                                            player4mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            break;
                                        case 4:
                                            playernum++;
                                            player5 = e.User.Name;
                                            player5mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            break;
                                        case 5:
                                            playernum++;
                                            player6 = e.User.Name;
                                            player6mem = e.User.Mention;
                                            await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                            break;
                                        case 6:
                                            await e.Channel.SendMessage("Sorry but the game is full.");
                                            break;
                                    }

                                }

                            });

            commands.CreateCommand("start")
                .Do(async (e) =>
                {

                    Random ran = new Random();
                    int ulucky = ran.Next(1, playernum);

                    if (russianstart == 2)
                    {

                        await e.Channel.SendMessage("The game has started and the players are:");
                        switch (playernum)
                        {
                            
                            case 2:
                                await e.Channel.SendMessage(player1mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem);
                                break;
                            case 3:
                                await e.Channel.SendMessage(player1mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player3mem);
                                break;
                            case 4:
                                await e.Channel.SendMessage(player1mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player3mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player4mem);
                                break;
                            case 5:
                                await e.Channel.SendMessage(player1mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player3mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player4mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player5mem);
                                break;
                            case 6:
                                await e.Channel.SendMessage(player1mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player3mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player4mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player5mem);
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player6mem);
                                break;
                        }
                        await e.Channel.SendMessage("test" + russianstart);
                        russianstart = 3;
                        await e.Channel.SendMessage("test" + russianstart);

                    } else if (russianstart == 1)
                    {

                        await e.Channel.SendMessage("Not enough player. the minimum is 2 players.");

                    }

                });

            commands.CreateCommand("shoot")
                .Do(async (e) =>
                {
                     

                    Random ran = new Random();
                    int unlucky = ran.Next(1, playernum);

                        await e.Channel.SendMessage("5");
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage("4");
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage("3");
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage("2");
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage("1");
                    System.Threading.Thread.Sleep(1000);

                    switch (unlucky)
                    {
                        case 1:
                            switch (playernum)
                            {
                                case 2:
                                    await e.Channel.SendMessage(player1mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                    break;
                                case 3:
                                    await e.Channel.SendMessage(player1mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                    break;
                                case 4:
                                        await e.Channel.SendMessage(player1mem + " Boom");
                                System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                    break;
                                case 5:
                                    await e.Channel.SendMessage(player1mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                    break;
                                case 6:
                                    await e.Channel.SendMessage(player1mem + " boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                    await e.Channel.SendMessage(player5mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player6mem + " Click");
                                    break;
                            }
                            break;
                        case 2:
                            switch (playernum)
                            {
                                case 2:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Boom");
                                    break;
                                case 3:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                    break;
                                case 4:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                    break;
                                case 5:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                    break;
                                case 6:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player6mem + " Click");
                                    break;
                            }
                            break;
                        case 3:
                            switch (playernum)
                            {
                                case 3:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Boom");
                                    break;
                                case 4:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                    break;
                                case 5:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                    break;
                                case 6:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player6mem + " Boom");
                                    break;
                            }
                            break;
                        case 4:
                            switch (playernum)
                            {
                                case 4:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Boom");
                                    break;
                                case 5:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                    break;
                                case 6:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player6mem + " Click");
                                    break;
                            }
                            break;
                        case 5:
                            switch (playernum)
                            {
                                case 5:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Boom");
                                    break;
                                case 6:
                                    await e.Channel.SendMessage(player1mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player2mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player3mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player4mem + " Click");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player5mem + " Boom");
                                        System.Threading.Thread.Sleep(500);
                                        await e.Channel.SendMessage(player6mem + " Click");
                                    break;
                            }
                            break;
                        case 6:

                            await e.Channel.SendMessage(player1mem + " Click");
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player2mem + " Click");
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player3mem + " Click");
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player4mem + " Click");
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player5mem + " Click");
                                System.Threading.Thread.Sleep(500);
                                await e.Channel.SendMessage(player6mem + " Boom");

                            break;

                    }
                    russianstart = 0;
                
                });


            commands.CreateCommand("oh")
                .Do(async (e) =>
                {

                    await e.Channel.SendMessage("Oh shit its Torbins");

                });

            commands.CreateCommand("torbinstart")
                .Do(async (e) =>
                {

                    torbins = 1;
                    await e.Channel.SendMessage("'Oh shit it's torbins' just started");

                });

            commands.CreateCommand("torbinstop")
                .Do(async (e) =>
                {

                    torbins = 0;
                    await e.Channel.SendMessage("'Oh shit it's torbins' just stopped");

                });

            discord.ExecuteAndWait(async () =>
            {

                await discord.Connect("MjM3ODYwNjk4OTMwMTUxNDI0.Cud2BA.Fq7eSEoiuBdXwgNk00GZILJ_gco", TokenType.Bot);
                 await discord.Connect("MjQzNjM4NDYwNjYzOTg4MjI1.Cvx5hQ.OzFeFIy2i9ESF0IrY68zBXRUFUA", TokenType.Bot);

            });

        }

        private void Discord_MessageReceived(object sender, MessageEventArgs e)
        {

            if (torbins == 1)
            {

                if (e.User.Name == ("Coby"))
                {

                    e.Channel.SendMessage("Oh shit its Torbins");

                }

            }

            string Commandtest = e.Message.Text.Substring(0, 7);

            if (Commandtest == "!bindec")
            {

                int maxchar = e.Message.Text.Length;

                int binstart = maxchar - 7;
                

                string bincode = e.Message.Text.Substring(8, binstart);

                string text = bincode;
                

                e.Channel.SendMessage(EncodingConverter.BinarytoString(text));

            } else if (Commandtest == "!binenc")
            {

                int maxchar = e.Message.Text.Length;

                int binstart = maxchar - 7;


                string text = e.Message.Text.Substring(8, binstart);

                string bincode = text;


                e.Channel.SendMessage(EncodingConverter.StringtoBinary(bincode));

            }
          

        }

        public class EncodingConverter
        {

            public static string BinarytoString(string Text)
            {
                string Data = Regex.Replace(Text, @"\s+", "");
                List<Byte> List = new List<Byte>();

                for (int i = 0; i < Data.Length; i += 8)
                {
                    List.Add(Convert.ToByte(Data.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(List.ToArray());

            }

            public static string StringtoBinary(string Text)
            {
                string Data = Regex.Replace(Text, @"\s+", "");
                List<Byte> List = new List<Byte>();

                for (int i = 0; i < Data.Length; i += 8)
                {
                    List.Add(Convert.ToByte(Data.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(List.ToArray());

            }
        }

        private void Log(object sender, LogMessageEventArgs e)
        {

            Console.WriteLine(e.Message);

        }

    }
}
