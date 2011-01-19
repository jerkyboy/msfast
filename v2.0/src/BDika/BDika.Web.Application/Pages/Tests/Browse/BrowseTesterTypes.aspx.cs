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
using BDika.Entities.Users;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;
using EYF.Web.Context;

namespace BDika.Web.Application.Pages.Tests.Browse
{
    [BDikaPageAttributes]
    public partial class BrowseTesterTypes : BasePage<BrowseTesterTypes>
    {
        public override string GetPageTitle()
        {
            return EYFResourcesManager.GetString("title");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BrowseTesterTypesEntities bdd = new BrowseTesterTypes_All();

            bdd.Load(BDikaContext.Current.User);

            Tests_TesterTypesPaging.BrowseTesterTypesEntities = bdd;
        }
    }
}







