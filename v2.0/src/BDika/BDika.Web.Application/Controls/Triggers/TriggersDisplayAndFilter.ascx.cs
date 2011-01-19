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
using BDika.Entities.Triggers;
using System.Collections.Generic;

namespace BDika.Web.Application.Controls.Triggers
{
    public partial class TriggersDisplayAndFilter : BaseControl<TriggersDisplayAndFilter>
    {
        public BrowseTriggersEntities_FreeBrowse BrowseTriggersEntities;

        private class ListObj
        {
            private string _name;
            private String _value;

            public String Name { get { return _name; } }
            public String Value { get { return _value; } }

            public ListObj(String s,String v)
            {
                this._name = s;
                this._value = v;
            }
        }

        private static ListObj[] TriggerTypeListObjects = new ListObj[]
        {
            new ListObj("All", ((uint)TriggerType.Unknown).ToString()),
            new ListObj("Manual", ((uint)TriggerType.Manual).ToString()),
            new ListObj("Timer", ((uint)TriggerType.Time).ToString())
        };

        private static ListObj[] EnabledListObjects = new ListObj[]
        {
            new ListObj("All", "-1"),
            new ListObj("Enabled", "1"),
            new ListObj("Disabled", "0")
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BrowseTriggersEntities == null) 
                return;

            this.Triggers_TriggersPaging.BrowseTriggersEntities = BrowseTriggersEntities;

            this.isType.DataTextField = "Name";
            this.isType.DataValueField = "Value";
            this.isType.DataSource = TriggerTypeListObjects;
            this.isType.DataBind();

            this.isEnabled.DataTextField = "Name";
            this.isEnabled.DataValueField = "Value";
            this.isEnabled.DataSource = EnabledListObjects;
            this.isEnabled.DataBind();
        }
    }
}