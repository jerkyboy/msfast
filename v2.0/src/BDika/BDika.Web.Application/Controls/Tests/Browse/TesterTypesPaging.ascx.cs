using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BDika.Providers.Tests.Browse;
using EYF.Web.Common;

namespace BDika.Web.Application.Controls.Tests.Browse
{
    public partial class TesterTypesPaging : BaseControl<TesterTypesPaging>
    {
        public BrowseTesterTypesEntities BrowseTesterTypesEntities = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.BrowseTesterTypesEntities != null && this.BrowseTesterTypesEntities.Data != null 
                && this.BrowseTesterTypesEntities.Data.Count > 0){

                    this.Tests_TesterTypesList.TesterTypes = this.BrowseTesterTypesEntities.Data;
            }

            this.bfBrowseTesterTypes.NextResultsURL = BDika.Web.Application.Handlers.Tests.Browse.BrowseTesterTypes.GetURL();
            this.bfBrowseTesterTypes.BrowseEntities = BrowseTesterTypesEntities;
        }
    }
}
