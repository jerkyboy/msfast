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
using BDika.Providers.Tests.Browse;
using BDika.Web.Core.Context;
using BDika.Web.Core.Common;
using EYF.Web.Common;
using BDika.Entities.Tests;
using BDika.Providers.Tests;
using System.Collections.Generic;
using BDika.Entities.Triggers;
using BDika.Providers.Collectors;

namespace BDika.Web.Application.Pages
{
    [BDikaPageAttributes]
    public partial class TestContainer : BasePage<TestContainer>
    {
        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        [RequestFieldAttributes("tid", false)]
        public TestID TestID;

        [RequestFieldAttributes("ttid", false)]
        public TesterTypeID TesterTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Collectors_UpdateCollectorsConfiguration.TesterTypeID = this.TesterTypeID;
            this.Collectors_UpdateCollectorsConfiguration.TestID = this.TestID;
            this.Collectors_UpdateCollectorsConfiguration.TriggerID = this.TriggerID;

            if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) && TriggerID.IsValidTriggerID(this.TriggerID))
            {
                this.Collectors_UpdateCollectorsConfiguration.Configuration = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TriggerID, this.TestID, this.TesterTypeID);
            }
            else if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                this.Collectors_UpdateCollectorsConfiguration.Configuration = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TestID, this.TesterTypeID);
            }
            else if (TestID.IsValidTestID(this.TestID))
            {
                this.Collectors_UpdateCollectorsConfiguration.Configuration = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TestID);
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                this.Collectors_UpdateCollectorsConfiguration.Configuration = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TesterTypeID);
            }
            else if (TriggerID.IsValidTriggerID(TriggerID))
            {
                this.Collectors_UpdateCollectorsConfiguration.Configuration = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TriggerID);
            }
        }
    }
}
