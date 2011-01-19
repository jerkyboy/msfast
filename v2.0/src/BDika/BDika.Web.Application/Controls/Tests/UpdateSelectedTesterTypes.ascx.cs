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
using BDika.Entities.Tests;
using System.Collections.Generic;
using EYF.Web.Common;
using EYF.Web.Controls.Common.Forms;
using BDika.Entities.Triggers;

namespace BDika.Web.Application.Controls.Tests
{
    public partial class UpdateSelectedTesterTypes : BaseControl<UpdateSelectedTesterTypes>
    {
        public TestID TestID;
        public TriggerID TriggerID;
        public ICollection<TesterType> TesterTypes = null;
        public ICollection<TesterTypeID> SelectedTesterTypes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TestID.IsValidTestID(TestID) == false || TesterTypes == null || TesterTypes.Count == 0)
            {
                this.cphNoResults.Visible = true;
                this.rptTesterTypesRpt.Visible = false;
                return;
            }

            this.cphNoResults.Visible = false;
            this.rptTesterTypesRpt.Visible = true;
            this.rptTesterTypesRpt.DataSource = TesterTypes;
            this.rptTesterTypesRpt.ItemDataBound += new RepeaterItemEventHandler(rptTesterTypesRpt_ItemDataBound);
            this.rptTesterTypesRpt.DataBind();

            this.ihTriggerID.Value = TriggerID.IsValidTriggerID(TriggerID) ? TriggerID.ColumnValue.ToString() : "";
            this.ihTestID.Value = TestID.IsValidTestID(TestID) ? TestID.ColumnValue.ToString() : "";

            this.afUpdateSelectedTesterTypes.Action = BDika.Web.Application.Handlers.Tests.UpdateSelectedTesterTypesHandler.GetURL();
        }

        void rptTesterTypesRpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltTesterTypeName = e.Item.FindControl("ltTesterTypeName") as Literal;
            InputCheckbox icSelectedTesterType = e.Item.FindControl("icSelectedTesterType") as InputCheckbox;

            if (ltTesterTypeName == null || icSelectedTesterType == null)
                return;

            TesterType t = e.Item.DataItem as TesterType;

            if (t == null)
                return;

            ltTesterTypeName.Text = t.Name;

            if (SelectedTesterTypes != null && SelectedTesterTypes.Contains(t.TesterTypeID))
                icSelectedTesterType.Checked = true;

            icSelectedTesterType.Value = t.TesterTypeID.ColumnValue.ToString();
        }
    }
}            
