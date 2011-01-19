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

            this.hrefEditTesterType.Ref = "#";
            this.hrefEditTesterType.Text = EYFResourcesManager.GetString("edit");
            this.hrefEditTesterType.AdditionalAttribute = "onclick=\"edittestertype(" + this.TesterType.TesterTypeID + ");\"";

            this.ttTesterTypeName.Text = this.TesterType.Name;
        }
    }
}