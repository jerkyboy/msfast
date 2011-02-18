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
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Collectors;
using EYF.Web.Context;
using EYF.Web.Handlers.Common;
using EYF.Web.Common.InputValidation;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Collectors
{
    [Linkable(Target = "/Handlers/Collectors/RemoveCollectorsArgumentHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class RemoveCollectorsArgumentHandler : AsyncFormHandler<RemoveCollectorsArgumentHandler>
    {
        [RequestFieldAttributes("k", true), HasValueWithLimit(45, FieldResourcesKey = "key")]
        public String Key;

        [RequestFieldAttributes("ttid", false)]
        public TesterTypeID TesterTypeID;

        [RequestFieldAttributes("tid", false)]
        public TestID TestID;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        public override void ProcessForm()
        {
            if (MSFAContext.Current.IsRequestValidationPassed == false)
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }

            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false &&
               TestID.IsValidTestID(TestID) == false &&
               TriggerID.IsValidTriggerID(TriggerID) == false)
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }

            bool ok = false;

            if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) && TriggerID.IsValidTriggerID(this.TriggerID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TriggerID, this.TestID, this.TesterTypeID, this.Key);
            }            
            else if (TestID.IsValidTestID(this.TestID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TestID, this.Key);
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TesterTypeID, this.Key);
            }
            else if (TriggerID.IsValidTriggerID(TriggerID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TriggerID, this.Key);
            }

            if (ok)
            {
                MSFAContext.Current.AddMessage(new ResponseMessage("ok", EYFWebResourcesManager.GetString("saved")));
                return;
            }

            MSFAContext.Current.AddUnexpectedError();
        }
    }
}
