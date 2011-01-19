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
using EYF.Web.Common.InputValidation;
using BDika.Entities.Tests;
using BDika.Web.Core.Context;
using BDika.Providers.Tests;
using EYF.Web.Context;

namespace BDika.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/UpdateOrCreateTestHandler.axd")]
    [BDikaPageAttributes]
    public class UpdateOrCreateTestHandler : AsyncFormHandler<UpdateOrCreateTestHandler>
    {
        [RequestFieldAttributes("n", true), HasValueWithLimit(45, FieldResourcesKey = "testname")]
        public String TestName;

        [RequestFieldAttributes("u", true), HasValueWithLimit(2083,FieldResourcesKey = "testurl")]
        public String TestURL;

        [RequestFieldAttributes("t", false)]
        public TestID TestID;

        public override void ProcessForm()
        {
            if (BDikaContext.Current.IsRequestValidationPassed == false)
                return;

            Test t = new Test();
            t.TestID = this.TestID;
            t.TestName = this.TestName;
            t.TestURL = new Uri(this.TestURL);

            t = TestsProvider.UpdateOrCreateTest(BDikaContext.Current.User.UserID, t);
            
            if (t != null)
            {
                BDikaContext.Current.AddMessage(new ResponseMessage("ok",EYFResourcesManager.GetString("saved")));
                return;
            }

            BDikaContext.Current.AddUnexpectedError();
        }
    }
}
