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
using BDika.Web.Core.Common;
using EYF.Web.Common;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;
using EYF.Web.Context;

namespace BDika.Web.Application.Pages.Tests.Browse
{
    [BDikaPageAttributes]
    public partial class BrowseTests : BasePage<BrowseTests>
    {
        public override string GetPageTitle()
        {
            return EYFResourcesManager.GetString("title");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BrowseTests_All bdd = new BrowseTests_All();
            bdd.PopTotal = true;
            bdd.Load(BDikaContext.Current.User);

            Tests_TestsPaging.ShowAddLink = false;
            Tests_TestsPaging.ShowRemoveLink = false;
            Tests_TestsPaging.BrowseTestsEntities = bdd;
        }
    }
}
