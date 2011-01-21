using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Client.API.Comm;
using BDika.Client.API.Tests;

namespace BDika.Client.API
{
    public class TestQueEmptyException : Exception { }

    public class BDikaTestingClient
    {
        private String clientID;
        private String clientKey;
        private String baseDomain;

        public BDikaTestingClient(String baseDomain, String clientID, String clientKey)
        {
            this.clientID = clientID;
            this.clientKey = clientKey;
            this.baseDomain = baseDomain;
        }

        public TestIteration GetNextTestQue()
        {
            GetNextTestQueServerResponse tcr = new GetNextTestQueCall().ExecuteCall(this.baseDomain,this.clientID, this.clientKey, 10000);

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

            return testRequest;
        }

        public void SaveSuccessfulTest(TestIteration testIteration)
        {
            if (testIteration == null || testIteration.ProcessedDataPackage == null) throw new NullReferenceException();

            new SaveSuccessfulTestCall() { 
                TestIteration = testIteration
            }.ExecuteCall(this.baseDomain,this.clientID, this.clientKey, 10000);
        }

        public void MarkFailedTest(TestIteration testIteration)
        {
            new MarkFailedTestCall() { TestIteration = testIteration }.ExecuteCall(this.baseDomain,this.clientID, this.clientKey, 10000);
        }
    }
}
