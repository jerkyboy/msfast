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
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Providers.Results.Browse;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Entities.Results;
using System.Collections.Generic;
using EYF.Web.Context;

namespace MySpace.MSFast.Automation.Web.Application.Controls.Results
{
    public partial class ResultsDisplayAndFilter : BaseControl<ResultsDisplayAndFilter>
    {
        public BrowseResultsEntities_FreeBrowse BrowseResultsEntities;
        public BrowseTesterTypesEntities BrowseTesterTypesEntities;

        private class ListObj
        {
            private string _name;
            private uint _value;

            public String Name { get { return _name; } }
            public uint Value{get{return _value;}}

            public ListObj(String s,uint v)
            {
                this._name = s;
                this._value = v;
            }
        }

        private static ListObj[] ResultsStateListObjects = new ListObj[]
        {
            new ListObj("All", (uint)ResultsState.Unknown),
            new ListObj("Pending", (uint)ResultsState.Pending),
            new ListObj("Testing", (uint)ResultsState.Testing),
            new ListObj("Processing", (uint)ResultsState.Processing),
            new ListObj("Failed", (uint)ResultsState.Failed),
            new ListObj("Succeeded", (uint)ResultsState.Succeeded),
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BrowseResultsEntities == null) 
                return;

            this.Results_ResultsPaging.BrowseResultsEntities = BrowseResultsEntities;
            
            if(TestID.IsValidTestID(BrowseResultsEntities.TestID))
                this.ihTestID.Value = BrowseResultsEntities.TestID.ToString();


            if (BrowseTesterTypesEntities != null && BrowseTesterTypesEntities.Data != null && BrowseTesterTypesEntities.Data.Count > 0)
            {
                this.phTestTypes.Visible = true;

                List<ListObj> lst = new List<ListObj>(BrowseTesterTypesEntities.Data.Count + 1);

                lst.Add(new ListObj(EYFWebResourcesManager.GetString("all"), 0));

                foreach (TesterType tt in BrowseTesterTypesEntities.Data)
                {
                    lst.Add(new ListObj(tt.Name, tt.TesterTypeID));
                }

                this.isTesterTypeIDs.DataTextField = "Name";
                this.isTesterTypeIDs.DataValueField = "Value";
                this.isTesterTypeIDs.DataSource = lst;
                this.isTesterTypeIDs.DataBind();
            }

            this.isResultsState.DataTextField = "Name";
            this.isResultsState.DataValueField = "Value";
            this.isResultsState.DataSource = ResultsStateListObjects;
            this.isResultsState.DataBind();
        }
    }
}