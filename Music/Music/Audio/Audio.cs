using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.Commands;
using Discord.Audio;
using NAudio.Wave;
using VideoLibrary;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Music;
using System.Windows.Forms;
using System.Collections;

namespace Music
{
    class Audio
    {
        private Discord.Audio.IAudioClient _audio = Program._audio;

        // Startup script to check if music folder is present could make check if ffmpeg is present.
        [STAThread]
        public void AudioStartup()
        {
            string path = Config.MusicFolder;
            //string FormatPath = path.Replace("\\", "\\\\");
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Music Folder not found please choose a music folder");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();

                FolderBrowserDialog Dialog = new FolderBrowserDialog();

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    string newPath = Dialog.SelectedPath.Replace("\\\\", "\\");
                    Config.MusicFolder = Dialog.SelectedPath;
                }
            }
            

            
         } 

        // Downloads youtube video and adds it to the queue
        public async Task Download(string Url, CommandEventArgs e)
        {
            YouTube _TubeClient = YouTube.Default;
            YouTubeVideo Video = _TubeClient.GetVideo(Url);

            //IEnumerable<YouTubeVideo> Videos = _TubeClient.GetAllVideos(Url);
            
            // Gets the lowest possible quality of the video this severely worsens the audio quality but makes downloads extremely fast
            // Consider finding a way to only get the best quality audio without video but this is only applicable with large files and slow connections and neither are very useful in the current scope.
            //YouTubeVideo GetLowRes = GetResolution(Videos, 144);

            string Title = Video.Title;
            string FullName = Video.FullName;
            byte[] bytes = Video.GetBytes();
            
            QueueHandler Queue = new QueueHandler();

            await Queue.AddItem(FullName.Replace(' ', '_'), bytes, e, Title);
        }

        // Streaming service for youtube audio stream
        public async static Task FFmpeg(string pathOrUrl, CommandEventArgs e)
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                { // FFmpeg requires us to spawn a process and hook into its stdout, so we will create a Process
                    FileName = "ffmpeg",
                    Arguments = $"-i {pathOrUrl} " + // Here we provide a list of arguments to feed into FFmpeg. -i means the location of the file/URL it will read from
                            "-f s16le -ar 48000 -ac 2 pipe:1", // Next, we tell it to output 16-bit 48000Hz PCM, over 2 channels, to stdout.
                    UseShellExecute = false,
                    RedirectStandardOutput = true // Capture the stdout of the process
                });
                Thread.Sleep(2000); // Sleep for a few seconds to FFmpeg can start processing data.

                int blockSize = 3840; // The size of bytes to read per frame; 1920 for mono
                byte[] buffer = new byte[blockSize];
                int byteCount;

                while (true) // Loop forever, so data will always be read
                {
                    byteCount = process.StandardOutput.BaseStream // Access the underlying MemoryStream from the stdout of FFmpeg
                            .Read(buffer, 0, blockSize); // Read stdout into the buffer

                    int breaklimit = 0;
                    while (byteCount == 0 /*&& breaklimit != 5*/) // counter for failed attempts and sleeps so ffmpeg can read more audio
                    {
                        Thread.Sleep(2500);
                        breaklimit++;
                    }
                    

                    Program._audio.Send(buffer, 0, byteCount); // Send our data to Discord
                    if (breaklimit == 6) // when the breaklimit reaches 6 failed attempts its fair to say that ffmpeg has either crashed or is finished with the song
                    {
                        break; // breaks the audio stream
                    }
                }
                Program._audio.Wait(); // Wait for the Voice Client to finish sending data, as ffMPEG may have already finished buffering out a song, and it is unsafe to return now.
                
                await QueueHandler.NextSong(e); // starts the stream for the next song
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Streaming service for the radio stream
        public static void RadioStream(string pathOrUrl, CommandEventArgs e)
        {
            // Runs ffmpeg in another thread so it does not block non async methods like Createcommands effectively blocking everything
            new Thread(() =>
            {
                try
                {
                    Process process = Process.Start(new ProcessStartInfo
                    { // FFmpeg requires us to spawn a process and hook into its stdout, so we will create a Process
                        FileName = "ffmpeg",
                        Arguments = $"-i {pathOrUrl} -y " + // Here we provide a list of arguments to feed into FFmpeg. -i means the location of the file/URL it will read from
                                "-f s16le -ar 48000 -ac 2 pipe:1", // Next, we tell it to output 16-bit 48000Hz PCM, over 2 channels, to stdout.
                        UseShellExecute = false,
                        RedirectStandardOutput = true // Capture the stdout of the process
                    });
                    Thread.Sleep(2000); // Sleep for a few seconds so FFmpeg can start processing data.

                    int blockSize = 3840; // The size of bytes to read per frame; 1920 for mono
                    byte[] buffer = new byte[blockSize];
                    int byteCount;

                    while (true) // Loop forever, so data will always be read
                    {
                        byteCount = process.StandardOutput.BaseStream // Access the underlying MemoryStream from the stdout of FFmpeg
                                .Read(buffer, 0, blockSize); // Read stdout into the buffer

                        while (byteCount == 0)
                        {
                            Thread.Sleep(2500);
                        }
                        // Call from leave command consider making boolean a method and making it return
                        if (Program.SoundStopCall == true)
                        {
                            Process[] Processes = Process.GetProcessesByName("ffmpeg"); // gets all processes called ffmpeg if more than one instance of ffmpeg is present unstable effects WILL occur
                            if (Processes.Length != 0)
                            {
                                foreach (Process Proc in Processes)
                                {
                                    Processes.FirstOrDefault().Kill(); // gets the first process called ffmpeg
                                }
                                Program._audio.Channel.LeaveAudio(); // leaves the audio channel
                            }
                            Program.SoundStopCall = false; // resets the soundstopcall

                        }
                        // call to change station, kills ffmpeg process to free up pipes or the pipe will break or overflow
                        if (Program.ChangeStation == true)
                        {
                            // gets all processes named ffmpeg.
                            Process[] Processes = Process.GetProcessesByName("ffmpeg"); // gets all processes called ffmpeg
                            if (Processes.Length != 0)
                            {
                                // there is a possibility that there are multiple ffmpeg processes running kills them all
                                foreach (Process Proc in Processes)
                                {
                                    Proc.Kill(); // kills the process
                                }
                            }
                            Program.ChangeStation = false; // resets the changestation call

                        }

                        Program._audio.Send(buffer, 0, byteCount); // Send our data to Discord
                    }

                    //Program._audio.Wait(); // Wait for the Voice Client to finish sending data, as ffMPEG may have already finished buffering out a song, and it is unsafe to return now.
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }).Start();
        }

        // Unused method of sending audio uses nAudio
        public void SendAudio(string filePath)
        {
            try
            {
                var channelCount = Program._client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
                var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
                using (var MP3Reader = new Mp3FileReader(filePath)) // Create a new Disposable MP3FileReader, to read audio from the filePath parameter
                using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
                {
                    resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                    int blockSize = OutFormat.AverageBytesPerSecond / 50; // Establish the size of our AudioBuffer
                    byte[] buffer = new byte[blockSize];
                    int byteCount;

                    while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0) // Read audio into our buffer, and keep a loop open while data is present
                    {
                        if (byteCount < blockSize)
                        {
                            // Incomplete Frame
                            for (int i = byteCount; i < blockSize; i++)
                                buffer[i] = 0;
                        }
                        Program._audio.Send(buffer, 0, blockSize); // Send the buffer to Discord
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        // Displays all radiostations could make it a config and make it dynamic making it possible to add more dynamically
        public static void RadioStations(string Url, CommandEventArgs e)
        {
            RadioStream(Url, e);
        }

        // Legacy code for getting the lowest possible resolution but due to the audio quality lowering HD video is used
        public YouTubeVideo GetResolution(IEnumerable<YouTubeVideo> Videos, int LowestRes)
        {
            return Videos.First(x => x.Resolution == LowestRes);
        }
    }
}
