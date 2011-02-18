//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Application)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
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
using MySpace.MSFast.Automation.Web.Core.Context;
using EYF.Web.Common;
using EYF.Web.Controls.Common;

namespace MySpace.MSFast.Automation.Web.Application.Masters
{
    public partial class Base : System.Web.UI.MasterPage
    {
        public String BodyClass = String.Empty;
        protected static String MachineName = null;

        public String ContentMetaTags_Title = EYFWebResourcesManager.GetString("ContentMetaTags_Title");
        public String ContentMetaTags_Description = EYFWebResourcesManager.GetString("ContentMetaTags_Description");
        public String ContentMetaTags_Image = EYFWebResourcesManager.GetString("ContentMetaTags_Image");

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

            PageAttributes pa = MSFAContext.Current.PageAttributes;

            String pageTitle = null;

            if (Context.Handler is BasePage)
            {
                pageTitle = ((BasePage)Context.Handler).GetPageTitle();
            }

            if (String.IsNullOrEmpty(pageTitle))
            {
                try
                {
                    pageTitle = EYFWebResourcesManager.GetString("Common.PageTitles", pa.PageTitleKey);
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
                (uint)MSFAContext.Current.User.UserID,
                (uint)MSFAContext.Current.User.UserAttributes));

            StaticContentWriter.AddScript(Context, sl);
        }
    }
}
