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
using System.Text.RegularExpressions;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using System.IO;
using MySpace.MSFast.DataValidators;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.DownloadDataValidators
{
    public class CompressComponentsWithGzipValidator : DataValidator<ValidationResults<DownloadStateOccurance>>
    {
        private Regex regexContentType = new Regex("Content-Type: (text|application)/[^\r\n]*(script|html)", RegexOptions.Compiled);
        private Regex regexContentEncoding = new Regex("Content-Encoding: (gzip|deflate)", RegexOptions.Compiled);
        
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

                            if (regexContentType.IsMatch(headerBuffer) == false)
                                continue;

                            if (regexContentEncoding.IsMatch(headerBuffer) == false)
                            {
                                results.Add(new DownloadStateOccurance(ds));
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
