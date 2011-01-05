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
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Engine.Events;

namespace MySpace.MSFast.GUI.Engine.Panels.Status
{

	

    public partial class TestStatutsPanel : Panel
	{
        public bool IsStandAlone;
        
        public TestStatutsPanel(bool isStandAlone):base()
		{
            this.IsStandAlone = isStandAlone;
            InitializeComponent();
		}

		public void SetTestStatus(TestEventType status, params object[] args)
        {
			if (status == TestEventType.TestEnded)
			{
                this.lblCurrentLabel.Text = "";
                this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
                //this.txtTraceout.Text = "";
                this.lstDownloadedFiles.Reset();

                this.startTestPanel.Visible = true;
                this.workingPanel.Visible = false;
            }
            else
            {
                this.startTestPanel.Visible = false; 
                this.workingPanel.Visible = true;
            }

            if (status == TestEventType.StartingTest)
            {
                this.lblCurrentLabel.Text = "Starting Test...";
                this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
            }
            else if(status == TestEventType.TestStarted)
            {
                this.lblCurrentLabel.Text = "Loading Page...";
                this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Completed);
                this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
            }
            else if(status == TestEventType.CapturingSegment || status == TestEventType.RenderingSegment)
            {
                int progress = (int)args[0];
                int total = (int)args[1];

                if (status == TestEventType.RenderingSegment)
                {
                    this.lblCurrentLabel.Text = "Rendering Segment " + Math.Max(0, Math.Min(progress, total)) + " out of " + total;
                    this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Completed);
                    this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.ProgressChange, progress, total);
                    this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Pending);
                    this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
                }
                else if (status == TestEventType.CapturingSegment)
                {
                    this.lblCurrentLabel.Text = "Capturing Segment " + Math.Max(0, Math.Min(progress, total)) + " out of " + total;
                    this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Completed);
                    this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Completed);
                    this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.ProgressChange, progress, total);
                    this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
                }

            }
            else if(status == TestEventType.AbortingTest)
            {
                this.lblCurrentLabel.Text = "Aborting Test...";
                this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Pending);
                this.lblProcessResults.SetStatus(TaskProgressLabelStatus.Pending);
            }
            else if(status == TestEventType.ProcessingResults)
            {
                this.lblCurrentLabel.Text = "Processing Results";
                this.lblPreparingTest.SetStatus(TaskProgressLabelStatus.Completed);
                this.lblRenderingSegment.SetStatus(TaskProgressLabelStatus.Completed);
                this.lblCaptureSegment.SetStatus(TaskProgressLabelStatus.Completed);
                this.lblProcessResults.SetStatus(TaskProgressLabelStatus.ProgressChange,-1,-1);
            }
            else if (status == TestEventType.RequestingFile)
            {
                this.lstDownloadedFiles.OnRequestingFile((String)args[2]);
                //this.txtTraceout.AppendText("Downloading " + args[2] + "\r\n");
            }
            else if (status == TestEventType.ResponseEnded)
            {
                this.lstDownloadedFiles.OnFileReceived((String)args[2]);
                //this.txtTraceout.AppendText("Finish Downloading " + args[2] + "\r\n");
            }
            else if (status == TestEventType.SendingData)
            {
                this.lstDownloadedFiles.OnSendingData((String)args[2], (int)args[0]);
                //this.txtTraceout.AppendText("Sending Data " + args[0] + " " + args[2] + "\r\n");
            }
            else if (status == TestEventType.ReceivingData)
            {
                this.lstDownloadedFiles.OnReceivingData((String)args[2], (int)args[0]);
                //this.txtTraceout.AppendText("Receiving Data " + args[0] + " " + args[2] + "\r\n");
            }

            if(status != TestEventType.CapturingSegment && 
                status != TestEventType.RenderingSegment &&
                status != TestEventType.RequestingFile &&
                status != TestEventType.SendingData &&
                status != TestEventType.ReceivingData &&
                status != TestEventType.ResponseEnded)
             
                Refresh();

		}

        
	}
}
