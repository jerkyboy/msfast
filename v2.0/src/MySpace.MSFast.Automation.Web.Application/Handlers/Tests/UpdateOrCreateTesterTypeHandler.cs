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
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common.InputValidation;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Tests;
using EYF.Web.Context;
using EYF.Core.Utils;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/UpdateOrCreateTesterTypeHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class UpdateOrCreateTesterTypeHandler : AsyncFormHandler<UpdateOrCreateTesterTypeHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "testername")]
        public String TesterTypeName;

        [RequestFieldAttributes("t", false)]
        public TesterTypeID TesterTypeID;

        public override void ProcessForm()
        {
            if (MSFAContext.Current.IsRequestValidationPassed == false)
                return;

            TesterType t = new TesterType();
            t.TesterTypeID = this.TesterTypeID;
            t.Name = this.TesterTypeName;

            t = TestsProvider.UpdateOrCreateTesterType(MSFAContext.Current.User.UserID, t);
            
            if (t != null)
            {
                MSFAContext.Current.AddIndicator(new ResponseIndicator("tester_type_" + (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) ? "updated" : "created"), String.Format("{{testertypeid:{0},testertypename:\"{1}\"}}", t.TesterTypeID, JSUtilities.EncodeJsString(t.Name))));
                MSFAContext.Current.AddMessage(new ResponseMessage("ok", EYFWebResourcesManager.GetString("saved")));
                return;
            }

            MSFAContext.Current.AddUnexpectedError();
        }
    }
}
