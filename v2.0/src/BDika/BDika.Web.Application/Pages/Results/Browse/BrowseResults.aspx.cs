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
using BDika.Providers.Results.Browse;
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;
using BDika.Providers.Tests;
using EYF.Web.Context;
using BDika.Providers.Triggers;

namespace BDika.Web.Application.Pages.Results.Browse
{
    [BDikaPageAttributes]
    public partial class BrowseResults : BasePage<BrowseResults>
    {
        [RequestFieldAttributes("tid")]
        public TestID TestID = 0;

        [RequestFieldAttributes("ttid")]
        public TesterTypeID TesterTypeID = 0;

        [RequestFieldAttributes("trid")]
        public TriggerID TriggerID = 0;

        #region GetURL
        public static String GetURL(Test Test)
        {
            return GetURL(Test.TestID);
        }

        public static String GetURL(TestID TestID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "tid", Value = ((uint)TestID).ToString() });
        }

        public static String GetURL(TesterType TesterType)
        {
            return GetURL(TesterType.TesterTypeID);
        }

        public static String GetURL(TesterTypeID TesterTypeID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "ttid", Value = ((uint)TesterTypeID).ToString() });
        }

        public static String GetURL(Trigger Trigger)
        {
            return GetURL(Trigger.TriggerID);
        }

        public static String GetURL(TriggerID TriggerID)
        {
            return BasePage<BrowseResults>.GetURL(new RequestQueryArgument() { Key = "trid", Value = ((uint)TriggerID).ToString() });
        }
        #endregion

        public String PageTitle;

        public override string GetPageTitle()
        {
            return PageTitle;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BrowseResultsEntities_FreeBrowse brfb = new BrowseResultsEntities_FreeBrowse();
            BrowseTesterTypesEntities btt = null;

            if (TestID.IsValidTestID(TestID))
            {
                Test test = TestsProvider.GetTest(BDikaContext.Current.User.UserID,TestID);
                if (test != null)
                    PageTitle = String.Format(EYFResourcesManager.GetString("title_test"),test.TestName);

                brfb.TestID = this.TestID;
                btt = new BrowseTesterTypes_ForTest() { TestID = this.TestID };
                
            }
            else if (TriggerID.IsValidTriggerID(this.TriggerID))
            {
                Trigger trigger = TriggersProvider.GetTrigger(BDikaContext.Current.User.UserID, TriggerID);
                
                if (trigger != null)
                    PageTitle = String.Format(EYFResourcesManager.GetString("title_trigger"), trigger.TriggerName);

                brfb.TriggerID = this.TriggerID;
                btt = new BrowseTesterTypes_ForTrigger() { TriggerID = this.TriggerID };
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                TesterType testerType = TestsProvider.GetTesterType(BDikaContext.Current.User.UserID, TesterTypeID);

                if (testerType != null)
                    PageTitle = String.Format(EYFResourcesManager.GetString("title_testerType"), testerType.Name);

                brfb.TesterTypeID = this.TesterTypeID;
            }
            else
            {
                PageTitle = EYFResourcesManager.GetString("title");
                btt = new BrowseTesterTypes_All();
            }
            
            brfb.PopTotal = true;

            if (btt != null) 
                btt.Load(BDikaContext.Current.User);
            
            brfb.Load(BDikaContext.Current.User);
            
            this.Results_ResultsDisplayAndFilter.BrowseResultsEntities = brfb;
            this.Results_ResultsDisplayAndFilter.BrowseTesterTypesEntities = btt;

        }
    }
}
