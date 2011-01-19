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
using BDika.Providers.Results.Browse;
using EYF.Web.Handlers.Browse;

namespace BDika.Web.Application.Handlers.Results.Browse
{
    public partial class BrowseResults : BrowseHandler<BrowseResults, BrowseResultsEntities, Entities.Results.Results>
    {
        public override void RenderResults(BrowseResultsEntities bd)
        {
            if (bd != null && bd.Data != null && bd.Data.Count > 0)
                this.Results_ResultsList.Results = bd.Data;

        }
    }
}
