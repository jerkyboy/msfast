using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataValidators;
using System.Text.RegularExpressions;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.Core.Logger;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.JavascriptValidators.JSShellOutputReader
{
    public class JavascriptOutputReader
    {
        private static readonly MSFastLogger log = MSFastLogger.GetLogger(typeof(JavascriptOutputReader));

        private static readonly Regex RESULTS = new Regex("^RESULTS\\[([^:]*):[^\\]]*\\]$", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex SOURCE = new Regex("^SOURCE\\[([^:]*):([^\\]]*)\\]$",RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex DOWNLOAD = new Regex("^DOWNLOAD\\[([^\\]]*)\\]$",RegexOptions.Compiled | RegexOptions.Multiline);

        private int score = -1;
        private IValidationResults results = null;
        private ProcessedDataPackage package = null;

        public JavascriptOutputReader(ProcessedDataPackage package)
        {
            this.package = package;
        }

        public IValidationResults GetResults()
        {
            if (results == null && score != -1)
            {
                results = new ValidationResults<SourceValidationOccurance>();
                results.Score = score;
            }
            return results;
        }

        public void OnData(string p)
        {
            try
            {
                if (package == null) return;

                Match match = null;

                if (score == -1 && (match = RESULTS.Match(p)) != null && match.Success)
                {
                    this.score = int.Parse(match.Groups[1].Value);
                    return;
                }

                if (score != -1 && results == null)
                {
                    if (SOURCE.IsMatch(p))
                    {
                        results = new ValidationResults<SourceValidationOccurance>();
                    }
                    else if (DOWNLOAD.IsMatch(p))
                    {
                        results = new ValidationResults<DownloadStateOccurance>();
                    }

                    if (results != null)
                    {
                        results.Score = this.score;
                    }
                }



                if (results is ValidationResults<DownloadStateOccurance> &&
                     package.ContainsKey(typeof(DownloadData)) &&
                    (match = DOWNLOAD.Match(p)) != null && match.Success)
                {
                    String url = match.Groups[1].Value;

                    DownloadData dd = package[typeof(DownloadData)] as DownloadData;
                    DownloadState ds = null;

                    if (dd != null)
                    {
                        foreach (DownloadState d in dd)
                        {
                            if (String.IsNullOrEmpty(d.URL) == false && d.URL.Equals(url))
                            {
                                ds = d;
                                break;
                            }
                        }
                    }

                    if (ds != null)
                    {
                        results.Add(new DownloadStateOccurance(ds));
                    }

                }
                else if (results is ValidationResults<SourceValidationOccurance> &&
                        (package.ContainsKey(typeof(BrokenSourceData)) || package.ContainsKey(typeof(PageSourceData))) &&
                        (match = SOURCE.Match(p)) != null && match.Success)
                {
                    PageSourceData psd = null;

                    if (package.ContainsKey(typeof(BrokenSourceData)))
                        psd = (PageSourceData)package[typeof(BrokenSourceData)];

                    if (psd == null && package.ContainsKey(typeof(PageSourceData)))
                        psd = (PageSourceData)package[typeof(PageSourceData)];

                    results.Add(
                                new SourceValidationOccurance(psd,
                                                                int.Parse(match.Groups[1].Value),
                                                                int.Parse(match.Groups[2].Value))
                                );
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Error parsing validator output", e);
            }
        }
    }
}