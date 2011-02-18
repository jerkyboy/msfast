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
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Providers.Results.Browse;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Tests;
using EYF.Web.Context;
using MySpace.MSFast.Automation.Providers.Triggers;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class BrowseResults : BasePage<BrowseResults>
    {
        [RequestFieldAttributes("tid")]
        public TestID TestID = 0;

        [RequestFieldAttributes("ttid")]
        public TesterTypeID TesterTypeID = 0;

        [RequestFieldAttributes("trid")]
        public TriggerID TriggerID = 0;

        #region GetURL
        public static String GetURL(Test Test)
        {
            return GetURL(Test.TestID);
        }

        public static String GetURL(TestID TestID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "tid", Value = ((uint)TestID).ToString() });
        }

        public static String GetURL(TesterType TesterType)
        {
            return GetURL(TesterType.TesterTypeID);
        }

        public static String GetURL(TesterTypeID TesterTypeID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "ttid", Value = ((uint)TesterTypeID).ToString() });
        }

        public static String GetURL(Trigger Trigger)
        {
            return GetURL(Trigger.TriggerID);
        }

        public static String GetURL(TriggerID TriggerID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "trid", Value = ((uint)TriggerID).ToString() });
        }
        #endregion

        public String PageTitle;

        public override string GetPageTitle()
        {
            return PageTitle;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BrowseResultsEntities_FreeBrowse brfb = new BrowseResultsEntities_FreeBrowse();

            

            if (TestID.IsValidTestID(TestID))
            {
                Test test = TestsProvider.GetTest(MSFAContext.Current.User.UserID,TestID);
                if (test != null) PageTitle = String.Format(EYFWebResourcesManager.GetString("title_test"), test.TestName);

                brfb.TestID = this.TestID; 
            }
            else if (TriggerID.IsValidTriggerID(this.TriggerID))
            {
                Trigger trigger = TriggersProvider.GetTrigger(MSFAContext.Current.User.UserID, TriggerID);
                if (trigger != null) PageTitle = String.Format(EYFWebResourcesManager.GetString("title_trigger"), trigger.TriggerName);

                brfb.TriggerID = this.TriggerID;
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                TesterType testerType = TestsProvider.GetTesterType(MSFAContext.Current.User.UserID, TesterTypeID);
                if (testerType != null) PageTitle = String.Format(EYFWebResourcesManager.GetString("title_testerType"), testerType.Name);

                brfb.TesterTypeID = this.TesterTypeID;
            }
            else
            {
                PageTitle = EYFWebResourcesManager.GetString("title");                
            }
            
            brfb.PopTotal = true;

            BrowseTesterTypes_All btt = new BrowseTesterTypes_All();
            btt.Load(MSFAContext.Current.User);
            
            brfb.Load(MSFAContext.Current.User);
            
            this.Results_ResultsDisplayAndFilter.BrowseResultsEntities = brfb;
            this.Results_ResultsDisplayAndFilter.BrowseTesterTypesEntities = btt;

        }
    }
}
