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
using EYF.Web.Handlers.Common;
using EYF.Web.Common;
using BDika.Web.Core.Common;
using EYF.Web.Common.InputValidation;
using BDika.Entities.Tests;
using BDika.Web.Core.Context;
using System.Collections.Generic;
using BDika.Providers.Tests;
using EYF.Web.Context;
using BDika.Entities.Triggers;

namespace BDika.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/UpdateSelectedTesterTypesHandler.axd")]
    [BDikaPageAttributes]
    public class UpdateSelectedTesterTypesHandler : AsyncFormHandler<UpdateSelectedTesterTypesHandler>
    {
        [RequestFieldAttributes("ttid")]
        public TesterTypeID TesterTypeID;

        [RequestFieldAttributes("trid")]
        public TriggerID TriggerID;

        [RequestFieldAttributes("tid")]
        public TestID TestID;

        [RequestFieldAttributes("state")]
        public bool Selected;

        public override void ProcessForm()
        {
            if (BDikaContext.Current.IsRequestValidationPassed == false)
                return;

            if (TestID.IsValidTestID(TestID) == false || TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false)
            {
                BDikaContext.Current.AddUnexpectedError();
                return;
            }

            if (TestsProvider.UpdateTesterTypeState(BDikaContext.Current.User.UserID, TestID, TesterTypeID, TriggerID, Selected))
            {
                BDikaContext.Current.AddMessage(new ResponseMessage("ok", EYFResourcesManager.GetString("saved")));
                return;
            }

            BDikaContext.Current.AddUnexpectedError();
        }
    }
}
