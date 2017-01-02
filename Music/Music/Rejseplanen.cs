using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Discord.Net;
using Discord.Commands;

namespace Music
{
    class Rejseplanen
    {
        private string BaseUrl = "http://xmlopen.rejseplanen.dk/bin/rest.exe";
        private bool OriginDone = false;
        private bool DestinationDone = false;
        private bool PlanTripDone = false;

        private WebClient _WebClient = new WebClient();

        // searches for the input given by the user
        public async Task UserInputSearch(string UserInput, CommandEventArgs e)
        {
            _WebClient.BaseAddress = BaseUrl;

            Uri URI = new Uri($"{BaseUrl}/location?input={UserInput}");
            
            // Downloads the string and stacks it in neat rows when migrating to 1.0 consider making this with a embedbuilder
           _WebClient.DownloadStringCompleted += async (s, m) =>
           {
               string Response = m.Result;

               XmlDocument doc = new XmlDocument();
               doc.LoadXml(Response);

               XmlElement root = doc.DocumentElement;
               XmlNodeList StopLocations = root.SelectNodes("//LocationList/StopLocation");
               StringBuilder sb = new StringBuilder();
               sb.Capacity = 2000;
               sb.Append("```");
               // TODO: Replace Break Function with stringbuilder buffer which works as a character counter. MIGHT NOT WORK
               foreach (XmlNode Node in StopLocations)
               {
                   sb.Append(Node.Attributes[0].OuterXml + Environment.NewLine);
                   sb.Append(Node.Attributes[1].OuterXml + Environment.NewLine);
                   sb.Append(Node.Attributes[2].OuterXml + Environment.NewLine);
                   sb.Append(Node.Attributes[3].OuterXml + Environment.NewLine);
                   sb.Append("-----------------------" + Environment.NewLine);
                   if (sb.Length > 1890)
                   {
                       break;
                   }
               }
               sb.Append("```");
               await e.Channel.SendMessage(sb.ToString());
           };
            _WebClient.DownloadStringAsync(URI);

        }

        // gets the starting point
        public async Task GetOrigin(CommandEventArgs e, string Origin, string Destination, int Iterations)
        {
            _WebClient.BaseAddress = BaseUrl;

            Uri OriginURI = new Uri($"{BaseUrl}/location?input={Origin}");

            string OriginName = null;
            string OriginX = null;
            string OriginY = null;
            string OriginID = null;

            // Subscribes to the event when the webclient has finished loading a page
            _WebClient.DownloadStringCompleted += async (s, m) =>
            {
                if (OriginDone != true)
                {
                    string Response = m.Result;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Response);

                    XmlElement root = doc.DocumentElement;
                    XmlNodeList StopLocation = root.SelectNodes("//LocationList/StopLocation");
                    XmlNodeList CoordLocation = root.SelectNodes("//LocationList/CoordLocation");
                    StringBuilder sb = new StringBuilder();

                    // Gets the Attributes if the current string is of the type stoplocation
                    if (root.FirstChild.Name == "StopLocation")
                    {
                        try
                        {
                            OriginName = StopLocation.Item(0).Attributes[0].Value;
                            OriginX = StopLocation.Item(0).Attributes[1].Value;
                            OriginY = StopLocation.Item(0).Attributes[2].Value;
                            OriginID = StopLocation.Item(0).Attributes[3].Value;
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    // Gets the Attributes if the current string is of the type coordlocation
                    if (root.FirstChild.Name == "CoordLocation")
                    {
                        try
                        {
                            // If the first Location is a CoordLocation get the first StopLocation 
                            OriginName = StopLocation.Item(0).Attributes[0].Value;
                            OriginX = StopLocation.Item(0).Attributes[1].Value;
                            OriginY = StopLocation.Item(0).Attributes[2].Value;
                            OriginID = StopLocation.Item(0).Attributes[3].Value;
                        }
                        // if the query gets nothing an indexoutofrangeexeption is thrown and an error message is sent
                        catch (IndexOutOfRangeException RangeEx)
                        {
                            await e.Channel.SendMessage("Query failed try again");
                            await e.Channel.SendMessage(RangeEx.ToString());
                        }
                    }
                    OriginDone = true;
                    await GetDestination(e, Destination, OriginID, Iterations);
                }
            };
            _WebClient.DownloadStringAsync(OriginURI);

            

        }

        // gets the destination
        public async Task GetDestination(CommandEventArgs e, string Destination, string OriginID, int Iterations)
        {
            _WebClient.BaseAddress = BaseUrl;

            Uri DestURI = new Uri($"{BaseUrl}/location?input={Destination}");

            string DestName = null;
            string DestX = null;
            string DestY = null;
            string DestID = null;

            // Subscribes to the event when the webclient has finished loading a page
            _WebClient.DownloadStringCompleted += async (s, m) =>
            {
                if (DestinationDone != true)
                {
                    string Response = m.Result;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Response);

                    XmlElement root = doc.DocumentElement;
                    XmlNodeList StopLocation = root.SelectNodes("//LocationList/StopLocation");
                    XmlNodeList CoordLocation = root.SelectNodes("//LocationList/CoordLocation");
                    StringBuilder sb = new StringBuilder();

                    if (root.FirstChild.Name == "StopLocation")
                    {
                        DestName = StopLocation.Item(0).Attributes[0].Value;
                        DestX = StopLocation.Item(0).Attributes[1].Value;
                        DestY = StopLocation.Item(0).Attributes[2].Value;
                        DestID = StopLocation.Item(0).Attributes[3].Value;

                    }
                    if (root.FirstChild.Name == "CoordLocation")
                    {
                        try
                        {
                            DestName = CoordLocation.Item(0).Attributes[0].Value;
                            DestX = CoordLocation.Item(0).Attributes[1].Value;
                            DestY = CoordLocation.Item(0).Attributes[2].Value;
                            DestID = CoordLocation.Item(0).Attributes[3].Value;
                        }
                        catch (IndexOutOfRangeException RangeEx)
                        {
                            await e.Channel.SendMessage("Query failed try again");
                            await e.Channel.SendMessage(RangeEx.ToString());
                        }
                    }
                    DestinationDone = true;
                    await PlanTrip(OriginID, DestX, DestY, DestName, e, Iterations);
                }
            };
            _WebClient.DownloadStringAsync(DestURI);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task PlanTrip(string OriginID, string DestCoordX, string DestCoordY, string DestCoordName, CommandEventArgs e, int Iterations)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                _WebClient.BaseAddress = BaseUrl;

                Uri URI = new Uri($"{BaseUrl}/trip?originId={OriginID}&destCoordX={DestCoordX}&destCoordY={DestCoordY}&destCoordName={DestCoordName}"/*&date={Date}&time={Time}"*/);

                _WebClient.DownloadStringCompleted += async (s, m) =>
                {
                    if (PlanTripDone != true)
                    {
                        string Response = m.Result;

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Response);

                        XmlElement root = doc.DocumentElement;
                        XmlNodeList Trip = root.SelectNodes("//Trip");
                        XmlNodeList Legs = root.SelectNodes("//Trip/Leg");
                        XmlNodeList Origin = root.SelectNodes("//Trip/Leg/Origin");
                        XmlNodeList Destination = root.SelectNodes("//Trip/Leg/Destination");
                        StringBuilder sb = new StringBuilder();
 
                        int Tripcount = 1;
                        int LegCount = 0;

                        try
                        {

                            foreach (XmlNode Node in Trip)
                            {
                                sb.Append("```");
                                sb.Append($"OPTION: {Tripcount}{Environment.NewLine}");

                                for (int i = 0; i < Node.ChildNodes.Count; i++)
                                {
                                    if (Node.ChildNodes.Item(i).Attributes[1].Value == "IC" | Node.ChildNodes.Item(i).Attributes[1].Value == "ICL" | Node.ChildNodes.Item(i).Attributes[1].Value == "LYN")
                                    {
                                        // TRAIN FORMAT
                                        sb.Append($"Train: {Node.ChildNodes.Item(i).Attributes[0].Value.ToString()}{Environment.NewLine}");
                                        sb.Append($"From: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"Departure: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(3).Value}{Environment.NewLine}");
                                        sb.Append($"To: {Node.ChildNodes.Item(i).ChildNodes.Item(1).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"Arrival: {Node.ChildNodes.Item(i).ChildNodes.Item(1).Attributes.Item(3).Value}{Environment.NewLine}");
                                        sb.Append($"Date: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(4).Value}{Environment.NewLine}");
                                        sb.Append($"Track: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(5).Value}{Environment.NewLine}");
                                        sb.Append($"-------------------{Environment.NewLine}");
                                    }
                                    if (Node.ChildNodes.Item(i).Attributes[1].Value.ToString() == "BUS")
                                    {
                                        // BUS FORMAT
                                        sb.Append($"Bus: {Node.ChildNodes.Item(i).Attributes[0].Value.ToString()}{Environment.NewLine}");
                                        sb.Append($"From: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"Departure: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(3).Value}{Environment.NewLine}");
                                        sb.Append($"To: {Node.ChildNodes.Item(i).ChildNodes.Item(1).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"Arrival: {Node.ChildNodes.Item(i).ChildNodes.Item(1).Attributes.Item(3).Value}{Environment.NewLine}");
                                        sb.Append($"Date: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(4).Value}{Environment.NewLine}");
                                        sb.Append($"-------------------{Environment.NewLine}");
                                    }
                                    if (Node.ChildNodes.Item(i).Attributes[1].Value.ToString() == "WALK")
                                    {
                                        sb.Append($"WALK{Environment.NewLine}");
                                        sb.Append($"From: {Node.ChildNodes.Item(i).ChildNodes.Item(0).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"To: {Node.ChildNodes.Item(i).ChildNodes.Item(1).Attributes.Item(0).Value}{Environment.NewLine}");
                                        sb.Append($"{Node.ChildNodes.Item(i).ChildNodes.Item(2).Attributes.Item(0).Value.Replace(";", string.Empty).Replace("Varighed", "Duration").Replace("Afstand", "Distance")}{Environment.NewLine}");
                                        sb.Append($"-------------------{Environment.NewLine}");
                                    }
                                    
                                }
                                sb.Append("```");
                                await e.Channel.SendMessage(sb.ToString());
                                sb.Clear();
                                Tripcount++;
                                if (Tripcount > Iterations)
                                    break;
                            }
                            Tripcount = 1;
                        } catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                        /*
                        foreach (XmlNode Node in Trip)
                        {
                            sb.Append("```");
                            sb.Append($"OPTION: {Tripcount}{Environment.NewLine}");

                            for (int i = 0; i < Legs.Count; i++)
                            {

                                // Ignore all Steps with the type WALK to limit character usage
                                if (Legs.Item(i).Attributes[1].Value != "WALK")
                                {
                                    // if Type of travel is Train
                                    if (Legs.Item(i).Attributes[1].Value == "IC" | Legs.Item(i).Attributes[1].Value == "LYN" | Legs.Item(i).Attributes[1].Value == "REG" | Legs.Item(i).Attributes[1].Value == "S" | Legs.Item(i).Attributes[1].Value == "TOG")
                                    {
                                        sb.Append($"Train: {Legs.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"From: {Origin.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"Departure: {Origin.Item(i).Attributes[3].Value}{Environment.NewLine}");
                                        sb.Append($"To: {Destination.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"Arrival: {Destination.Item(i).Attributes[3].Value}{Environment.NewLine}");
                                        sb.Append($"Date: {Origin.Item(i).Attributes[4].Value}{Environment.NewLine}");
                                        sb.Append($"Track: {Origin.Item(i).Attributes[5].Value}{Environment.NewLine}");
                                        sb.Append($"-------------------{Environment.NewLine}");
                                    }
                                    // If Type of travel is by bus
                                    if (Legs.Item(i).Attributes[1].Value == "BUS" | Legs.Item(i).Attributes[1].Value == "EXB" | Legs.Item(i).Attributes[1].Value == "NB" | Legs.Item(i).Attributes[1].Value == "TB")
                                    {
                                        sb.Append($"Bus: {Legs.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"From: {Origin.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"Departure: {Origin.Item(i).Attributes[3].Value}{Environment.NewLine}");
                                        sb.Append($"To: {Destination.Item(i).Attributes[0].Value}{Environment.NewLine}");
                                        sb.Append($"Arrival: {Destination.Item(i).Attributes[3].Value}{Environment.NewLine}");
                                        sb.Append($"Date: {Origin.Item(i).Attributes[4].Value}{Environment.NewLine}");
                                        sb.Append($"-------------------{Environment.NewLine}");
                                    }
                                }
                            }
                            sb.Append("```");
                            await e.Channel.SendMessage(sb.ToString());
                            sb.Clear();
                            Tripcount++;
                        }*/
                    }
                    PlanTripDone = true;
                };
                _WebClient.DownloadStringAsync(URI);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
