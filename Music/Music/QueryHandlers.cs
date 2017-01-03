using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolframAlphaNET;
using WolframAlphaNET.Objects;
using WeatherNet;
using WeatherNet.Clients;
using WeatherNet.Model;
using WeatherNet.Util;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Music
{
    class QueryHandlers
    {
        // Generates a string of Discord Emojis from a string
        public static string Embolden(string query, CommandEventArgs e)
        {
            string[] carr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", " ", "[", "]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", ".", " ", "[", "]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string[] larr = new string[] { ":regional_indicator_a:", ":regional_indicator_b:", ":regional_indicator_c:", ":regional_indicator_d:", ":regional_indicator_e:", ":regional_indicator_f:", ":regional_indicator_g:", ":regional_indicator_h:", ":regional_indicator_i:", ":regional_indicator_j:", ":regional_indicator_k:", ":regional_indicator_l:", ":regional_indicator_m:", ":regional_indicator_n:", ":regional_indicator_o:", ":regional_indicator_p:", ":regional_indicator_q:", ":regional_indicator_r:", ":regional_indicator_s:", ":regional_indicator_t:", ":regional_indicator_u:", ":regional_indicator_v:", ":regional_indicator_w:", ":regional_indicator_x:", ":regional_indicator_y:", ":regional_indicator_z:", ".", " ", "[", "]", ":regional_indicator_1:", ":regional_indicator_2:", ":regional_indicator_3:", ":regional_indicator_4:", ":regional_indicator_5:", ":regional_indicator_6:", ":regional_indicator_7:", ":regional_indicator_8:", ":regional_indicator_9:", ":regional_indicator_0:" };

            string RESULT = "";
            if (query.Length <= 0)
            {
                e.Channel.SendMessage("Message must Contain characters to convert to Bold letters");
            }
            else
            {
                foreach (char ch in query)
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

        // Queries Wolfram Alpha stuff
        public async Task WolframQueryHandler(string query, CommandEventArgs e)
        {
            WolframAlpha wolfram = new WolframAlpha(Config.WolframToken);
            StringBuilder sb = new StringBuilder();

            string Image = null;

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
                            Image = subPod.Image.Src;
                            sb.AppendLine(subPod.Title);
                            sb.AppendLine(subPod.Plaintext);
                            if (Image != null)
                                sb.AppendLine(Image);
                        }
                    }
                    await e.Channel.SendMessage(sb.ToString());
                    sb.Clear();
                }

            }
            string Value = sb.ToString();
            if (Value == string.Empty)
            {
                Value = "The Query was not accepted please try again";
            }
        }
        

        public async Task Forecast(string City, string Country, CommandEventArgs e, string iterations)
        {
            int cycles = Convert.ToInt32(iterations);
            StringBuilder sb = new StringBuilder();
            sb.Append("```");
            sb.Append(Environment.NewLine + "Weather Forecast for");
            Result<FiveDaysForecastResult> Weather = FiveDaysForecast.GetByCityName(City, Country, "en", "metric");
            var WeatherArray = Weather.Items.ToList();
            for (int i = 0; i < cycles; i++)
            {
                sb.Append($"{Environment.NewLine}{WeatherArray[i].City}");
                sb.Append($"{Environment.NewLine}Date: {WeatherArray[i].Date}");
                sb.Append($"{Environment.NewLine}Clouds: {WeatherArray[i].Clouds}");
                sb.Append($"{Environment.NewLine}Description: {WeatherArray[i].Description}");
                sb.Append($"{Environment.NewLine}Humidity: {WeatherArray[i].Humidity}");
                sb.Append($"{Environment.NewLine}Temperature: {WeatherArray[i].Temp}C");
                sb.Append($"{Environment.NewLine}Wind Speed {WeatherArray[i].WindSpeed}");
                sb.Append($"{Environment.NewLine}");
            }
            sb.Append("```");
            await e.Channel.SendMessage(sb.ToString());
        }

        public async Task Weather(string City, string Country, CommandEventArgs e)
        {
            Random rnd = new Random();
            var Weather = CurrentWeather.GetByCityName(City, Country, "en", "metric");
            StringBuilder sb = new StringBuilder();
            sb.Append("```");
            sb.Append($"Current Weather for {Weather.Item.City}{Environment.NewLine}");
            sb.Append($"Date: {Weather.Item.Date}{Environment.NewLine}");
            sb.Append($"Description: {Weather.Item.Description}{Environment.NewLine}");
            sb.Append($"Humidity {Weather.Item.Humidity}{Environment.NewLine}");
            sb.Append($"Temperature: {Weather.Item.Temp}C{Environment.NewLine}");
            sb.Append($"Windspeed: {Weather.Item.WindSpeed}{Environment.NewLine}");
            sb.Append($"Dankness {rnd.Next(0, 100)}%");
            sb.Append("```");


            await e.Channel.SendMessage(sb.ToString());

        }

        public async Task CelciustoFahrenheit(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double Celcius = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToFahrenheit = (Celcius * 9 / 5 + 32);
                await e.Channel.SendMessage(ConvertToFahrenheit.ToString() + "F");
            }
        }

        public async Task CelciusToKelvin(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double KelvinAdd = 273.15;
                double Celcius = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToKelvin = (Celcius + KelvinAdd);
                await e.Channel.SendMessage(ConvertToKelvin.ToString() + "K");
            }
        }

        public async Task FahrenheitToKelvin(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double Kelvin = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToKelvin = ((Kelvin + 459.67) * 5/9);
                await e.Channel.SendMessage(ConvertToKelvin.ToString() + "K");
            }
        }

        public async Task FahrenheitToCelcius(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double Fahrenheit = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToCelcius = ((Fahrenheit-32) * 5/9);
                await e.Channel.SendMessage(ConvertToCelcius.ToString() + "C");
            }
        }

        public async Task KelvinToCelcius(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double KelvinSubtract = 273.15;
                double Kelvin = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToCelcius = (Kelvin - KelvinSubtract);
                await e.Channel.SendMessage(ConvertToCelcius.ToString() + "C");
            }
        }

        public async Task KelvinToFahrenheit(CommandEventArgs e)
        {
            if (Regex.IsMatch(e.GetArg("Temperature"), @"-?\d+(\.\d+)?"))
            {
                double KelvinSubtract = 459.67;
                double Kelvin = Convert.ToInt32(e.GetArg("Temperature"));
                double ConvertToFahrenheit = (Kelvin * 9/5 - KelvinSubtract);
                await e.Channel.SendMessage(ConvertToFahrenheit.ToString() + "C");
            }
        }
    }
}