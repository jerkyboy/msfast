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
using BDika.Providers.Triggers.Browse;
using BDika.Entities.Triggers;
using EYF.Web.Handlers.Browse;

namespace BDika.Web.Application.Handlers.Triggers.Browse
{
    public partial class BrowseTriggers : BrowseHandler<BrowseTriggers, BrowseTriggersEntities, Trigger>
    {
        public override void RenderResults(BrowseTriggersEntities bd)
        {
            if (bd != null && bd.Data != null && bd.Data.Count > 0)
                this.Triggers_TriggersList.Triggers = bd.Data;

        }
    }
}
