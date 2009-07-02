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
using System.IO;
using System.Text.RegularExpressions;

namespace MySpace.MSFast.DataProcessors.Download
{
	public class DownloadDataProcessor : DataProcessor<DownloadData>
	{
        private static Regex download_Dump_Pattern = new Regex("GUID([a-z0-9]{32})CNST([\\-0-9]{13,15})SRST([\\-0-9]{13,15})SRET([\\-0-9]{13,15})RRST([\\-0-9]{13,15})RRET([\\-0-9]{13,15})CNET([\\-0-9]{13,15})TTSN([0-9]*)TTRC([0-9]*)~(.*)\r\n", RegexOptions.Compiled | RegexOptions.Multiline);

		private String filenameFormat_Winpcap = "wdownload_{0}.dat";
		private String filenameFormat_Proxy = "pdownload_{0}.dat";

		#region DataProcessor<DownloadData> Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
			String filename = GetDownloadDumpFilename(state.DumpFolder, state.CollectionID);
			
			if(filename == null)
				return false;
			
			try{
				FileInfo fi = new FileInfo(filename);
				return (fi.Exists && fi.Length > 0);
			}
			catch
			{
			}
			return false;
		}

		public override DownloadData GetProcessedData(ProcessedDataPackage state)
		{
			if (this.IsDataExists(state) == false)
				return null;

			String filename = GetDownloadDumpFilename(state.DumpFolder, state.CollectionID);
			StreamReader source = new StreamReader(filename);
			String buffer = source.ReadToEnd();
			
			source.Close();
			source.Dispose();

			if (buffer == null)
			{
				return null;
			}

			DownloadData processedData = new DownloadData();

			processedData.DownloadDataDumpFilename = new FileInfo(filename).Name;

			DownloadState downloadState = null;

			
			MatchCollection matchs = null;

			matchs = download_Dump_Pattern.Matches(buffer);

			Match match = null;
			URLType type = URLType.Unknown;

			processedData.TotalFiles = matchs.Count;

			for (int i = 0; i < matchs.Count; i++)
			{
				match = matchs[i];

				try
				{
					downloadState = new DownloadState();
                    downloadState.FileGUID = match.Groups[1].ToString();
                    downloadState.ConnectionStartTime = Math.Max(-1, long.Parse(match.Groups[2].ToString()));
					downloadState.SendingRequestStartTime = Math.Max(-1, long.Parse(match.Groups[3].ToString()));
					downloadState.SendingRequestEndTime = Math.Max(-1, long.Parse(match.Groups[4].ToString()));
					downloadState.ReceivingResponseStartTime = Math.Max(-1, long.Parse(match.Groups[5].ToString()));
					downloadState.ReceivingResponseEndTime = Math.Max(-1, long.Parse(match.Groups[6].ToString()));
					downloadState.ConnectionEndTime = Math.Max(-1, long.Parse(match.Groups[7].ToString()));
					downloadState.TotalSent = Math.Max(-1, int.Parse(match.Groups[8].ToString()));
					downloadState.TotalReceived = Math.Max(-1, int.Parse(match.Groups[9].ToString()));

					processedData.TotalDataReceived += downloadState.TotalReceived;
					processedData.TotalDataSent += downloadState.TotalSent;

					if (state != null)
					{
						if (downloadState.ConnectionStartTime > 0)
							state.CollectionStartTime = Math.Min(state.CollectionStartTime, downloadState.ConnectionStartTime);
						state.CollectionEndTime = Math.Max(state.CollectionEndTime, downloadState.ConnectionEndTime);
					}

					downloadState.URL = match.Groups[10].ToString();
					
					processedData.AddLast(downloadState);

					type = GetURLType(downloadState.URL);
					
					downloadState.URLType = type;

					if (type == URLType.CSS)
					{
						processedData.TotalCSS++;
						processedData.TotalCSSWeight += downloadState.TotalReceived;
					}
					else if (type == URLType.Image)
					{
						processedData.TotalImages++;
						processedData.TotalImagesWeight += downloadState.TotalReceived;
					}
					else if (type == URLType.JS)
					{
						processedData.TotalJS++;
						processedData.TotalJSWeight += downloadState.TotalReceived;
					}
				}
				catch
				{
				}
			}

			return processedData;
		}

		#endregion

		#region Helpers
		private static Regex css = new Regex(".*?\\.css(\\?|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex js = new Regex(".*?\\.js(\\?|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex image = new Regex(".*?\\.(jpg|png|jpeg|gif|giff|jiff|bmp|ico)(\\?|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private URLType GetURLType(string p)
		{
			if (css.IsMatch(p))
			{
				return URLType.CSS;
			}
			else if (js.IsMatch(p))
			{
				return URLType.JS;
			}
			else if (image.IsMatch(p))
			{
				return URLType.Image;
			}
			return URLType.Unknown;
		}

		private string GetDownloadDumpFilename(string folder, int collectionId)
		{
			FileInfo w = new FileInfo(folder + "\\" + String.Format(filenameFormat_Winpcap, collectionId));
			FileInfo p = new FileInfo(folder + "\\" + String.Format(filenameFormat_Proxy, collectionId));

			if (w.Exists && w.Length > 0)
				return w.FullName;
			else if (p.Exists && p.Length > 0)
				return p.FullName;

			return null;
		}
		#endregion

	}
}
