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
using BDika.Entities.Tests;
using System.Collections.Generic;
using EYF.Web.Common;
using EYF.Web.Context;
using EYF.Web.Controls.Common.Tags;
using EYF.Web.Controls.Common.Forms;

namespace BDika.Web.Application.Controls.Tests.Browse
{
    public partial class TestsList : BaseControl<TestsList>
    {
        public ICollection<Test> Tests;

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
            Href hrefAddTest = e.Item.FindControl("hrefAddTest") as Href;
            
            if (hrefTestURL == null || hrefTestName == null || hrefEditTest == null)
                return;

            Test t = e.Item.DataItem as Test;
            
            if( t == null)
                return;

            hrefTestName.Text = t.TestName;
            hrefTestName.Ref = "#";

            hrefTestURL.Text = t.TestURL != null ? t.TestURL.ToString() : "n/a";
            hrefTestURL.Ref = "#";

            hrefEditTest.Text = EYFResourcesManager.GetString("edit");
            hrefEditTest.Ref = "#";

            hrefRemoveTest.Text = EYFResourcesManager.GetString("remove");
            hrefRemoveTest.Ref = "#";

            hrefAddTest.Text = EYFResourcesManager.GetString("add");
            hrefAddTest.Ref = "#";

            hrefEditTest.AdditionalAttribute = "onclick=\"$(document).trigger('editTestClicked',{tid:" + t.TestID + ",sender:$(this)});\"";
            hrefRemoveTest.AdditionalAttribute = "onclick=\"$(document).trigger('removeTestClicked',{tid:" + t.TestID + ",sender:$(this)});\"";
            hrefAddTest.AdditionalAttribute = "onclick=\"$(document).trigger('addTestClicked',{tid:" + t.TestID + ",sender:$(this)});\"";
            hrefTestURL.AdditionalAttribute = "onclick=\"$(document).trigger('testURLClicked',{tid:" + t.TestID + ",sender:$(this)});\"";
            hrefTestName.AdditionalAttribute = "onclick=\"$(document).trigger('testNameClicked',{tid:" + t.TestID + ",sender:$(this)});\"";

            BDika.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(t);

        }
    }
}