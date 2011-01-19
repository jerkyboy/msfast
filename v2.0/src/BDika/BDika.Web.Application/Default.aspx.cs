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
using BDika.Web.Core.Common;
using BDika.Entities.Users;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;
using EYF.Web.Context;

namespace BDika.Web.Application
{
    [Linkable(Target = "/Default.aspx", Path = "/Default.aspx")]
    [BDikaPageAttributes(RequieredMissingUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class _Default : BasePage<_Default>
    {
        public override string GetPageTitle()
        {
            return EYFResourcesManager.GetString("title");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hrefLatestResults.Text = EYFResourcesManager.GetString("latestResults");
            hrefLatestResults.Ref = Pages.Results.Browse.BrowseResults.GetURL();

            hrefManageTests.Text = EYFResourcesManager.GetString("manageTests");
            hrefManageTests.Ref = Pages.Tests.Browse.BrowseTests.GetURL();

            hrefManageTestBox.Text = EYFResourcesManager.GetString("manageTestBox");
            hrefManageTestBox.Ref = Pages.Tests.Browse.BrowseTesterTypes.GetURL();

            hrefManageManageTriggers.Text = EYFResourcesManager.GetString("manageManageTriggers");
            hrefManageManageTriggers.Ref = Pages.Triggers.Browse.BrowseTriggers.GetURL();
        }        
    }
}
