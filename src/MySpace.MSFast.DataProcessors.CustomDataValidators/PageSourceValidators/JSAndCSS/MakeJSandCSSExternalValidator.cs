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
    public class MakeJSandCSSExternalValidator : DataValidator<ValidationResults<SourceValidationOccurance>>
	{
		#region Validator Members

		Regex style = new Regex("<\\s*(?i:\\s*s\\s*t\\s*y\\s*l\\s*e)[^>]*>.*?<\\s*\\/\\s*(?i:\\s*s\\s*t\\s*y\\s*l\\s*e)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		Regex script = new Regex("<\\s*(?i:\\s*s\\s*t\\s*y\\s*l\\s*e)[^>]*>.*?<\\s*\\/\\s*(?i:\\s*s\\s*t\\s*y\\s*l\\s*e)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);


        public override ValidationResults<SourceValidationOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<SourceValidationOccurance> results = new ValidationResults<SourceValidationOccurance>();
            results.Score = -1;

            PageSourceData data = package.GetData<PageSourceData>();

            if (data == null || String.IsNullOrEmpty(data.PageSource))
                return results;

			MatchCollection mc = style.Matches(data.PageSource);
			int i = 0;

			foreach (Match m in mc)
			{
				String ma = m.ToString();
                results.Add(new SourceValidationOccurance(data, m.Index, m.Length));
				i++;
			}

			if (results.Count == 0)
			{
				results.Score = 100;
			}
			else if (results.Count <= 6)
			{
				results.Score = 85;
			}
            else if (results.Count <= 10)
            {
                results.Score = 75;
            }
            else if (results.Count <= 15)
            {
                results.Score = 65;
            }
            else
			{
				results.Score = 0;
			}


			return results;
		}

		#endregion

	}
}
