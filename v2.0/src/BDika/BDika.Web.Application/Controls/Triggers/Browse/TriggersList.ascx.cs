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
using BDika.Entities.Triggers;
using System.Collections.Generic;
using EYF.Web.Controls.Common.Tags;
using EYF.Web.Context;

namespace BDika.Web.Application.Controls.Triggers.Browse
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
            
            if (hrefTriggerName == null ||
                hrefTriggerType == null ||
                hrefLastTriggered == null)
                return;

            Trigger r = e.Item.DataItem as Trigger;
            
            if(r == null) return;

            hrefTriggerName.Text = r.TriggerName;
            hrefLastTriggered.Text = (r.LastTriggered < new DateTime(2000,1,1)) ? "n/a" : r.LastTriggered.ToString("hh:mm dd/MM/yyyy");

            if (r.TriggerType == TriggerType.Manual)
            {
                hrefTriggerType.Text = EYFResourcesManager.GetString("trigger_type_manual");
            }
            else if (r.TriggerType == TriggerType.Time)
            {
                hrefTriggerType.Text = EYFResourcesManager.GetString("trigger_type_time");
            }
            else
            {
                hrefTriggerType.Text = EYFResourcesManager.GetString("trigger_type_unknown");
            }

            hrefEditTrigger.Text = EYFResourcesManager.GetString("edit");
            hrefEditTrigger.Ref = "#";
            hrefEditTrigger.AdditionalAttribute = "onclick=\"edittrigger(" + r.TriggerID + ");\"";

            hrefTriggerName.Ref = hrefTriggerType.Ref = hrefLastTriggered.Ref = BDika.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(r);
        }
    }
}