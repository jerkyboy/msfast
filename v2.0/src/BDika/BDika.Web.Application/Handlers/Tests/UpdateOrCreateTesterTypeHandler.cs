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
using BDika.Providers.Tests;
using EYF.Web.Context;

namespace BDika.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/UpdateOrCreateTesterTypeHandler.axd")]
    [BDikaPageAttributes]
    public class UpdateOrCreateTesterTypeHandler : AsyncFormHandler<UpdateOrCreateTesterTypeHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "testername")]
        public String TesterTypeName;

        [RequestFieldAttributes("t", false)]
        public TesterTypeID TesterTypeID;

        public override void ProcessForm()
        {
            if (BDikaContext.Current.IsRequestValidationPassed == false)
                return;

            TesterType t = new TesterType();
            t.TesterTypeID = this.TesterTypeID;
            t.Name = this.TesterTypeName;

            t = TestsProvider.UpdateOrCreateTesterType(BDikaContext.Current.User.UserID, t);
            
            if (t != null)
            {
                BDikaContext.Current.AddMessage(new ResponseMessage("ok",EYFResourcesManager.GetString("saved")));
                return;
            }

            BDikaContext.Current.AddUnexpectedError();
        }
    }
}
