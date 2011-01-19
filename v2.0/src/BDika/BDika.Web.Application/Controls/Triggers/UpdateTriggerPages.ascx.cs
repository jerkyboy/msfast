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
using BDika.Entities.Triggers;
using BDika.Providers.Tests.Browse;
using BDika.Entities.Collectors;

namespace BDika.Web.Application.Controls.Triggers
{
    public partial class UpdateTriggerPages : BaseControl<UpdateTriggerPages>
    {
        public Trigger Trigger;
        public BrowseTestsEntities SelectedTests = null;
        public BrowseTestsEntities AllTests = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Trigger == null || TriggerID.IsValidTriggerID(Trigger.TriggerID) == false)
            {
                this.phInvalidTrigger.Visible = true;
                this.phTrigger.Visible = false;
                return;
            }

            this.phInvalidTrigger.Visible = false;
            this.phTrigger.Visible = true;

            this.Triggers_UpdateTriggerToTestAndTesterType.TriggerID = Trigger.TriggerID;
            this.Triggers_UpdateTriggerToTestAndTesterType.SelectedTests = SelectedTests;
            this.Triggers_UpdateTriggerToTestAndTesterType.AllTests = AllTests;

            this.Triggers_UpdateOrCreateTrigger.Trigger = Trigger; 
        }
    }
}