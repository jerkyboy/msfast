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
using BDika.Entities.Triggers;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;

namespace BDika.Web.Application.ControlLoaders.Tests
{
    public partial class UpdateSelectedTesterTypesControlLoader : BasePage<UpdateSelectedTesterTypesControlLoader>
    {
        [RequestFieldAttributes("tid", false)]
        public TestID TestID = 0;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID = 0;

        public static String GetURL(Test t)
        {
            return GetURL(t.TestID);
        }
        public static String GetURL(TestID t)
        {
            return BasePage<UpdateSelectedTesterTypesControlLoader>.GetURL(new RequestQueryArgument() { Key = "tid", Value = t.ColumnValue.ToString() });
        }
        public static String GetURL(Trigger Trigger, Test Test)
        {
            return GetURL(Trigger.TriggerID, Test.TestID);
        }
        public static String GetURL(TriggerID TriggerID, TestID TestID)
        {
            return BasePage<UpdateSelectedTesterTypesControlLoader>.GetURL(new RequestQueryArgument() { Key = "tid", Value = TestID.ColumnValue.ToString() }, new RequestQueryArgument() { Key = "trid", Value = TriggerID.ColumnValue.ToString() });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TestID.IsValidTestID(this.TestID) == false)
                return;

            BrowseTesterTypesEntities bdd = null;
            BrowseTesterTypesEntities bdd_fortest = null;

            if (TriggerID.IsValidTriggerID(this.TriggerID))
            {
                bdd = new BrowseTesterTypes_ForTest();
                ((BrowseTesterTypes_ForTest)bdd).TestID = TestID;

                bdd_fortest = new BrowseTesterTypes_ForTestAndTrigger();
                ((BrowseTesterTypes_ForTestAndTrigger)bdd_fortest).TriggerToTestID = new TriggerToTestID(TriggerID,TestID);
                bdd_fortest = bdd_fortest.LoadBrowseEntity(BDikaContext.Current.User) as BrowseTesterTypes_ForTestAndTrigger;
            }
            else
            {
                bdd = new BrowseTesterTypes_All();
                bdd_fortest = new BrowseTesterTypes_ForTest();
                ((BrowseTesterTypes_ForTest)bdd_fortest).TestID = TestID;
                bdd_fortest = bdd_fortest.LoadBrowseEntity(BDikaContext.Current.User) as BrowseTesterTypes_ForTest;
            }
            
            bdd.Load(BDikaContext.Current.User);

            this.Tests_UpdateSelectedTesterTypes.TestID = this.TestID;
            this.Tests_UpdateSelectedTesterTypes.TriggerID = this.TriggerID;
            this.Tests_UpdateSelectedTesterTypes.TesterTypes = bdd.Data;
            this.Tests_UpdateSelectedTesterTypes.SelectedTesterTypes = bdd_fortest != null ? bdd_fortest.GetEntitiesIDs() : null;
        }
    }
}
