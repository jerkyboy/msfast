//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Application)
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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Entities.Results;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Providers.Results;
using MySpace.MSFast.Automation.Web.Core.Context;
using EYF.Web.Context;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Pages.Results
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class ShowResultsDetails : BasePage<ShowResultsDetails>
    {
        [RequestFieldAttributes("r", false)]
        public ResultsID ResultsID = 0;

        public static String GetURL(Entities.Results.Results t)
        {
            if (t != null)
                return GetURL(t.ResultsID);

            return BasePage<ShowResultsDetails>.GetURL();
        }
        public static String GetURL(ResultsID t)
        {
            if (ResultsID.IsValidResultsID(t))
                return BasePage<ShowResultsDetails>.GetURL(new RequestQueryArgument() { Key = "r", Value = t.ColumnValue.ToString() });

            return BasePage<ShowResultsDetails>.GetURL();
        }

        public Entities.Results.Results results;

        public override string GetPageTitle()
        {
            if (results != null && results.Test != null)
                return results.Test.TestName;

            return EYFWebResourcesManager.GetString("invalidresults");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.phInvalidResults.Visible = false;
            this.phValidResults.Visible = false;

            if (ResultsID.IsValidResultsID(this.ResultsID) &&
                (results = ResultsProvider.GetResults(MSFAContext.Current.User.UserID, this.ResultsID)) != null)
            {
                this.Results_ResultsThumbnails.ResultsID = this.ResultsID;
                this.Results_ResultsGraph.ResultsID = this.ResultsID;
                this.sbTestURL.Text = results.Test.TestURL != null ? results.Test.TestURL.ToString() : "n/a";
                this.phValidResults.Visible = true;
                return;
            }

            this.phInvalidResults.Visible = true;
        }
    }
}