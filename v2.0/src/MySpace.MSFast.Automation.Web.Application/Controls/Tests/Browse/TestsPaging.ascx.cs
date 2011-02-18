//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Application)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
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
using MySpace.MSFast.Automation.Providers.Tests.Browse;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Tests.Browse
{
    public partial class TestsPaging : BaseControl<TestsPaging>
    {
        public BrowseTestsEntities BrowseTestsEntities = null;

        public String FORMID;

        public bool ShowEditLink = true;
        public bool ShowTestURL = true;
        public bool ShowRemoveLink = true;
        public bool ShowSelected = false;        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BrowseTestsEntities != null && this.BrowseTestsEntities.Data != null
                && this.BrowseTestsEntities.Data.Count > 0)
            {

                this.Tests_TestsList.Tests = this.BrowseTestsEntities.Data;
            }

            this.Tests_TestsList.ShowEditLink = ShowEditLink;
            this.Tests_TestsList.ShowTestURL = ShowTestURL;
            this.Tests_TestsList.ShowRemoveLink = ShowRemoveLink;
            this.Tests_TestsList.ShowSelected = ShowSelected;

            FORMID = this.bfBrowseTests.FormID;

            this.bfBrowseTests.NextResultsURL = MySpace.MSFast.Automation.Web.Application.Handlers.Tests.Browse.BrowseTests.GetURL();
            this.bfBrowseTests.BrowseEntities = BrowseTestsEntities;


        }
    }
}