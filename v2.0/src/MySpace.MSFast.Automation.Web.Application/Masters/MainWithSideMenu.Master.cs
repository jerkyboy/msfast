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

namespace MySpace.MSFast.Automation.Web.Application.Masters
{
    public partial class MainWithSideMenu : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hrefLatestResults.Text = EYFWebResourcesManager.GetString("latestResults");
            hrefLatestResults.Ref = Pages.Results.Browse.BrowseResults.GetURL();

            hrefManageTests.Text = EYFWebResourcesManager.GetString("manageTests");
            hrefManageTests.Ref = Pages.Tests.Browse.BrowseTests.GetURL();

            hrefManageTestBox.Text = EYFWebResourcesManager.GetString("manageTestBox");
            hrefManageTestBox.Ref = Pages.Tests.Browse.BrowseTesterTypes.GetURL();

            hrefManageManageTriggers.Text = EYFWebResourcesManager.GetString("manageManageTriggers");
            hrefManageManageTriggers.Ref = Pages.Triggers.Browse.BrowseTriggers.GetURL();

        }
    }
}
