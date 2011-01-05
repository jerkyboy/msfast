//=======================================================================
/* Project: MSFast (MySpace.MSFast.SysImpl.Win32)
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
using System.Diagnostics;
using System.IO;
using System.Threading;
using MySpace.MSFast.Core.Logger;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.SysImpl.Win32.Utils
{
	public class PerformanceTracker
	{
        private static readonly MySpace.MSFast.Core.Logger.MSFastLogger log = MSFastLogger.GetLogger(typeof(PerformanceTracker));

        private Thread TrackingThread = null;
        private bool isRunning = true;
        private object trackingLock = new Object();

        public PerformanceTracker(int pid)
		{
            if (log.IsDebugEnabled) log.Debug("Starting performance tracker for PID(" + pid + ")");
			try
			{
				String instanceName = GetProcessInstanceName(pid);
				this.perfCtrProcessorTime = GetProcessorTimePerfCounter(instanceName);
				this.perfCtrUserTime = GetUserTimePerfCounter(instanceName);
				this.perfCtrWorkingSet = GetWorkingSetPerfCounter(instanceName);
				this.perfCtrWorkingSetPrivate = GetWorkingSetPrivatePerfCounter(instanceName);
			}
			catch
			{
			}

			this.allPerfChunks = new LinkedList<PerfCuhnk>();
		}

		public void StartTracking()
        {
            if (log.IsDebugEnabled) log.Debug("Starting tracking performance...");

            lock (trackingLock)
            {

                if (TrackingThread != null)
                {
                    if (log.IsDebugEnabled)
                        log.Debug("performance tracker already running...");
                    return;
                }

                TrackingThread = new Thread(this.TimerThread);
                TrackingThread.Start();
            }
            if (log.IsDebugEnabled) log.Debug("Performance tracking Started!");
		}

		public void StopTracking(Stream saveTo)
		{
            if (log.IsDebugEnabled) log.Debug("Stop tracking performance...");

			LinkedList<PerfCuhnk> pc = null;
            
            isRunning = false;

            if (TrackingThread == null)
			{
                if (log.IsDebugEnabled) log.Debug("performance tracker never started...");
                return;
			}

            if (saveTo != null)
			{
                lock (trackingLock)
                {
                    pc = new LinkedList<PerfCuhnk>(this.allPerfChunks);
                    this.allPerfChunks.Clear();
                }
			}

            
            if (pc != null && log.IsDebugEnabled) log.Debug("Collected " + pc.Count + " Performance Segments");
            
            if (saveTo != null && pc != null && pc.Count > 0)
			{
                TextWriter streamWriter = new StreamWriter(saveTo);
				
                foreach (PerfCuhnk chunk in pc)
				{
					streamWriter.WriteLine("?" + Converter.DateTimeToEpoch(chunk.MarkingDateTime) + ":PT" + Math.Min(100,chunk.ProcessorTime) + ":UT" + Math.Min(100,chunk.UserTime) + ":WS" + chunk.WorkingSet + ":PWS" + chunk.WorkingSetPrivate);
				}
                streamWriter.Flush();
				streamWriter.Close();
			}
            if (log.IsDebugEnabled) log.Debug("Performance tracking ended!");

		}

		#region Performance Monitor

		private LinkedList<PerfCuhnk> allPerfChunks = null;

		private PerformanceCounter perfCtrProcessorTime = null;
		private PerformanceCounter perfCtrUserTime = null;
		private PerformanceCounter perfCtrWorkingSet = null;
		private PerformanceCounter perfCtrWorkingSetPrivate = null;


		public void Dispose()
		{
			StopTracking(null);
		}

        private void TimerThread()
        {
            while (isRunning)
            {
                Thread.Sleep(100);


                if (isRunning == false) return;
                lock (trackingLock)
                {
                    if (isRunning == false) return;
                    if (this.allPerfChunks.Count > 10000)
                    {
                        ///Someone forgot to kill me...
                        StopTracking(null);
                        return;
                    }

                    PerfCuhnk pc = new PerfCuhnk();

                    pc.ProcessorTime = ((perfCtrProcessorTime == null) ? 0 : perfCtrProcessorTime.NextValue());
                    pc.UserTime = ((perfCtrUserTime == null) ? 0 : perfCtrUserTime.NextValue());
                    pc.WorkingSet = ((perfCtrWorkingSet == null) ? 0 : ((((uint)perfCtrWorkingSet.NextValue() / 1024))));
                    pc.WorkingSetPrivate = ((perfCtrWorkingSetPrivate == null) ? 0 : ((((uint)perfCtrWorkingSetPrivate.NextValue() / 1024))));

                    this.allPerfChunks.AddLast(pc);
                }
            }
        }

		#endregion

		private class PerfCuhnk
		{
			public float ProcessorTime = 0;
			public float UserTime = 0;
			public float WorkingSet = 0;
			public float WorkingSetPrivate = 0;
			public DateTime MarkingDateTime = DateTime.Now;
		}

		#region Perf Counters

		private static string GetProcessInstanceName(int pid)
		{
			PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
			string[] instances = cat.GetInstanceNames();

			foreach (string instance in instances)
			{
				using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
				{
					int val = (int)cnt.RawValue;

					if (val == pid)
					{
						return instance;
					}
				}
			}

			return null;
		}
		
		private static PerformanceCounter GetProcessorTimePerfCounter(String instanceName)
		{
			try
			{
				PerformanceCounter perf = new PerformanceCounter();
				perf.BeginInit();
				perf.CategoryName = "Process";
				perf.CounterName = "% user Time";
				perf.InstanceName = instanceName;
				perf.MachineName = Environment.MachineName;
				perf.EndInit();

				return perf;
			}
			catch
			{
				return null;
			}

		}
		private static PerformanceCounter GetUserTimePerfCounter(String instanceName)
		{
			try
			{
				PerformanceCounter perf = new PerformanceCounter();
				perf.BeginInit();
				perf.CategoryName = "Process";
				perf.CounterName = "% Processor Time";
				perf.InstanceName = instanceName;
				perf.MachineName = Environment.MachineName;
				perf.EndInit();

				return perf;
			}
			catch
			{
				return null;
			}

		}
		private static PerformanceCounter GetWorkingSetPerfCounter(String instanceName)
		{
			try
			{

				PerformanceCounter perf = new PerformanceCounter();
				perf.BeginInit();
				perf.CategoryName = "Process";
				perf.CounterName = "Working Set";
				perf.InstanceName = instanceName;
				perf.MachineName = Environment.MachineName;
				perf.EndInit();

				return perf;
			}
			catch
			{
				return null;
			}

		}
		private static PerformanceCounter GetWorkingSetPrivatePerfCounter(String instanceName)
		{
			try
			{

				PerformanceCounter perf = new PerformanceCounter();
				perf.BeginInit();
				perf.CategoryName = "Process";
				perf.CounterName = "Working Set - Private";
				perf.InstanceName = instanceName;
				perf.MachineName = Environment.MachineName;
				perf.EndInit();

				return perf;
			}
			catch
			{
				return null;
			}
		}
		#endregion
	}
}













