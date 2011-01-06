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
using MySpace.MSFast.GUI.Engine.Panels.ValidationResults;
using MySpace.MSFast.DataProcessors.PageSource;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Render;

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView
{
    public partial class PageSourceViewForm : MSFastForm
    {
        private static Regex RemoveCaretReturn = new Regex("\r\n", RegexOptions.Compiled);
        private static Regex CountCaretReturn = new Regex("\n", RegexOptions.Compiled);

        private Dictionary<String,double> connectionSpeeds = new Dictionary<string, double>();

        private SourcePiece[] currentResults = null;

        private int currentResultsIndex = 0;
        private int lastPackageHashNumber = 0;
        private object setResultsLock = new Object();
        private long lastRenderTimestamp = 0;

        private ProcessedDataPackage package;
        
        public PageSourceViewForm() : base()
        {
            connectionSpeeds.Add("9 Kbps.", 1);
            connectionSpeeds.Add("14 Kbps.", 0.6428);
            connectionSpeeds.Add("28 Kbps.", 0.3214);
            connectionSpeeds.Add("56 Kbps.", 0.1607);
            connectionSpeeds.Add("128 Kbps.", 0.0703);
            connectionSpeeds.Add("256 Kbps.", 0.0351);
            connectionSpeeds.Add("1.5 Mbps.", 0.0090);

            InitializeComponent();

            String[] s = new String[connectionSpeeds.Count];
            int i = 0;
            foreach(String ss in connectionSpeeds.Keys){
                s[i] = ss;
                i++;
            }

            this.comboBox1.Items.AddRange(s);
            this.comboBox1.SelectedItem = "9 Kbps.";
            this.comboBox1.SelectedValueChanged += new EventHandler(comboBox1_SelectedValueChanged);
        }

        void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            RedrawData();
        }

        private void RedrawData()
        {
            if (this.package == null ||
                 this.package.ContainsKey(typeof(RenderData)) == false ||
                 this.package.ContainsKey(typeof(BrokenSourceData)) == false)
            {
                return;
            }

            RenderData renderData = (RenderData)this.package[typeof(RenderData)];

            if (renderData.ContainsKey(currentResultsIndex))
            {
                double t = (lastRenderTimestamp - this.package.CollectionStartTime);

                this.lblFirstByteToBeginingOfSection.Text = GetTime(renderData[currentResultsIndex].StartTime - package.CollectionStartTime);
                this.lblFirstByteToEndOfSection.Text = GetTime(renderData[currentResultsIndex].EndTime - package.CollectionStartTime);
                this.lblBeginingOfSectionToEndOfSection.Text = GetTime(renderData[currentResultsIndex].EndTime - renderData[currentResultsIndex].StartTime);

                this.lblPercentsOfPageRenderTime.Text = GetPercents(renderData[currentResultsIndex].EndTime - renderData[currentResultsIndex].StartTime, t);
                this.lblPageRenderPercentsUntilBegining.Text = GetPercents(renderData[currentResultsIndex].StartTime - this.package.CollectionStartTime, t);
                this.lblPageRenderPercentsUntilEnd.Text = GetPercents(renderData[currentResultsIndex].EndTime - this.package.CollectionStartTime, t);

            }
            else
            {
                this.lblPercentsOfPageRenderTime.Text = "N/A";
                this.lblFirstByteToBeginingOfSection.Text = "N/A";
                this.lblFirstByteToEndOfSection.Text = "N/A";
                this.lblBeginingOfSectionToEndOfSection.Text = "N/A";
                this.lblPageRenderPercentsUntilBegining.Text = "N/A";
                this.lblPageRenderPercentsUntilEnd.Text = "N/A";
            }
        }
        
        internal void SetResults(ProcessedDataPackage package, int index)
        {
            lastRenderTimestamp = 0;
            if (package == null){
              //  Clear();
                return;
            }
            
            lock (setResultsLock)
            {
                this.currentResultsIndex = index;

                if (lastPackageHashNumber != package.GetHashCode())
                {
                    this.currentResults = null;
                    this.package = package;
                    this.lastPackageHashNumber = package.GetHashCode();

                    if(     package.ContainsKey(typeof(BrokenSourceData)) != false &&
                            String.IsNullOrEmpty(((BrokenSourceData)package[typeof(BrokenSourceData)]).PageSource) == false &&
                            ((BrokenSourceData)package[typeof(BrokenSourceData)]).InjectionBreaks != null){

                        BrokenSourceData brokenSourceData = ((BrokenSourceData)package[typeof(BrokenSourceData)]);

                        SourcePiece[] sp = new SourcePiece[brokenSourceData.InjectionBreaks.Count];
                        int i = 0;
                        foreach (SourcePiece s in brokenSourceData.InjectionBreaks)
                        {
                            sp[i] = s;
                            i++;
                        }
                        this.currentResults = sp;
                        SetBuffer(brokenSourceData.PageSource);
                    }
                    if(package.ContainsKey(typeof(RenderData)) != false){
                        RenderData rd = (RenderData)package[typeof(RenderData)];
                        foreach(RenderedSegment rs in rd.Values){
                            lastRenderTimestamp = Math.Max(lastRenderTimestamp, rs.EndTime);
                        }
                    }
                }
                
                RedrawResultsNextPrev();
            }
        }


        private void NextOccurance()
        {
            if (currentResults == null || currentResults.Length == 0)
                return;

            if (currentResultsIndex < currentResults.Length - 1)
            {
                currentResultsIndex++;
            }
            RedrawResultsNextPrev();
        }

        private void PreviousOccurance()
        {
            if (currentResults == null || currentResults.Length == 0)
                return;

            if (currentResultsIndex > 0)
            {
                currentResultsIndex--;
            }
            RedrawResultsNextPrev();
        }

        private void SetBuffer(String buffer)
        {
            lock (setResultsLock)
            {
                if (String.IsNullOrEmpty(buffer))
                    this.sourceTextBox.Text = "";
                else
                    this.sourceTextBox.Text = buffer;
            }
        }

        private String AppendZero(int id)
        {
            String s = id.ToString();
            while (s.Length < 5)
                s = "0" + s;
            return s;
        }

        private void SetThumbnailsAndData()
        {
            this.picAfter.ImageLocation = null;
            this.picBefore.ImageLocation = null;

            if (this.package == null || 
                this.package.ContainsKey(typeof(RenderData)) == false ||
                this.package.ContainsKey(typeof(BrokenSourceData)) == false)
            {
                return;
            }
            
            RedrawData();
            
            #region Thumbnails

            if (this.package.CollectionID != -1 && String.IsNullOrEmpty(this.package.ThumbnailsRoot) == false)
            {
                String thumbnailRoot = this.package.ThumbnailsRoot;
                if (thumbnailRoot.EndsWith("/") == false)
                    thumbnailRoot += "/";

                try
                {
                    this.picBefore.ImageLocation = thumbnailRoot + "TC_" + this.package.CollectionID + "_" + AppendZero(Math.Max(0,currentResultsIndex-1)) + ".jpg";
                    this.picAfter.ImageLocation = thumbnailRoot + "TC_" + this.package.CollectionID + "_" + AppendZero(((currentResultsIndex >= currentResults.Length - 1) ? currentResultsIndex - 1 : currentResultsIndex)) + ".jpg";
                }
                catch
                {
                }
            }
            #endregion
        }
        private String GetTime(long t)
        {
            double tt = t;
            if (this.connectionSpeeds.ContainsKey((String)this.comboBox1.SelectedItem))
                tt *= this.connectionSpeeds[(String)this.comboBox1.SelectedItem];
            if(((int)tt)>0){
                return String.Format("{0:0,0} ms.", Math.Max(0,tt));
            }else{
                return "~0 ms.";
            }
        }
        private String GetPercents(double r, double t)
        {
            if ((r / t) * 100 > 0)
            {
                return String.Format("{0:0.00}%", ((r / t) * 100));
            }
            else
            {
                return "~0%";
            }
        }

        private void RedrawResultsNextPrev()
        {
            this.sourceTextBox.SelectionBackColor = Color.White;
            this.sourceTextBox.SelectionColor = Color.FromArgb(0xCACACA);

            this.sourceTextBox.SelectionStart = 0;
            this.sourceTextBox.SelectionLength = Math.Max(0,this.sourceTextBox.Text.Length-1);
            
            this.sourceTextBox.SelectionBackColor = Color.White;
            this.sourceTextBox.SelectionColor = Color.FromArgb(0xCACACA);
            
            this.sourceTextBox.ScrollToCaret();

            if (currentResults == null || currentResults.Length == 0)
            {
                this.nextBtn.Enabled = false;
                this.prevBtn.Enabled = false;
            }
            else
            {
                this.nextBtn.Enabled = (currentResultsIndex < currentResults.Length - 1);
                this.prevBtn.Enabled = (currentResultsIndex > 0);
            }
            
            if (currentResults != null)
            {
                if (currentResultsIndex < currentResults.Length)
                {
                    SourcePiece bvf = currentResults[currentResultsIndex];
                    int l = 0;
                    int s = 0;
                    int c = 0;
                    try
                    {
                        c = CountCaretReturs(bvf.SourceData.PageSource, bvf.StartIndex, bvf.Length);
                        l = bvf.Length - c;
                        s = bvf.StartIndex - CountCaretReturs(bvf.SourceData.PageSource, 0, bvf.StartIndex);

                        if (s < sourceTextBox.Text.Length)
                        {
                            this.sourceTextBox.SelectionStart = s;
                            this.sourceTextBox.SelectionLength = Math.Max(0,Math.Min(sourceTextBox.Text.Length - s, l));
                            this.sourceTextBox.SelectionBackColor = Color.FromArgb(0xFFFFCC);
                            this.sourceTextBox.SelectionColor = Color.FromArgb(0x000000);

                            if (c > 0)
                            {
                                this.sourceTextBox.SelectionStart = s;
                            }
                            else
                            {
                                this.sourceTextBox.SelectionStart = s + (Math.Min(sourceTextBox.Text.Length - s, l));
                            }
                            
                            this.sourceTextBox.ScrollToCaret();

                            this.sourceTextBox.SelectionLength = 0;
                        }
                    }
                    catch 
                    {
                    
                    }
                }
            }
            SetThumbnailsAndData();
        }

        private int CountCaretReturs(String b, int from, int to)
        {
            return CountCaretReturn.Matches(b.Substring(from, Math.Min(b.Length-from,to))).Count;
        }
    }
}

               
                
