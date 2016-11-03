using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace Music
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();

        private DiscordClient _client;

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
            });






            
            _client.MessageReceived += async (s, e) =>
            {

                if (e.Message.Text.StartsWith("!Voice"))
                {
                    _client.UsingAudio(x =>
                    {
                        x.Mode = AudioMode.Outgoing;
                    });

                    Channel VoiceChannel = _client.FindServers("Discord-Botts").FirstOrDefault().VoiceChannels.FirstOrDefault();

                    IAudioClient _VClient = await _client.GetService<AudioService>()
                                 .Join(VoiceChannel);
                }
                
            };


            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect("MjQwNzUyMDgxOTI4MDYwOTI4.Cvy1UQ.wmqwZri4SqxkUSpcI4Evc099Ja0", TokenType.Bot);
                Console.WriteLine("Connected");
            });


        }

        public void Log(object sender, LogMessageEventArgs e)
        {

        }

        public async void JoinMusic(string Channel)
        {
            _client = new DiscordClient();

            _client.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            var VoiceChannel = _client.FindServers(Channel).FirstOrDefault().VoiceChannels.FirstOrDefault();

            var VoiceClient = await _client.GetService<AudioService>()
                .Join(VoiceChannel);

            await VoiceClient.Join(VoiceChannel);

            
        }

    }
}
