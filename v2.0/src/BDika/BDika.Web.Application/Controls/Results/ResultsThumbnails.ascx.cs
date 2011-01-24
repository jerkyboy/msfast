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
using BDika.Entities.Results;
using EYF.Web.Common;
using BDika.Providers.Results;

namespace BDika.Web.Application.Controls.Results
{
    public partial class ResultsThumbnails : BaseControl<ResultsThumbnails>
    {
        public ResultsID ResultsID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ThumbnailAndTimestamp[] thumbnails = ResultsProvider.GetResultsThumbnails(this.ResultsID);

            if (thumbnails == null || thumbnails.Length == 0)
            {
                this.cphNoThumbnails.Visible = true;
                this.rptThumbnails.Visible = false;
                return;
            }

            this.rptThumbnails.DataSource = thumbnails;
            this.rptThumbnails.ItemDataBound += new RepeaterItemEventHandler(rptThumbnails_ItemDataBound);
            this.rptThumbnails.DataBind();
            this.cphNoThumbnails.Visible = false;
        }

        void rptThumbnails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltImage = e.Item.FindControl("ltImage") as Literal;
            Literal ltTimeStamp = e.Item.FindControl("ltTimeStamp") as Literal;
            
            ThumbnailAndTimestamp src = e.Item.DataItem as ThumbnailAndTimestamp;

            if (ltImage == null || src == null || String.IsNullOrEmpty(src.ThumbnailSrc))
                return;
            ltTimeStamp.Text = (src.Timestamp == 0) ? "<span>n/a</span>" : String.Format("{0}<span>secs.</span>", (src.Timestamp / 1000.00));
            ltImage.Text = String.Format("<img src=\"{0}\" onerror=\"thumbnailError($(this));\"/>", src.ThumbnailSrc);
       }
    }
}