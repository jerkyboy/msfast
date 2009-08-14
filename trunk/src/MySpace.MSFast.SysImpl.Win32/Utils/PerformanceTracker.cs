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

namespace MySpace.MSFast.SysImpl.Win32.Utils
{
	public class PerformanceTracker
	{

        private static readonly MySpace.MSFast.Core.Logger.MSFastLogger log = MSFastLogger.GetLogger(typeof(PerformanceTracker));
		
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

			Thread t = new Thread(this.TimerThread);
			t.Start();
			
			lock (trackingLock)
			{
				if (isTracking)
				{
					StopTracking(null);
				}

				isTracking = true;

				Monitor.PulseAll(trackingLock);
			}
            if (log.IsDebugEnabled) log.Debug("Performance tracking Started!");
		}

		public void StopTracking(Stream saveTo)
		{
            if (log.IsDebugEnabled) log.Debug("Stop tracking performance...(" + isTracking + ")");

			LinkedList<PerfCuhnk> pc = null;
			lock (trackingLock)
			{
				if (!isTracking)
				{
					return;
				}

				isTracking = false;

                if (saveTo != null)
				{
					pc = new LinkedList<PerfCuhnk>(this.allPerfChunks);
				}

				this.allPerfChunks.Clear();
			}
            if (log.IsDebugEnabled) log.Debug("Collected " + pc.Count + " Performance Segments");
            
            if (saveTo != null && pc != null && pc.Count > 0)
			{
                TextWriter streamWriter = new StreamWriter(saveTo);
				foreach (PerfCuhnk chunk in pc)
				{
					streamWriter.WriteLine("?" + SecondsFromEpoch(chunk.MarkingDateTime) + ":PT" + Math.Min(100,chunk.ProcessorTime) + ":UT" + Math.Min(100,chunk.UserTime) + ":WS" + chunk.WorkingSet + ":PWS" + chunk.WorkingSetPrivate);
				}
                streamWriter.Flush();
				streamWriter.Close();
			}
            if (log.IsDebugEnabled) log.Debug("Performance tracking ended!");

		}

		#region Performance Monitor

		public static readonly DateTime JAN_01_1970 = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0), DateTimeKind.Utc);

		private LinkedList<PerfCuhnk> allPerfChunks = null;

		private PerformanceCounter perfCtrProcessorTime = null;
		private PerformanceCounter perfCtrUserTime = null;
		private PerformanceCounter perfCtrWorkingSet = null;
		private PerformanceCounter perfCtrWorkingSetPrivate = null;

		private bool isDisposed = false;
		private bool isTracking = false;
		private object trackingLock = new Object();

		public void Dispose()
		{
			StopTracking(null);

			isDisposed = true;

			lock (trackingLock)
			{
				Monitor.PulseAll(trackingLock);
			}
		}

		private void TimerThread()
		{
			while (isDisposed == false)
			{
				Thread.Sleep(100);

				lock (trackingLock)
				{
					if (isTracking == false && isDisposed == false)
					{
						Monitor.Wait(trackingLock);
					}

					if (isTracking && isDisposed == false)
					{
						if (this.allPerfChunks.Count > 10000)
						{
							///Someone forgot to kill me...
							StopTracking(null);
							continue;
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

		// Get Unix Timestamp for given DateTime
		public static long SecondsFromEpoch(DateTime date)
		{
			DateTime dt = date.ToUniversalTime();
			TimeSpan ts = dt.Subtract(JAN_01_1970);
			return (long)ts.TotalMilliseconds;
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













