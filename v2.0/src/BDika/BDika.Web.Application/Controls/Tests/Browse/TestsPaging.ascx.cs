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
using BDika.Providers.Tests.Browse;

namespace BDika.Web.Application.Controls.Tests.Browse
{
    public partial class TestsPaging : BaseControl<TestsPaging>
    {
        public BrowseTestsEntities BrowseTestsEntities = null;

        public String FORMID;

        public bool ShowEditLink = true;
        public bool ShowTestURL = true;
        public bool ShowHeaders = true;
        public bool ShowRemoveLink = true;
        public bool ShowAddLink = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BrowseTestsEntities != null && this.BrowseTestsEntities.Data != null
                && this.BrowseTestsEntities.Data.Count > 0)
            {

                this.Tests_TestsList.Tests = this.BrowseTestsEntities.Data;
            }
            
            FORMID = this.bfBrowseTests.FormID;

            this.bfBrowseTests.NextResultsURL = BDika.Web.Application.Handlers.Tests.Browse.BrowseTests.GetURL();
            this.bfBrowseTests.BrowseEntities = BrowseTestsEntities;


        }
    }
}