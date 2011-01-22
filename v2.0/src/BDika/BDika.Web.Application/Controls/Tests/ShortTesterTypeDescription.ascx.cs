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
using EYF.Web.Context;

namespace BDika.Web.Application.Controls.Tests
{
    public partial class ShortTesterTypeDescription : BaseControl<ShortTesterTypeDescription>
    {
        public TesterType TesterType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TesterType == null)
            {
                this.Visible = false;
                return;
            }


            this.ltClientID.Text = TesterType.ClientID;
            this.ltClientKey.Text = TesterType.ClientKey;
            this.ltEnabled.Text = TesterType.Enabled ? "Yes" : "No";
            this.ltLastPing.Text = "n/a";
            this.ttTesterTypeName.Text = this.TesterType.Name;
        }
    }
}