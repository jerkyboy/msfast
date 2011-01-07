//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace MySpace.MSFast.Engine.CollectorsConfiguration
{
    public class CollectorScript
    {
        public String Script;
        public String Name;

        public CollectorScript(XmlNode x)
        {
            this.Name = x.Attributes["name"].Value;
            this.Script = x.InnerText;
        }
    }
	
	public class CollectorScriptsConfig : Dictionary<String,CollectorScript>
	{
        private static CollectorScriptsConfig _instance;
        private static object syncLock = new object();
        public static CollectorScriptsConfig Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                
                lock (syncLock)
                {
                    if (_instance != null)
                        return _instance;

                    Stream configStream = null;

                    String config = Path.GetDirectoryName(Assembly.GetAssembly(typeof(CollectorScriptsConfig)).Location) + "\\conf\\DataCollectors.config";

                    try
                    {
                        if (File.Exists(config))
                        {
                            configStream = File.Open(config, FileMode.Open);
                        }
                        else
                        {
                            configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MySpace.MSFast.Engine.DataCollectors.config");
                        }

                        _instance = new CollectorScriptsConfig(configStream);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        try
                        {
                            if (configStream != null)
                                configStream.Close();
                        }
                        catch
                        {
                        }
                        configStream = null;
                    }
                }

                return _instance;
            }
        }

        public String EmptyHTML = String.Empty;

        public String PageDataCollector = String.Empty;
        
        public String Constructor = String.Empty;
        
        public String Event_OnInit = String.Empty;
        
        public String Event_OnStartingTest = String.Empty;
        public String Event_OnLoadingFirstCollectionPage = String.Empty;

        public String Event_OnStartDocument = String.Empty;
        public String Event_OnStartHtml = String.Empty;
        public String Event_OnStartHead = String.Empty;
        public String Event_OnEndHead = String.Empty;
        public String Event_OnStartBody = String.Empty;
        public String Event_OnSegment = String.Empty;
        public String Event_OnEndBody = String.Empty;
        public String Event_OnEndHtml = String.Empty;
        public String Event_OnEndDocument = String.Empty;

        private CollectorScriptsConfig(Stream xmlStream)
		{
            XmlDocument xml = new XmlDocument();

            try
            {
                xml.Load(xmlStream);
                ParseXML(xml);
            }
            catch
            {
            
            }
        }

		private void ParseXML(XmlDocument configXml)
		{
			XmlNode config = configXml.ChildNodes[1];

			if (config != null)
			{
				foreach (XmlNode node in config.ChildNodes)
				{
					if (node.Name.ToLower().Equals("global"))
					{
                        ParseConfigNode(node);
					}
					else if(node.Name.ToLower().Equals("collectors"))
					{
						foreach (XmlNode collectorsNode in node.ChildNodes)
						{
							if (collectorsNode.Name.ToLower().Equals("collector"))
							{
                                CollectorScript cs = new CollectorScript(collectorsNode);
                                
                                if (this.ContainsKey(cs.Name) == false)
                                    this.Add(cs.Name, cs);
							}
						}
					}
				}
			}
		}

        private void ParseConfigNode(XmlNode x)
        {
            foreach (XmlNode xmlNd in x)
            {
                String n = xmlNd.Name.ToLower();

                if (n.Equals("emptyhtml")) { this.EmptyHTML = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector")) { this.PageDataCollector = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_constructor")) { this.Constructor = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_oninit")) { this.Event_OnInit = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onloadingfirstcollectionpage")) { this.Event_OnLoadingFirstCollectionPage = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartingtest")) { this.Event_OnStartingTest = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartdocument")) { this.Event_OnStartDocument = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstarthtml")) { this.Event_OnStartHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstarthead")) { this.Event_OnStartHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendhead")) { this.Event_OnEndHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartbody")) { this.Event_OnStartBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onsegment")) { this.Event_OnSegment = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendbody")) { this.Event_OnEndBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendhtml")) { this.Event_OnEndHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onenddocument")) { this.Event_OnEndDocument = xmlNd.InnerText; }
            }

        }

        public uint GroupCount
        {
            get { return 2; }
        }
        
        public ICollection<CollectorScript> GetGroupScripts(int groupId)
        {
            if (groupId == 0)
            {
                return new CollectorScript[]{
                    this["Start_Download_Tracking_Using_Proxy"],
                    this["Stop_Download_Tracking_Using_Proxy"],
                    this["Start_Browser_Performance_Tracking"],
                    this["Stop_Browser_Performance_Tracking"],
                    this["Clear_Browser_Cache"],
                    this["Track_Page_Render_Segments"],
                    this["Progress_Tracker_Render"]
                };
            }
            else
            {
                return new CollectorScript[]{
                    this["Screenshots_Small"],
                    this["Progress_Tracker_Screenshots"]
                };
            }
        }
	}
}
