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
using MySpace.MSFast.Automation.Providers.Triggers.Browse;
using MySpace.MSFast.Automation.Entities.Triggers;
using System.Collections.Generic;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Triggers
{
    public partial class TriggersDisplayAndFilter : BaseControl<TriggersDisplayAndFilter>
    {
        public BrowseTriggersEntities_FreeBrowse BrowseTriggersEntities;

        private class ListObj
        {
            private string _name;
            private String _value;

            public String Name { get { return _name; } }
            public String Value { get { return _value; } }

            public ListObj(String s,String v)
            {
                this._name = s;
                this._value = v;
            }
        }

        private static ListObj[] TriggerTypeListObjects = new ListObj[]
        {
            new ListObj("All", ((uint)TriggerType.Unknown).ToString()),
            new ListObj("Manual", ((uint)TriggerType.Manual).ToString()),
            new ListObj("Timer", ((uint)TriggerType.Time).ToString())
        };

        private static ListObj[] EnabledListObjects = new ListObj[]
        {
            new ListObj("All", "-1"),
            new ListObj("Enabled", "1"),
            new ListObj("Disabled", "0")
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BrowseTriggersEntities == null) 
                return;

            this.Triggers_TriggersPaging.BrowseTriggersEntities = BrowseTriggersEntities;

            this.isType.DataTextField = "Name";
            this.isType.DataValueField = "Value";
            this.isType.DataSource = TriggerTypeListObjects;
            this.isType.DataBind();

            this.isEnabled.DataTextField = "Name";
            this.isEnabled.DataValueField = "Value";
            this.isEnabled.DataSource = EnabledListObjects;
            this.isEnabled.DataBind();
        }
    }
}