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
using BDika.Entities.Tests;
using System.Collections.Generic;

namespace BDika.Web.Application.Controls.Tests
{
    public partial class UpdateTestPages : BaseControl<UpdateTestPages>
    {
        public Test Test;
        public ICollection<TesterType> TesterTypes = null;
        public ICollection<TesterTypeID> SelectedTesterTypes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Test == null)
            {
                this.phInvalidTest.Visible = true;
                this.phTest.Visible = false;
                return;
            }
            this.phTest.Visible = true;
            this.phInvalidTest.Visible = false;
            
            this.Tests_UpdateOrCreateTest.Test = Test;
            this.Tests_UpdateSelectedTesterTypes.TestID = Test.TestID;
            this.Tests_UpdateSelectedTesterTypes.TesterTypes = TesterTypes;
            this.Tests_UpdateSelectedTesterTypes.SelectedTesterTypes = SelectedTesterTypes;

        }
    }
}