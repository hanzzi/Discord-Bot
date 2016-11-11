using Discord;
using Discord.Commands;
using Discord.Audio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace test_bot
{
    class MyBot
    {
        int salty = 0;
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
                x.HelpMode = HelpMode.Public;
            });

            discord.UsingAudio(x => 
            {
                x.Mode = AudioMode.Outgoing;
            });
            

            var commands = discord.GetService<CommandService>();
        

            commands.CreateCommand("clear")
                .Description("Deletes messages(only for admins)")
                .Parameter("amount", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(1);
                    await e.Channel.DeleteMessages(messageToDelete);

                Role admin = e.Server.GetRole(244091603621249024);
                bool testforadmin = e.User.HasRole(admin);

                    if (testforadmin == true)
                    {
                        var hans = Convert.ToInt32(e.GetArg("amount"));
                        int i = hans;

                        while (i > 0)
                        {

                            await e.Channel.DeleteMessages(messageToDelete);
                            i--;
                        }
                    }

                });

            commands.CreateCommand("pm")
                .Description("Don't know try it.")
                .Do(async (e) =>
                {
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(1);

                    await e.Channel.DeleteMessages(messageToDelete);

                    Random ran = new Random();
                    int test = ran.Next(1, 7);

                    switch (test)
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

            commands.CreateCommand("Dice")
                .Description("Roll the dizes. the maxium number of sides is 100.")
                .Parameter("sides", ParameterType.Optional)
                .Do(async (e) =>
                {
                    string sides1 = e.GetArg("sides");
                    if (sides1 == "")
                    {
                        sides1 = "6";
                    }

                    int sides = Int32.Parse(sides1);

                    if (sides > 100)
                    {
                        sides = 100;
                    }


                    await e.Channel.SendMessage("Your dice has " + sides + " sides");
                    Random rnd = new Random();
                    sides++;
                    
                    int Roll = rnd.Next(1, sides);
                    await e.Channel.SendMessage("The Dice Landed on " + Roll.ToString());
                });
            
            commands.CreateCommand("salt")
                .Description("salt")
                .Do(async (e) =>
                {
                Role admin = e.Server.GetRole(244091603621249024);
                bool testforadmin = e.User.HasRole(admin);

                    if (testforadmin == true)
                    {
                        salty = 1;
                        await e.Channel.SendMessage("Bad day?");
                    }
                });

            commands.CreateCommand("pepper")
                .Description("no more salt")
                .Do(async (e) =>
                {
                Role admin = e.Server.GetRole(244091603621249024);
                bool testforadmin = e.User.HasRole(admin);

                    if (testforadmin == true)
                    {
                        salty = 0;
                        await e.Channel.SendMessage("Good to see you in a better mood!");
                    }
                });

            commands.CreateCommand("vc")
                .Description("join a voice channel does not work!!!")
                .Do(async (e) =>
                {

                    Channel test = e.Server.GetChannel(243705158372950018);
                    string don = test.ToString();

                    await e.Channel.SendMessage(don);

                    var audio = await discord.GetService<AudioService>()
                        .Join(test);


                });



            commands.CreateCommand("ruslet")
                .Description("russian roulette. Ruslet commands: ruslet, join, start, shoot")
                .Parameter("commands", ParameterType.Optional)
                .Do(async (e) =>
                {

                    string command = e.GetArg("commands");
                    
                    Message[] messageToDelete;
                    messageToDelete = await e.Channel.DownloadMessages(1);

                    await e.Channel.DeleteMessages(messageToDelete);

                    if (command == "")
                    {

                        if (russianstart == 0)
                        {

                            russianstart = 1;
                            player1 = "";
                            player2 = "";
                            player3 = "";
                            player4 = "";
                            player5 = "";
                            player6 = "";
                            player1mem = "";
                            player2mem = "";
                            player3mem = "";
                            player4mem = "";
                            player5mem = "";
                            player6mem = "";
                            playernum = 0;

                            await e.Channel.SendMessage("everyone");
                            await e.Channel.SendMessage("A game of russian roulette has started to join type ''!ruslet join'' ");
                            await e.Channel.SendMessage("When all the players have joined type ''!ruslet start'' ");
                            await e.Channel.SendMessage("When you are ready to shoot type ''!ruslet shoot'' ");
                        }
                        else if (russianstart != 0)
                        {

                            await e.Channel.SendMessage("Wait for the last game to end before starting a new one!");

                        }
                    } else if (command == "join")
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
                                    if (e.User.Name == player1)
                                    {
                                        await e.Channel.SendMessage("You have already joined the game");
                                    }
                                    else
                                    {
                                        playernum++;
                                        player2 = e.User.Name;
                                        player2mem = e.User.Mention;
                                        await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                        russianstart = 2;
                                    }
                                    break;
                                case 2:
                                    if (e.User.Name == player1 || e.User.Name == player2)
                                    {
                                        await e.Channel.SendMessage("You have already joined the game");
                                    }
                                    else
                                    {
                                        playernum++;
                                        player3 = e.User.Name;
                                        player3mem = e.User.Mention;
                                        await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                    }
                                    break;
                                case 3:
                                    if (e.User.Name == player1 || e.User.Name == player2 || e.User.Name == player3)
                                    {
                                        await e.Channel.SendMessage("You have already joined the game");
                                    }
                                    else
                                    {
                                        playernum++;
                                        player4 = e.User.Name;
                                        player4mem = e.User.Mention;
                                        await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                    }
                                    break;
                                case 4:
                                    if (e.User.Name == player1 || e.User.Name == player2 || e.User.Name == player3 || e.User.Name == player4)
                                    {
                                        await e.Channel.SendMessage("You have already joined the game");
                                    }
                                    else
                                    {
                                        playernum++;
                                        player5 = e.User.Name;
                                        player5mem = e.User.Mention;
                                        await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                    }
                                    break;
                                case 5:
                                    if (e.User.Name == player1 || e.User.Name == player2 || e.User.Name == player3 || e.User.Name == player4 || e.User.Name == player5)
                                    {
                                        await e.Channel.SendMessage("You have already joined the game");
                                    }
                                    else
                                    {
                                        playernum++;
                                        player6 = e.User.Name;
                                        player6mem = e.User.Mention;
                                        await e.Channel.SendMessage("Welcome to this game of russian roulette.");
                                    }
                                    break;
                                case 6:
                                    await e.Channel.SendMessage("Sorry but the game is full.");
                                    break;
                            }

                        }
                    } else if (command == "start")
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
                            russianstart = 3;

                        }
                        else if (russianstart == 1)
                        {

                            await e.Channel.SendMessage("Not enough player. the minimum is 2 players.");

                        }
                    } else if (command == "shoot")
                    {
                        Random rnd = new Random();
                        int unluck = playernum;
                        unluck++;
                        int unlucky = rnd.Next(1, unluck);

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
                                        System.Threading.Thread.Sleep(500);
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
                                        await e.Channel.SendMessage(player6mem + " Click");
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
                    }
                });

            commands.CreateCommand("fix")
                .Description("Fix it")
                .Do(async (e) =>
                {
                    
                    await e.Channel.SendMessage("Fix it riot!");
                });

            commands.CreateCommand("bindec")
                .Description("Binary to text converter.")
                .Parameter("bin", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int maxchar = e.Message.Text.Length;

                    int binstart = maxchar - 8;


                    string bincode = e.Message.Text.Substring(8, binstart);

                    string text = bincode;


                    await e.Channel.SendMessage(EncodingConverter.BinarytoString(text));
                });

            commands.CreateCommand("binenc")
                .Description("Text to binary converter.")
                .Parameter("Text", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int maxchar = e.Message.Text.Length;

                    int binstart = maxchar - 8;


                    string text = e.Message.Text.Substring(8, binstart);


                   await e.Channel.SendMessage(EncodingConverter.StringtoBinary(text));
                });

            commands.CreateCommand("hexdec")
                .Description("Hexadecimal to text converter.")
                .Parameter("Hex", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int maxchar = e.Message.Text.Length;

                    int hexstart = maxchar - 8;


                    string text = e.Message.Text.Substring(8, hexstart);

                    string bincode = text;


                   await e.Channel.SendMessage(EncodingConverter.HexToString(text));
                });

            commands.CreateCommand("hexenc")
                .Description("Text to hexadecimal converter.")
                .Parameter("Text", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int maxchar = e.Message.Text.Length;

                    int hexstart = maxchar - 8;


                    string text = e.Message.Text.Substring(8, hexstart);


                    await e.Channel.SendMessage(EncodingConverter.StringToHex(text));
                });


            discord.ExecuteAndWait(async () =>
            {

                await discord.Connect("MjQzNjM4NDYwNjYzOTg4MjI1.Cvx5hQ.OzFeFIy2i9ESF0IrY68zBXRUFUA", TokenType.Bot);
                

            });

        }

        private void Discord_MessageReceived(object sender, MessageEventArgs e)
        {

            
             

            if (salty == 1)
            {

                if (e.Message.Text != "salt" && e.User.Name != "dank_bot")
                {

                    e.Channel.SendMessage("salt");

                }

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

            public static string StringtoBinary(string data)
            {

                string value = string.Empty;

                byte[] ByteText;
                ByteText = System.Text.Encoding.UTF8.GetBytes(data);
                Array.Reverse(ByteText);
                BitArray bit = new BitArray(ByteText);
                StringBuilder sb = new StringBuilder();

                for (int i = bit.Length - 1; i >= 0; i--)
                {
                    if (bit[i] == true)
                    {
                        sb.Append(1);
                    }
                    else
                    {
                        sb.Append(0);
                    }
                }
                return sb.ToString();

            }
            public static string StringToHex(string Data)
            {
                byte[] bytearray = Encoding.Default.GetBytes(Data);
                string HexString = BitConverter.ToString(bytearray);
                HexString = HexString.Replace("-", "");
                return HexString;
            }

            public static string HexToString(string Data)
            {
                if (Data == null)
                {
                    throw new ArgumentNullException("Empty string");
                }
                if (Data.Length % 2 != 0)
                {
                    throw new ArgumentException("String must have an even length");
                }
                byte[] bytes = new byte[Data.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    string CurrentHex = Data.Substring(i * 2, 2);
                    bytes[i] = Convert.ToByte(CurrentHex, 16);
                }
                string ReturnValue = Encoding.GetEncoding("UTF-8").GetString(bytes);
                return ReturnValue.ToString();
            }
        }

        private void Log(object sender, LogMessageEventArgs e)
        {

            Console.WriteLine(e.Message);
            
        }

    }
}
