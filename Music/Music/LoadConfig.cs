using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Music
{
    class LoadConfig
    { 

        public static void Load()
        {
            try
            {
                // Loads the Config.xml file which contains options and tokens
                string XML = Path.GetFullPath("Config.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(XML);
                SetInfo(doc);
            }
            catch (FileNotFoundException)
            {
                XMLGeneration.GenerateXML();
            }
        }

        private static void SetInfo(XmlDocument doc)
        {
            Config.DiscordToken = doc.ChildNodes.Item(1).ChildNodes.Item(0).InnerText.ToString();

            Config.MusicFolder = doc.ChildNodes.Item(1).ChildNodes.Item(1).InnerText.ToString();
        }
    }
}
