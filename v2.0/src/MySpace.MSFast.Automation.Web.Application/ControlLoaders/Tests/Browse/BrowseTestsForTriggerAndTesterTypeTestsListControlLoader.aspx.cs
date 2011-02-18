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
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.Browse
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class BrowseTestsForTriggerAndTesterTypeTestsListControlLoader : BasePage<BrowseTestsForTriggerAndTesterTypeTestsListControlLoader>
    {
        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID = 0;

        [RequestFieldAttributes("ttid", false)]
        public TesterTypeID TesterTypeID = 0;

        public static String GetURL(TriggerID trid, TesterTypeID ttid)
        {
            return BasePage<BrowseTestsForTriggerAndTesterTypeTestsListControlLoader>.GetURL(
                new RequestQueryArgument() { Key = "ttid", Value = ttid.ColumnValue.ToString() },
                new RequestQueryArgument() { Key = "trid", Value = trid.ColumnValue.ToString() });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false || TriggerID.IsValidTriggerID(TriggerID) == false)
            {
                this.Visible = false;
                return;
            }

            BrowseTests_ForTriggerAndTesterType b = new BrowseTests_ForTriggerAndTesterType();
            
            b.TriggerID = this.TriggerID;
            b.TesterTypeID = this.TesterTypeID;
            
            b.Load(MSFAContext.Current.User);
            
            Tests_TestsList.Tests = b.Data;
            Tests_TestsList.SelectedTests = b.SelectedTests;
            Tests_TestsList.ShowEditLink = false;
            Tests_TestsList.ShowTestURL = false;
            Tests_TestsList.ShowRemoveLink = false;
            Tests_TestsList.ShowSelected = true;

        }
    }
}
