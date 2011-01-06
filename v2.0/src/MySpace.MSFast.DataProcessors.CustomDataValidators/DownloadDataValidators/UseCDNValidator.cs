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
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.DownloadDataValidators
{
    public class UseCDNValidator : DataValidator<ValidationResults<DownloadStateOccurance>>
    {
        private Regex regex = null;
        private String message_single = "";
        private String message_more = "";

        public override void Init(Dictionary<string, string> config)
        {
            String rawDomains = config["domains"];
            String[] domains = rawDomains.Split(',');

            String pattern = "http[s]{0,1}://(";
            bool first = true;
            foreach (String domain in domains)
            {
                if (!first) pattern += "|";
                else first = false;
                pattern += domain.Replace(".", "\\.");
            }
            pattern += ")/.*";
            regex = new Regex(pattern, RegexOptions.Compiled);

            message_single = config["msg"];
            message_more = config["msgs"];

        }


        public override ValidationResults<DownloadStateOccurance> ValidateData(ProcessedDataPackage package)
        {
           ValidationResults<DownloadStateOccurance> results = new ValidationResults<DownloadStateOccurance>();
            results.Score = -1;

            DownloadData data = package.GetData<DownloadData>();

            if (data == null || data.Count == 0)
                return results;

            results.Score = 100;

            foreach(DownloadState ds in data)
            {
                if(regex.IsMatch(ds.URL) == false)
                {
                    results.Add(new DownloadStateOccurance(ds));
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
                    results.ResultsExplenation = String.Format(message_more,results.Count);
                }
            }

            return results;
        }
    }
}
