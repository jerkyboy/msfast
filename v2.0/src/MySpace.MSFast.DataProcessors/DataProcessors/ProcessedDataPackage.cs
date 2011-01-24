//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors)
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
using MySpace.MSFast.DataProcessors.Performance;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.DataProcessors.Markers;

namespace MySpace.MSFast.DataProcessors
{
    public class ProcessedDataPackage : Dictionary<Type, ProcessedData>, CollectionMetaInfo
	{
		public long CollectionStartTime = long.MaxValue;
		public long CollectionEndTime = long.MinValue;
		public String ThumbnailsRoot = null;

        private int _collectionID;
        private String _dumpFolder;

        public ProcessedDataPackage(int collectionID, String dumpFolder)
        {
            this._dumpFolder = dumpFolder;
            this._collectionID = collectionID;
        }

        public T GetData<T>()
        {
            try
            {
                return (T)this[typeof(T)];
            }
            catch { }
            
            return default(T);
        }

		#region XML Serialization

        public static ProcessedDataPackage Deserialize(XmlDocument xml)
        {
            XmlNode config = xml.ChildNodes[1];
            
            ProcessedDataPackage pd = new ProcessedDataPackage(0,"");

            if (config != null && config.Name.ToLower().Equals("results"))
            {
                foreach (XmlNode node in config.ChildNodes)
                {
                    if (node.Name.ToLower().Equals("performance"))
                    {
                        ParsePerformanceData(node, pd);
                    }
                    else if (node.Name.ToLower().Equals("starttime"))
                    {
                        try { pd.CollectionStartTime = long.Parse(node.InnerText.Trim()); } catch { }
                    }
                    else if (node.Name.ToLower().Equals("endtime"))
                    {
                        try { pd.CollectionEndTime = long.Parse(node.InnerText.Trim()); } catch { }
                    }
                    else if (node.Name.ToLower().Equals("markers"))
                    {
                        ParseMarkersData(node, pd);
                    }
                }
            }

            return pd;
        }

        private static void ParsePerformanceData(XmlNode node, ProcessedDataPackage pd){}

        private static void ParseMarkersData(XmlNode node, ProcessedDataPackage pd)
        {
            MarkersData md = new MarkersData();
            try { md.MaxMarkerTime = long.Parse(node.Attributes["mx"].Value.Trim()); } catch { }
            try { md.MinMarkerTime = long.Parse(node.Attributes["mn"].Value.Trim()); } catch { }
            
            foreach (XmlNode xmlNd in node.ChildNodes)
            {
                if (xmlNd.Name.Equals("m")) { 
                    Marker m = new Marker("",0);
                    try{m.Name = xmlNd.Attributes["n"].Value.Trim();}catch{}
                    try{m.Timestamp = long.Parse(xmlNd.Attributes["t"].Value.Trim());}catch{}
                    if(String.IsNullOrEmpty(m.Name) == false && m.Timestamp > 0)
                        md.AddLast(m);
                }
            }
            if(md.Count > 0)
                pd.Add(typeof(MarkersData),md);            
        }

		public XmlDocument Serialize() 
		{
			XmlDocument xml = new XmlDocument();
			XmlElement results = xml.CreateElement("results");
			
			xml.InsertBefore(xml.CreateXmlDeclaration("1.0", "utf-8", null), xml.DocumentElement);
			
			xml.AppendChild(results);

			AddGlobalData(xml, results);
			AddMarkersTimesData(xml, results);
			AddDownloadData(xml, results);
			AddPerformanceData(xml, results);

			return xml;
		}

		private void AddPerformanceData(XmlDocument xml, XmlElement results)
		{
			if (this.ContainsKey(typeof(PerformanceData)) == false)
				return;

			PerformanceData r = (PerformanceData)this[typeof(PerformanceData)];

			XmlElement b = xml.CreateElement("performance");
			results.AppendChild(b);

			b.SetAttribute("xut", r.MaxUserTime.ToString());
			b.SetAttribute("xpt", r.MaxProcessorTime.ToString());
			b.SetAttribute("xws", r.MaxWorkingSet.ToString());
			b.SetAttribute("xpws", r.MaxPrivateWorkingSet.ToString());
			b.SetAttribute("nut", r.MinUserTime.ToString());
			b.SetAttribute("npt", r.MinProcessorTime.ToString());
			b.SetAttribute("nws", r.MinWorkingSet.ToString());
			b.SetAttribute("npws", r.MinPrivateWorkingSet.ToString());

			XmlElement t = null;

			foreach (PerformanceState ps in r)
			{
				t = xml.CreateElement("p");
				t.SetAttribute("ts", ps.TimeStamp.ToString());
				t.SetAttribute("ut", ps.UserTime.ToString());
				t.SetAttribute("pt", ps.ProcessorTime.ToString());
				t.SetAttribute("ws", ps.WorkingSet.ToString());
				t.SetAttribute("pws", ps.PrivateWorkingSet.ToString());
				b.AppendChild(t);
			}
		}

        private void AddMarkersTimesData(XmlDocument xml, XmlElement results)
        {
            if (this.ContainsKey(typeof(MarkersData)) == false)
                return;

            MarkersData r = (MarkersData)this[typeof(MarkersData)];

            XmlElement b = xml.CreateElement("markers");

            b.SetAttribute("mx", r.MaxMarkerTime.ToString());
            b.SetAttribute("mn", r.MinMarkerTime.ToString());

            results.AppendChild(b);

            XmlElement t = null;

            /*
            SourcePiece[] ib = null;

            if (this.ContainsKey(typeof(BrokenSourceData)))
            {
                BrokenSourceData bs = (BrokenSourceData)this[typeof(BrokenSourceData)];
                if (bs.InjectionBreaks != null)
                {
                    ib = new SourcePiece[bs.InjectionBreaks.Count];
                    int i = 0;

                    foreach (SourcePiece sp in bs.InjectionBreaks)
                    {
                        ib[i] = sp;
                        i++;
                    }
                }
            }
            */

            foreach (Marker marker in r)
            {
                t = xml.CreateElement("m");
                t.SetAttribute("n", marker.Name);
                t.SetAttribute("t", marker.Timestamp.ToString());
                b.AppendChild(t);
            }
        }

        /*
		private void AddRenderTimesData(XmlDocument xml, XmlElement results)
		{
			if (this.ContainsKey(typeof(RenderData)) == false)
				return;

			RenderData r = (RenderData)this[typeof(RenderData)];

			XmlElement b = xml.CreateElement("render");

            b.SetAttribute("un", r.ReadyStateUninitialized.ToString());
            b.SetAttribute("li", r.ReadyStateLoading.ToString());
            b.SetAttribute("lo", r.ReadyStateLoaded.ToString());
            b.SetAttribute("in", r.ReadyStateInteractive.ToString());
            b.SetAttribute("co", r.ReadyStateComplete.ToString());
            b.SetAttribute("ol", r.OnLoad.ToString());

			results.AppendChild(b);

			XmlElement t = null;
			int ss = 0;
			int sl = 0;

            SourcePiece[] ib = null;
			if(this.ContainsKey(typeof(BrokenSourceData)))
			{
                BrokenSourceData bs = (BrokenSourceData)this[typeof(BrokenSourceData)];
                if(bs.InjectionBreaks != null)
                {
                    ib = new SourcePiece[bs.InjectionBreaks.Count];
                    int i = 0;

                    foreach (SourcePiece sp in bs.InjectionBreaks)
                    {
                        ib[i] = sp;
                        i++;
                    }
                }
            }
            foreach (RenderedSegment rp in r.Values)
			{
				ss = 0;
				sl = 0;

				t = xml.CreateElement("r");
				t.SetAttribute("s", rp.StartTime.ToString());
				t.SetAttribute("e", rp.EndTime.ToString());
				t.SetAttribute("i", rp.InjectionID.ToString());
                t.SetAttribute("st", rp.SegmentType.ToString());
                
                if(ib != null && ib.Length > rp.InjectionID)
                {
                    ss = ib[rp.InjectionID].StartIndex;
                    sl = ib[rp.InjectionID].Length;
				}

				t.SetAttribute("ss", ss.ToString());
				t.SetAttribute("sl", sl.ToString());
				
				b.AppendChild(t);
			}
		}*/

		private void AddDownloadData(XmlDocument xml, XmlElement results)
		{
			if (this.ContainsKey(typeof(DownloadData)) == false)
				return;

			DownloadData r = (DownloadData)this[typeof(DownloadData)];

			XmlElement b = xml.CreateElement("download");
			results.AppendChild(b);

			XmlElement t = null;

			foreach (DownloadState ds in r)
			{
				t = xml.CreateElement("d");

				t.InnerText = ds.URL;

				t.SetAttribute("srst", ds.SendingRequestStartTime.ToString());
				t.SetAttribute("sret", ds.SendingRequestEndTime.ToString());
				t.SetAttribute("rrst", ds.ReceivingResponseStartTime.ToString());
				t.SetAttribute("rret", ds.ReceivingResponseEndTime.ToString());
				t.SetAttribute("cnst", ds.ConnectionStartTime.ToString());
                t.SetAttribute("cnet", ds.ConnectionEndTime.ToString());
                t.SetAttribute("ttrc", ds.TotalReceived.ToString());
                t.SetAttribute("ttsn", ds.TotalSent.ToString());

				b.AppendChild(t);
			}
		}
		private void AddGlobalData(XmlDocument xml, XmlElement results)
		{
			XmlElement t = null;
			
			t = xml.CreateElement("testid");
            t.InnerText = CollectionID.ToString();
			results.AppendChild(t);

			t = xml.CreateElement("starttime");
			t.InnerText = CollectionStartTime.ToString();
			results.AppendChild(t);

			t = xml.CreateElement("endtime");
			t.InnerText = CollectionEndTime.ToString();
			results.AppendChild(t);

			if (String.IsNullOrEmpty(this.ThumbnailsRoot) == false)
			{
				t = xml.CreateElement("thumbnailroot");
				t.InnerText = this.ThumbnailsRoot;
				results.AppendChild(t);
			}
		}

		#endregion

        #region CollectionMetaInfo Members

        public int CollectionID
        {
            get {
                return this._collectionID;
            }
        }

        public string DumpFolder
        {
            get {
                return _dumpFolder;
            }
        }
     
        #endregion
    }
}
