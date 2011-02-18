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
using MySpace.MSFast.Automation.Entities.Tests;
using System.Collections.Generic;
using EYF.Web.Common;
using EYF.Web.Context;
using EYF.Web.Controls.Common.Tags;
using EYF.Web.Controls.Common.Forms;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Tests.Browse
{
    public partial class TestsList : BaseControl<TestsList>
    {
        public ICollection<Test> Tests;
        public ICollection<TestID> SelectedTests;

        public bool ShowEditLink = true;
        public bool ShowTestURL = true;
        public bool ShowRemoveLink = true;
        public bool ShowSelected = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cphNoResults.Visible = false;
            this.rptTestsList.Visible = false;

            if (Tests == null || Tests.Count == 0)
            {
                this.cphNoResults.Visible = true;
                return;
            }

            this.rptTestsList.DataSource = this.Tests;
            this.rptTestsList.Visible = true;
            this.rptTestsList.ItemDataBound += new RepeaterItemEventHandler(rptTesterTypesList_ItemDataBound);

            rptTestsList.DataBind();
        }

        void rptTesterTypesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Href hrefTestURL = e.Item.FindControl("hrefTestURL") as Href;
            Href hrefTestName = e.Item.FindControl("hrefTestName") as Href;
            Href hrefEditTest = e.Item.FindControl("hrefEditTest") as Href;
            Href hrefRemoveTest = e.Item.FindControl("hrefRemoveTest") as Href;
            Literal icTestSelected = e.Item.FindControl("icTestSelected") as Literal;

            if (hrefTestURL == null || hrefTestName == null || hrefEditTest == null)
                return;
            
            if (e.Item.FindControl("phTestslstSelectedL") != null) e.Item.FindControl("phTestslstSelectedL").Visible = ShowSelected;
            if (e.Item.FindControl("phTestslstURLL") != null) e.Item.FindControl("phTestslstURLL").Visible = ShowTestURL;
            if (e.Item.FindControl("phActionsL") != null) e.Item.FindControl("phActionsL").Visible = ShowRemoveLink || ShowEditLink;
            
            Test t = e.Item.DataItem as Test;
            
            if( t == null)
                return;

            hrefTestName.Text = t.TestName;
            hrefTestName.Ref = "#";

            hrefTestURL.Text = t.TestURL != null ? t.TestURL.ToString() : "n/a";
            hrefTestURL.Ref = "#";

            hrefEditTest.Text = EYFWebResourcesManager.GetString("edit");
            hrefEditTest.Ref = "#";
            hrefEditTest.Visible = ShowEditLink;

            hrefRemoveTest.Text = EYFWebResourcesManager.GetString("remove");
            hrefRemoveTest.Ref = "#";
            hrefRemoveTest.Visible = ShowRemoveLink;
             
            hrefEditTest.AdditionalAttribute = "onclick=\"$(document).trigger('editTestClicked',{tid:" + t.TestID + ",sender:$(this)});return false;\"";
            hrefRemoveTest.AdditionalAttribute = "onclick=\"$(document).trigger('removeTestClicked',{tid:" + t.TestID + ",sender:$(this)});return false;\"";
            hrefTestURL.AdditionalAttribute = "onclick=\"$(document).trigger('testURLClicked',{tid:" + t.TestID + ",sender:$(this)});return false;\"";
            hrefTestName.AdditionalAttribute = "onclick=\"$(document).trigger('testNameClicked',{tid:" + t.TestID + ",sender:$(this)});return false;\"";

            if (SelectedTests != null && SelectedTests.Contains(t.TestID))
            {
                icTestSelected.Text = "<input type=\"checkbox\" name=\"testSelected\" checked=\"checked\" value=\"" + t.TestID + "\" onclick=\"$(document).trigger('selectTestClicked',{tid:" + t.TestID + ",sender:$(this)});\">";
            }
            else
            {
                icTestSelected.Text = "<input type=\"checkbox\" name=\"testSelected\" value=\"" + t.TestID + "\" onclick=\"$(document).trigger('selectTestClicked',{tid:" + t.TestID + ",sender:$(this)});\">";
            }

            MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(t);

        }
    }
}

