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
using System.Drawing;

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView
{
    public partial class PageSourceViewForm
    {
        private void InitializeComponent()
        {
            this.sourceTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPercentsOfPageRenderTime = new System.Windows.Forms.Label();
            this.lblPageRenderPercentsUntilBegining = new System.Windows.Forms.Label();
            this.lblPageRenderPercentsUntilEnd = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFirstByteToBeginingOfSection = new System.Windows.Forms.Label();
            this.lblFirstByteToEndOfSection = new System.Windows.Forms.Label();
            this.lblBeginingOfSectionToEndOfSection = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picAfter = new System.Windows.Forms.PictureBox();
            this.picBefore = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.nextBtn = new System.Windows.Forms.ToolStripButton();
            this.prevBtn = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBefore)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.BackColor = System.Drawing.Color.White;
            this.sourceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceTextBox.Location = new System.Drawing.Point(0, 229);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.ReadOnly = true;
            this.sourceTextBox.Size = new System.Drawing.Size(921, 350);
            this.sourceTextBox.TabIndex = 1;
            this.sourceTextBox.Text = "";
            this.sourceTextBox.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 229);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::MySpace.MSFast.GUI.Engine.Resources.Resources.bottomBackground;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.picAfter);
            this.panel2.Controls.Add(this.picBefore);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 204);
            this.panel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lblPercentsOfPageRenderTime);
            this.groupBox2.Controls.Add(this.lblPageRenderPercentsUntilBegining);
            this.groupBox2.Controls.Add(this.lblPageRenderPercentsUntilEnd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(596, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 74);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Estimated percent of entire render time";
            // 
            // lblPercentsOfPageRenderTime
            // 
            this.lblPercentsOfPageRenderTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPercentsOfPageRenderTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercentsOfPageRenderTime.Location = new System.Drawing.Point(246, 16);
            this.lblPercentsOfPageRenderTime.Name = "lblPercentsOfPageRenderTime";
            this.lblPercentsOfPageRenderTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPercentsOfPageRenderTime.Size = new System.Drawing.Size(61, 18);
            this.lblPercentsOfPageRenderTime.TabIndex = 15;
            this.lblPercentsOfPageRenderTime.Text = "12350 sec.";
            this.lblPercentsOfPageRenderTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageRenderPercentsUntilBegining
            // 
            this.lblPageRenderPercentsUntilBegining.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPageRenderPercentsUntilBegining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageRenderPercentsUntilBegining.Location = new System.Drawing.Point(246, 33);
            this.lblPageRenderPercentsUntilBegining.Name = "lblPageRenderPercentsUntilBegining";
            this.lblPageRenderPercentsUntilBegining.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPageRenderPercentsUntilBegining.Size = new System.Drawing.Size(61, 18);
            this.lblPageRenderPercentsUntilBegining.TabIndex = 14;
            this.lblPageRenderPercentsUntilBegining.Text = "12350 sec.";
            this.lblPageRenderPercentsUntilBegining.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageRenderPercentsUntilEnd
            // 
            this.lblPageRenderPercentsUntilEnd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPageRenderPercentsUntilEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageRenderPercentsUntilEnd.Location = new System.Drawing.Point(246, 50);
            this.lblPageRenderPercentsUntilEnd.Name = "lblPageRenderPercentsUntilEnd";
            this.lblPageRenderPercentsUntilEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPageRenderPercentsUntilEnd.Size = new System.Drawing.Size(61, 18);
            this.lblPageRenderPercentsUntilEnd.TabIndex = 13;
            this.lblPageRenderPercentsUntilEnd.Text = "12350 sec.";
            this.lblPageRenderPercentsUntilEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(196, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Page render percent until end of section";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(218, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Page render percent until begining of section";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Percent of page render time";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblFirstByteToBeginingOfSection);
            this.groupBox1.Controls.Add(this.lblFirstByteToEndOfSection);
            this.groupBox1.Controls.Add(this.lblBeginingOfSectionToEndOfSection);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(596, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 104);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Estimated Render Times";
            // 
            // lblFirstByteToBeginingOfSection
            // 
            this.lblFirstByteToBeginingOfSection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFirstByteToBeginingOfSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstByteToBeginingOfSection.Location = new System.Drawing.Point(222, 45);
            this.lblFirstByteToBeginingOfSection.Name = "lblFirstByteToBeginingOfSection";
            this.lblFirstByteToBeginingOfSection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFirstByteToBeginingOfSection.Size = new System.Drawing.Size(85, 18);
            this.lblFirstByteToBeginingOfSection.TabIndex = 12;
            this.lblFirstByteToBeginingOfSection.Text = "12350 sec.";
            this.lblFirstByteToBeginingOfSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFirstByteToEndOfSection
            // 
            this.lblFirstByteToEndOfSection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFirstByteToEndOfSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstByteToEndOfSection.Location = new System.Drawing.Point(222, 62);
            this.lblFirstByteToEndOfSection.Name = "lblFirstByteToEndOfSection";
            this.lblFirstByteToEndOfSection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFirstByteToEndOfSection.Size = new System.Drawing.Size(85, 18);
            this.lblFirstByteToEndOfSection.TabIndex = 11;
            this.lblFirstByteToEndOfSection.Text = "12350 sec.";
            this.lblFirstByteToEndOfSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBeginingOfSectionToEndOfSection
            // 
            this.lblBeginingOfSectionToEndOfSection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBeginingOfSectionToEndOfSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeginingOfSectionToEndOfSection.Location = new System.Drawing.Point(222, 79);
            this.lblBeginingOfSectionToEndOfSection.Name = "lblBeginingOfSectionToEndOfSection";
            this.lblBeginingOfSectionToEndOfSection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBeginingOfSectionToEndOfSection.Size = new System.Drawing.Size(85, 18);
            this.lblBeginingOfSectionToEndOfSection.TabIndex = 10;
            this.lblBeginingOfSectionToEndOfSection.Text = "12350 sec.";
            this.lblBeginingOfSectionToEndOfSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Begining of section to end of section";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "First byte to end of section";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "First byte to begining of section";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(188, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(119, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Connection Speed";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::MySpace.MSFast.GUI.Engine.Resources.Resources.aboutPrecent;
            this.pictureBox2.Location = new System.Drawing.Point(553, 59);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = global::MySpace.MSFast.GUI.Engine.Resources.Resources.aboutPrecent.Size;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MySpace.MSFast.GUI.Engine.Resources.Resources.arrow;
            this.pictureBox1.Location = new System.Drawing.Point(267, 83);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = global::MySpace.MSFast.GUI.Engine.Resources.Resources.arrow.Size;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // picAfter
            // 
            this.picAfter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAfter.InitialImage = global::MySpace.MSFast.GUI.Engine.Resources.Resources.noThumbnail;
            this.picAfter.ErrorImage = global::MySpace.MSFast.GUI.Engine.Resources.Resources.noThumbnail;
            this.picAfter.Location = new System.Drawing.Point(298, 12);
            this.picAfter.Name = "picAfter";
            this.picAfter.Size = new System.Drawing.Size(249, 183);
            this.picAfter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAfter.TabIndex = 1;
            this.picAfter.TabStop = false;
            // 
            // picBefore
            // 
            this.picBefore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBefore.InitialImage = global::MySpace.MSFast.GUI.Engine.Resources.Resources.noThumbnail;
            this.picBefore.ErrorImage = global::MySpace.MSFast.GUI.Engine.Resources.Resources.noThumbnail;
            this.picBefore.Location = new System.Drawing.Point(12, 12);
            this.picBefore.Name = "picBefore";
            this.picBefore.Size = new System.Drawing.Size(249, 183);
            this.picBefore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBefore.TabIndex = 0;
            this.picBefore.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextBtn,
            this.prevBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 204);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(921, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // nextBtn
            // 
            this.nextBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.nextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextBtn.Image = global::MySpace.MSFast.GUI.Engine.Resources.Resources.next;
            this.nextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(23, 22);
            this.nextBtn.Text = "Next Occurance";
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // prevBtn
            // 
            this.prevBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.prevBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.prevBtn.Image = global::MySpace.MSFast.GUI.Engine.Resources.Resources.prev;
            this.prevBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(23, 22);
            this.prevBtn.Text = "Previous Occurance";
            this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // PageSourceViewForm
            // 
            this.ClientSize = new System.Drawing.Size(921, 579);
            this.Controls.Add(this.sourceTextBox);
            this.Controls.Add(this.panel1);
            this.Name = "PageSourceViewForm";
            this.Text = "Page Source";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBefore)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        private RichTextBox sourceTextBox;
        
        void nextBtn_Click(object sender, EventArgs e)
        {
            NextOccurance();
        }

        void prevBtn_Click(object sender, EventArgs e)
        {
            PreviousOccurance();
        }

        private Panel panel1;
        private ToolStrip toolStrip1;
        private ToolStripButton nextBtn;
        private ToolStripButton prevBtn;
        private Panel panel2;
        private PictureBox picBefore;
        private PictureBox picAfter;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label4;
        private Label label3;
        private ComboBox comboBox1;
        private Label label2;
        private Label label5;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label lblBeginingOfSectionToEndOfSection;
        private Label lblFirstByteToEndOfSection;
        private Label lblFirstByteToBeginingOfSection;
        private Label lblPercentsOfPageRenderTime;
        private Label lblPageRenderPercentsUntilBegining;
        private Label lblPageRenderPercentsUntilEnd;
    }
}
