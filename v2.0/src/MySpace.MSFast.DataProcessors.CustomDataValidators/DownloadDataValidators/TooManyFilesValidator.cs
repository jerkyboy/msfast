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
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.DownloadDataValidators
{
    public class TooManyFilesValidator : DataValidator<ValidationResults<DownloadStateOccurance>>
	{
        private String js_message;
        private String css_message;

        private int[] js_grades;
        private int[] css_grades;
        
		public override void Init(Dictionary<string, string> config)
		{
			base.Init(config);
            
            js_message = config["js_message"];
            css_message = config["css_message"];
        
            String[] s = config["js_grades"].Split('|');
            this.js_grades = new int[s.Length];
            
            for(int i = 0; i < s.Length ; i++)
            {
                js_grades[i] = int.Parse(s[i]);
            }

            s = config["css_grades"].Split('|');
            this.css_grades = new int[s.Length];

            for(int i = 0; i < s.Length ; i++)
            {
                css_grades[i] = int.Parse(s[i]);
            }
		}



        public override ValidationResults<DownloadStateOccurance> ValidateData(ProcessedDataPackage package)
        {

            ValidationResults<DownloadStateOccurance> results = new ValidationResults<DownloadStateOccurance>();
            results.Score = -1;

            DownloadData data = package.GetData<DownloadData>();

            if (data == null || data.Count == 0)
                return results;

            results.Score = 100;

            List<DownloadState> list = GetAllDownloadsFor(URLType.JS, data);

            String expl = "";
            
            if (list.Count > 0){
                foreach(DownloadState ds in list)
				{
                    DownloadStateOccurance downloadDataValidatorOccurance = new DownloadStateOccurance(ds);
					results.Add(downloadDataValidatorOccurance);
				}
                expl += String.Format(js_message,list.Count);

                int m = 0;
                
                for (int i = 0; i < js_grades.Length; i++)
                {
                    if (js_grades[i] < list.Count)
                        m++;
                    else
                        break;
                }
                results.Score -= m * 10;
            }




            list = GetAllDownloadsFor(URLType.CSS, data);

            if (list.Count > 0)
            {
                foreach (DownloadState ds in list)
                {
                    DownloadStateOccurance downloadDataValidatorOccurance = new DownloadStateOccurance(ds);
                    results.Add(downloadDataValidatorOccurance);
                }

                expl += String.Format(css_message, list.Count);

                int m = 0;

                for (int i = 0; i < css_grades.Length; i++)
                {
                    if (css_grades[i] < list.Count)
                        m++;
                    else
                        break;
                }
                results.Score -= m * 10;
            }




            results.ResultsExplenation = expl;

			return results;

		}

		private List<DownloadState> GetAllDownloadsFor(URLType uRLType, DownloadData data)
		{
			List<DownloadState> list = new List<DownloadState>();

			foreach (DownloadState state in data)
			{
				if (state.URLType == uRLType)
					list.Add(state);
			}
			return list;
		}
	}
}
