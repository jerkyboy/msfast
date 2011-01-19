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
using BDika.Entities.Tests;
using System.Collections.Generic;
using EYF.Web.Common;

namespace BDika.Web.Application.Controls.Tests.Browse
{
    public partial class TesterTypesList : BaseControl<TesterTypesList>
    {
        public ICollection<TesterType> TesterTypes;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cphNoResults.Visible = false;
            this.rptTesterTypesList.Visible = false;

            if (TesterTypes == null || TesterTypes.Count == 0)
            {
                this.cphNoResults.Visible = true;
                return;
            }

            this.rptTesterTypesList.DataSource = this.TesterTypes;
            this.rptTesterTypesList.Visible = true;
            this.rptTesterTypesList.ItemDataBound += new RepeaterItemEventHandler(rptTesterTypesList_ItemDataBound);
            rptTesterTypesList.DataBind();



        }

        void rptTesterTypesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ShortTesterTypeDescription spd = e.Item.FindControl("Tests_ShortTesterTypeDescription") as ShortTesterTypeDescription;
            
            if (spd != null)
            {
                spd.TesterType = e.Item.DataItem as TesterType;
            }
        }
    }
}