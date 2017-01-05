using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Music
{
    class XMLGeneration
    { 
        // When a Config file is not present this method will be fired and instruct the user to either create a new one or get one
        public void GenerateXML()
        {
            Console.WriteLine("Config.xml file not found do you wish to generate a new template");
            Console.WriteLine("Y/N");
            string key = Console.ReadLine();

            // Acknowlegdement with generating a new config.xml
            if (key == "y" | key == "Y")
            {

                // If no Config file exists generate a new config.xml
                if (!File.Exists(Path.GetFullPath("Config.xml")))
                {
                    XmlWriterSettings Settings = new XmlWriterSettings();
                    //Settings.Encoding = System.Text.Encoding.UTF8;
                    Settings.Indent = true;

                    // Generates a new Config file.
                    using (XmlWriter Writer = XmlWriter.Create(Path.GetFullPath("Config.xml"), Settings))
                    {

                        Writer.WriteStartDocument();
                        Writer.WriteStartElement("Config");

                        Writer.WriteElementString("DiscordToken", "InsertDiscordTokenHere");
                       
                        Writer.WriteElementString("MusicFolder", "MusicFolderHere");

                        Writer.WriteEndElement();
                        Writer.WriteEndDocument();
                    }

                    Console.WriteLine("Config File Generated Please Restart Application");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);

                }
                else
                {
                    Console.WriteLine("ERROR Unknown Error has been detected please restart" + Environment.NewLine + "Press any key to continue");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else if (key == "n" | key == "N")
            {
                Console.WriteLine("ERROR program cannot continue without a config.xml, please generate Config.xml");
                Console.WriteLine("Press any key to shutdown");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
