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
using MySpace.MSFast.Automation.Entities.Triggers;
using EYF.Web.Common.InputValidation;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Triggers;
using EYF.Web.Context;
using EYF.Core.Utils;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Triggers
{
    [Linkable(Target = "/Handlers/Triggers/UpdateOrCreateTriggerHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class UpdateOrCreateTriggerHandler : AsyncFormHandler<UpdateOrCreateTriggerHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "triggername")]
        public String TriggerName;

        [RequestFieldAttributes("t", true), HasValue(FieldResourcesKey = "triggertype")]
        public ushort TriggerTypeID;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        [RequestFieldAttributes("m", false)]
        public uint Timeout;

        public override void ProcessForm()
        {
            if (MSFAContext.Current.IsRequestValidationPassed == false)
                return;

            Trigger t = new Trigger();
            t.TriggerID = this.TriggerID;
            t.TriggerName = this.TriggerName;
            t.TriggerTypeID = this.TriggerTypeID;
            t.Timeout = this.Timeout;

            t = TriggersProvider.UpdateOrCreateTrigger(MSFAContext.Current.User.UserID, t);
            
            if (t != null)
            {
                MSFAContext.Current.AddIndicator(new ResponseIndicator("trigger_" + (TriggerID.IsValidTriggerID(this.TriggerID) ? "updated" : "created"), String.Format("{{triggerid:{0},triggername:\"{1}\"}}", t.TriggerID, JSUtilities.EncodeJsString(t.TriggerName))));
                MSFAContext.Current.AddMessage(new ResponseMessage("ok", EYFWebResourcesManager.GetString("saved")));
                return;
            }

            MSFAContext.Current.AddUnexpectedError();
        }
    }
}
