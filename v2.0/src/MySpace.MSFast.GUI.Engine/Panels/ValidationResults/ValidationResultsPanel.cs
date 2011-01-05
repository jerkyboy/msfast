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
	public partial class ValidationResultsPanel : Panel
	{
        private IEnumerable<IValidationResults> rsults;
 
		public ValidationResultsPanel()
		{
			InitializeComponent();
		}

        public void SetResults(IEnumerable<IValidationResults> rsults)
		{
            this.selectGroup.Items.Clear();
			this.ls_content.ClearList();
            this.rsults = rsults;

			if (rsults != null)
            {
                foreach (IValidationResults rr in rsults)
				{
                    if (this.selectGroup.Items.Contains(rr.GroupName) == false)
                    {
                        this.selectGroup.Items.Add(rr.GroupName);
                    }
				}
                if (this.selectGroup.Items.Count > 0)
                    this.selectGroup.SelectedIndex = 0;
            }
            ShowResults();
		}

        public void ShowResults()
        {
            this.ls_content.ClearList();

            if (this.rsults != null)
            {
                foreach (IValidationResults rr in rsults)
                {
                    if (rr.GroupName == (String)this.selectGroup.SelectedItem)
                    {
                        this.ls_content.Add(rr);
                    }
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if(this.Visible){
                if(this.ls_content.Items.Count >0){
                    this.ls_content.Items[0].Selected = true;
                }
           }
        }
		public void ResultsSelected(IValidationResults results)
		{
            viewSourceDataPanel.Visible = false;
            viewDownloadDataPanel.Visible = false;
            viewStringPanel.Visible = false;

            bool hasResults = false;

            if (results is ValidationResults<SourceValidationOccurance>)
            {
                viewSourceDataPanel.Visible = results.Count > 0;
                viewSourceDataPanel.SetResults((ValidationResults<SourceValidationOccurance>)results);
                hasResults = viewSourceDataPanel.Visible;
            }
            else if (results is ValidationResults<DownloadStateOccurance>)
            {
                viewDownloadDataPanel.Visible = results.Count > 0;
                viewDownloadDataPanel.SetResults((ValidationResults<DownloadStateOccurance>)results);
                hasResults = viewDownloadDataPanel.Visible;
            }
            else if (results is ValidationResults<String>)
            {
                viewStringPanel.Visible = results.Count > 0;
                viewStringPanel.SetResults((ValidationResults<String>)results);
                hasResults = viewStringPanel.Visible;
            }

            if(results != null && results.Validator != null)
                SetValidatorDescription(results.Validator.Description);
            else
                SetValidatorDescription(null);

            viewResultsEmptyPanel.Visible = !hasResults;
		}

        
	}
}
