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
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Engine;

#if WIN32
using MySpace.MSFast.SysImpl.Win32.Utils;
using MySpace.MSFast.SysImpl.Win32;
#endif

namespace MySpace.MSFast.GUI.Engine.DataCollector
{
	public class AsyncBufferPageDataCollector
	{
		public delegate void StartingTestEventHandler(AsyncBufferPageDataCollector sender);
		public delegate void TestEndedEventHandler(AsyncBufferPageDataCollector sender,PageDataCollectorStartInfo settings, bool success, PageDataCollectorErrors errCode, int resultid);

        public event OnTestEventHandler OnTestEvent;
		public event StartingTestEventHandler OnStartingTest;
		public event TestEndedEventHandler OnTestEnded;

		private bool started = false;
		private object startedLock = new object();

#if WIN32
		private static int WM_TEST_ENDED = -1;
#endif

        static AsyncBufferPageDataCollector()
		{
#if WIN32
			if (WM_TEST_ENDED == -1)
			{
				WM_TEST_ENDED = Win32API.RegisterWindowMessage("WM_TEST_ENDED");
			}
#endif
        }

		public void AbortTest()
		{
#if WIN32
            Win32API.PostMessage(Win32API.HWND_BROADCAST, WM_TEST_ENDED, 0, 0);
#endif
		}

		public void StartTest(PageDataCollectorStartInfo settings)
		{
			if (OnStartingTest != null)
				OnStartingTest(this);

			if (settings == null || settings.IsValid() == false)
			{
				if (OnTestEnded != null)
                    OnTestEnded(this, settings, false, PageDataCollectorErrors.InvalidOrMissingArguments, -1);
				
				return;
			}

			lock (startedLock)
			{
				if (started)
				{
					if (OnTestEnded != null)
                        OnTestEnded(this, settings, false, PageDataCollectorErrors.ObjectDisposed, -1);

					return;
				}
				started = true;
			}

			new Thread(delegate() { this.StartTestThread(settings); }).Start();

		}

		private void StartTestThread(PageDataCollectorStartInfo settings)
		{
			PageDataCollector pd = new PageDataCollector();
            pd.OnTestProgress += new OnTestEventHandler(pd_OnTestProgress);
            int res = pd.StartTest(settings);

			if (OnTestEnded != null)
                OnTestEnded(this, settings, (res == 0), (PageDataCollectorErrors)Enum.ToObject(typeof(PageDataCollectorErrors), res), (res == 0) ? settings.CollectionId : -1);
		}

        void pd_OnTestProgress(TestEventType progressEventType, int progress, int total, string url)
        {
            if (this.OnTestEvent != null)
                OnTestEvent(progressEventType, progress, total, url);
        }

		public void Dispose()
		{
            this.OnTestEvent = null;
			this.OnStartingTest = null;
			this.OnTestEnded = null;
		}
	}
}
