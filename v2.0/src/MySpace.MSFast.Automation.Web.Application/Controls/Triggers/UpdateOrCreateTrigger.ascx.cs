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
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Triggers
{
    public partial class UpdateOrCreateTrigger : BaseControl<UpdateOrCreateTrigger>
    {
        public Trigger Trigger;

        private class ListObj
        {
            private string _name;
            private String _value;

            public String Name { get { return _name; } }
            public String Value { get { return _value; } }

            public ListObj(String s, String v)
            {
                this._name = s;
                this._value = v;
            }
        }

        private static ListObj[] TriggerTypeListObjects = new ListObj[]
        {
            new ListObj("Select", ((uint)TriggerType.Unknown).ToString()),
            new ListObj("Manual", ((uint)TriggerType.Manual).ToString()),
            new ListObj("Timer", ((uint)TriggerType.Time).ToString())
        };


        protected void Page_Load(object sender, EventArgs e)
        {
            this.isTriggerType.DataTextField = "Name";
            this.isTriggerType.DataValueField = "Value";
            this.isTriggerType.DataSource = TriggerTypeListObjects;
            this.isTriggerType.DataBind();

            if (this.Trigger != null && TriggerID.IsValidTriggerID(this.Trigger.TriggerID))
            {
                this.ihTriggerID.Value = this.Trigger.TriggerID.ToString();
                this.itName.Value = this.Trigger.TriggerName;
                this.isTriggerType.Value = this.Trigger.TriggerTypeID.ToString();
                this.itTimeout.Value = this.Trigger.Timeout.ToString();

                this.Collectors_UpdateCollectorsConfiguration.TriggerID = this.Trigger.TriggerID;
                this.Collectors_UpdateCollectorsConfiguration.Visible = true;
                this.Collectors_UpdateCollectorsConfiguration.ConfigurableEntity = this.Trigger;
            }
            else
            {
                this.bxUpdateCollectorsConfiguration.Visible = false;
                this.Collectors_UpdateCollectorsConfiguration.Visible = false;
            }

            this.afUpdateOrCreateTrigger.Action = MySpace.MSFast.Automation.Web.Application.Handlers.Triggers.UpdateOrCreateTriggerHandler.GetURL();
        }
    }
}