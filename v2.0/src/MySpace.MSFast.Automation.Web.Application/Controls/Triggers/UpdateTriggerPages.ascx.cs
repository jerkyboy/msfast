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
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Entities.Collectors;
using System.Collections.Generic;
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Triggers
{
    public partial class UpdateTriggerPages : BaseControl<UpdateTriggerPages>
    {
        public Trigger Trigger;
        public ICollection<TesterType> TesterTypes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Trigger == null || TriggerID.IsValidTriggerID(Trigger.TriggerID) == false)
            {
                this.phInvalidTrigger.Visible = true;
                this.phTrigger.Visible = false;
                return;
            }

            this.phInvalidTrigger.Visible = false;
            this.phTrigger.Visible = true;

            this.Triggers_UpdateTriggerToTestAndTesterType.TriggerID = Trigger.TriggerID;
            this.Triggers_UpdateTriggerToTestAndTesterType.TesterTypes = TesterTypes;

            this.Triggers_UpdateOrCreateTrigger.Trigger = Trigger; 
        }
    }
}