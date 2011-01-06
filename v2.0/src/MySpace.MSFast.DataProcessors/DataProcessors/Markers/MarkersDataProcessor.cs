using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MySpace.MSFast.Core.Configuration.Common;
using System.IO;

namespace MySpace.MSFast.DataProcessors.Markers
{
    public class MarkersDataProcessor : DataProcessor<MarkersData>
    {
        private static Regex splitPattern = new Regex("([a-z0-9\\s\\(\\)]*?):([0-9]{10,15});", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #region DataProcessor<MarkersData> Members

        public override bool IsDataExists(ProcessedDataPackage state)
        {
            MarkersDumpFilesInfo rdfi = new MarkersDumpFilesInfo(state);

            if (String.IsNullOrEmpty(rdfi.GetFullPath()))
                return false;

            FileInfo fi = new FileInfo(rdfi.GetFullPath());
            return (fi.Exists && fi.Length > 0);
        }

        public override MarkersData GetProcessedData(ProcessedDataPackage state)
        {
            MarkersDumpFilesInfo rdfi = new MarkersDumpFilesInfo(state);
            StreamReader source = new StreamReader(rdfi.Open(FileAccess.Read));

            MarkersData markersData = new MarkersData();

            if (source == null)
                return markersData;

            markersData.MarkersDumpFilename = rdfi.GetFilename();

            String buffer = source.ReadToEnd();

            source.Close();
            source.Dispose();

            MatchCollection matchs = splitPattern.Matches(buffer);
            Match match = null;

            Marker marker = null;

            for (int i = 0; i < matchs.Count; i++)
            {
                match = matchs[i];

                if (match.Success == false)
                    continue;

                try
                {
                    marker = new Marker(match.Groups[1].Value, long.Parse(match.Groups[2].Value));
                }
                catch
                {
                }
                
                if(marker == null)
                    continue;

                if (marker.Timestamp <= 0 || String.IsNullOrEmpty(marker.Name))
                    continue;

                if (marker.Timestamp > 0)
                {
                    state.CollectionStartTime = Math.Min(state.CollectionStartTime, marker.Timestamp);
                    markersData.MinMarkerTime = Math.Min(markersData.MinMarkerTime, marker.Timestamp);
                }

                state.CollectionEndTime = Math.Max(state.CollectionEndTime, marker.Timestamp);
                markersData.MaxMarkerTime = Math.Max(markersData.MaxMarkerTime, marker.Timestamp);
                markersData.AddLast(marker);
            }

            return markersData;
        }

        #endregion
    }
}
