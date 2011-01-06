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
using MySpace.MSFast.GUI.Engine.Panels.GraphView.DelayCOM;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Drawing;

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView.Controls
{
	public class FlashPlayer : Panel, ShockwavePlayer
	{
		private AxFlashPlayer flashPlayer = null;

		public bool IsAvailable()
		{
			return (flashPlayer != null);
		}

		public FlashPlayer()
        {            
            if (LoadFlash() == false)
			{
				DisplayNoFlashWarning();
			}
		}

		private void DisplayNoFlashWarning()
		{			
			Label label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = System.Drawing.Color.White;
			label1.Dock = System.Windows.Forms.DockStyle.Fill;
			label1.ForeColor = System.Drawing.Color.DarkGray;
			label1.Location = new System.Drawing.Point(20, 20);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 13);
			label1.TabIndex = 1;
			label1.Text = "This feature require Adobe Flash Player 9 and up.\r\n Please install the latest version of \"Adobe Flash Player\"";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			// 
			// Form2
			// 
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Controls.Add(label1);			
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private bool LoadFlash()
		{
			try
			{
				this.SuspendLayout();

				flashPlayer = new AxFlashPlayer();
                flashPlayer.Dock = System.Windows.Forms.DockStyle.Fill;

				this.Controls.Add(flashPlayer);

				((System.ComponentModel.ISupportInitialize)(flashPlayer)).BeginInit();
				((System.ComponentModel.ISupportInitialize)(flashPlayer)).EndInit();

				this.ResumeLayout(false);
				this.PerformLayout();
			}
			catch
			{
				this.Controls.Clear();
				flashPlayer = null;
				return false;
			}
			return true;
		}
		public void Play()
		{
			if (this.flashPlayer != null)
				this.flashPlayer.Play();
		}
		public void LoadMovie(int level, String location)
		{
			if (this.flashPlayer != null)
				this.flashPlayer.LoadMovie(level, location);
		}
		public void SetVariable(String name, String value)
		{
			if (this.flashPlayer != null)
				this.flashPlayer.SetVariable(name, value);
		}

		public String FlashVars
		{
			get
			{
				if (this.flashPlayer != null)
					return this.flashPlayer.FlashVars;
				return "";
			}
			set
			{
				if (this.flashPlayer != null)
					this.flashPlayer.FlashVars = value;
			}
		}
		public String AllowScriptAccess
		{
			get
			{
				if (this.flashPlayer != null)
					return this.flashPlayer.AllowScriptAccess;
				return "";
			}
			set
			{
				if (this.flashPlayer != null)
					this.flashPlayer.AllowScriptAccess = value;
			}
		}		


		public event FlashCallEventHandler FlashCall
		{
			add
			{
				if (this.flashPlayer != null)
					this.flashPlayer.FlashCall += value;
			}
			remove
			{
				if (this.flashPlayer != null)
					this.flashPlayer.FlashCall -= value;
			}
		}

        public int FlashVersion()
        {
           if (this.flashPlayer != null)
               return this.flashPlayer.Version;
           return -1;
        }
    }

    [System.ComponentModel.DesignTimeVisibleAttribute(true)]
    public class AxFlashPlayer : AxHost, ShockwavePlayer
    {
        private ShockwavePlayer player = null;

        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_KEYDOWN = 0x100;

        public AxFlashPlayer()
            : base("D27CDB6E-AE6D-11cf-96B8-444553540000")//D27CDB6E-AE6D-11cf-96B8-444553540000
        {
        }

        protected override void AttachInterfaces()
        {
            try
            {

                this.player = (ShockwavePlayer)COMWrapper.Wrap(this.GetOcx(), typeof(ShockwavePlayer));
            }
            catch (System.Exception)
            {
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_RBUTTONDOWN ||
                (m.Msg == WM_KEYDOWN && 93 == (int)m.WParam))
                return;

            base.WndProc(ref m);
        }

        public int Version
        {
            get
            {
                try
                {
                    if (this.player != null)
                        return this.player.FlashVersion();
                }
                catch
                {
                }
                 return -1;
                
            }
        }

        public void Play()
        {
            try
            {
                if (this.player != null)
                    this.player.Play();
            }
            catch
            {
            }
        }

        public void LoadMovie(int level, String location)
        {
            try
            {
                if (this.player != null)
                    this.player.LoadMovie(level, location);
            }
            catch
            {
            }
        }

        public void SetVariable(String name, String value)
        {
            try
            {
                if (this.player != null)
                    this.player.SetVariable(name, value);
            }
            catch
            {
            }
        }

        public String FlashVars
        {
            get
            {
                try
                {
                    if (this.player != null)
                        return this.player.FlashVars;
                }
                catch
                {
                }
                return "";
            }
            set
            {
                try
                {
                    if (this.player != null)
                        this.player.FlashVars = value;
                }
                catch
                {
                }
            }
        }
        public String AllowScriptAccess
        {
            get
            {
                try
                {
                    if (this.player != null)
                        return this.player.AllowScriptAccess;
                }
                catch
                {
                }
                return "";
            }
            set
            {
                try
                {
                    if (this.player != null)
                        this.player.AllowScriptAccess = value;
                }
                catch
                {
                }
            }
        }

        public event FlashCallEventHandler FlashCall
        {
            add
            {
                try
                {
                    if (this.player != null)
                        this.player.FlashCall += value;
                }
                catch
                {
                }
            }
            remove
            {
                try
                {
                    if (this.player != null)
                        this.player.FlashCall -= value;
                }
                catch
                {
                }
            }
        }

        #region ShockwavePlayer Members


        public int FlashVersion()
        {
            try
            {
                if (this.player != null)
                    return this.player.FlashVersion();
            }
            catch { 
            }
            return -1;
        }

        #endregion
    }
	
	public class FlashCallEvent : EventArgs
	{
		private static Regex rx = new Regex("<invoke name=\"(?<name>.*?)\" returntype=\"xml\"><arguments><string>(?<args>.*?)</string></arguments></invoke>", RegexOptions.Compiled);
		public string request;
		public string name;
		public string args;

		public FlashCallEvent(String re) { 
			this.request = re;

			try
			{
				Match m = rx.Match(re);
				if (m.Success)
				{
					this.name = m.Groups["name"].Value;
					this.args = m.Groups["args"].Value;
				}
			}
			catch
			{
			}
		}
	}

	public delegate void FlashCallEventHandler(object sender, FlashCallEvent e);


	[ComProgId("ShockwaveFlash.ShockwaveFlash.1")]
	public interface ShockwavePlayer : IDisposable, ShockwavePlayerEvents
	{
		void Play();
		void LoadMovie(int level, String location);
		void SetVariable(String name, String value);
		String FlashVars { get; set; }
		String AllowScriptAccess { get; set; }
        int FlashVersion();
    }


	[ComEvents(typeof(ShockwavePlayerEventsSink), ShockwavePlayerEventsSink.InterfaceID)]
	public interface ShockwavePlayerEvents
	{
		event FlashCallEventHandler FlashCall;
	}


	[ClassInterface(ClassInterfaceType.None)]
	public sealed class ShockwavePlayerEventsSink : ComEventSink, UCOMIShockwavePlayerEvents
	{															   
		internal const string InterfaceID = "D27CDB6D-AE6D-11cf-96B8-444553540000";
		static readonly object FlashCallEvent = new object();
		public void FlashCall(String call)
		{
			RaiseEvent(FlashCallEvent, this.Sender, new FlashCallEvent(call));
		}
	}


	[
	ComImport,
	Guid(ShockwavePlayerEventsSink.InterfaceID),
	InterfaceType(ComInterfaceType.InterfaceIsIDispatch),
	TypeLibType(TypeLibTypeFlags.FDispatchable)
	]
	public interface UCOMIShockwavePlayerEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(197)]
		void FlashCall(string flcall);
	}




}
