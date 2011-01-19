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
using BDika.Web.Core.Common;
using EYF.Web.Common;
using BDika.Entities.Triggers;
using EYF.Web.Common.InputValidation;
using BDika.Web.Core.Context;
using BDika.Providers.Triggers;
using EYF.Web.Context;

namespace BDika.Web.Application.Handlers.Triggers
{
    [Linkable(Target = "/Handlers/Triggers/UpdateOrCreateTriggerHandler.axd")]
    [BDikaPageAttributes]
    public class UpdateOrCreateTriggerHandler : AsyncFormHandler<UpdateOrCreateTriggerHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "triggername")]
        public String TriggerName;

        [RequestFieldAttributes("t", true), HasValue(FieldResourcesKey = "triggertype")]
        public ushort TriggerTypeID;

        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID;

        public override void ProcessForm()
        {
            if (BDikaContext.Current.IsRequestValidationPassed == false)
                return;

            Trigger t = new Trigger();
            t.TriggerID = this.TriggerID;
            t.TriggerName = this.TriggerName;
            t.TriggerTypeID = this.TriggerTypeID;

            t = TriggersProvider.UpdateOrCreateTrigger(BDikaContext.Current.User.UserID, t);
            
            if (t != null)
            {
                BDikaContext.Current.AddMessage(new ResponseMessage("ok",EYFResourcesManager.GetString("saved")));
                return;
            }

            BDikaContext.Current.AddUnexpectedError();
        }
    }
}
