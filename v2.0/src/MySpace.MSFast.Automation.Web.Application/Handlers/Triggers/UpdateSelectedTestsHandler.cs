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
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Web.Core.Common;
using System.Collections.Generic;
using MySpace.MSFast.Automation.Providers.Triggers;
using EYF.Web.Context;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Triggers
{
    [Linkable(Target = "/Handlers/Triggers/UpdateSelectedTestsHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class UpdateSelectedTestsHandler : AsyncFormHandler<UpdateSelectedTestsHandler>
    {
        [RequestFieldAttributes("ttid", true)]
        public TesterTypeID TesterTypeID;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        [RequestFieldAttributes("ad", false)]
        public String AddTests;

        [RequestFieldAttributes("rm", false)]
        public String RemoveTests;

        public override void ProcessForm()
        {
            if(MSFAContext.Current.IsRequestValidationPassed == false || 
               TriggerID.IsValidTriggerID(this.TriggerID) == false || 
               TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) == false)
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }
            if (String.IsNullOrEmpty(AddTests) == false)
            {
                String[] add = AddTests.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                HashSet<TestID> tids = new HashSet<TestID>();
                foreach (String a in add)
                {
                    try
                    {
                        tids.Add(uint.Parse(a));
                    }
                    catch
                    {
                    }
                }
                if (tids.Count > 0)
                {
                    if (TriggersProvider.SetTestsForTriggerAndBox(this.TriggerID, this.TesterTypeID, tids) == false)
                    {
                        MSFAContext.Current.AddUnexpectedError();
                        return;
                    }
                }
            } 
            if (String.IsNullOrEmpty(RemoveTests) == false)
            {
                String[] rem = RemoveTests.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                HashSet<TestID> tids = new HashSet<TestID>();
                foreach (String a in rem)
                {
                    try
                    {
                        tids.Add(uint.Parse(a));
                    }
                    catch
                    {
                    }
                }
                if (tids.Count > 0)
                {
                    if (TriggersProvider.RemoveTestsForTriggerAndBox(this.TriggerID, this.TesterTypeID, tids) == false)
                    {
                        MSFAContext.Current.AddUnexpectedError();
                        return;
                    }
                }
            }

            MSFAContext.Current.AddIndicator(new ResponseIndicator("ok", "ok"));
        }
    }
}
