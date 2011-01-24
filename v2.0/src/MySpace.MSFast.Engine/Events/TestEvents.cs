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
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.Engine.Events
{
    public enum TestEventType : int
    {
        RenderingSegment = 0,
        CapturingSegment = 1,
        RequestingFile = 2,
        SendingData = 3,
        ReceivingData = 4,
        ResponseEnded = 5,
        StartingTest = 6,
        TestStarted = 7,
        TestEnded = 8,
        AbortingTest = 9,
        ProcessingResults = 10
    }

    public delegate void OnTestEventHandler(TestEventType progressEventType, int progress, int total, String url);

    public class TestEvents
    {
        public static bool IsVerbose = false;

        private static Dictionary<Regex, ProcessMessage> progressMessages;

        private delegate void ProcessMessage(Match match, OnTestEventHandler callback);

        static TestEvents()
        {
            progressMessages = new Dictionary<Regex, ProcessMessage>();
            progressMessages.Add(new Regex("Capturing segment ([0-9]{1,3}) out of ([0-9]{1,3})", RegexOptions.Compiled), new ProcessMessage(ProcessProgressScreenCapture));
            progressMessages.Add(new Regex("Rendering segment ([0-9]{1,3}) out of ([0-9]{1,3})", RegexOptions.Compiled), new ProcessMessage(ProcessProgressRenderSegment));
            progressMessages.Add(new Regex("Test Started...", RegexOptions.Compiled), new ProcessMessage(ProcessProgressTestStarted));
            progressMessages.Add(new Regex("Requesting (.*)", RegexOptions.Compiled), new ProcessMessage(ProcessProgressRequestingFile));
            progressMessages.Add(new Regex("Done Receiving (.*)", RegexOptions.Compiled), new ProcessMessage(ProcessProgressResponseEnded));
            progressMessages.Add(new Regex("Sending Data \\((.*?)\\) (.*)", RegexOptions.Compiled), new ProcessMessage(ProcessProgressSendingData));
            progressMessages.Add(new Regex("Receiving Data \\((.*?)\\) (.*)", RegexOptions.Compiled), new ProcessMessage(ProcessProgressReceivingData));
        }

        public static void FireProgressEvent(TestEventType progressEventType)
        {
            FireProgressEvent(progressEventType, -1, -1, null);
        }

        public static void FireProgressEvent(TestEventType progressEventType, int progress, int total)
        {
            FireProgressEvent(progressEventType, progress, total, null);
        }

        public static void FireProgressEvent(TestEventType progressEventType, Uri url)
        {
            FireProgressEvent(progressEventType, -1, -1, url);
        }

        public static void FireProgressEvent(TestEventType progressEventType, int progress, int total, Uri url)
        {
            if (IsVerbose)
            {
                if (progressEventType == TestEventType.RequestingFile)
                {
                    Console.WriteLine("Requesting " + url);
                }
                else if (progressEventType == TestEventType.SendingData)
                {
                    Console.WriteLine("Sending Data (" + progress + ") " + url);
                }
                else if (progressEventType == TestEventType.ReceivingData)
                {
                    Console.WriteLine("Receiving Data (" + progress + ") " + url);
                }
                else if (progressEventType == TestEventType.ResponseEnded)
                {
                    Console.WriteLine("Done Receiving " + url);
                }
            }
            
            if (progressEventType == TestEventType.CapturingSegment)
            {
                Console.WriteLine(String.Concat("Capturing segment ", progress, " out of ", total));
            }
            else if (progressEventType == TestEventType.RenderingSegment)
            {
                Console.WriteLine(String.Concat("Rendering segment ", progress, " out of ", total));
            }
            else if (progressEventType == TestEventType.TestStarted)
            {
                Console.WriteLine("Test Started...");
            }
            else if (progressEventType == TestEventType.TestEnded)
            {
                Console.WriteLine("Test Ended!");
            }
        }

        public static void ProcessProgress(string p, OnTestEventHandler OnTestProgress)
        {
            if (String.IsNullOrEmpty(p) || OnTestProgress == null)
                return;

            try
            {
                Match m = null;

                foreach (Regex rg in progressMessages.Keys)
                {
                    m = rg.Match(p);
                    if (m.Success)
                    {
                        progressMessages[rg](m, OnTestProgress);
                    }
                }
            }
            catch
            {
            }
        }

        private static void ProcessProgressTestStarted(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.TestStarted, -1, -1, null);
            }
        }
        
        private static void ProcessProgressRenderSegment(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.RenderingSegment, int.Parse(m.Groups[1].ToString()), int.Parse(m.Groups[2].ToString()), null);
            }
        }

        private static void ProcessProgressScreenCapture(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.CapturingSegment, int.Parse(m.Groups[1].ToString()), int.Parse(m.Groups[2].ToString()), null);
            }
        }

        private static void ProcessProgressRequestingFile(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.RequestingFile, -1, -1, m.Groups[1].Value);
            }
        }

        private static void ProcessProgressResponseEnded(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.ResponseEnded, -1, -1, m.Groups[1].Value);
            }
        }
 
        private static void ProcessProgressSendingData(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.SendingData, int.Parse(m.Groups[1].Value), -1, m.Groups[2].Value);
            }
        }

        private static void ProcessProgressReceivingData(Match m, OnTestEventHandler OnTestProgress)
        {
            if (m != null && m.Success && OnTestProgress != null)
            {
                OnTestProgress(TestEventType.ReceivingData, int.Parse(m.Groups[1].Value), -1, m.Groups[2].Value);
            }
        }
        

    }
}
