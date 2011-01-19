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
using BDika.Entities.Collectors;

namespace BDika.Web.Application.Controls.Triggers
{
    public partial class UpdateOrCreateTrigger : BaseControl<UpdateOrCreateTrigger>
    {
        public Trigger Trigger;

        private class ListObj
        {
            private string _name;
            private String _value;

            public String Name { get { return _name; } }
            public String Value { get { return _value; } }

            public ListObj(String s, String v)
            {
                this._name = s;
                this._value = v;
            }
        }

        private static ListObj[] TriggerTypeListObjects = new ListObj[]
        {
            new ListObj("Select", ((uint)TriggerType.Unknown).ToString()),
            new ListObj("Manual", ((uint)TriggerType.Manual).ToString()),
            new ListObj("Timer", ((uint)TriggerType.Time).ToString())
        };


        protected void Page_Load(object sender, EventArgs e)
        {
            this.isTriggerType.DataTextField = "Name";
            this.isTriggerType.DataValueField = "Value";
            this.isTriggerType.DataSource = TriggerTypeListObjects;
            this.isTriggerType.DataBind();

            if (this.Trigger != null && TriggerID.IsValidTriggerID(this.Trigger.TriggerID))
            {
                this.ihTriggerID.Value = this.Trigger.TriggerID.ToString();
                this.ltName.Value = this.Trigger.TriggerName;
                this.isTriggerType.Value = this.Trigger.TriggerTypeID.ToString();

                this.Collectors_UpdateCollectorsConfiguration.TriggerID = this.Trigger.TriggerID;
                this.Collectors_UpdateCollectorsConfiguration.Visible = true;
                this.Collectors_UpdateCollectorsConfiguration.ConfigurableEntity = this.Trigger;
            }
            else
            {
                this.Collectors_UpdateCollectorsConfiguration.Visible = false;
            }

            this.afUpdateOrCreateTrigger.Action = BDika.Web.Application.Handlers.Triggers.UpdateOrCreateTriggerHandler.GetURL();
        }
    }
}