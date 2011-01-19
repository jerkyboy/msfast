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
using BDika.Entities.Tests;
using BDika.Providers.Tests;
using BDika.Web.Core.Context;
using BDika.Providers.Tests.Browse;
using BDika.Providers.Results.Browse;
using EYF.Web.Context;

namespace BDika.Web.Application.Pages.Tests
{
    [BDikaPageAttributes]
    public partial class ShowTestDetails : BasePage<ShowTestDetails>
    {
        [RequestFieldAttributes("t", false)]
        public TestID TestID = 0;

        public override string GetPageTitle()
        {
            if (Test != null)
                return this.Test.TestName;

            return EYFResourcesManager.GetString("invalidtest");
        }

        public Test Test;

        public static String GetURL(Test t)
        {           
            return GetURL(t.TestID);
        }
        public static String GetURL(TestID t)
        {
            return BasePage<ShowTestDetails>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.phInvalidTest.Visible = false;
            this.phValidTest.Visible = false;

            if (TestID.IsValidTestID(this.TestID) &&
                (Test = TestsProvider.GetTest(BDikaContext.Current.User.UserID, this.TestID)) != null)
            {

                this.phValidTest.Visible = true;
                this.sbTestURL.Text = Test.TestURL != null ? Test.TestURL.ToString() : "n/a"; ;

                BrowseResultsEntities_FreeBrowse brfb = new BrowseResultsEntities_FreeBrowse();
                brfb.TestID = Test.TestID;
                brfb.Load(BDikaContext.Current.User);

                BrowseTesterTypes_ForTest btt = new BrowseTesterTypes_ForTest();
                btt.TestID = Test.TestID;
                btt.Load(BDikaContext.Current.User);

                this.Results_ResultsDisplayAndFilter.BrowseResultsEntities = brfb;
                this.Results_ResultsDisplayAndFilter.BrowseTesterTypesEntities = btt;

                return;
            }

            this.phInvalidTest.Visible = true;
        }
    }
}