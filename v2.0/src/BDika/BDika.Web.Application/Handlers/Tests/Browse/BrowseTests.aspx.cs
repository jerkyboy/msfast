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
using BDika.Entities.Tests;
using EYF.Web.Handlers.Browse;
using BDika.Providers.Tests.Browse;

namespace BDika.Web.Application.Handlers.Tests.Browse
{
    public partial class BrowseTests : BrowseHandler<BrowseTests, BrowseTestsEntities, Test>
    {
        public override void RenderResults(BrowseTestsEntities bd)
        {
            if (bd != null && bd.Data != null && bd.Data.Count > 0)
                this.Tests_TestsList.Tests = bd.Data;

        }
    }
}
