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
using EYF.Web.Common;
using BDika.Web.Core.Common;
using BDika.Web.Core.Context;
using BDika.Providers.Tests;
using BDika.Entities.Tests;

namespace BDika.Web.Application.ControlLoaders.Tests
{
    [BDikaPageAttributes]
    public partial class UpdateOrCreateTesterTypeControlLoader : BasePage<UpdateOrCreateTesterTypeControlLoader>
    {
        [RequestFieldAttributes("t", false)]
        public TesterTypeID TesterTypeID = 0;

        public static String GetURL(TesterType t)
        {
            if (t != null)
                return GetURL(t.TesterTypeID);
            return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL();
        }

        public static String GetURL(TesterTypeID t)
        {
            if (TesterTypeID.IsValidTesterTypeID(t))
                return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });
            return BasePage<UpdateOrCreateTesterTypeControlLoader>.GetURL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                this.Tests_UpdateOrCreateTesterType.TesterType = TestsProvider.GetTesterType(BDikaContext.Current.User.UserID, this.TesterTypeID);
            }
        }
    }
}