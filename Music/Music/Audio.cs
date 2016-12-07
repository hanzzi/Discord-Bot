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


namespace Music
{
    class Audio
    {
        private Discord.Audio.IAudioClient _audio = Program._audio;
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

        public void Test()
        {
            string Filepath = Path.GetFullPath("YouTube\\Test999.mp4");
            FFmpeg(Filepath);
        }

        public void Download(string Url)
        {

            var _TubeClient = YouTube.Default;
            var Video = _TubeClient.GetVideo(Url);

            VideoFormat Format = Video.Format; // Gets the format of the video ie. wmv, avi, mp4, webm and so on
            AudioFormat AudioFormat = Video.AudioFormat; // gets the format of the audio

            string Title = Video.Title;
            string FileExtension = Video.FileExtension;
            string FullName = Video.FullName;

            byte[] bytes = Video.GetBytes();
            var stream = Video.Stream();

            File.WriteAllBytes(Config.MusicFolder + "\\" + "Test5000" + FileExtension, bytes);
            string FilePath = (Config.MusicFolder + "\\" + "Test5000" + FileExtension);
            FFmpeg(FilePath);
        }

        public void FFmpeg(string pathOrUrl)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
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

                    if (byteCount == 0) // FFmpeg did not output anything
                    {
                        Thread.Sleep(3000); // Really bad hack consider a way of checking when its loading sound
                        if (byteCount == 0)
                        {
                            
                            break; // Break out of the while(true) loop, since there was nothing to read.
                        }
                    }
                     

                    Program._audio.Send(buffer, 0, byteCount); // Send our data to Discord
                }
                Program._audio.Wait(); // Wait for the Voice Client to finish sending data, as ffMPEG may have already finished buffering out a song, and it is unsafe to return now.
                _audio.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Converter()
        {

        }

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
    }
}
