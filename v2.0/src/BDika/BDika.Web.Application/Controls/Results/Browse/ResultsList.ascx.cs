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
using System.Collections.Generic;
using EYF.Web.Common;
using EYF.Web.Controls.Common.Tags;

namespace BDika.Web.Application.Controls.Results.Browse
{
    public partial class ResultsList : BaseControl<ResultsList>
    {
        public ICollection<Entities.Results.Results> Results;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cphNoResults.Visible = false;
            this.rptResultsList.Visible = false;

            if (Results == null || Results.Count == 0)
            {
                this.cphNoResults.Visible = true;
                return;
            }

            this.rptResultsList.DataSource = this.Results;
            this.rptResultsList.Visible = true;
            this.rptResultsList.ItemDataBound += new RepeaterItemEventHandler(rptResultsList_ItemDataBound);
            this.rptResultsList.DataBind();
        }

        void rptResultsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Href hrefResultsDate = e.Item.FindControl("hrefResultsDate") as Href;
            Href hrefTestName = e.Item.FindControl("hrefTestName") as Href;
            Href hrefTesterTypeName = e.Item.FindControl("hrefTesterTypeName") as Href;
            
            if (hrefResultsDate == null || 
                hrefTestName == null ||
                hrefTesterTypeName == null)
                return;

            Entities.Results.Results r = e.Item.DataItem as Entities.Results.Results;
            
            if( r == null || r.Test == null)
                return;

            hrefTesterTypeName.Text = r.TesterType.Name;
            hrefTestName.Text = r.Test.TestName;
            hrefResultsDate.Text = r.CreatedOn.ToString("hh:mm dd/MM/yyyy");

            hrefTesterTypeName.Ref = hrefResultsDate.Ref = hrefTestName.Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalTime")).Text = (r.EndTime-r.StartTime).ToString();
            ((Href)e.Item.FindControl("hrefTotalDownloadsCount")).Text = r.TotalDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalJSDownloadsCount")).Text = r.TotalJSDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalCSSDownloadsCount")).Text = r.TotalCSSDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalImagesDownloadsCount")).Text = r.TotalImagesDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalDownloadSize")).Text = r.TotalDownloadSize.ToString();
            ((Href)e.Item.FindControl("hrefTotalJSDownloadSize")).Text = r.TotalJSDownloadSize.ToString();
            ((Href)e.Item.FindControl("hrefTotalCSSDownloadSize")).Text = r.TotalCSSDownloadSize.ToString();
            ((Href)e.Item.FindControl("hrefTotalImagesDownloadSize")).Text = r.TotalImagesDownloadSize.ToString();
            ((Href)e.Item.FindControl("hrefProcessorTimeAvg")).Text = r.ProcessorTimeAvg.ToString();
            ((Href)e.Item.FindControl("hrefUserTimeAvg")).Text = r.UserTimeAvg.ToString();
            ((Href)e.Item.FindControl("hrefPrivateWorkingSetDelta")).Text = r.PrivateWorkingSetDelta.ToString();
            ((Href)e.Item.FindControl("hrefWorkingSetDelta")).Text = r.WorkingSetDelta.ToString();
        }
    }
}
