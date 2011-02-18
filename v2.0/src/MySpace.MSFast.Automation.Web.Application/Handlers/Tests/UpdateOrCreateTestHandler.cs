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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Handlers.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common;
using EYF.Web.Common.InputValidation;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Tests;
using EYF.Web.Context;
using EYF.Core.Utils;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/UpdateOrCreateTestHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class UpdateOrCreateTestHandler : AsyncFormHandler<UpdateOrCreateTestHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "testname")]
        public String TestName;

        [RequestFieldAttributes("u", true), HasValueWithLimit(2083,FieldResourcesKey = "testurl")]
        public String TestURL;

        [RequestFieldAttributes("t", false)]
        public TestID TestID;

        public override void ProcessForm()
        {
            if (MSFAContext.Current.IsRequestValidationPassed == false)
                return;

            Test t = new Test();
            t.TestID = this.TestID;
            t.TestName = this.TestName;
            t.TestURL = new Uri(this.TestURL);

            t = TestsProvider.UpdateOrCreateTest(MSFAContext.Current.User.UserID, t);
            
            if (t != null)
            {
                MSFAContext.Current.AddIndicator(new ResponseIndicator("test_" + (TestID.IsValidTestID(this.TestID) ? "updated" : "created"), String.Format("{{testid:{0},testname:\"{1}\",testurl:\"{2}\"}}", t.TestID, JSUtilities.EncodeJsString(t.TestName), JSUtilities.EncodeJsString(t.TestURL.ToString()))));
                MSFAContext.Current.AddMessage(new ResponseMessage("ok", EYFWebResourcesManager.GetString("saved")));
                return;
            }

            MSFAContext.Current.AddUnexpectedError();
        }
    }
}
