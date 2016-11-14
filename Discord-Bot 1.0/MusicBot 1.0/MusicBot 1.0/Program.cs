using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Rest;
using Discord.Addons;
using Discord.API;
using Discord.Audio;
using Discord.Net;
using Discord.Rpc;
using Discord.Addons.WS4NetCompatibility;


namespace MusicBot_1._0
{
    class Program
    {
        
        static void Main(string[] args) => new Program().Run().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commands;
        //private CommandHandler handler;

        public async Task Run()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                WebSocketProvider = () => new WS4NetProvider()

            });

            client.Log += Client_Log;

            commands = new CommandService();

            string token = "MjQwNzUyMDgxOTI4MDYwOTI4.Cvy1UQ.wmqwZri4SqxkUSpcI4Evc099Ja0";

            await client.LoginAsync(TokenType.Bot, token);
            await client.ConnectAsync();

            var map = new DependencyMap();
            map.Add(client);

            await InstallCommands();

            /*
            handler = new CommandHandler();
            await handler.Install(map);
            */

            

            await Task.Delay(-1);
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;

            await commands.AddModules(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage MessageParam)
        {
            var message = MessageParam as SocketUserMessage;
            if (message == null) return;

            int argpos = 0;

            if (message.HasCharPrefix('!', ref argpos) || message.HasMentionPrefix(client.CurrentUser, ref argpos))
            {
                var context = new CommandContext(client, message);

                var result = await commands.Execute(context, argpos);
                if (!result.IsSuccess)
                    await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

    }
    
}
