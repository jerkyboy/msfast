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
using BDika.Providers.Tests.Browse;
using System.Collections.Generic;
using BDika.Entities.Tests;
using BDika.Entities.Triggers;

namespace BDika.Web.Application.Controls.Triggers
{
    public partial class UpdateTriggerToTestAndTesterType : BaseControl<UpdateTriggerToTestAndTesterType>
    {
        public TriggerID TriggerID;

        public BrowseTestsEntities SelectedTests = null;
        public BrowseTestsEntities AllTests = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Tests_TestsPaging_SelectedTests.ShowAddLink = false;
            Tests_TestsPaging_SelectedTests.ShowEditLink = false;
            Tests_TestsPaging_SelectedTests.ShowHeaders = false;
            Tests_TestsPaging_SelectedTests.ShowTestURL = false;
            Tests_TestsPaging_SelectedTests.ShowRemoveLink = true;
            Tests_TestsPaging_SelectedTests.BrowseTestsEntities = SelectedTests;
            
            Tests_TestsPaging_AllTests.ShowEditLink = false;
            Tests_TestsPaging_AllTests.ShowRemoveLink = false;
            Tests_TestsPaging_AllTests.BrowseTestsEntities = AllTests;
        }
    }
}