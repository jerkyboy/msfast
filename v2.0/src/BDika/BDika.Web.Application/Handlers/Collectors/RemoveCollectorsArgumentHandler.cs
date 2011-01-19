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
using BDika.Entities.Tests;
using BDika.Entities.Triggers;
using BDika.Web.Core.Context;
using BDika.Providers.Collectors;
using EYF.Web.Context;
using EYF.Web.Handlers.Common;
using EYF.Web.Common.InputValidation;

namespace BDika.Web.Application.Handlers.Collectors
{
    [Linkable(Target = "/Handlers/Collectors/RemoveCollectorsArgumentHandler.axd")]
    [BDikaPageAttributes]
    public class RemoveCollectorsArgumentHandler : AsyncFormHandler<RemoveCollectorsArgumentHandler>
    {
        [RequestFieldAttributes("k", true), HasValueWithLimit(45, FieldResourcesKey = "key")]
        public String Key;

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
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TriggerID, this.TestID, this.TesterTypeID, this.Key);
            }
            else if (TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TestID, this.TesterTypeID, this.Key);
            }
            else if (TestID.IsValidTestID(this.TestID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TestID, this.Key);
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TesterTypeID, this.Key);
            }
            else if (TriggerID.IsValidTriggerID(TriggerID))
            {
                ok = CollectorsConfigurationProvider.RemoveArgument(this.TriggerID, this.Key);
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
