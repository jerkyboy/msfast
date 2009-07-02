//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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

namespace MySpace.MSFast.Engine.BrowserWrapper
{

    /// <summary>
    /// Possible browser states
    /// </summary>
	public enum BrowserStatus
	{
		NotInitiated,
		Loading,
		Ready,
		Processing,
		Disposed,
		Error
	}

    /// <summary>
    /// Use this delegate when listening to the browser state
    /// </summary>
    /// <param name="browser"></param>
    /// <param name="state"></param>
	public delegate void BrowserStateChanged(Browser browser, BrowserStatus state);


    /// <summary>
    /// This class is a browser wrapper. all references to a browser 
    /// should be proxied thru this class.
    /// </summary>
	public abstract class Browser
	{
        /// <summary>
        /// Returns a HWND handle of the main canvas area of the browser
        /// </summary>
		public abstract IntPtr CanvasHWND { get; }

        /// <summary>
        /// Returns a HWND handle of the browser
        /// </summary>
        public abstract IntPtr MainHWND { get; }

        /// <summary>
        /// This event should be fired everytime the browser state changes
        /// </summary>
		public event BrowserStateChanged OnBrowserStateChanged;

        /// <summary>
        /// This property holds the current state of the browser
        /// </summary>
		private BrowserStatus state = BrowserStatus.NotInitiated;
        
        /// <summary>
        /// Current browser state.
        /// Should be set by inheriting class
        /// </summary>
        public BrowserStatus State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					if (this.OnBrowserStateChanged != null)
					{
						this.OnBrowserStateChanged(this, state);
					}
				}
			}
		}

        /// <summary>
        /// Get or Set the current browser URL
        /// </summary>
		public abstract String URL
		{
			get;
			set;
		}
		
        /// <summary>
        /// A callback delegate for GetBuffer
        /// </summary>
        /// <param name="buffer">The HTML buffer currently displayed in the browser</param>
		public delegate void GetBufferCallback(String buffer);

        /// <summary>
        /// This method retreived the HTML of the browser and pass it on to the gelegated function
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
		public abstract bool GetBuffer(GetBufferCallback callBack);

	}
}
