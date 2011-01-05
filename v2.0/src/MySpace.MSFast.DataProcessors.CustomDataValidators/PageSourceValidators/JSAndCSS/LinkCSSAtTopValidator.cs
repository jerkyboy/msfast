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
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.PageSourceValidators.JSAndCSS
{
    public class LinkCSSAtTopValidator : DataValidator<ValidationResults<SourceValidationOccurance>>
	{
		#region Validator Members

		Regex linkoutsideofhead = new Regex("<\\s*(?i:\\s*l\\s*i\\s*n\\s*k)[^>\\)\\(]*(?i:\\s*h\\s*r\\s*e\\s*f)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		Regex head = new Regex("<\\s*/?\\s*(?i:\\/\\s*h\\s*e\\s*a\\s*d)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);


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

        public override ValidationResults<SourceValidationOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<SourceValidationOccurance> results = new ValidationResults<SourceValidationOccurance>();
            results.Score = -1;

            PageSourceData data = package.GetData<PageSourceData>();

            if (data == null || String.IsNullOrEmpty(data.PageSource))
                return results;

            int headIndex = 0;

            results.Score = 100;

            Match headMatch = head.Match(data.PageSource);

            if (headMatch != null && headMatch.Success)
            {
                headIndex = headMatch.Index;
            }

            MatchCollection mc = linkoutsideofhead.Matches(data.PageSource);

            int i = 0;

            foreach (Match m in mc)
            {
                if (m.Index > headIndex)
                {
                    String ma = m.ToString();
                    results.Add(new SourceValidationOccurance(data, m.Index, m.Length));
                    i++;
                }
            }

            results.ResultsExplenation = String.Format(message, results.Count);

            int c = 0;

            for (int xx = 0;xx < grades.Length; xx++)
            {
                if (grades[xx] < results.Count)
                    c++;
                else
                    break;
            }
            results.Score -= c * 10;

            return results;
        }

        #endregion
    }
}