//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Entities)
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
using System.Linq;
using System.Text;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Entities.Results
{
    [Serializable]
    [DBEntity("results")]
    public class AvgResults : Entity
    {
        [EntityIdentity("resultsid")]
        public ResultsID LatestResultsID;

        [EntityField("testid")]
        public TestID TestID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [NonSerialized]
        [DelayedEntityReferenceField("testid")]
        public Test Test;

        [NonSerialized]
        [DelayedEntityReferenceField("testertypeid")]
        public TesterType TesterType;

        [EntityField("totalrendertime")]
        public double RenderTime = 0;        

        [EntityField("firstrequesttime")]
        public double FirstRequestTime = 0;

        [EntityField("totaldownloadscount")]
        public double TotalDownloadsCount = 0;

        [EntityField("totaljsdownloadscount")]
        public double TotalJSDownloadsCount = 0;

        [EntityField("totalcssdownloadscount")]
        public double TotalCSSDownloadsCount = 0;

        [EntityField("totalimagesdownloadscount")]
        public double TotalImagesDownloadsCount = 0;

        [EntityField("totaldownloadsize")]
        public double TotalDownloadSize = 0;

        [EntityField("totaljsdownloadsize")]
        public double TotalJSDownloadSize = 0;

        [EntityField("totalcssdownloadsize")]
        public double TotalCSSDownloadSize = 0;

        [EntityField("totalimagesdownloadsize")]
        public double TotalImagesDownloadSize = 0;

        [EntityField("processortimeavg")]
        public double ProcessorTimeAvg = 0;

        [EntityField("usertimeavg")]
        public double UserTimeAvg = 0;

        [EntityField("privateworkingsetdelta")]
        public double PrivateWorkingSetDelta = 0;

        [EntityField("workingsetdelta")]
        public double WorkingSetDelta = 0;
    }
}
