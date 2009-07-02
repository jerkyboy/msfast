using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MySpace.MSFast.GUI.Engine.Panels.Status
{
    public enum TaskProgressLabelStatus
    {
        ProgressChange,
        Completed,
        Pending
    }

    public class TaskProgressLabel : Panel
    {
        private Label label = null;
        private ProgressBar progressBar = null;

        public TaskProgressLabel(String label)
        {
            this.SuspendLayout();

            this.label = new Label();
            this.label.AutoSize = true;
            this.label.Image = Resources.Resources.bullet_p;
            this.label.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label.Name = "label";
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.TabIndex = 0;
            this.label.Text = "      " + label;

            this.progressBar = new ProgressBar();
            this.progressBar.Location = new System.Drawing.Point(this.label.Width, 1);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 10);
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;
            this.progressBar.Style = ProgressBarStyle.Continuous;

            this.Controls.Add(this.label);
            this.Controls.Add(this.progressBar);

            this.Height = this.label.Height;
            this.Width = 400;
            this.BackColor = System.Drawing.Color.Transparent;
            SetStatus(TaskProgressLabelStatus.Pending);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public void SetStatus(TaskProgressLabelStatus status)
        {
            SetStatus(status, -1, -1);
        }
        public void SetStatus(TaskProgressLabelStatus status, int progress, int total)
        {
            if (status == TaskProgressLabelStatus.Pending)
            {
                this.label.Image = Resources.Resources.bullet_p;
                this.label.ForeColor = Color.FromArgb(0xbfc0cb);
                this.progressBar.Visible = false;
            }
            else
            {
                this.label.ForeColor = Color.FromArgb(0x6e7088);

                if (status == TaskProgressLabelStatus.Completed)
                {
                    this.progressBar.Visible = false;
                    this.label.Image = Resources.Resources.bullet_v;
                }
                else
                {
                    this.label.Image = Resources.Resources.bullet_r;

                    if (total == -1)
                    {
                        this.progressBar.Visible = false;
                    }
                    else
                    {
                        this.progressBar.Location = new System.Drawing.Point(this.label.Width, 1);
                        this.progressBar.Visible = true;
                        this.progressBar.Minimum = 0;
                        this.progressBar.Maximum = total;
                        this.progressBar.Value = progress;
                    }
                }
            }
        }
    }
}
