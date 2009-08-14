//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors.CustomDataValidators)
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
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using System.Text.RegularExpressions;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using System.IO;
using MySpace.MSFast.DataValidators;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.DownloadDataValidators
{
    public class AddExpiresHeadersValidator : DataValidator<ValidationResults<DownloadStateOccurance>>
    {
        private Regex regex = new Regex("Expires: (.*?)\r\n",RegexOptions.Compiled);
        private String message_single = "";
        private String message_more = "";

        public override void Init(Dictionary<string, string> config)
        {
            message_single = config["msg"];
            message_more = config["msgs"];
        }

        public override ValidationResults<DownloadStateOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<DownloadStateOccurance> results = new ValidationResults<DownloadStateOccurance>();
            results.Score = -1;

            DownloadData data = package.GetData<DownloadData>();

            ResponseHeaderDumpFilesInfo rbdfi = new ResponseHeaderDumpFilesInfo(package);         

            if (data == null || 
                data.Count == 0)

                return results;

            results.Score = 100;

            String headerBuffer = null;
            StreamReader sr = null;
            Match m = null;

            DateTime now = DateTime.Now.AddDays(2);

            Stream stream = null;

            foreach (DownloadState ds in data)
            {
                try
                {
                    stream = rbdfi.Open(FileAccess.Read, ds.FileGUID);

                    if (stream != null)
                    {
                        sr = new StreamReader(stream);

                        if (sr != null)
                        {
                            headerBuffer = sr.ReadToEnd();
                            sr.Close();
                            sr.Dispose();

                            m = regex.Match(headerBuffer);

                            if (m.Success == false || m.Groups.Count <= 1 || String.IsNullOrEmpty(m.Groups[1].ToString()))
                            {
                                DownloadStateOccurance dso = new DownloadStateOccurance(ds);
                                dso.Comment = "(no expires)";
                                results.Add(dso);
                            }
                            else
                            {
                                try
                                {
                                    if (m.Groups[1].ToString() == "-1")
                                    {
                                        DownloadStateOccurance dso = new DownloadStateOccurance(ds);
                                        dso.Comment = "(-1)";
                                        results.Add(dso);
                                    }
                                    else
                                    {
                                        DateTime dt = DateTime.Parse(m.Groups[1].ToString());

                                        if (dt < now)
                                        {
                                            DownloadStateOccurance dso = new DownloadStateOccurance(ds);
                                            dso.Comment = String.Format("({0})", dt.Date);
                                            results.Add(dso);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            results.Score -= Math.Min(results.Count, 4) * 10;

            if (results.Count > 0)
            {
                if (results.Count == 1)
                {
                    results.ResultsExplenation = message_single;
                }
                else
                {
                    results.ResultsExplenation = String.Format(message_more, results.Count);
                }
            }

            return results;
        }
    }
}
