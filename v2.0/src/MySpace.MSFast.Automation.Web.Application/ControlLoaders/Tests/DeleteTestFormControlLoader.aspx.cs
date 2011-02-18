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
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Tests;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class DeleteTestFormControlLoader : BasePage<DeleteTestFormControlLoader>
    {
        [RequestFieldAttributes("tid", false)]
        public TestID TestID = 0;

        public static String GetURL(Test t)
        {
            return GetURL(t.TestID);
        }

        public static String GetURL(TestID t)
        {
            return BasePage<DeleteTestFormControlLoader>.GetURL(new RequestQueryArgument() { Key = "tid", Value = t.ColumnValue.ToString() });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TestID.IsValidTestID(this.TestID) == false)
                return;

            Test t = TestsProvider.GetTest(MSFAContext.Current.User.UserID, this.TestID);

            this.Tests_DeleteTestForm.Test = t;
        }
    }
}
