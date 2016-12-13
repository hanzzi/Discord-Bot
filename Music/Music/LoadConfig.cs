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

        public static void Load(string Method, string MusicPath)
        {
            try
            {

                // Loads the Config.xml file which contains Config files
                string XML = Path.GetFullPath("Config.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(XML);
                if (Method == "SetInfo")
                {
                    SetInfo(doc);
                }
                if (Method == "SavePath")
                {
                    SavePath(doc, MusicPath);
                }
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

        public static void SavePath(XmlDocument doc, string MusicPath)
        {
            XmlWriterSettings Settings = new XmlWriterSettings();
            //Settings.Encoding = System.Text.Encoding.UTF8;
            Settings.Indent = true;

            // Generates a new Config file.
            using (XmlWriter Writer = XmlWriter.Create(Path.GetFullPath("Config.xml"), Settings))
            {

                Writer.WriteStartDocument();
                Writer.WriteStartElement("Config");

                Writer.WriteElementString("DiscordToken", Config.DiscordToken);

                Writer.WriteElementString("MusicFolder", MusicPath);

                Writer.WriteEndElement();
                Writer.WriteEndDocument();
            }
        }

        public static Dictionary<string, string> GetRadioStations()
        {
            string xml = Path.GetFullPath("RadioStreams.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            foreach (XmlNode Node in doc.ChildNodes.Item(1))
            {
                Dictionary.Add(Node.Name, Node.InnerText);
            }
            
            return Dictionary;

        }
    }

}
