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
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Providers.Triggers;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.ControlLoaders.Triggers
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class UpdateTriggerPagesControlLoader : BasePage<UpdateTriggerPagesControlLoader>
    {
        [RequestFieldAttributes("t", false)]
        public TriggerID TriggerID = 0;

        public static String GetURL(Trigger t)
        {
            if (t != null)
                return GetURL(t.TriggerID);

            return BasePage<UpdateOrCreateTriggerControlLoader>.GetURL();
        }

        public static String GetURL(TriggerID t)
        {
            if (TriggerID.IsValidTriggerID(t))
                return BasePage<UpdateTriggerPagesControlLoader>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });

            return BasePage<UpdateOrCreateTriggerControlLoader>.GetURL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TriggerID.IsValidTriggerID(this.TriggerID) == false)
                return;

            Trigger trigger = TriggersProvider.GetTrigger(MSFAContext.Current.User.UserID, this.TriggerID);

            if (trigger == null)
                return;

            BrowseTesterTypes_All btt = new BrowseTesterTypes_All();
            btt.Load(MSFAContext.Current.User);

            Triggers_UpdateTriggerPages.TesterTypes = btt.Data;
            Triggers_UpdateTriggerPages.Trigger = trigger;

        }
    }
}
