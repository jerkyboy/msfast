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
using EYF.Web.Common;
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Tests
{
    public partial class DeleteTesterTypeForm : BaseControl<DeleteTesterTypeForm>
    {
        public TesterType TesterType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TesterType == null || TesterTypeID.IsValidTesterTypeID(TesterType.TesterTypeID) == false)
            {
                ShowUnexpectedError();
                return;
            }

            this.phUnexpectedError.Visible = false;
            this.afDeleteTesterTypeFrm.Visible = true;

            this.ltTesterTypeName.Text = TesterType.Name;
            this.afDeleteTesterTypeFrm.Action = MySpace.MSFast.Automation.Web.Application.Handlers.Tests.DeleteTesterTypeHandler.GetURL();
            this.ihTid.Value = MySpace.MSFast.Automation.Web.Application.Handlers.Tests.DeleteTesterTypeHandler.GetTesterTypeToken(this.TesterType.TesterTypeID, this.TesterType.Name);
        }

        private void ShowUnexpectedError()
        {
            this.phUnexpectedError.Visible = true;
            this.afDeleteTesterTypeFrm.Visible = false;
        }
    }
}