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
using BDika.Providers.Results.Browse;
using EYF.Web.Common;

namespace BDika.Web.Application.Controls.Results.Browse
{
    public partial class ResultsPaging : BaseControl<ResultsPaging>
    {
        public BrowseResultsEntities BrowseResultsEntities = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BrowseResultsEntities != null && this.BrowseResultsEntities.Data != null
                && this.BrowseResultsEntities.Data.Count > 0)
            {

                this.Results_ResultsList.Results = this.BrowseResultsEntities.Data;
            }

            this.bfBrowseResults.NextResultsURL = BDika.Web.Application.Handlers.Results.Browse.BrowseResults.GetURL();
            this.bfBrowseResults.BrowseEntities = BrowseResultsEntities;
        }
    }
}