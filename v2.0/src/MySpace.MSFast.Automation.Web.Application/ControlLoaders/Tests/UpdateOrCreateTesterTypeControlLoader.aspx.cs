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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Tests;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class UpdateOrCreateTesterTypeControlLoader : BasePage<UpdateOrCreateTesterTypeControlLoader>
    {
        [RequestFieldAttributes("t", false)]
        public TesterTypeID TesterTypeID = 0;

        public static String GetURL(TesterType t)
        {
            if (t != null)
                return GetURL(t.TesterTypeID);
            return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL();
        }

        public static String GetURL(TesterTypeID t)
        {
            if (TesterTypeID.IsValidTesterTypeID(t))
                return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });
            return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                this.Tests_UpdateOrCreateTesterType.TesterType = TestsProvider.GetTesterType(MSFAContext.Current.User.UserID, this.TesterTypeID);
            }
        }
    }
}