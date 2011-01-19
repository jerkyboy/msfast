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
using BDika.Providers.Collectors;
using EYF.Web.Controls.Common.Forms;
using BDika.Entities.Collectors;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Web.Application.Controls.Collectors
{
    public partial class UpdateCollectorsConfiguration : BaseControl<UpdateCollectorsConfiguration>
    {
        public TriggerID TriggerID;
        public TesterTypeID TesterTypeID;
        public TestID TestID;

        public ExtCollectorsConfig Configuration;
        public ConfigurableEntity ConfigurableEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

            CollectorsArgument[] ca = null;

            if (Configuration == null)
            {
                if (ConfigurableEntity != null)
                {
                    Configuration = ConfigurableEntity.Configuration;
                    if (ConfigurableEntity is Test)
                    {
                        this.TestID = ((Test)ConfigurableEntity).TestID;
                    }
                    else if (ConfigurableEntity is Trigger)
                    {
                        this.TriggerID = ((Trigger)ConfigurableEntity).TriggerID;
                    }
                    else if (ConfigurableEntity is TesterType)
                    {
                        this.TesterTypeID = ((TesterType)ConfigurableEntity).TesterTypeID;
                    }
                }
            }
            
            if (Configuration == null || (ca = Configuration.GetAllArguments()) == null || ca.Length == 0)
            {
                Configuration = new ExtCollectorsConfig();
                Configuration.SetArgument("Key", "Value");
                ca = Configuration.GetAllArguments();
            }

            afUpdateCollectorsConfiguration.Action = BDika.Web.Application.Handlers.Collectors.UpdateCollectorsArgumentHandler.GetURL();
            afRemoveCollectorsConfiguration.Action = BDika.Web.Application.Handlers.Collectors.RemoveCollectorsArgumentHandler.GetURL();

            this.ihVal_Update.Value = this.ihKey_Update.Value = this.ihKey_Remove.Value = "";

            this.ihTesterTypeID_Update.Value = this.ihTesterTypeID_Remove.Value = (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID) ? this.TesterTypeID.ColumnValue.ToString() : "");
            this.ihTestID_Update.Value = this.ihTestID_Remove.Value = (TestID.IsValidTestID(this.TestID) ? this.TestID.ColumnValue.ToString() : "");
            this.ihTriggerID_Update.Value = this.ihTriggerID_Remove.Value = (TriggerID.IsValidTriggerID(this.TriggerID) ? this.TriggerID.ColumnValue.ToString() : "");

            if (ca != null)
            {
                rptConfiguration.DataSource = ca;
                rptConfiguration.ItemDataBound += new RepeaterItemEventHandler(rptConfiguration_ItemDataBound);
                rptConfiguration.DataBind();
            }
        }
        
        void rptConfiguration_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltTR = e.Item.FindControl("ltTR") as Literal;
            InputText itKey = e.Item.FindControl("itKey") as InputText;
            InputText itVal = e.Item.FindControl("itVal") as InputText;
            InputCheckbox icInherit = e.Item.FindControl("icInherit") as InputCheckbox;

            CollectorsArgument cv = e.Item.DataItem as CollectorsArgument;

            if (ltTR == null || itKey == null || itVal == null || icInherit == null || cv == null)
                return;

            bool isNewVal = Configuration.IsNewVal(cv);
            bool isOverride = Configuration.IsOverride(cv);
            String originalValue = Configuration.GetOriginalValue(cv);

            ltTR.Text = "<tr class=\"" + (isOverride ? "override" : (isNewVal ? "newval" : "inherit")) + "\" originalval=\"" + (isOverride ? originalValue : cv.Value) + "\" latestval=\"" + cv.Value + "\">";

            itVal.Value = cv.Value;
            itKey.Value = cv.Key;

            if (isOverride)
                icInherit.Checked = true;

            if (isNewVal == false || isOverride)
            {
                itKey.Disabled = true;
            }
        }
    }
}