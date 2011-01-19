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
using BDika.Providers.Triggers.Browse;

namespace BDika.Web.Application.Controls.Triggers.Browse
{
    public partial class TriggersPaging : BaseControl<TriggersPaging>
    {
        public BrowseTriggersEntities BrowseTriggersEntities;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BrowseTriggersEntities != null && this.BrowseTriggersEntities.Data != null
                && this.BrowseTriggersEntities.Data.Count > 0)
            {

                this.Triggers_TriggersList.Triggers = this.BrowseTriggersEntities.Data;
            }

            this.bfBrowssTriggers.NextResultsURL = BDika.Web.Application.Handlers.Triggers.Browse.BrowseTriggers.GetURL();
            this.bfBrowssTriggers.BrowseEntities = BrowseTriggersEntities;
        }
    }
}