using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.StandardObjects;
using BDika.Entities.Results;
using EYF.Providers.Gateway;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Entities.Tests;
using BDika.Providers.Collectors.Browse;
using BDika.Providers.Triggers;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Performance;
using MySpace.MSFast.Core.Configuration.Common;
using EYF.Core.Configuration;
using MySpace.MSFast.DataProcessors.Screenshots;
using System.IO;
using System.Xml;
using MySpace.MSFast.DataProcessors.Markers;

namespace BDika.Providers.Results
{
    public class ResultsProvider
    {
        public static String DefaultThumbnailsContextRoot = AppConfig.Instance["Default.Thumbnails.Root.Context"];
        public static String DefaultThumbnailsServerRoot = AppConfig.Instance["Default.Thumbnails.Root.Server"];

        public static String DefaultCachedDataRootContext = AppConfig.Instance["Default.CachedData.Root.Context"];
        public static String DefaultCachedDataRootServer = AppConfig.Instance["Default.CachedData.Root.Server"];

        public static Entities.Results.Results GetResults(UserID userid, ResultsID ResultsID)
        {
            if (ResultsID.IsValidResultsID(ResultsID) == false) throw new InvalidResultsIDException();

            return EntitiesGateway.GetEntity<Entities.Results.Results>(ResultsID);
        }

        public static Entities.Results.Results GetPendingResults(TesterType TesterType)
        {
            if (TesterType == null) throw new NullReferenceException();
            if (TesterTypeID.IsValidTesterTypeID(TesterType.TesterTypeID) == false) throw new InvalidTesterTypeIDException();

            PendingResultsEntityIndex indx = EntitiesGateway.GetEntity<PendingResultsEntityIndex>(TesterType.TesterTypeID);

            if (indx == null || indx.Results == null)
            {
                TriggersProvider.TriggerAllTimedBased();
                indx = EntitiesGateway.GetEntity<PendingResultsEntityIndex>(TesterType.TesterTypeID);
            }

            if (indx == null)
                return null;

            if (indx.Results == null)
                throw new NullReferenceException();

            Entities.Results.Results res = indx.Results;

            if (String.IsNullOrEmpty(res.RawConfig))
                throw new Exception("Invalid Configuration");

            res.ResultsState = ResultsState.Testing;

            if (EntitiesGateway.UpdateEntity(res))
            {
                return res;
            }

            return null;
        }

        public static bool MarkFailedResults(ResultsID resultsID)
        {
            if (ResultsID.IsValidResultsID(resultsID) == false) throw new InvalidResultsIDException();

            return EntitiesGateway.UpdateEntity(new UpdateResultsStateEntityCommand()
            {
                ResultsID = resultsID,
                ResultsState = ResultsState.Failed
            });
        }
       
        public static bool SaveSuccessfulTest(ResultsID resultsID, ProcessedDataPackage pdp)
        {
            if (pdp == null) throw new NullReferenceException();
            if (ResultsID.IsValidResultsID(resultsID) == false) throw new InvalidResultsIDException();

            DownloadData downloadData = null;
            PerformanceData performanceData = null;
            ScreenshotsData screenshotsData = null;
            MarkersData markersData = null;

            if (pdp.ContainsKey(typeof(DownloadData)))
                downloadData = pdp[typeof(DownloadData)] as DownloadData;

            if (pdp.ContainsKey(typeof(PerformanceData)))
                performanceData = pdp[typeof(PerformanceData)] as PerformanceData;

            if (pdp.ContainsKey(typeof(ScreenshotsData)))
                screenshotsData = pdp[typeof(ScreenshotsData)] as ScreenshotsData;

            if (pdp.ContainsKey(typeof(MarkersData)))
                markersData = pdp[typeof(MarkersData)] as MarkersData;
            
            #region Save Results Object

            UpdateResultsStateEntityCommand res = new UpdateResultsStateEntityCommand();

            res.ResultsID = resultsID;
            res.ResultsState = ResultsState.Succeeded;

            res.StartTime = (ulong)pdp.CollectionStartTime;
            res.EndTime = (ulong)pdp.CollectionEndTime;

            if (downloadData != null)
            {
                res.FirstRequestTime = (uint)(downloadData.First.Value.ConnectionEndTime - downloadData.First.Value.ConnectionStartTime);
                res.TotalDownloadsCount = (uint)downloadData.TotalFiles;
                res.TotalJSDownloadsCount = (uint)downloadData.TotalJS;
                res.TotalCSSDownloadsCount = (uint)downloadData.TotalCSS;
                res.TotalImagesDownloadsCount = (uint)downloadData.TotalImages;
                res.TotalDownloadSize = (uint)downloadData.TotalDataReceived;
                res.TotalJSDownloadSize = (uint)downloadData.TotalJSWeight;
                res.TotalCSSDownloadSize = (uint)downloadData.TotalCSSWeight;
                res.TotalImagesDownloadSize = (uint)downloadData.TotalImagesWeight;
            }

            if (performanceData != null)
            {
                res.ProcessorTimeAvg = (uint)performanceData.AvgProcessorTime;
                res.UserTimeAvg = (uint)performanceData.AvgUserTime;
                res.PrivateWorkingSetDelta = (uint)(performanceData.MaxPrivateWorkingSet - performanceData.MinPrivateWorkingSet);
                res.WorkingSetDelta = (uint)(performanceData.MaxWorkingSet - performanceData.MinWorkingSet);
            }

            if (markersData != null)
            {
                res.RenderTime = 0;
                
                foreach (Marker m in markersData)
                {
                    if (res.StartTime < (ulong)m.Timestamp)
                        res.RenderTime = Math.Max(res.RenderTime, (uint)((ulong)m.Timestamp - res.StartTime));
                }
            }

            if (EntitiesGateway.UpdateEntity(res) == false)
                return false;

            #endregion

            #region Save Thumbnails

            if (screenshotsData != null)
            {
                String folder = String.Format(DefaultThumbnailsServerRoot,resultsID);

                if(Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                foreach(Screenshot ss in screenshotsData)
                {
                    FileInfo fi = new FileInfo(ss.Filename);
                    File.Copy(ss.Filename,folder + fi.Name);
                }
            }
            
            #endregion

            pdp.ThumbnailsRoot = String.Format(DefaultThumbnailsContextRoot, resultsID);
            
            XmlDocument xml = pdp.Serialize();
            xml.Save(String.Format(DefaultCachedDataRootServer, resultsID));

            return true;
        }

        public static string GetResultsXMLContextLocation(ResultsID resultsID)
        {
            return String.Format(DefaultCachedDataRootContext, (uint)resultsID);
        }

        public static ThumbnailAndTimestamp[] GetResultsThumbnails(ResultsID resultsID)
        {
            String resxml = String.Format(DefaultCachedDataRootServer, (uint)resultsID);
            
            if (File.Exists(resxml) == false)
                return null;

            XmlDocument x = new XmlDocument();
            x.Load(resxml);
            ProcessedDataPackage pdp = ProcessedDataPackage.Deserialize(x);
            
            if (pdp == null || pdp.CollectionStartTime <=0 || pdp.CollectionEndTime <= 0 || pdp.ContainsKey(typeof(MarkersData)) == false)
                return null;

            LinkedList<ThumbnailAndTimestamp> ls = new LinkedList<ThumbnailAndTimestamp>();
            
            int missed = 0;

            for(int i = 1; i < 100;i++){
                try
                {
                    String filename = String.Format("TC_{0}_{1}.jpg", resultsID, i);

                    if (File.Exists(String.Format(DefaultThumbnailsServerRoot, resultsID) + filename) == false)
                    {
                        if ((missed++) > 15)
                            break;
                    }
                    else
                    {
                        ls.AddLast(new ThumbnailAndTimestamp()
                        {
                            MarkerName = "onSegment(" + i + ")",
                            ThumbnailSrc = String.Format(DefaultThumbnailsContextRoot, resultsID) + filename
                        });
                    }
                }
                catch
                {
                    break;
                }
            }

            LinkedList<Marker> lst = new LinkedList<Marker>(pdp[typeof(MarkersData)] as MarkersData);

            foreach (ThumbnailAndTimestamp t in ls)
            {
                Marker rem = null;
                foreach (Marker m in lst)
                {
                    if (m.Name.Trim().ToLower().Equals(t.MarkerName.Trim().ToLower()))
                    {
                        rem = m;
                        t.Timestamp = (uint)(m.Timestamp - pdp.CollectionStartTime);
                        break;
                    }
                }
                if (rem != null)
                {
                    lst.Remove(rem);
                }
            }

            return ls.ToArray();
        }
    }
    public class ThumbnailAndTimestamp
    {
        public String MarkerName = String.Empty;
        public String ThumbnailSrc = "";
        public uint Timestamp = 0;
    }
}
