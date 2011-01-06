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
using System.Text.RegularExpressions;
using System.IO;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.DataProcessors.Render
{
	class RenderDataProcessor : DataProcessor<RenderData>
	{
        private static Regex render_Dump_Pattern = new Regex("(S|E)([0-9]*?)(\\|(PE|PB|BP|BC|HB|HC|UN|LI|LO|IN|CO|OL)\\|){0,1}:([0-9]{13,15});", RegexOptions.Compiled);
		
		#region DataProcessor<RenderData> Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
            RenderDumpFilesInfo rdfi = new RenderDumpFilesInfo(state);
            
            if (String.IsNullOrEmpty(rdfi.GetFullPath()))
                return false;

            FileInfo fi = new FileInfo(rdfi.GetFullPath());
			return (fi.Exists && fi.Length > 0);
		}

		public override RenderData GetProcessedData(ProcessedDataPackage state)
		{
            RenderDumpFilesInfo rdfi = new RenderDumpFilesInfo(state);
            StreamReader source = new StreamReader(rdfi.Open(FileAccess.Read));

			RenderData renderData = new RenderData();

            if (source == null)
                return renderData;

            renderData.RenderDumpFilename = rdfi.GetFilename();

			String buffer = source.ReadToEnd();

			source.Close();
			source.Dispose();

			MatchCollection matchs = render_Dump_Pattern.Matches(buffer);
			Match match = null;

			bool isStart = false;
			int id = 0;
			long timestamp = 0;
			String type = "";
			RenderedSegment renderedPeice = null;

			for (int i = 0; i < matchs.Count; i++)
			{
				match = matchs[i];

				if (match.Success)
				{
					isStart = (match.Groups[1].Value == "S");
					id = int.Parse(match.Groups[2].Value);
					timestamp = long.Parse(match.Groups[5].Value);
					type = match.Groups[4].Value;

                    if (type == "UN")
                    {
                        renderData.ReadyStateUninitialized = timestamp;
                    }
                    else if (type == "LI")
                    {
                        renderData.ReadyStateLoading = timestamp;
                    }
                    else if (type == "LO")
                    {
                        renderData.ReadyStateLoaded = timestamp;
                    }
                    else if (type == "IN")
                    {
                        renderData.ReadyStateInteractive = timestamp;
                    }
                    else if (type == "CO")
                    {
                        renderData.ReadyStateComplete = timestamp;
                    }
                    else if (type == "OL")
                    {
                        renderData.OnLoad = timestamp;
                    }
                    else
                    {
                        if (renderData.ContainsKey(id) == false)
                        {
                            renderedPeice = new RenderedSegment(id);
                            renderData.Add(id, renderedPeice);
                        }
                        else
                        {
                            renderedPeice = renderData[id];
                        }
                        if (isStart)
                            renderedPeice.StartTime = timestamp;
                        else
                            renderedPeice.EndTime = timestamp;

                        renderedPeice.SegmentType = GetSegmentType(type);
                    }

					if (timestamp > 0)
						state.CollectionStartTime = Math.Min(state.CollectionStartTime, timestamp);
					state.CollectionEndTime = Math.Max(state.CollectionEndTime, timestamp);
					
				}
			}
			renderData.TotalRenderTime = state.CollectionEndTime - state.CollectionStartTime;
			FixAvgMaxMin(renderData);

			return renderData;
		}

		#endregion
		#region Helpers
		
		private SegmentType GetSegmentType(String name)
		{
			if(String.IsNullOrEmpty(name))
				return SegmentType.Unknown;
			
			name = name.ToUpper().Trim();

			if(name == "PE")
				return SegmentType.PostBodyToEnd;
			else if(name == "PB")
				return SegmentType.BodyToPostBody;
			else if(name == "BP")
				return SegmentType.BodyToPostBody;
			else if(name == "BC")
				return SegmentType.BodyContent;
			else if(name == "HB")
				return SegmentType.HeadToBody;
			else if(name == "HC")
				return SegmentType.Head;
				
			return SegmentType.Unknown;
		}
		
		private void FixAvgMaxMin(RenderData renderData)
		{
			double avg = 0;
			foreach (RenderedSegment renderedPeice in renderData.Values)
			{
				if (renderedPeice.StartTime != 0 && renderedPeice.EndTime != 0)
				{
					renderData.AvgRenderTime += renderedPeice.EndTime - renderedPeice.StartTime;
					renderData.MaxRenderTime = Math.Max(renderData.MaxRenderTime, renderedPeice.EndTime - renderedPeice.StartTime);
					renderData.MinRenderTime = Math.Min(renderData.MinRenderTime, renderedPeice.EndTime - renderedPeice.StartTime);
					avg++;
				}
			}
			renderData.AvgRenderTime = (int)(((double)renderData.AvgRenderTime) / avg);
		}
		#endregion
	}
}
