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
using EYF.Web.Handlers.Browse;
using BDika.Entities.Tests;
using BDika.Providers.Tests.Browse;

namespace BDika.Web.Application.Handlers.Tests.Browse
{
    public partial class BrowseTesterTypes : BrowseHandler<BrowseTesterTypes, BrowseTesterTypesEntities, TesterType>
    {
        public override void RenderResults(BrowseTesterTypesEntities bd)
        {
            if (bd != null && bd.Data != null && bd.Data.Count > 0)
                this.Tests_TesterTypesList.TesterTypes = bd.Data;

        }
    }
}

