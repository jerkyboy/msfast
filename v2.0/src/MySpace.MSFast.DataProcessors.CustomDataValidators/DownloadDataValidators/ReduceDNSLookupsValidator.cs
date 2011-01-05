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
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.DownloadDataValidators
{
    public class ReduceDNSLookupsValidator : DataValidator<ValidationResults<String>>
    {
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


        public override ValidationResults<String> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<String> results = new ValidationResults<String>();
            results.Score = -1;

            DownloadData data = package.GetData<DownloadData>();

            if (data == null || data.Count == 0)
                return results;

            results.Score = 100;

            foreach (DownloadState ds in data)
            {
                try{
                    Uri u = new Uri(ds.URL);
                    if(results.Contains(u.Host.Trim().ToLower()) == false)
                    {
                        results.Add(u.Host);
                    }
                }
                catch
                {
                }
            }

            results.ResultsExplenation = message;

            int c = 0;

            for (int xx = 0; xx < grades.Length; xx++)
            {
                if (grades[xx] < results.Count)
                    c++;
                else
                    break;
            }
            results.Score -= c * 10;

            return results;
        }
    }
}
