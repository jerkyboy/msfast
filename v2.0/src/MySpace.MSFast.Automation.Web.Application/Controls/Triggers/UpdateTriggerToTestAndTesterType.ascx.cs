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
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using System.Collections.Generic;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Web.Core.Context;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Triggers
{
    public partial class UpdateTriggerToTestAndTesterType : BaseControl<UpdateTriggerToTestAndTesterType>
    {
        public TriggerID TriggerID;

        public ICollection<TesterType> TesterTypes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false)
                this.Visible = false;

            this.asUpdateSelectedTests.Action = MySpace.MSFast.Automation.Web.Application.Handlers.Triggers.UpdateSelectedTestsHandler.GetURL();
            this.asUpdateSelectedTests.ShowGUI = false;

            this.ihTriggerID.Value = this.TriggerID.ColumnValue.ToString();

            if (TesterTypes == null || TesterTypes.Count == 0)
            {
                this.cphNoResults.Visible = true;
                this.rptTesterTypesRpt.Visible = false;
            }
            else
            {
                this.cphNoResults.Visible = false;
                this.rptTesterTypesRpt.Visible = true;
                this.rptTesterTypesRpt.DataSource = TesterTypes;
                this.rptTesterTypesRpt.ItemDataBound += new RepeaterItemEventHandler(rptTesterTypesRpt_ItemDataBound);
                this.rptTesterTypesRpt.DataBind();
            }
        }

        void rptTesterTypesRpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltTesterTypeName = e.Item.FindControl("ltTesterTypeName") as Literal;
            Literal ltLI = e.Item.FindControl("ltLI") as Literal;
            
            if (ltLI == null || ltTesterTypeName == null) return;

            TesterType t = e.Item.DataItem as TesterType;

            if (t == null) return;

            ltLI.Text = String.Format("<li class=\"r{1}\" onclick=\"selecttestertype($(this));\" ttid=\"{0}\">", t.TesterTypeID.ColumnValue.ToString(), (e.Item.ItemIndex % 2 == 0) ? "1" : "2");
            ltTesterTypeName.Text = t.Name;
        }
    }
}