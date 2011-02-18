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
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Collectors;
using MySpace.MSFast.Automation.Entities.Collectors;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.ControlLoaders.Collectors
{
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public partial class UpdateCollectorsConfigurationControlLoader : BasePage<UpdateCollectorsConfigurationControlLoader>
    {
        [RequestFieldAttributes("trid", false)]
        public TriggerID TriggerID = 0;

        [RequestFieldAttributes("ttid", false)]
        public TesterTypeID TesterTypeID = 0;

        [RequestFieldAttributes("tid", false)]
        public TestID TestID = 0;

        public static String GetURL(TriggerID TriggerID, TesterTypeID TesterTypeID, TestID TestID)
        {
            return BasePage<UpdateCollectorsConfigurationControlLoader>.GetURL(
                    new RequestQueryArgument() { Key = "ttid", Value = TesterTypeID.ColumnValue.ToString() },
                    new RequestQueryArgument() { Key = "tid", Value = TestID.ColumnValue.ToString() },
                    new RequestQueryArgument() { Key = "trid", Value = TriggerID.ColumnValue.ToString() });
        }
        public static String GetURL(TriggerID TriggerID)
        {
            return BasePage<UpdateCollectorsConfigurationControlLoader>.GetURL(new RequestQueryArgument() { Key = "trid", Value = TriggerID.ColumnValue.ToString() });
        }
        public static String GetURL(TesterTypeID TesterTypeID)
        {
            return BasePage<UpdateCollectorsConfigurationControlLoader>.GetURL(new RequestQueryArgument() { Key = "ttid", Value = TesterTypeID.ColumnValue.ToString() });
        }
        public static String GetURL(TestID TestID)
        {
            return BasePage<UpdateCollectorsConfigurationControlLoader>.GetURL(new RequestQueryArgument() { Key = "tid", Value = TestID.ColumnValue.ToString() });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ExtCollectorsConfig ecc = null;

            if (TriggerID.IsValidTriggerID(this.TriggerID) && TestID.IsValidTestID(this.TestID) && TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ecc = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TriggerID, this.TestID, this.TesterTypeID);
            }            
            else if (TestID.IsValidTestID(this.TestID))
            {
                ecc = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TestID);
            }
            else if (TesterTypeID.IsValidTesterTypeID(this.TesterTypeID))
            {
                ecc = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TesterTypeID);
            }
            else if (TriggerID.IsValidTriggerID(this.TriggerID))
            {
                ecc = CollectorsConfigurationProvider.GetExtCollectorsConfig(this.TriggerID);
            }

            this.Collectors_UpdateCollectorsConfiguration.TriggerID = this.TriggerID;
            this.Collectors_UpdateCollectorsConfiguration.TestID = this.TestID;
            this.Collectors_UpdateCollectorsConfiguration.TesterTypeID = this.TesterTypeID;
            this.Collectors_UpdateCollectorsConfiguration.Configuration = ecc;
        }
    }
}
