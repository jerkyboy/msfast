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
using System.Collections.Generic;
using EYF.Web.Common;
using EYF.Web.Controls.Common.Tags;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Results.Browse
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
            hrefResultsDate.Text = r.CreatedOn.ToString("f");

            hrefTesterTypeName.Ref = hrefResultsDate.Ref = hrefTestName.Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalTime")).Text = String.Format("{0}<span>secs.</span>", ((r.EndTime - r.StartTime) / 1000.00));
            ((Href)e.Item.FindControl("hrefTotalTime")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefServerTime")).Text = (r.FirstRequestTime == 0) ? "<span>n/a</span>" : String.Format("{0}<span>secs.</span>", (r.FirstRequestTime / 1000.00));
            ((Href)e.Item.FindControl("hrefServerTime")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefRenderTime")).Text = (r.RenderTime == 0) ? "<span>n/a</span>" : String.Format("{0}<span>secs.</span>", (r.RenderTime / 1000.00));
            ((Href)e.Item.FindControl("hrefRenderTime")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);
            

            ((Href)e.Item.FindControl("hrefTotalDownloadsCount")).Text = r.TotalDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalDownloadsCount")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);
            ((Href)e.Item.FindControl("hrefTotalJSDownloadsCount")).Text = r.TotalJSDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalJSDownloadsCount")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);
            ((Href)e.Item.FindControl("hrefTotalCSSDownloadsCount")).Text = r.TotalCSSDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalCSSDownloadsCount")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);
            ((Href)e.Item.FindControl("hrefTotalImagesDownloadsCount")).Text = r.TotalImagesDownloadsCount.ToString();
            ((Href)e.Item.FindControl("hrefTotalImagesDownloadsCount")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalDownloadSize")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.TotalDownloadSize / 1024));
            ((Href)e.Item.FindControl("hrefTotalDownloadSize")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalJSDownloadSize")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.TotalJSDownloadSize / 1024));
            ((Href)e.Item.FindControl("hrefTotalJSDownloadSize")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalCSSDownloadSize")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.TotalCSSDownloadSize / 1024));
            ((Href)e.Item.FindControl("hrefTotalCSSDownloadSize")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefTotalImagesDownloadSize")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.TotalImagesDownloadSize / 1024));
            ((Href)e.Item.FindControl("hrefTotalImagesDownloadSize")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefProcessorTimeAvg")).Text = r.ProcessorTimeAvg.ToString();
            ((Href)e.Item.FindControl("hrefProcessorTimeAvg")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefUserTimeAvg")).Text = String.Format("{0}%", Math.Min(100, r.UserTimeAvg));
            ((Href)e.Item.FindControl("hrefUserTimeAvg")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefPrivateWorkingSetDelta")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.PrivateWorkingSetDelta / 1024));
            ((Href)e.Item.FindControl("hrefPrivateWorkingSetDelta")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            ((Href)e.Item.FindControl("hrefWorkingSetDelta")).Text = String.Format("{0:0,0}<span>kb.</span>", (r.WorkingSetDelta / 1024));
            ((Href)e.Item.FindControl("hrefWorkingSetDelta")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);

            String ico = "error";
            String title = EYFWebResourcesManager.GetString("error");

            if (r.ResultsState == MySpace.MSFast.Automation.Entities.Results.ResultsState.Pending)
            {
                ico = "pending";
                title = EYFWebResourcesManager.GetString("pending");
            }
            else if (r.ResultsState == MySpace.MSFast.Automation.Entities.Results.ResultsState.Processing)
            {
                ico = "processing";
                title = EYFWebResourcesManager.GetString("processing");
            }
            else if (r.ResultsState == MySpace.MSFast.Automation.Entities.Results.ResultsState.Succeeded)
            {
                ico = "succeeded";
                title = EYFWebResourcesManager.GetString("succeeded");
            }
            else if (r.ResultsState == MySpace.MSFast.Automation.Entities.Results.ResultsState.Testing)
            {
                ico = "testing";
                title = EYFWebResourcesManager.GetString("testing");
            }

            ((Href)e.Item.FindControl("hrefResultsState")).Text = String.Concat("<span class=\"ico ico-",ico,"\" title=\"", title, "\"></span>");
            ((Href)e.Item.FindControl("hrefResultsState")).Ref = Pages.Results.ShowResultsDetails.GetURL(r);
            
        }
    }
}
