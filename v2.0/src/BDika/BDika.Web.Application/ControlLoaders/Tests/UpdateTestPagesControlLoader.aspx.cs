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
using BDika.Entities.Tests;
using BDika.Providers.Tests;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;

namespace BDika.Web.Application.ControlLoaders.Tests
{
    public partial class UpdateTestPagesControlLoader : BasePage<UpdateTestPagesControlLoader>
    {

        [RequestFieldAttributes("t", false)]
        public TestID TestID = 0;

        public static String GetURL(Test t)
        {
            if (t != null)
                return GetURL(t.TestID);

            return BasePage<UpdateOrCreateTestControlLoader>.GetURL();
        }

        public static String GetURL(TestID t)
        {
            if (TestID.IsValidTestID(t))
                return BasePage<UpdateTestPagesControlLoader>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });
            
            return BasePage<UpdateOrCreateTestControlLoader>.GetURL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TestID.IsValidTestID(this.TestID))
            {
                Test test = TestsProvider.GetTest(BDikaContext.Current.User.UserID, this.TestID);

                if (test == null)
                    return;

                BrowseTesterTypesEntities bdd = new BrowseTesterTypes_All();
                bdd.Load(BDikaContext.Current.User);

                BrowseTesterTypes_ForTest bdd_fortest = new BrowseTesterTypes_ForTest();
                bdd_fortest.TestID = test.TestID;
                bdd_fortest = bdd_fortest.LoadBrowseEntity(BDikaContext.Current.User) as BrowseTesterTypes_ForTest;

                this.Tests_UpdateTestPages.Test = test;
                this.Tests_UpdateTestPages.TesterTypes = bdd.Data;
                this.Tests_UpdateTestPages.SelectedTesterTypes = bdd_fortest != null ? bdd_fortest.GetEntitiesIDs() : null;

                
            }
        }
    }
}
