using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.GUI.Engine.Panels.ValidationResults;
using System.Windows.Forms;
using System.Drawing;

namespace MySpace.MSFast.GUI.Engine.Panels.Status
{
    public class DownloadingFilesList : StatusPanelBase
    {
        private bool isRemoved = false;
        private LinkedList<DownloadedFile> results = new LinkedList<DownloadedFile>();
        private object resultsLock = new object();

        private TextBox txtTraceout = null;

        public DownloadingFilesList()
        {
            txtTraceout = new TextBox();

            this.SuspendLayout();
        
            txtTraceout.SuspendLayout();

            txtTraceout.Dock = DockStyle.Fill;
            txtTraceout.BackColor = Color.White;
            txtTraceout.ForeColor = Color.FromArgb(0xbfc0cb);
            txtTraceout.Text = "";
            txtTraceout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtTraceout.Multiline = true;
            txtTraceout.Name = "textBox";
            txtTraceout.ReadOnly = true;
            txtTraceout.TabIndex = 0;
            txtTraceout.WordWrap = false;

            this.Controls.Add(txtTraceout);
            this.Padding = new Padding(55,10,10,10);

            txtTraceout.ResumeLayout(true);
       
            this.ResumeLayout(true);

            Reset();
        }

        public void Reset()
        {
            lock (this.resultsLock)
            {
                if (this.results != null)
                    this.results.Clear();
                
                isRemoved = false;
                txtTraceout.Text = "";
            }
        }

        public void OnRequestingFile(String url)
        {
            lock(this.resultsLock)
            {
                DownloadedFile df = GetDownloadedFile(url);
                
                if (df == null)
                    return;

                df.Done = false;

                Redraw();
            }
        }

        public void OnSendingData(String url, int length)
        {
            if (length <= 0)
                return;

            lock (this.resultsLock)
            {
                DownloadedFile df = GetDownloadedFile(url);

                if (df == null)
                    return;

                df.Out += length;

                Redraw();

            }
        }
        
        public void OnReceivingData(String url, int length)
        {
            if (length <= 0)
                return;

            lock (this.resultsLock)
            {
                DownloadedFile df = GetDownloadedFile(url);

                if (df == null)
                    return;

                df.In += length;

                Redraw();
            }
        }

        public void OnFileReceived(String url)
        {
            lock (this.resultsLock)
            {
                DownloadedFile df = GetDownloadedFile(url);

                if (df == null)
                    return;

                df.Done = true;

                if (this.results.Count > 6)
                {
                    LinkedList<DownloadedFile> dff = new LinkedList<DownloadedFile>(this.results);
                    foreach (DownloadedFile dd in dff)
                    {
                        if (this.results.Count <= 6)
                            break;

                        if (dd.Done)
                        {
                            isRemoved = true;
                            this.results.Remove(dd);
                        }
                    }
                }
                Redraw();
            }
        }

        private DownloadedFile GetDownloadedFile(string url)
        {
            lock (this.resultsLock)
            {
                if (this.results == null)
                    return null;

                foreach (DownloadedFile df in this.results)
                {
                    if (df.URL == url)
                    {
                        return df;
                    }
                }

                DownloadedFile d = new DownloadedFile();
                d.URL = url;

                this.results.AddLast(d);

                return d;
            }
        }

        private void Redraw()
        {
            this.txtTraceout.Text = (isRemoved) ? "...\r\n" : "";

            lock (this.resultsLock)
            {
                foreach (DownloadedFile d in this.results)
                {
                    if (d.Done == false)
                    {
                        this.txtTraceout.AppendText(String.Format("[{0} in / {1} out] {2}\r\n", d.In, d.Out, d.URL));
                    }
                    else {
                        this.txtTraceout.AppendText(String.Format("[Done] {0}\r\n", d.URL));
                    }
                }
            }


        }      

        private class DownloadedFile 
        {
            public String URL;
            public int Out = 0;
            public int In = 0;
            public bool Done = false;
        }
    }
}
