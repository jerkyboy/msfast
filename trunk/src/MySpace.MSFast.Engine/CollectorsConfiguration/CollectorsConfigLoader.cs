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



	[Flags]
	public enum CollectPageInformation : int
	{
		Render = 1,
		Screenshots_Small = 2,
		Screenshots_Full = 4,
		Download_Proxy = 8,
		Performance = 16,
		Download_Sniffer = 32,
        ClearCache = 64
	}


	public class CollectorsConfig : List<CollectorsConfig.Collector>
	{
		public _Collection Collection;

		public CollectorsConfig(Stream xmlStream)
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

		public CollectorsConfig(String xmlLocationOrString)
		{
			XmlDocument xml = new XmlDocument();

			try
			{
				xml.Load(xmlLocationOrString);
			}
			catch
			{
				xml.LoadXml(xmlLocationOrString);
			}
			try
			{
				ParseXML(xml);
			}
			catch 
			{
			}
		}

		private void ParseXML(XmlDocument configXml)
		{
			if (configXml.ChildNodes.Count > 2)
			{
				return;
			}

			XmlNode config = configXml.ChildNodes[1];

			if (config != null)
			{
				foreach (XmlNode node in config.ChildNodes)
				{
					if (node.Name.ToLower().Equals("global"))
					{
						this.Collection = new _Collection(node);
					}
					else if(node.Name.ToLower().Equals("collectors"))
					{
						foreach (XmlNode collectorsNode in node.ChildNodes)
						{
							if (collectorsNode.Name.ToLower().Equals("collector"))
							{
								this.Add(new Collector(collectorsNode));
							}
						}
					}
				}
			}
		}

		public class _Collection
		{
			public String JSMain;
			public String JSSetTestID;
			public String JSInit;
			public String JSSection;
			public String JSDone;
			public String JSNextCollect;
			public String JSCollectStarted;
			public String JSCollectEnded;

			public String JSStartCollecting;
			public String Html;
 
			public _Collection(XmlNode x)
			{
				foreach (XmlNode xmlNd in x)
				{
					if (xmlNd.Name.ToLower().Equals("jsmain")){this.JSMain = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jssettestid")){this.JSSetTestID = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jsinit")){this.JSInit = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jssection")){this.JSSection = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jsdone")){this.JSDone = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jsnextcollect")){this.JSNextCollect = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jscollectstarted")){this.JSCollectStarted = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jscollectended")){this.JSCollectEnded = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("jsprecollection")){this.JSStartCollecting = xmlNd.InnerText;}
					else if(xmlNd.Name.ToLower().Equals("html")){this.Html = xmlNd.InnerText;}
				}
			}
		}
		
		public class Collector
		{
			public CollectPageInformation CollectType;
			public bool IsStandalone = false;
			public String Collection;

			public Collector(XmlNode x)
			{
				String collectorName = x.Attributes["name"].Value;
				this.CollectType = (CollectPageInformation)Enum.Parse(typeof(CollectPageInformation),collectorName,true);
				
				try{this.IsStandalone = x.Attributes["standalone"].Value.ToLower().Equals("true");}catch{}

				this.Collection = x.InnerText;
			}
		}
	
		 
	}
	

}
