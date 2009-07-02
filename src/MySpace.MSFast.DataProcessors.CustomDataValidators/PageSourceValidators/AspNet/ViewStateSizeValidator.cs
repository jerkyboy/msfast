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
using MySpace.MSFast.DataProcessors.PageSource;
using System.Text.RegularExpressions;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataValidators;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.PageSourceValidators.AspNet
{
    public class ViewStateSizeValidator : DataValidator<ValidationResults<SourceValidationOccurance>>
	{
		private int errorThreshold;
		private int warningThreshold;
		private int barelyPassedThreshold;

		public override void Init(Dictionary<string, string> config)
		{
			base.Init(config);
			
			errorThreshold = int.Parse(config["error"]) * 1024;
			warningThreshold = int.Parse(config["warning"]) * 1024;
			barelyPassedThreshold = int.Parse(config["barelypassed"]) * 1024;
		}

        public override ValidationResults<SourceValidationOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<SourceValidationOccurance> results = new ValidationResults<SourceValidationOccurance>();
            results.Score = -1;

            PageSourceData data = package.GetData<PageSourceData>();

            if (data == null || String.IsNullOrEmpty(data.PageSource))
                return results;

            String buffer = data.PageSource;

            var res = Regex.Match(buffer, "(?<=__VIEWSTATE\" value=\")(?<val>.*?)(?=\")").Groups["val"];

            if (res.Success == false)
            {
                results.Score = 100;
                return results;
            }

            SourceValidationOccurance occurrence = new SourceValidationOccurance(data, res.Index, res.Length);

            results.Add(occurrence);

            if (res.Length > errorThreshold)
            {
                results.Score = 45;
            }
            else if (res.Length > warningThreshold)
            {
                results.Score = 65;
            }
            else if (res.Length > barelyPassedThreshold)
            {
                results.Score = 85;
            }
            else
            {
                results.Score = 100;
            }

            return results;
        }
    }
}
