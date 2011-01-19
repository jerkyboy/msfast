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
using BDika.Entities.Tests;
using EYF.Web.Common;

namespace BDika.Web.Application.Controls.Tests
{
    public partial class UpdateOrCreateTesterType : BaseControl<UpdateOrCreateTesterType>
    {
        public TesterType TesterType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.TesterType != null && TesterTypeID.IsValidTesterTypeID(this.TesterType.TesterTypeID))
            {
                this.ihTesterTypeID.Value = this.TesterType.TesterTypeID.ToString();
                this.ltName.Value = this.TesterType.Name;
            }

            this.afUpdateOrCreateTesterType.Action = BDika.Web.Application.Handlers.Tests.UpdateOrCreateTesterTypeHandler.GetURL();
        }
    }
}