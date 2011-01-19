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
using BDika.Web.Core.Context;
using EYF.Web.Common;
using EYF.Web.Context;

namespace BDika.Web.Application.Masters
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageAttributes pa = BDikaContext.Current.PageAttributes;

            String pageTitle = null;

            if (Context.Handler is BasePage)
            {
                pageTitle = ((BasePage)Context.Handler).GetPageTitle();
            }

            if (String.IsNullOrEmpty(pageTitle))
            {
                try
                {
                    pageTitle = EYFResourcesManager.GetString("Common.PageTitles", pa.PageTitleKey);
                }
                catch
                {
                }
            }

            if (String.IsNullOrEmpty(pageTitle))
            {
                this.h2PageTitle.Visible = false;
            }
            else
            {
                this.h2PageTitle.InnerHtml = pageTitle;
            }
        }
    }
}
