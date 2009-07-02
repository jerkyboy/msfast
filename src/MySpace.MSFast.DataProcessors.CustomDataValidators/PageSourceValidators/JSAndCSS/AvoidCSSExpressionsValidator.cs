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
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors;
using System.Text.RegularExpressions;
using System.IO;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.PageSourceValidators.JSAndCSS
{
    public class AvoidCSSExpressionsValidator : DataValidator<ValidationResults<DownloadStateOccurance>>
    {
        private Regex regex = new Regex(".*?expression\\(.*?\\);", RegexOptions.Compiled | RegexOptions.IgnoreCase );

        private String message;
        private int[] grades;

        public override void Init(Dictionary<string, string> config)
        {
            base.Init(config);

            message = config["message"];

            String[] s = config["grades"].Split('|');
            this.grades = new int[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                grades[i] = int.Parse(s[i]);
            }
        }


        public override ValidationResults<DownloadStateOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<DownloadStateOccurance> results = new ValidationResults<DownloadStateOccurance>();
            results.Score = -1;

            DownloadData data = package.GetData<DownloadData>();

            if (data == null ||
                data.Count == 0 ||
                String.IsNullOrEmpty(package.DumpFolder) ||
                Directory.Exists(package.DumpFolder) == false)

                return results;

            results.Score = 100;

            String bodyBuffer = null;
            StreamReader sr = null;
            MatchCollection m = null;
            String folder = package.DumpFolder.Replace("/", "\\");
            if (folder.EndsWith("\\") == false)
                folder += "\\";

            int count = 0;

            foreach (DownloadState ds in data)
            {
                if (ds.URLType != URLType.CSS)
                    continue;

                try
                {
                    sr = new StreamReader(String.Format("{0}B{1}", folder, ds.FileGUID));

                    if (sr != null)
                    {
                        bodyBuffer = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();

                        m = regex.Matches(bodyBuffer);

                        if (m.Count > 0)
                        {
                            count += m.Count;
                            results.Add(new DownloadStateOccurance(ds));
                        }                       
                    }
                }
                catch
                {
                }
            }

            results.ResultsExplenation = String.Format(message, count);

            int c = 0;

            for (int xx = 0; xx < grades.Length; xx++)
            {
                if (grades[xx] < count)
                    c++;
                else
                    break;
            }
            results.Score -= c * 10;

            return results;           
        }
    }
}
