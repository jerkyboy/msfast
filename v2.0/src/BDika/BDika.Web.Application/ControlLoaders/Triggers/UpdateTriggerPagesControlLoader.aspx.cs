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
using EYF.Web.Common;
using BDika.Entities.Triggers;
using BDika.Providers.Triggers;
using BDika.Web.Core.Context;
using BDika.Providers.Tests.Browse;

namespace BDika.Web.Application.ControlLoaders.Triggers
{
    public partial class UpdateTriggerPagesControlLoader : BasePage<UpdateTriggerPagesControlLoader>
    {
        [RequestFieldAttributes("t", false)]
        public TriggerID TriggerID = 0;

        public static String GetURL(Trigger t)
        {
            if (t != null)
                return GetURL(t.TriggerID);

            return BasePage<UpdateOrCreateTriggerControlLoader>.GetURL();
        }

        public static String GetURL(TriggerID t)
        {
            if (TriggerID.IsValidTriggerID(t))
                return BasePage<UpdateTriggerPagesControlLoader>.GetURL(new RequestQueryArgument() { Key = "t", Value = t.ColumnValue.ToString() });

            return BasePage<UpdateOrCreateTriggerControlLoader>.GetURL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TriggerID.IsValidTriggerID(this.TriggerID) == false)
                return;

            Trigger trigger = TriggersProvider.GetTrigger(BDikaContext.Current.User.UserID, this.TriggerID);

            if (trigger == null)
                return;

            BrowseTests_ForTrigger bt = new BrowseTests_ForTrigger();
            bt.TriggerID = this.TriggerID;
            bt.PopTotal = true;
            bt.Load(BDikaContext.Current.User);

            BrowseTests_All ba = new BrowseTests_All();
            ba.PopTotal = true;
            ba.Load(BDikaContext.Current.User);

            Triggers_UpdateTriggerPages.SelectedTests = bt;
            Triggers_UpdateTriggerPages.AllTests = ba;
            Triggers_UpdateTriggerPages.Trigger = trigger;

        }
    }
}
