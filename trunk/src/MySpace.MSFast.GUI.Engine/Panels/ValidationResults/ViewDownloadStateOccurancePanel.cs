//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MySpace.MSFast.DataValidators;
using MySpace.MSFast.DataValidators.ValidationResultTypes;

namespace MySpace.MSFast.GUI.Engine.Panels.ValidationResults
{
	public partial class ViewDownloadStateOccurancePanel : Panel
	{
        public ViewDownloadStateOccurancePanel()
		{
			InitializeComponent();
		}

        internal void SetResults(ValidationResults<DownloadStateOccurance> validationResults)
		{
            if(this.webBrowser1.Document == null)
                return;

            HtmlDocument doc = this.webBrowser1.Document;

            HtmlElement h = doc.GetElementById("comment");
            
            if(h!=null)
                h.InnerHtml = validationResults.ResultsExplenation;

            HtmlElement filesList = doc.GetElementById("filesList");

            if (filesList != null)
                filesList.InnerHtml = "";
            else
                return;

            if (validationResults == null || validationResults.Count == 0)
                return;

            HtmlElement li;

            foreach (DownloadStateOccurance ds in validationResults)
            {
                li = doc.CreateElement("li");

                if(String.IsNullOrEmpty(ds.Comment))
                    li.InnerHtml = String.Format("<a href=\"{0}\">{0}</a> ({1} bytes)",ds.URL,ds.TotalReceived);
                else
                    li.InnerHtml = String.Format("{2} <a href=\"{0}\">{0}</a> ({1} bytes)", ds.URL, ds.TotalReceived, ds.Comment);

                filesList.AppendChild(li);
            }
		}
	}
}
