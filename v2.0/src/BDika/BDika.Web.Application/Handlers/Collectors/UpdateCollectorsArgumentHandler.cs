using System;
using System.Data;
using System.Configuration;
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
using EYF.Web.Handlers.Common;
using EYF.Web.Common.InputValidation;
using BDika.Entities.Tests;
using BDika.Entities.Triggers;
using BDika.Web.Core.Context;
using BDika.Providers.Collectors;
using EYF.Web.Context;

namespace BDika.Web.Application.Handlers.Collectors
{

    [Linkable(Target = "/Handlers/Collectors/UpdateCollectorsArgumentHandler.axd")]
    [BDikaPageAttributes]
    public class UpdateCollectorsArgumentHandler : AsyncFormHandler<UpdateCollectorsArgumentHandler>
    {
        [RequestFieldAttributes("k", true), HasValueWithLimit(45, FieldResourcesKey = "key")]
        public String Key;

        [RequestFieldAttributes("v", true), HasValueWithLimit(1024, FieldResourcesKey = "value")]
        public String Value;

        [RequestFieldAttributes("ttid", false)]
        public TesterTypeID TesterTypeID;

        [RequestFieldAttributes("tid", false)]
        public TestID TestID;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        public override void ProcessForm()
        {
            if (BDikaContext.Current.IsRequestValidationPassed == false)
            {
                BDikaContext.Current.AddUnexpectedError();
                return;
            }

            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false &&
               TestID.IsValidTestID(TestID) == false &&
               TriggerID.IsValidTriggerID(TriggerID) == false)
            {
                BDikaContext.Current.AddUnexpectedError();
                return;
            }

            bool ok = false;

            if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) && TriggerID.IsValidTriggerID(this.TriggerID))
            {
                ok = CollectorsConfigurationProvider.SetArgument(this.TriggerID, this.TestID, this.TesterTypeID, this.Key, this.Value);
            }
            else if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ok = CollectorsConfigurationProvider.SetArgument(this.TestID, this.TesterTypeID, this.Key, this.Value);
            }
            else if (TestID.IsValidTestID(this.TestID))
            {
                ok = CollectorsConfigurationProvider.SetArgument(this.TestID, this.Key, this.Value);
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ok = CollectorsConfigurationProvider.SetArgument(this.TesterTypeID, this.Key, this.Value);
            }
            else if (TriggerID.IsValidTriggerID(TriggerID))
            {
                ok = CollectorsConfigurationProvider.SetArgument(this.TriggerID, this.Key, this.Value);
            }

            if (ok)
            {
                BDikaContext.Current.AddMessage(new ResponseMessage("ok", EYFResourcesManager.GetString("saved")));
                return;
            }

            BDikaContext.Current.AddUnexpectedError();
        }
    }
}
