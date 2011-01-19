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
using BDika.Providers.Triggers.Browse;
using BDika.Web.Core.Context;
using BDika.Web.Core.Common;
using EYF.Web.Common;
using BDika.Providers.Tests.Browse;
using EYF.Web.Context;

namespace BDika.Web.Application.Pages.Triggers.Browse
{
    [BDikaPageAttributes]
    public partial class BrowseTriggers : BasePage<BrowseTriggers>
    {
        public override string GetPageTitle()
        {
            return EYFResourcesManager.GetString("title");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            BrowseTriggersEntities_FreeBrowse brfb = new BrowseTriggersEntities_FreeBrowse();
            brfb.Load(BDikaContext.Current.User);

            this.Triggers_TriggersDisplayAndFilter.BrowseTriggersEntities = brfb;
        }
    }
}
