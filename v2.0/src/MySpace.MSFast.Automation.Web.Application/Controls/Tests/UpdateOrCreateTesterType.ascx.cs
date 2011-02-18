﻿//=======================================================================
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
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Web.Common;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Tests
{
    public partial class UpdateOrCreateTesterType : BaseControl<UpdateOrCreateTesterType>
    {
        public TesterType TesterType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.TesterType != null && TesterTypeID.IsValidTesterTypeID(this.TesterType.TesterTypeID))
            {
                this.ihTesterTypeID.Value = this.TesterType.TesterTypeID.ToString();
                this.ltName.Value = this.TesterType.Name;
                this.Collectors_UpdateCollectorsConfiguration.ConfigurableEntity = this.TesterType;
            }
            else
            {
                this.bxUpdateCollectorsConfiguration.Visible = false;
                this.Collectors_UpdateCollectorsConfiguration.Visible = false;
            }

            this.afUpdateOrCreateTesterType.Action = MySpace.MSFast.Automation.Web.Application.Handlers.Tests.UpdateOrCreateTesterTypeHandler.GetURL();
        }
    }
}