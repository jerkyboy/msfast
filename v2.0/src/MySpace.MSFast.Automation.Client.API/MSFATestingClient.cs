//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.API)
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
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.Automation.Client.API.Comm;
using MySpace.MSFast.Automation.Client.API.Tests;

namespace MySpace.MSFast.Automation.Client.API
{
    public class TestQueEmptyException : Exception { }

    public class MSFATestingClient
    {
        private String clientID;
        private String clientKey;
        private String baseDomain;
        private int timeout = 60000;

        public MSFATestingClient(String baseDomain, String clientID, String clientKey)
        {
            this.clientID = clientID;
            this.clientKey = clientKey;
            this.baseDomain = baseDomain;
        }

        public MSFATestingClient(String baseDomain, String clientID, String clientKey, int defaultTimeout)
        {
            this.clientID = clientID;
            this.clientKey = clientKey;
            this.baseDomain = baseDomain;
            this.timeout = defaultTimeout;
        }

        public TestIteration GetNextTestQue()
        {
            GetNextTestQueServerResponse tcr = new GetNextTestQueCall().ExecuteCall(this.baseDomain,this.clientID, this.clientKey, timeout);

            if (tcr == null)
                throw new TestingClientException(ErrorCodes.UnexpectedError);

            if(tcr.IsSucceeded == false)
                throw new TestingClientException(tcr.ErrorCode);

            if(String.IsNullOrEmpty(tcr.RawConfig) || tcr.ResultsID == 0)
                throw new TestQueEmptyException();

            CollectorsConfig cc = new CollectorsConfig();
            cc.AppendConfig(new JSONCollectorsConfigLoader(tcr.RawConfig));

            TestIteration testRequest = new TestIteration();

            testRequest.CollectorsConfig = cc;
            testRequest.ResultsID = tcr.ResultsID;
            testRequest.TestName = tcr.TestName;

            return testRequest;
        }

        public void SaveSuccessfulTest(TestIteration testIteration)
        {
            if (testIteration == null || testIteration.ProcessedDataPackage == null) throw new NullReferenceException();

            new SaveSuccessfulTestCall() { 
                TestIteration = testIteration
            }.ExecuteCall(this.baseDomain, this.clientID, this.clientKey, timeout);
        }

        public void MarkFailedTest(TestIteration testIteration)
        {
            new MarkFailedTestCall() { TestIteration = testIteration }.ExecuteCall(this.baseDomain, this.clientID, this.clientKey, timeout);
        }
    }
}
