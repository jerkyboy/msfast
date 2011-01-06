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
using System.Drawing;
using System.Text.RegularExpressions;
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.DataValidators.ValidationResultTypes;

namespace MySpace.MSFast.GUI.Engine.Panels.ValidationResults
{
	public partial class ViewSourceValidationOccurancePanel : Panel
	{
		private static Regex RemoveCaretReturn = new Regex("\r\n", RegexOptions.Compiled);
		private static Regex CountCaretReturn = new Regex("\n", RegexOptions.Compiled);

        private ValidationResults<SourceValidationOccurance> currentResults = null;

		private int currentResultsIndex = 0;
		private int lastSourceHashNumber = 0;
		private object setBufferLock = new Object();

		public ViewSourceValidationOccurancePanel()
		{
			InitializeComponent();
            FixSize();
		}
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            FixSize();
        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            FixSize();
        }
        internal void SetResults(ValidationResults<SourceValidationOccurance> validationResults)
		{
			this.currentResults = validationResults;
			this.currentResultsIndex = 0;

			lock (setBufferLock)
			{
                if (validationResults != null && validationResults.Count > 0)
				{
					if (validationResults[0].SourceData.GetHashCode() != lastSourceHashNumber)
					{
						lastSourceHashNumber = validationResults[0].SourceData.GetHashCode();
						SetBuffer(validationResults[0].SourceData.PageSource);
					}
				}
				else
				{
					lastSourceHashNumber = 0;
					SetBuffer("");
				}
			}

			RedrawResultsNextPrev();
		}

		private void OpenHelp()
		{
			
		}

		private void NextOccurance()
		{
			if (currentResults == null || currentResults.Count == 0)
				return;

			if (currentResultsIndex < currentResults.Count - 1) 
			{
				currentResultsIndex++;
			}
			RedrawResultsNextPrev();
		}

		private void PreviousOccurance()
		{
			if (currentResults == null || currentResults.Count == 0)
				return;

			if (currentResultsIndex > 0)
			{ 
				currentResultsIndex--;
			}
			RedrawResultsNextPrev();
		}

		private void SetBuffer(String buffer)
		{
			lock (setBufferLock)
			{
				if (String.IsNullOrEmpty(buffer))
                    this.sourceText.Text = "";
				else
                    this.sourceText.Text = buffer;
			}
		}

		private void RedrawResultsNextPrev()
		{
            this.sourceText.SelectionBackColor = Color.White;
            this.sourceText.SelectionColor = Color.FromArgb(0xCACACA);

            this.sourceText.SelectionStart = 0;
            this.sourceText.SelectionLength = Math.Max(0, this.sourceText.Text.Length - 1);

            this.sourceText.SelectionBackColor = Color.White;
            this.sourceText.SelectionColor = Color.FromArgb(0xCACACA);

            this.sourceText.ScrollToCaret();
            
            if (currentResults == null || currentResults.Count == 0)
			{
				this.nextBtn.Enabled = false;
				this.prevBtn.Enabled = false;
			}
			else
			{
				int resultsCount = currentResults.Count;
				this.nextBtn.Enabled = (currentResultsIndex < resultsCount - 1);
				this.prevBtn.Enabled = (currentResultsIndex > 0);
			}

            if (currentResults != null && currentResults.Count > 0 && currentResultsIndex < currentResults.Count)
            {
                SourceValidationOccurance bvf = currentResults[currentResultsIndex];

                if (String.IsNullOrEmpty(bvf.Comment))
                {
                    this.commentText.Visible = false;
                    this.commentText.Text = "";
                }
                else
                {
                    this.commentText.Visible = true;
                    this.commentText.Text = bvf.Comment;
                }

                FixSize();

                this.helpBtn.Enabled = (String.IsNullOrEmpty(bvf.HelpURL) == false);

                int l = 0;
                int s = 0;
                int c = 0;

                try
                {
                    c = CountCaretReturs(bvf.SourceData.PageSource, bvf.StartIndex, bvf.Length);
                    l = bvf.Length - c;
                    s = bvf.StartIndex - CountCaretReturs(bvf.SourceData.PageSource, 0, bvf.StartIndex);

                    if (s < sourceText.Text.Length)
                    {
                        this.sourceText.SelectionStart = s;
                        this.sourceText.SelectionLength = Math.Max(0, Math.Min(this.sourceText.Text.Length - s, l));
                        this.sourceText.SelectionBackColor = Color.FromArgb(0xFFFFCC);
                        this.sourceText.SelectionColor = Color.FromArgb(0x000000);

                        if (c > 0)
                        {
                            this.sourceText.SelectionStart = s;
                        }
                        else
                        {
                            this.sourceText.SelectionStart = s + (Math.Min(sourceText.Text.Length - s, l));
                        }

                        this.sourceText.ScrollToCaret();

                        this.sourceText.SelectionLength = 0;
                    }
                }
                catch
                {

                }

            }                       
		}

        private void FixSize()
        {
            this.topPanel.Left = 0;
            this.topPanel.Top = 0;
            this.topPanel.Width = this.Width;

            this.toolStrip.Top = 0;
            this.toolStrip.Left = this.topPanel.Width - this.toolStrip.Width;

            this.commentText.Top = 0;
            this.commentText.Left = 0;

            using (System.Drawing.Graphics g = this.commentText.CreateGraphics())
            {
                int w = (int)g.MeasureString(commentText.Text, commentText.Font).Width + 10;
                this.commentText.Height = ((int)Math.Max(1, Math.Ceiling(((double)w) / ((double)this.toolStrip.Left))) * 15) + 10;
            }


            if (commentText.Visible)
            {
                this.topPanel.Height = Math.Max(this.toolStrip.Height,this.commentText.Height);
            }
            else
            {
                this.topPanel.Height = this.toolStrip.Height;
            }

            this.commentText.Left = 0;
            this.commentText.Width = this.toolStrip.Left;

            this.sourceText.Top = this.topPanel.Height;
            this.sourceText.Height = this.Height - this.topPanel.Height;
            this.sourceText.Left = 0;
            this.sourceText.Width = this.Width;
        }

		private int CountCaretReturs(String b, int from, int to)
		{
			return CountCaretReturn.Matches(b.Substring(from, to)).Count;
		}

    }
}
