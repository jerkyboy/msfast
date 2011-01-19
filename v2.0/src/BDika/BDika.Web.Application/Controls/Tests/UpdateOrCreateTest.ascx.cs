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
using EYF.Web.Common;
using BDika.Entities.Tests;

namespace BDika.Web.Application.Controls.Tests
{
    public partial class UpdateOrCreateTest : BaseControl<UpdateOrCreateTest>
    {

        public Test Test;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Test != null && TestID.IsValidTestID(this.Test.TestID))
            {
                this.ihTestID.Value = this.Test.TestID.ToString();
                this.ltName.Value = this.Test.TestName;
                this.ltURL.Value = Test.TestURL != null ? Test.TestURL.ToString() : "n/a";
                this.Collectors_UpdateCollectorsConfiguration.ConfigurableEntity = this.Test;
            }
            else
            {
                this.Collectors_UpdateCollectorsConfiguration.Visible = false;
            }


            this.afUpdateOrCreateTest.Action = BDika.Web.Application.Handlers.Tests.UpdateOrCreateTestHandler.GetURL();
        }
    }
}