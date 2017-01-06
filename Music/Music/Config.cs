using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class Config
    {
        public static string DiscordToken { get; set; }
        public static string MusicFolder { get; set; }
        //public static string RadioStreamFolder { get; set; }
        public static string WeatherAPIToken { get; set; }
        public static string WolframToken { get; set; }
        public static char Prefix { get; set; }
    }
}
