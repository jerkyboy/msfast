using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.GUI.Engine.Panels.ValidationResults;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace MySpace.MSFast.GUI.Engine.Panels.Status
{
    class DownloadProgress : ObjectListView
    {
        private List<DownloadedFile> results = null;
        private object resultsLock = new object();

        private OLVColumn ls_content_column_url;
        private OLVColumn ls_content_column_sent;
        private OLVColumn ls_content_column_received;

        public DownloadProgress()
        {
            this.ls_content_column_url = new OLVColumn();
            this.ls_content_column_url.AspectName = "URL";
            this.ls_content_column_url.Text = "URL";
            this.ls_content_column_url.Width = 420;

            this.ls_content_column_sent = new OLVColumn();
            this.ls_content_column_sent.AspectName = "Out";
            this.ls_content_column_sent.Text = "Sent";
            this.ls_content_column_sent.Width = 60;

            this.ls_content_column_received = new OLVColumn();
            this.ls_content_column_received.AspectName = "In";
            this.ls_content_column_received.Text = "Received";
            this.ls_content_column_received.Width = 60;

            this.AllColumns.Add(this.ls_content_column_url);
            this.AllColumns.Add(this.ls_content_column_sent);
            this.AllColumns.Add(this.ls_content_column_received);

            this.Columns.AddRange(new ColumnHeader[] { ls_content_column_url, ls_content_column_sent, ls_content_column_received });

            this.EmptyListMsg = "";
            this.FullRowSelect = false;
            this.MultiSelect = false;
            this.RowHeight = 10;
            this.ShowGroups = false;
            ImageList il = new ImageList();
            il.ImageSize = new Size(1, this.RowHeight);
            this.SmallImageList = il;

            Reset();
        }

        public void Reset()
        {
            lock (this.resultsLock)
            {
                if (this.results != null)
                    this.results.Clear();

                this.results = new List<DownloadedFile>();
                this.SetObjects(results);
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

                base.RefreshObject(df);

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

                base.RefreshObject(df);

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

                base.RefreshObject(df);
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
                
                base.RefreshObject(df);
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

                this.results.Add(d);

                UpdateProgress();
                
                return d;
            }
        }


        private void UpdateProgress()
        {
            this.BeginUpdate();
            this.SetObjects(results);
            this.EndUpdate();
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
