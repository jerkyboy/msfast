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
using BDika.Entities.Results;
using BDika.Web.Core.Common;
using BDika.Providers.Results;
using BDika.Web.Core.Context;
using EYF.Web.Context;

namespace BDika.Web.Application.Pages.Results
{
    [BDikaPageAttributes]
    public partial class ShowResultsDetails : BasePage<ShowResultsDetails>
    {
        [RequestFieldAttributes("r", false)]
        public ResultsID ResultsID = 0;

        public static String GetURL(Entities.Results.Results t)
        {
            if (t != null)
                return GetURL(t.ResultsID);

            return BasePage<ShowResultsDetails>.GetURL();
        }
        public static String GetURL(ResultsID t)
        {
            if (ResultsID.IsValidResultsID(t))
                return BasePage<ShowResultsDetails>.GetURL(new RequestQueryArgument() { Key = "r", Value = t.ColumnValue.ToString() });

            return BasePage<ShowResultsDetails>.GetURL();
        }

        public Entities.Results.Results results;

        public override string GetPageTitle()
        {
            if (results != null && results.Test != null)
                return results.Test.TestName;

            return EYFResourcesManager.GetString("invalidresults");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.phInvalidResults.Visible = false;
            this.phValidResults.Visible = false;

            if (ResultsID.IsValidResultsID(this.ResultsID) &&
                (results = ResultsProvider.GetResults(BDikaContext.Current.User.UserID, this.ResultsID)) != null)
            {
                this.Results_ResultsThumbnails.ResultsID = this.ResultsID;
                this.Results_ResultsGraph.ResultsID = this.ResultsID;
                this.sbTestURL.Text = results.Test.TestURL != null ? results.Test.TestURL.ToString() : "n/a";
                this.phValidResults.Visible = true;
                return;
            }

            this.phInvalidResults.Visible = true;
        }
    }
}