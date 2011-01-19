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
using EYF.Web.Context;
using BDika.Web.Core.Context;
using EYF.Web.Common;
using EYF.Web.Controls.Common;

namespace BDika.Web.Application.Masters
{
    public partial class Base : System.Web.UI.MasterPage
    {
        public String BodyClass = String.Empty;
        protected static String MachineName = null;

        public String ContentMetaTags_Title = "BDika";
        public String ContentMetaTags_Description = "BDika Description";
        public String ContentMetaTags_Image = "http://bdika/logo.gif";

        private static String Title = "{0}";
        private static String DefaultTitle = "";


        protected override void OnPreRender(EventArgs e)
        {
            if (MachineName == null)
                MachineName = Environment.MachineName;

            if (String.IsNullOrEmpty(BodyClass) == false)
            {
                BodyClass = " class=\"" + BodyClass + "\"";
            }

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
                this.ltTitle.Text = DefaultTitle;
            }
            else
            {
                this.ltTitle.Text = String.Format(Title, pageTitle);
            }

            if (String.IsNullOrEmpty(ContentMetaTags_Description) == false)
                ContentMetaTags_Description = System.Security.SecurityElement.Escape(ContentMetaTags_Description);

            if (String.IsNullOrEmpty(ContentMetaTags_Image) == false)
                ContentMetaTags_Image = System.Security.SecurityElement.Escape(ContentMetaTags_Image);

            if (String.IsNullOrEmpty(ContentMetaTags_Title) == false)
                ContentMetaTags_Title = System.Security.SecurityElement.Escape(ContentMetaTags_Title);

            ScriptLiteral sl = new ScriptLiteral(String.Format("$user = {{userid:{0},type:{1}}};",
                (uint)BDikaContext.Current.User.UserID,
                (uint)BDikaContext.Current.User.UserAttributes));

            StaticContentWriter.AddScript(Context, sl);
        }
    }
}
