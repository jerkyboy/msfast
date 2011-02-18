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
using System.Collections.Generic;
using EYF.Web.Controls.Common.Tags;
using EYF.Web.Context;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Triggers.Browse
{
    public partial class TriggersList : BaseControl<TriggersList>
    {
        public ICollection<Trigger> Triggers;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cphNoResults.Visible = false;
            this.rptTriggersList.Visible = false;

            if (Triggers == null || Triggers.Count == 0)
            {
                this.cphNoResults.Visible = true;
                return;
            }

            this.rptTriggersList.DataSource = this.Triggers;
            this.rptTriggersList.Visible = true;
            this.rptTriggersList.ItemDataBound += new RepeaterItemEventHandler(rptTriggersList_ItemDataBound);
            this.rptTriggersList.DataBind();
        }

        void rptTriggersList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Href hrefTriggerName = e.Item.FindControl("hrefTriggerName") as Href;
            Href hrefTriggerType = e.Item.FindControl("hrefTriggerType") as Href;
            Href hrefLastTriggered = e.Item.FindControl("hrefLastTriggered") as Href;
            Href hrefEditTrigger = e.Item.FindControl("hrefEditTrigger") as Href;
            Href hrefRemoveTrigger = e.Item.FindControl("hrefRemoveTrigger") as Href;
            
            if (hrefTriggerName == null ||
                hrefTriggerType == null ||
                hrefLastTriggered == null)
                return;

            Trigger r = e.Item.DataItem as Trigger;
            
            if(r == null) return;

            hrefTriggerName.Text = r.TriggerName;
            hrefLastTriggered.Text = (r.LastTriggered < new DateTime(2000,1,1)) ? "n/a" : r.LastTriggered.ToString("f");

            if (r.TriggerType == TriggerType.Manual)
            {
                hrefTriggerType.Text = EYFWebResourcesManager.GetString("trigger_type_manual");
            }
            else if (r.TriggerType == TriggerType.Time)
            {
                hrefTriggerType.Text = EYFWebResourcesManager.GetString("trigger_type_time");
            }
            else
            {
                hrefTriggerType.Text = EYFWebResourcesManager.GetString("trigger_type_unknown");
            }

            hrefRemoveTrigger.Text = EYFWebResourcesManager.GetString("remove");
            hrefRemoveTrigger.Ref = "#";
            hrefRemoveTrigger.AdditionalAttribute = "onclick=\"removetrigger(" + r.TriggerID + ");\"";
            

            hrefEditTrigger.Text = EYFWebResourcesManager.GetString("edit");
            hrefEditTrigger.Ref = "#";
            hrefEditTrigger.AdditionalAttribute = "onclick=\"edittrigger(" + r.TriggerID + ");\"";

            hrefTriggerName.Ref = hrefTriggerType.Ref = hrefLastTriggered.Ref = MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(r);
        }
    }
}